package polymtl.equipe3.clientlegerv2

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.databinding.DataBindingUtil.setContentView
import com.auth0.android.provider.WebAuthProvider
import polymtl.equipe3.clientlegerv2.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView<ActivityMainBinding>(this, R.layout.activity_main)
    }
    // Auth0 triggers an intent on a successful login
    override fun onNewIntent(intent: Intent) {
        if (WebAuthProvider.resume(intent)) {
            return
        }
        super.onNewIntent(intent)
    }
}
