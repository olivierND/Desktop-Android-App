package polymtl.equipe3.clientlegerv2.ui.draw

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.SeekBar
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import androidx.lifecycle.Observer
import com.skydoves.colorpickerview.ColorEnvelope
import com.skydoves.colorpickerview.ColorPickerDialog
import com.skydoves.colorpickerview.listeners.ColorEnvelopeListener
import polymtl.equipe3.clientlegerv2.R
import polymtl.equipe3.clientlegerv2.databinding.FragmentDrawBinding

class DrawFragment : Fragment() {
    private lateinit var binding: FragmentDrawBinding

    private val viewModel: DrawViewModel by viewModels()

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentDrawBinding.inflate(inflater, container, false)
        binding.viewModel = viewModel

        binding.drawView.initialize(
            this,
            viewModel.drawing,
            viewModel.currentStrokeWidth,
            viewModel.currentColor,
            viewModel.currentMode
        )

        binding.seekWeight.progress = (viewModel.currentStrokeWidth.value!! / 10F).toInt()

        binding.imgColor.setOnClickListener {
            ColorPickerDialog.Builder(context)
                .setPreferenceName("MyColorPickerDialog")
                .setPositiveButton(getString(R.string.confirm),
                    object : ColorEnvelopeListener {
                        override fun onColorSelected(envelope: ColorEnvelope?, fromUser: Boolean) {
                            viewModel.currentColor.value = envelope!!.color
                        }
                    })
                .setNegativeButton(getString(R.string.cancel))
                { dialogInterface, _ -> dialogInterface.dismiss(); }
                .attachAlphaSlideBar(false) // default is true. If false, do not show the AlphaSlideBar.
                .attachBrightnessSlideBar(true)  // default is true. If false, do not show the BrightnessSlideBar.
                .show()

        }

        binding.imgClear.setOnClickListener { v ->
            binding.drawView.clearAll()
        }

        binding.lifecycleOwner = this
        return binding.root
    }
}