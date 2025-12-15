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
    public class FrequencyToForegroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Default text color is Black (for light backgrounds and defaults)
            IBrush defaultBrush = Brushes.Black;

            if (value is string frequency)
            {
                string normalizedFrequency = frequency.Trim().ToLower();

                switch (normalizedFrequency)
                {
                    case "daily":
                    case "utility":
                        // These typically use dark background colors (Dark Gray/Blue)
                        return Brushes.White;
                    case "encounter":
                    case "at-will":
                        // These typically use light background colors (Red/Green)
                        return Brushes.Black;
                }
            }

            // Fallback: Black text for all others
            return defaultBrush;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // One-way conversion is sufficient for coloring
            throw new NotImplementedException();
        }
    }
}