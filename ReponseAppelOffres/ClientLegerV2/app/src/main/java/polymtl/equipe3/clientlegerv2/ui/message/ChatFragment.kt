package polymtl.equipe3.clientlegerv2.ui.message

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.inputmethod.EditorInfo
import androidx.appcompat.app.AppCompatActivity
import androidx.core.os.bundleOf
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.navigation.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import polymtl.equipe3.clientlegerv2.R
import polymtl.equipe3.clientlegerv2.databinding.FragmentChatBinding
import polymtl.equipe3.clientlegerv2.injection.ViewModelFactory


class ChatFragment : Fragment() {

    private lateinit var binding: FragmentChatBinding
    private val viewModel: MessageListViewModel by viewModels {
        ViewModelFactory(context as AppCompatActivity)
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentChatBinding.inflate(inflater, container, false)
        val layout = LinearLayoutManager(context, LinearLayoutManager.VERTICAL, false)
        layout.stackFromEnd = true
        binding.messages.layoutManager = layout
        (binding.messages as View).addOnLayoutChangeListener { _: View,
                                                               _: Int, _: Int, _: Int, bottom: Int,
                                                               _: Int, _: Int, _: Int, oldBottom: Int ->
            if (bottom != oldBottom) {
                binding.messages.post {
                    val pos = binding.messages.adapter?.itemCount
                    if (pos != null && pos > 0) {
                        binding.messages.smoothScrollToPosition(pos - 1)
                    }
                }
            }
        }
        binding.viewModel = viewModel
        binding.lifecycleOwner = this
        binding.btnLogout.setOnClickListener { btnView ->
            var bundle = bundleOf("isLogout" to true)
            btnView.findNavController().navigate(R.id.action_chatFragment_to_loginFragment, bundle)

        }
        binding.enterMessage.setOnEditorActionListener { _, actionId, _ ->
            return@setOnEditorActionListener when (actionId) {
                EditorInfo.IME_ACTION_SEND -> {
                    viewModel.sendMessage()
                    true
                }
                else -> false
            }
        }

        return binding.root
    }

}