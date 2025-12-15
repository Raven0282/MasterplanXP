// --- Converters/FrequencyToColorConverter.cs (Refined) ---

using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace MasterplanXP.Converters
{
    public class FrequencyToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Default color for Monsters or unknown frequency (Neutral Light Gray)
            IBrush defaultBrush = new SolidColorBrush(Color.Parse("#F0F0F0"));

            if (value is string frequency)
            {
                string normalizedFrequency = frequency.Trim().ToLower();

                switch (normalizedFrequency)
                {
                    case "encounter":
                        // Light Red/Orange
                        return new SolidColorBrush(Color.Parse("#FFCCCC"));
                    case "at-will":
                        // Light Green
                        return new SolidColorBrush(Color.Parse("#CCFFCC"));
                    case "daily":
                        // Dark Gray (Requires White text)
                        return new SolidColorBrush(Color.Parse("#333333"));
                    case "utility":
                        // Blue (Requires White text)
                        return new SolidColorBrush(Color.Parse("#3399FF"));
                }
            }

            return defaultBrush;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}