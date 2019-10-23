package polymtl.equipe3.clientlegerv2.model

import com.google.gson.annotations.SerializedName
import java.util.*


data class Message(

    @field:SerializedName("senderId")
    val SenderId: String?,
    @field:SerializedName("senderUsername")
    val SenderUsername: String?,
    @field:SerializedName("channel")
    val Channel: String?,
    @field:SerializedName("content")
    val Content: String?,
    @field:SerializedName("timestamp")
    val Timestamp: Date
)

