using System;
using System.Diagnostics;
using System.Globalization;
using PolyPaint.DataModels;
using PolyPaint.Views.Pages;

namespace PolyPaint.Converters
{
    public class ApplicationPageValueConverter: BaseConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPage) value)
            {
                case ApplicationPage.WelcomePage:
                    return new WelcomePage();

                case ApplicationPage.DrawingPage:
                    return new FenetreDessin();

                case ApplicationPage.LobbyPage:
                    return new LobbyPage();

                case ApplicationPage.ProfilePage:
                    return new ProfilePage();

                case ApplicationPage.LoadingPage:
                    return new LoadingPage();

                case ApplicationPage.WaitingRoomPage:
                    return new WaitingRoomPage();

                case ApplicationPage.InviteFriendOrBotPage:
                    return new InviteFriendOrBotPage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}