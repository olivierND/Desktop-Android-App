package polymtl.equipe3.clientlegerv2.injection

import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import polymtl.equipe3.clientlegerv2.SecureCredentialsManager
import polymtl.equipe3.clientlegerv2.ui.message.ChatViewModel

class ViewModelFactory(private val activity: AppCompatActivity) : ViewModelProvider.Factory {
    override fun <T : ViewModel?> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(ChatViewModel::class.java)) {
            val credentialsManager = SecureCredentialsManager(activity)
            @Suppress("UNCHECKED_CAST")
            return ChatViewModel(credentialsManager) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}