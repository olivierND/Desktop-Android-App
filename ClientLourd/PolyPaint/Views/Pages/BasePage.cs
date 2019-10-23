using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PolyPaint.Animations;

namespace PolyPaint.Views.Pages
{
    public class BasePage : Page
    {
        #region Public Properties

        public PageAnimation PageLoadAnimation { get; set; }

        public PageAnimation PageUnloadAnimation { get; set; }

        public float SlideSeconds { get; set; } = 0.4f;

        public bool ShouldAnimateOut { get; set; } 

        #endregion

        #region Contructor

        public BasePage()
        {
            PageLoadAnimation = GetPageLoadAnimation();
            PageUnloadAnimation = GetPageUnloadAnimation();

            Visibility = Visibility.Collapsed;
            Loaded += BasePage_Loaded;
        }

        #endregion

        #region Functions

        private PageAnimation GetPageLoadAnimation()
        {
            switch (GetType().Name)
            {
                case nameof(WelcomePage):
                    return PageAnimation.SlideAndFadeInFromLeft;

                case nameof(ProfilePage) :
                case nameof(InviteFriendOrBotPage):
                    return PageAnimation.SlideAndFadeInFromBottom;

                default:
                    return PageAnimation.None;
            }
        }

        private PageAnimation GetPageUnloadAnimation()
        {
            switch (GetType().Name)
            {
                case nameof(WelcomePage):
                    return PageAnimation.SlideAndFadeOutToRight;

                case nameof(ProfilePage):
                case nameof(InviteFriendOrBotPage):
                    return PageAnimation.SlideAndFadeOutToBottom;

                default:
                    return PageAnimation.None;
            }
        }

        private async void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ShouldAnimateOut)
            {
                await AnimateOut();
            }
            else
            {
                await AnimateIn();
            }
        }

        public async Task AnimateOut()
        {
            if (PageUnloadAnimation == PageAnimation.None)
            {
                Visibility = Visibility.Collapsed;
                return;
            }

            switch (PageUnloadAnimation)
            {
                case PageAnimation.SlideAndFadeOutToRight:

                    await this.SlideAndFadeOutToRight(SlideSeconds);

                    break;

                case PageAnimation.SlideAndFadeOutToLeft:

                    await this.SlideAndFadeOutToLeft(SlideSeconds);

                    break;
                case PageAnimation.SlideAndFadeOutToBottom:

                    await this.SlideAndFadeOutToBottom(SlideSeconds);

                    break;

                case PageAnimation.SlideAndFadeOutToTop:

                    await this.SlideAndFadeOutToTop(SlideSeconds);

                    break;
            }
        }

        public async Task AnimateIn()
        {
            if (PageLoadAnimation == PageAnimation.None)
            {
                Visibility = Visibility.Visible;
                return;
            }

            switch (PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeInFromRight:

                    await this.SlideAndFadeInFromRight(SlideSeconds);

                    break;

                case PageAnimation.SlideAndFadeInFromLeft:

                    await this.SlideAndFadeInFromLeft(SlideSeconds);

                    break;
                case PageAnimation.SlideAndFadeInFromTop:

                    await this.SlideAndFadeInFromTop(SlideSeconds);

                    break;

                case PageAnimation.SlideAndFadeInFromBottom:

                    await this.SlideAndFadeInFromBottom(SlideSeconds);

                    break;
            }
        }



        #endregion

    }

}

