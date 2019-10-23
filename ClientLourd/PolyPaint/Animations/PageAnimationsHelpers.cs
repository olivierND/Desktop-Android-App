using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PolyPaint.Animations
{
    public static class PageAnimationsHelpers
    {
        public static async Task SlideAndFadeInFromRight(this Page page, float seconds)
        {
            Storyboard sb = new Storyboard();
            sb.AddSlideFromRight(seconds, page.WindowWidth);
            sb.AddFadeIn(seconds);

            await Animate(sb, page, seconds);
        }

        public static async Task SlideAndFadeInFromLeft(this Page page, float seconds)
        {
            Storyboard sb = new Storyboard();
            sb.AddSlideFromLeft(seconds, page.WindowWidth);
            sb.AddFadeIn(seconds);

            await Animate(sb, page, seconds);
        }

        public static async Task SlideAndFadeOutToLeft(this Page page, float seconds)
        {
            Storyboard sb = new Storyboard();
            sb.AddSlideToLeft(seconds, page.WindowWidth);
            sb.AddFadeOut(seconds);

            await Animate(sb, page, seconds);
        }

        public static async Task SlideAndFadeOutToRight(this Page page, float seconds)
        {
            Storyboard sb = new Storyboard();
            sb.AddSlideToRight(seconds, page.WindowWidth);
            sb.AddFadeOut(seconds);

            await Animate(sb, page, seconds);
        }

        public static async Task SlideAndFadeOutToTop(this Page page, float seconds)
        {
            Storyboard sb = new Storyboard();
            sb.AddSlideToTop(seconds, page.WindowHeight);
            sb.AddFadeOut(seconds);

            await Animate(sb, page, seconds);
        }

        public static async Task SlideAndFadeOutToBottom(this Page page, float seconds)
        {
            Storyboard sb = new Storyboard();
            sb.AddSlideToBottom(seconds, page.WindowHeight);
            sb.AddFadeOut(seconds);

            await Animate(sb, page, seconds);
        }

        public static async Task SlideAndFadeInFromTop(this Page page, float seconds)
        {
            Storyboard sb = new Storyboard();
            sb.AddSlideFromTop(seconds, page.WindowHeight);
            sb.AddFadeIn(seconds);

            await Animate(sb, page, seconds);
        }

        public static async Task SlideAndFadeInFromBottom(this Page page, float seconds)
        {
            Storyboard sb = new Storyboard();
            sb.AddSlideFromBottom(seconds, page.WindowHeight);
            sb.AddFadeIn(seconds);

            await Animate(sb, page, seconds);
        }

        private static async Task Animate(Storyboard sb, Page page, float seconds)
        {
            sb.Begin(page);

            page.Visibility = Visibility.Visible;

            await Task.Delay((int)seconds * 1000);
        }
    }
}