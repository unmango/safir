// <copyright file="BooleanToVisibilityConverter.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Resources.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    ///     Converts Boolean Values to Control.Visibility values.
    ///     Source: http://www.codeproject.com/Tips/285358/All-purpose-Boolean-to-Visibility-Converter
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        // Set to true if you want to show control when boolean value is true
        // Set to false if you want to hide/collapse control when value is true

        // Set to true if you just want to hide the control
        // else set to false if you want to collapse the control
        public BooleanToVisibilityConverter() {
            TriggerValue = false;
        }

        public bool TriggerValue { get; set; }

        public bool IsHidden { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            bool booleanValue = value != null;

            if (value is string) {
                booleanValue = !string.IsNullOrEmpty(value as string);
            } else if (value is bool) {
                booleanValue = (bool)value;
            }

            return GetVisibility(booleanValue);
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {
            throw new NotImplementedException();
        }

        public object GetVisibility(bool toggleValue) {
            if ((toggleValue && TriggerValue && IsHidden)
                || (!toggleValue && !TriggerValue && IsHidden))
                return Visibility.Hidden;
            if ((toggleValue && TriggerValue && !IsHidden)
                || (!toggleValue && !TriggerValue && !IsHidden))
                return Visibility.Collapsed;
            return Visibility.Visible;
        }
    }
}
