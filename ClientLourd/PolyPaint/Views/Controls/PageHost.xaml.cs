using System.Windows;
using System.Windows.Controls;
using PolyPaint.Views.Pages;

namespace PolyPaint.Views.Controls
{
    /// <summary>
    /// Interaction logic for PageHost.xaml
    /// </summary>
    public partial class PageHost : UserControl
    {
        public BasePage CurrentPage
        {
            get =>  (BasePage)GetValue(CurrentPageProperty); 
            set => SetValue(CurrentPageProperty, value); 
        }

        // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(PageHost), new UIPropertyMetadata(CurrentPagePropertyChanged));

        public PageHost()
        {
            InitializeComponent();
        }

        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Frame newPageFrame = (d as PageHost)?.NewPage;
            Frame oldPageFrame = (d as PageHost)?.OldPage;

            BasePage oldPageContent = (BasePage)newPageFrame?.Content;

            if (newPageFrame != null) newPageFrame.Content = null;

            if (oldPageContent != null)
            {
                oldPageFrame.Content = oldPageContent;
                oldPageContent.ShouldAnimateOut = true;
            }

            if (newPageFrame != null) newPageFrame.Content = e.NewValue;
        }
    }
}
