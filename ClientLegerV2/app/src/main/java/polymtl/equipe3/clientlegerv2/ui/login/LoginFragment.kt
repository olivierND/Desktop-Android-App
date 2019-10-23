package polymtl.equipe3.clientlegerv2.ui.login

import android.app.Dialog
import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.navigation.findNavController
import androidx.navigation.fragment.findNavController
import com.auth0.android.Auth0
import com.auth0.android.Auth0Exception
import com.auth0.android.authentication.AuthenticationAPIClient
import com.auth0.android.authentication.AuthenticationException
import com.auth0.android.callback.BaseCallback
import com.auth0.android.provider.AuthCallback
import com.auth0.android.provider.VoidCallback
import com.auth0.android.provider.WebAuthProvider
import com.auth0.android.result.Credentials
import com.auth0.android.result.UserProfile
import polymtl.equipe3.clientlegerv2.MainActivity
import polymtl.equipe3.clientlegerv2.R
import polymtl.equipe3.clientlegerv2.SecureCredentialsManager
import polymtl.equipe3.clientlegerv2.databinding.FragmentLoginBinding

class LoginFragment : Fragment() {
    private lateinit var binding: FragmentLoginBinding
    private lateinit var auth0: Auth0
    private lateinit var credManager: SecureCredentialsManager


    override fun onAttach(context: Context) {
        super.onAttach(context)
        auth0 = Auth0(getString(R.string.com_auth0_client_id), getString((R.string.com_auth0_domain)))
        auth0.isOIDCConformant = true
        credManager = SecureCredentialsManager(context)
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        binding = FragmentLoginBinding.inflate(inflater, container, false)
        binding.btnLogin.setOnClickListener {
            login()
        }

        val token = credManager.getAccessToken()
        if (arguments!!.getBoolean("isLogout", false)) {
            if (token != null) {
                logout()
            }
        } else {
            if (token != null) {
                findNavController().navigate(R.id.action_loginFragment_to_homeFragment)
            }
        }

        return binding.root
    }



    private fun login() {
        WebAuthProvider.login(auth0)
            .withScheme("clientleger")
            .withScope("openid profile email offline_access read:current_user update:current_user_metadata")
            .start(activity as MainActivity, object : AuthCallback {

                //Failed with a dialog
                override fun onFailure(dialog: Dialog) {
                    activity!!.runOnUiThread { dialog.show() }
                }

                override fun onFailure(exception: AuthenticationException?) {
                    activity!!.runOnUiThread {
                        Toast.makeText(
                            context, "Problème de connexion, veuillez réessayer",
                            Toast.LENGTH_SHORT
                        ).show()
                    }
                }

                override fun onSuccess(credentials: Credentials) {
                    credManager.saveCredentials(credentials)
                    val authentication = AuthenticationAPIClient(auth0)
                    authentication
                        .userInfo(credentials.accessToken!!)
                        .start( object : BaseCallback<UserProfile, AuthenticationException> {
                            override fun onSuccess(payload: UserProfile?) {
                                credManager.saveUserinfo(payload!!)
                                this@LoginFragment.findNavController().navigate(R.id.action_loginFragment_to_homeFragment)
                            }

                            override fun onFailure(error: AuthenticationException?) {
                                activity!!.runOnUiThread {
                                    Toast.makeText(
                                        context, "Problème d'obtention des données utilisateur, veuillez réessayer",
                                        Toast.LENGTH_SHORT
                                    ).show()
                                }
                            }
                        })
                }
            })
    }

    private fun logout() {
        WebAuthProvider.logout(auth0)
            .withScheme("clientleger")
            .start(activity, object : VoidCallback {
                override fun onSuccess(payload: Void?) {
                    credManager.removeCredentials()
                }

                override fun onFailure(error: Auth0Exception) {
                    activity!!.runOnUiThread {
                        Toast.makeText(
                            context, "Problème de déconnexion, veuillez réessayer",
                            Toast.LENGTH_SHORT
                        ).show()
                    }
                }
            })
    }

}