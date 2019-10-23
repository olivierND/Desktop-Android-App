package polymtl.equipe3.clientlegerv2.ui.gameLobby

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.databinding.DataBindingUtil
import androidx.recyclerview.widget.RecyclerView
import polymtl.equipe3.clientlegerv2.R
import polymtl.equipe3.clientlegerv2.databinding.GameListElementBinding
import polymtl.equipe3.clientlegerv2.model.GameListElement

class GameLobbyAdapter: RecyclerView.Adapter<GameLobbyAdapter.ViewHolder>() {
    private lateinit var gameList:MutableList<GameListElement>

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val binding: GameListElementBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.context), R.layout.game_list_element, parent, false)
        return ViewHolder(binding)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        holder.bind(gameList[position])
        if (position %2 == 1)
            holder.itemView.setBackgroundResource(R.color.grayMedium)
        else
            holder.itemView.setBackgroundResource(R.color.grayLight)
    }

    override fun getItemCount(): Int {
        return if(::gameList.isInitialized) gameList.size else 0
    }

    fun updateGameList(gameList:List<GameListElement>){
        this.gameList = gameList.toMutableList()
        notifyDataSetChanged()
    }

    class ViewHolder(private val binding: GameListElementBinding):
        RecyclerView.ViewHolder(binding.root){
        private val viewModel = GameListElementViewModel()

        fun bind(gameListElement: GameListElement){
            viewModel.bind(gameListElement)
            binding.viewModel = viewModel
        }
    }
}