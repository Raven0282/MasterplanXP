// --- Converters/MonsterColorConverter.cs ---

using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace MasterplanXP.Converters
{
    // Usage: ConverterParameter = 'HeaderBackground', 'BodyBackground', 'NeutralBackground', or 'Foreground'
    public class MonsterColorConverter : IValueConverter
    {
        // Define the Olive Drab colors
        private static readonly IBrush DarkOliveDrab = new SolidColorBrush(Color.Parse("#4C5340")); // Dark Green/Gray (Header Background)
        private static readonly IBrush LightOliveDrab = new SolidColorBrush(Color.Parse("#ADB5A3")); // Light Green/Gray (Body Background)
        private static readonly IBrush DarkForeground = Brushes.Black; // ALWAYS Black foreground as requested
        private static readonly IBrush NeutralBackground = new SolidColorBrush(Color.Parse("#F0F8FF")); // Light Blueish-White for non-power sections

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string param = parameter?.ToString()?.ToLower() ?? "";

            if (targetType == typeof(IBrush))
            {
                // Backgrounds
                if (param == "headerbackground")
                    return DarkOliveDrab;
                if (param == "bodybackground")
                    return LightOliveDrab;
                if (param == "neutralbackground")
                    return NeutralBackground;

                // Foregrounds (Always Black)
                if (param == "headerforeground" || param == "bodyforeground")
                    return DarkForeground; // Black text on ALL power sections, as requested
            }

            return NeutralBackground; // Default fallback
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}