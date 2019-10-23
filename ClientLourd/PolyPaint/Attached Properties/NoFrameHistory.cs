using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PolyPaint.Attached_Properties
{
    public class NoFrameHistory : BaseAttachedProperty<NoFrameHistory, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Frame frame = (Frame) sender;

            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            frame.Navigated += (ss, ee) => ((Frame) ss).NavigationService.RemoveBackEntry();
        }
    }
}