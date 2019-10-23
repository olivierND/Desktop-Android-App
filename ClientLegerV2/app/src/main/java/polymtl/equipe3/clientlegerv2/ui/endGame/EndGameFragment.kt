package polymtl.equipe3.clientlegerv2.ui.endGame

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.navigation.findNavController
import polymtl.equipe3.clientlegerv2.R
import polymtl.equipe3.clientlegerv2.databinding.FragmentEndGameBinding


class EndGameFragment : Fragment() {

    private lateinit var binding: FragmentEndGameBinding
    private val viewModel:EndGameViewModel by viewModels()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentEndGameBinding.inflate(inflater, container, false)

        binding.viewModel = viewModel
        binding.lifecycleOwner = this

        binding.btnBack.setOnClickListener { btnView ->
            btnView.findNavController().navigate(R.id.action_endGameFragment_to_gameLobbyFragment)
        }

        return binding.root
    }

}
