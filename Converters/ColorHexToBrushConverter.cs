using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;

namespace MasterplanXP.Converters
{
    /// <summary>
    /// Converts a string representing a hex color code (e.g., "#FF0000") into an Avalonia SolidColorBrush.
    /// </summary>
    public class ColorHexToBrushConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string hexColor && targetType.IsAssignableFrom(typeof(IBrush)))
            {
                try
                {
                    return new SolidColorBrush(Color.Parse(hexColor));
                }
                catch
                {
                    // Fallback to transparent or a default error color
                    return Brushes.Transparent;
                }
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}