package polymtl.equipe3.clientlegerv2.ui.message

import android.util.Log
import android.view.View
import androidx.databinding.ObservableField
import androidx.databinding.ObservableInt
import androidx.lifecycle.MutableLiveData
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.disposables.Disposable
import polymtl.equipe3.clientlegerv2.SecureCredentialsManager
import polymtl.equipe3.clientlegerv2.base.BaseViewModel
import polymtl.equipe3.clientlegerv2.model.Message
import polymtl.equipe3.clientlegerv2.repository.MessageRepository
import javax.inject.Inject


class ChatViewModel(private val credentialsManager: SecureCredentialsManager) :
    BaseViewModel() {

    @Inject
    lateinit var messageRepo: MessageRepository

    private lateinit var subscription: Disposable

    val loadingVisibility: MutableLiveData<Int> = MutableLiveData()
    private val errorMessage: MutableLiveData<String> = MutableLiveData()
    var chatAdapter: ChatAdapter

    var inputText = ObservableField("")
    var scrollTo = ObservableInt()


    init {
        // loadMessages()
        messageRepo.joinChat(credentialsManager.getAccessToken()!!)
        chatAdapter = ChatAdapter(credentialsManager.getUserId())
    }

    override fun onCleared() {
        super.onCleared()
        messageRepo.closeChat()
        subscription.dispose()
    }

    private fun loadMessages() {
        subscription = messageRepo.getAllMessages(credentialsManager.getAccessToken()!!)
            //.subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .doOnSubscribe { onRetrievePostListStart() }
            .doOnNext {
                Log.d("TEST_OBSERVABLE", "doOnNext called")
            }
            .doOnComplete {
                Log.d("TEST_OBSERVABLE", "onComplete called")
            }
            .doOnError{
                onRetrievePostListError()
            }
            .subscribe(
                { result ->
                    onRetrievePostListFinish()
                    onRetrievePostListSuccess(result)
                },
                { onRetrievePostListError() }
            )
    }

    fun sendMessage() {
        if (!inputText.get().isNullOrBlank()) {
            messageRepo.sendChatMessage(credentialsManager.getUsername()!!, inputText.get().toString())
            inputText.set("")
            scrollTo.set(chatAdapter.itemCount - 1)
        }
    }

    private fun onRetrievePostListStart() {
        errorMessage.value = null
        loadingVisibility.value = View.VISIBLE
    }

    private fun onRetrievePostListFinish() {
        loadingVisibility.value = View.GONE
    }

    private fun onRetrievePostListSuccess(messages: List<Message>) {
        chatAdapter.updatePostList(messages)
        scrollTo.set(chatAdapter.itemCount - 1)
    }

    private fun onRetrievePostListError() {
        errorMessage.value = "Il y a eu une erreur lors de l'obtention des messages. Réessayez plus tard"
    }
}