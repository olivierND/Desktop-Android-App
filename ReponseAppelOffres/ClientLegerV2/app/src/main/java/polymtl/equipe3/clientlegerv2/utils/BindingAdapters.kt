package polymtl.equipe3.clientlegerv2.utils

import android.view.Gravity
import android.view.View
import android.widget.LinearLayout
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.databinding.BindingAdapter
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.RecyclerView

@BindingAdapter("mutableVisibility")
fun setMutableVisibility(view: View, visibility: MutableLiveData<Int>?) {
    val parentActivity: AppCompatActivity? = view.getParentActivity()
    if (parentActivity != null && visibility != null) {
        visibility.observe(
            parentActivity,
            Observer { value -> view.visibility = value ?: View.VISIBLE })
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