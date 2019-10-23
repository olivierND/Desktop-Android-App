package polymtl.equipe3.clientlegerv2.ui.gameLobby

import androidx.lifecycle.ViewModel
import polymtl.equipe3.clientlegerv2.model.GameListElement

class GameLobbyViewModel : ViewModel() {

    var gameLobbyAdapter: GameLobbyAdapter = GameLobbyAdapter()

    init {
        loadCurrentGames()
    }

    private fun loadCurrentGames() {
        gameLobbyAdapter.updateGameList(listOf(GameListElement(), GameListElement()))
    }

}
