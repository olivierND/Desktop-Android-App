package polymtl.equipe3.clientlegerv2.ui.message

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.databinding.DataBindingUtil
import androidx.recyclerview.widget.RecyclerView
import polymtl.equipe3.clientlegerv2.R
import polymtl.equipe3.clientlegerv2.databinding.MessageBinding
import polymtl.equipe3.clientlegerv2.model.Message

class ChatAdapter(private val userId: String?): RecyclerView.Adapter<ChatAdapter.ViewHolder>() {
    private lateinit var messageList:MutableList<Message>

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val binding: MessageBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.context), R.layout.message, parent, false)
        return ViewHolder(binding, userId)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        holder.bind(messageList[position])
    }

    override fun getItemCount(): Int {
        return if(::messageList.isInitialized) messageList.size else 0
    }

    fun updatePostList(postList:List<Message>){
        this.messageList = postList.toMutableList()
        notifyDataSetChanged()
    }

    class ViewHolder(private val binding: MessageBinding, private val userId: String?):RecyclerView.ViewHolder(binding.root){
        private val viewModel = MessageViewModel(userId)

        fun bind(message:Message){
            viewModel.bind(message)
            binding.viewModel = viewModel
        }
    }
}