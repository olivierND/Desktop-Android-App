package polymtl.equipe3.clientlegerv2.injection.component

import dagger.Component
import polymtl.equipe3.clientlegerv2.injection.module.NetworkModule
import polymtl.equipe3.clientlegerv2.ui.message.ChatViewModel
import polymtl.equipe3.clientlegerv2.ui.message.MessageViewModel
import javax.inject.Singleton

/**
 * Component providing inject() methods for presenters.
 */
@Singleton
@Component(modules = [(NetworkModule::class)])
interface ViewModelInjector {
    /**
     * Injects required dependencies into the specified ChatViewModel.
     * @param chatViewModel ChatViewModel in which to inject the dependencies
     */
    fun inject(chatViewModel: ChatViewModel)

    /**
     * Injects required dependencies into the specified MessageViewModel.
     * @param messageListViewModel ChatViewModel in which to inject the dependencies
     */
    fun inject(messageViewModel: MessageViewModel)

    @Component.Builder
    interface Builder {
        fun build(): ViewModelInjector

        fun networkModule(networkModule: NetworkModule): Builder
    }
}