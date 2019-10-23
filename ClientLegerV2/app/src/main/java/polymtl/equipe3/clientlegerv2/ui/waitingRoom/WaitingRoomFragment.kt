package polymtl.equipe3.clientlegerv2.ui.waitingRoom

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.navigation.findNavController
import polymtl.equipe3.clientlegerv2.R
import polymtl.equipe3.clientlegerv2.databinding.FragmentWaitingRoomBinding


class WaitingRoomFragment : Fragment() {

    private lateinit var binding: FragmentWaitingRoomBinding
    private val viewModel: WaitingRoomViewModel by viewModels()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentWaitingRoomBinding.inflate(inflater, container, false)

        binding.viewModel = viewModel
        binding.lifecycleOwner = this

        binding.btnStart.setOnClickListener { btnView ->
            btnView.findNavController().navigate(R.id.action_waitingRoomFragment_to_classicGameFragment)
        }

        return binding.root
    }

}
