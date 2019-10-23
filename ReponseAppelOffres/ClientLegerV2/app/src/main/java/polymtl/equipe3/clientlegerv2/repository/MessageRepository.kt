package polymtl.equipe3.clientlegerv2.repository

import android.os.AsyncTask
import android.util.Log
import com.google.gson.GsonBuilder
import com.microsoft.signalr.HubConnection
import com.microsoft.signalr.HubConnectionBuilder
import io.reactivex.Observable
import io.reactivex.Single
import io.reactivex.subjects.BehaviorSubject
import polymtl.equipe3.clientlegerv2.model.Message
import polymtl.equipe3.clientlegerv2.network.MessageApi
import polymtl.equipe3.clientlegerv2.utils.MESSAGE_HUB_URL
import java.lang.Exception
import javax.inject.Inject
import javax.inject.Singleton

@Singleton
class MessageRepository @Inject constructor(private val messageApi: MessageApi) {

    private var allMessages: BehaviorSubject<List<Message>> = BehaviorSubject.create()
    private lateinit var hubConnection: HubConnection

    fun getAllMessages(authToken: String): Observable<List<Message>> {
        messageApi.getMessages("Bearer $authToken").doOnSubscribe { disp ->
            allMessages.onSubscribe(disp)
        }.doOnError{t ->
            allMessages.onError(t)
        }
        .subscribe { messages ->
            allMessages.onNext(messages.reversed())
        }
        return allMessages
    }

    fun joinChat(authToken: String) {

        val gson = GsonBuilder().setDateFormat("yyyy-MM-dd'T'HH:mm:ss").create()
        val hubURL = MESSAGE_HUB_URL
        hubConnection = HubConnectionBuilder.create(hubURL)
            .withAccessTokenProvider(Single.defer {
                Single.just(authToken)
            }).build()
        hubConnection.on(
            "ReceiveMessage",
            { message ->
                Log.d("signalR", "message reçu")
                val chatMessage = gson.fromJson(message, Message::class.java)
                if (chatMessage != null) {
                    val existingMessages = allMessages.value
                    if (existingMessages == null) {
                        allMessages.onNext(listOf(chatMessage))
                    } else {
                        allMessages.onNext(existingMessages + chatMessage)
                    }
                }
            },
            String::class.java
        )
        hubConnection.on(
            "OnConnected",
            { message -> Log.d("signalR", "Connected: $message") },
            String::class.java
        )
        HubConnectionTask().execute(hubConnection)
    }

    fun sendChatMessage(username: String, messageContent: String) {
        hubConnection.send("SendMessage", username, messageContent)
    }

    fun closeChat(){
        hubConnection.stop()
    }

    internal inner class HubConnectionTask : AsyncTask<HubConnection, Void, Void>() {

        override fun doInBackground(vararg hubConnections: HubConnection): Void? {
            val hubConnection = hubConnections[0]
            hubConnection.start().blockingAwait()
            return null
        }
    }
}