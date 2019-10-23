package polymtl.equipe3.clientlegerv2.ui.message

import androidx.lifecycle.MutableLiveData
import polymtl.equipe3.clientlegerv2.base.BaseViewModel
import polymtl.equipe3.clientlegerv2.model.Message
import java.text.SimpleDateFormat
import java.time.LocalDateTime
import java.time.LocalDateTime.now
import java.time.ZonedDateTime
import java.util.*

class MessageViewModel(private val userId: String?) : BaseViewModel() {
    private val messageSender = MutableLiveData<String>()
    private val messageContent = MutableLiveData<String>()
    private val messageIsSent = MutableLiveData<Boolean>()
    private val messageTimeSent = MutableLiveData<Date>()
    private val localDateTime = SimpleDateFormat("HH:mm:ss", Locale.CANADA_FRENCH)

    fun bind(message: Message) {
        messageSender.value = message.SenderUsername
        messageContent.value = message.Content
        messageIsSent.value = message.SenderId == userId
        messageTimeSent.value = message.Timestamp
    }

    fun getMessageSender(): MutableLiveData<String> {
        return messageSender
    }

    fun getMessageContent(): MutableLiveData<String> {
        return messageContent
    }

    fun getMessageIsSent(): MutableLiveData<Boolean> {
        return messageIsSent
    }

    fun getMessageTimeSent(): MutableLiveData<String> {
        localDateTime.timeZone = TimeZone.getTimeZone("GMT-8")
        val formattedTime = localDateTime.format(
           if (messageTimeSent.value != null) messageTimeSent.value else Date(0)
        )

        return MutableLiveData(formattedTime)
    }
}