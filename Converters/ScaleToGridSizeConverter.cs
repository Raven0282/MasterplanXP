using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using System.Globalization;

namespace MasterplanXP.Converters
{
    /// <summary>
    /// Converts a logical ScaleFactor (from TokenViewModel) into a pixel size 
    /// by multiplying it with the GridCellSize (passed as the Parameter).
    /// </summary>
    public class ScaleToGridSizeConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double scaleFactor && parameter is double gridSize && targetType == typeof(double))
            {
                // Pixel Size = Logical Scale Factor * Grid Cell Size
                return scaleFactor * gridSize;
            }

            // Fallback: Return a default size if conversion fails
            return 50.0;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}