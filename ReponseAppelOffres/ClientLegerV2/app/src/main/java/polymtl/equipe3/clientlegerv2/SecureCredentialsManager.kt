package polymtl.equipe3.clientlegerv2

import android.content.Context
import com.auth0.android.jwt.JWT
import com.auth0.android.result.Credentials
import com.auth0.android.result.UserProfile
import polymtl.equipe3.clientlegerv2.utils.*
import java.util.*


class SecureCredentialsManager(private val context: Context) {

    fun saveCredentials(credentials: Credentials) {
        val sp = context.getSharedPreferences(PREFS_FILENAME, Context.MODE_PRIVATE)
        // Decodes the JWT token and get the "subject" from it, which is the userId
        val jwt = JWT(credentials.idToken!!)
        sp!!.edit()
            .putString(AUTH_TOKEN, credentials.accessToken)
            .putString(AUTH0_ID, jwt.subject)
            .putLong(AUTH_TOKEN_EXPIRES_AT, credentials.expiresAt!!.time)
            .apply()
    }

    fun saveUserinfo(userInfo: UserProfile) {
        val sp = context.getSharedPreferences(PREFS_FILENAME, Context.MODE_PRIVATE)
        val actualName = if (userInfo.nickname != null) userInfo.nickname else userInfo.name

        sp!!.edit()
            .putString(AUTH0_NAME, actualName)
            .apply()
    }

    fun getAccessToken(): String? {
        val sp = context.getSharedPreferences(PREFS_FILENAME, Context.MODE_PRIVATE)
        val expDate = sp!!.getLong(AUTH_TOKEN_EXPIRES_AT, 0L)
        if (expDate < Date().time) {
            return null
        }

        return sp.getString(AUTH_TOKEN, null)
    }

    fun getUserId(): String? {
        val sp = context.getSharedPreferences(PREFS_FILENAME, Context.MODE_PRIVATE)
        return sp!!.getString(AUTH0_ID, null)
    }

    fun getUsername(): String? {
        val sp = context.getSharedPreferences(PREFS_FILENAME, Context.MODE_PRIVATE)
        return sp!!.getString(AUTH0_NAME, null)
    }

    fun removeCredentials() {
        val sp = context.getSharedPreferences(PREFS_FILENAME, Context.MODE_PRIVATE)

        sp!!.edit()
            .remove(AUTH_TOKEN)
            .remove(AUTH_TOKEN_EXPIRES_AT)
            .remove(AUTH0_ID)
            .remove(AUTH0_NAME)
            .apply()
    }
}