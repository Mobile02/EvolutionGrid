using EvolutionGrid.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace EvolutionGrid.ViewModel
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brushes = Brushes.Transparent;

            switch (value)
            {
                case ColorSquare.WHITESMOKE:
                    brushes = Brushes.WhiteSmoke;
                    break;
                case ColorSquare.RED:
                    brushes = Brushes.Red;
                    break;
                case ColorSquare.GREEN:
                    brushes = Brushes.Green;
                    break;
                case ColorSquare.BLUE:
                    brushes = Brushes.Blue;
                    break;
                case ColorSquare.SELECTEDBLUE:
                    brushes = Brushes.LightBlue;
                    break;
                case ColorSquare.GRAY:
                    brushes = Brushes.Gray;
                    break;
            }

            return brushes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
