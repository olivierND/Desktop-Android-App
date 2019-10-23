package polymtl.equipe3.clientlegerv2.ui.draw

import android.graphics.Color
import android.view.View
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import polymtl.equipe3.clientlegerv2.model.DrawModeEnum
import polymtl.equipe3.clientlegerv2.model.Drawing
import android.widget.SeekBar

class DrawViewModel: ViewModel() {
    var drawing: Drawing = Drawing()
    val currentMode: MutableLiveData<DrawModeEnum> = MutableLiveData(DrawModeEnum.PENCIL)
    val currentStrokeWidth: MutableLiveData<Float> = MutableLiveData(20F)
    val currentColor: MutableLiveData<Int> = MutableLiveData(Color.BLACK)
    val lineWeightVisibility: MutableLiveData<Int> = MutableLiveData()

    fun setMode(mode: DrawModeEnum) {
        currentMode.value = mode
    }

    fun toggleLineWeightVisibility() {
        if (lineWeightVisibility.value == View.VISIBLE) {
            lineWeightVisibility.value = View.GONE
        }
        else {
            lineWeightVisibility.value = View.VISIBLE
        }
    }

    fun onStrokeChanged(seekBar: SeekBar, progress: Int, fromUser: Boolean) {
        if (fromUser) {
            currentStrokeWidth.value = progress * 10.0F
        }
    }

}