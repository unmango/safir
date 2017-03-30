/**************************************************************************\
    Copyright Microsoft Corporation. All Rights Reserved.
\**************************************************************************/
using System;
using System.Windows;
using System.Windows.Data;

namespace CustomChromeLibrary
{

    public class CaptionButtonRectToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var captionLocation = (Rect)value;

            return new Thickness(0, captionLocation.Top + 6, -captionLocation.Right, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
