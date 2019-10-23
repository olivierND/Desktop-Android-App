package polymtl.equipe3.clientlegerv2.utils

import android.view.Gravity
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.content.res.AppCompatResources.getDrawable
import androidx.core.content.ContextCompat
import androidx.databinding.BindingAdapter
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.RecyclerView
import polymtl.equipe3.clientlegerv2.R
import polymtl.equipe3.clientlegerv2.model.DrawModeEnum

@BindingAdapter("mutableVisibility")
fun setMutableVisibility(view: View, visibility: MutableLiveData<Int>?) {
    val parentActivity: AppCompatActivity? = view.getParentActivity()
    if (parentActivity != null && visibility != null) {
        visibility.observe(
            parentActivity,
            Observer { value -> view.visibility = value ?: View.VISIBLE })
    }
}

@BindingAdapter("mutableVisibility")
fun setMutableVisibility(viewGroup: ViewGroup, visibility: MutableLiveData<Int>?) {
    val parentActivity: AppCompatActivity? = viewGroup.getParentActivity()
    if (parentActivity != null && visibility != null) {
        visibility.observe(
            parentActivity,
            Observer { value -> viewGroup.visibility = value ?: View.VISIBLE })
    }
}

@BindingAdapter("mutableText")
fun setMutableText(view: TextView, text: MutableLiveData<String>?) {
    val parentActivity: AppCompatActivity? = view.getParentActivity()
    if (parentActivity != null && text != null) {
        text.observe(parentActivity, Observer { value -> view.text = value ?: "" })
    }
}

@BindingAdapter("adapter")
fun setAdapter(view: RecyclerView, adapter: RecyclerView.Adapter<*>) {
    view.adapter = adapter
}

@BindingAdapter("mutableIsSentGravity")
fun setAdapter(view: LinearLayout, boolIsSent: MutableLiveData<Boolean>) {
    val parentActivity: AppCompatActivity? = view.getParentActivity()
    if (parentActivity != null) {
        boolIsSent.observe(
            parentActivity,
            Observer { value -> view.gravity = if (value) Gravity.END else Gravity.START })
    }
}

@BindingAdapter("scrollTo")
fun setScrollTo(recyclerView: RecyclerView, position: Int) {
    recyclerView.scrollToPosition(position)
}


@BindingAdapter("mutableDrawModeBackground")
fun setAdapter(view: View, drawMode: MutableLiveData<DrawModeEnum>) {
    val parentActivity: AppCompatActivity? = view.getParentActivity()
    if (parentActivity != null) {
        if ((drawMode.value == DrawModeEnum.PENCIL && view.id == R.id.img_pencil) ||
            (drawMode.value == DrawModeEnum.BRUSH && view.id == R.id.img_brush) ||
            (drawMode.value == DrawModeEnum.ERASER && view.id == R.id.img_erase) ||
            (drawMode.value == DrawModeEnum.SEGMENT_ERASER && view.id == R.id.img_erase_segment) ) {
            view.background = ContextCompat.getDrawable(parentActivity, R.drawable.button_background)
        }
        else {
            view.background = ContextCompat.getDrawable(parentActivity, R.drawable.nothing)
        }
    }
}