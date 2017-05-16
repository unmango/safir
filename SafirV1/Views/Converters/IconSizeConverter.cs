using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Safir.Views.Converters
{
    /// <summary>
    /// Example Usage:
    /// Image Margin="20,20,20,20"
    ///        Grid.Column="0"
    ///        Stretch="None"
    ///        Source="{Binding Path=ImagePath,
    ///                 Converter={StaticResource IconSizeConverter},
    ///                 ConverterParameter=64}">
    /// Image
    /// </summary>
    internal class IconSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var uri = new Uri("pack://application:,,," + value.ToString());
            var decoder = BitmapDecoder.Create(uri,
                                                BitmapCreateOptions.DelayCreation,
                                                BitmapCacheOption.OnDemand);

            int size = int.Parse(parameter.ToString());
            var result = decoder.Frames.FirstOrDefault(f => (f.Width == size));
            if (result == default(BitmapFrame))
                result = decoder.Frames.OrderBy(f => f.Width).First();

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
