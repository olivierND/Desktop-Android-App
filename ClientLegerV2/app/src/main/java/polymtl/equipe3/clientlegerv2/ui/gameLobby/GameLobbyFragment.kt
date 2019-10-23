package polymtl.equipe3.clientlegerv2.ui.gameLobby

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.navigation.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import polymtl.equipe3.clientlegerv2.R
import polymtl.equipe3.clientlegerv2.databinding.FragmentGameLobbyBinding


class GameLobbyFragment : Fragment() {

    private lateinit var binding: FragmentGameLobbyBinding
    private val viewModel: GameLobbyViewModel by viewModels()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentGameLobbyBinding.inflate(inflater, container, false)

        val layout = LinearLayoutManager(context, LinearLayoutManager.VERTICAL, false)
        binding.listElements.layoutManager = layout

        binding.viewModel = viewModel
        binding.lifecycleOwner = this

        binding.btnDraw.setOnClickListener { btnView ->
            btnView.findNavController().navigate(R.id.action_gameLobbyFragment_to_waitingRoomFragment)

        }

        return binding.root
    }

}
