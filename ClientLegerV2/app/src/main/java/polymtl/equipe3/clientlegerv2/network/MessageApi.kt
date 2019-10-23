package polymtl.equipe3.clientlegerv2.network

import io.reactivex.Observable
import polymtl.equipe3.clientlegerv2.model.Message
import retrofit2.http.GET
import retrofit2.http.Header

/**
 * The interface which provides methods to get result of webservices
 */
interface MessageApi {
    /**
     * Get the list of messages from the API
     */
    @GET("message")
    fun getMessages(@Header("Authorization") authToken: String): Observable<List<Message>>
}