package polymtl.equipe3.clientlegerv2.ui.gameLobby

import androidx.lifecycle.MutableLiveData
import polymtl.equipe3.clientlegerv2.base.BaseViewModel
import polymtl.equipe3.clientlegerv2.model.GameListElement

class GameListElementViewModel: BaseViewModel() {
    private val gameName = MutableLiveData<String>()
    private val gameMode = MutableLiveData<String>()
    private val numberOfPlayers = MutableLiveData<String>()

    fun bind(gameListElement: GameListElement) {
        gameName.value = gameListElement.gameName
        gameMode.value = gameListElement.gameMode
        numberOfPlayers.value = gameListElement.numberOfPlayers.toString()
    }

    fun getGameName(): MutableLiveData<String> {
        return gameName
    }

    fun getGameMode(): MutableLiveData<String> {
        return gameMode
    }

    fun getNumberOfPlayers(): MutableLiveData<String> {
        return numberOfPlayers
    }
}