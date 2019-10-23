package polymtl.equipe3.clientlegerv2.ui.classicGame

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.viewModels
import androidx.navigation.findNavController
import polymtl.equipe3.clientlegerv2.R
import polymtl.equipe3.clientlegerv2.databinding.FragmentClassicGameBinding


class ClassicGameFragment : Fragment() {

    private lateinit var binding: FragmentClassicGameBinding
    private val viewModel: ClassicGameViewModel by viewModels()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentClassicGameBinding.inflate(inflater, container, false)

        binding.viewModel = viewModel
        binding.lifecycleOwner = this

        binding.btnEndGame.setOnClickListener { btnView ->
            btnView.findNavController().navigate(R.id.action_classicGameFragment_to_endGameFragment)
        }
        return binding.root
    }

}
