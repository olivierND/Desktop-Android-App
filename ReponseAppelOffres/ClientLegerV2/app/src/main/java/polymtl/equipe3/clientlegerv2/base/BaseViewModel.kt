package polymtl.equipe3.clientlegerv2.base

import androidx.lifecycle.ViewModel
import polymtl.equipe3.clientlegerv2.injection.component.DaggerViewModelInjector
import polymtl.equipe3.clientlegerv2.injection.component.ViewModelInjector
import polymtl.equipe3.clientlegerv2.injection.module.NetworkModule
import polymtl.equipe3.clientlegerv2.ui.message.MessageListViewModel
import polymtl.equipe3.clientlegerv2.ui.message.MessageViewModel

abstract class BaseViewModel : ViewModel() {
    private val injector: ViewModelInjector = DaggerViewModelInjector
        .builder()
        .networkModule(NetworkModule)
        .build()

    init {
        inject()
    }

    /**
     * Injects the required dependencies
     */
    private fun inject() {
        when (this) {
            is MessageListViewModel -> injector.inject(this)
            is MessageViewModel -> injector.inject(this)
        }
    }
}