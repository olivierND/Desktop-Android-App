using System;
using System.Diagnostics;
using System.Globalization;
using PolyPaint.DataModels;

namespace PolyPaint.Converters
{
    public class GameModeToStringConverter : BaseConverter<GameModeToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((GameMode) value)
            {
                case GameMode.Classic:
                    return "Mode classique";

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