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
    public class NameToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brushes = Brushes.Transparent;
            
            switch (value)
            {
                case NameSquare.BIO:
                    brushes = Brushes.Blue;
                    break;
                case NameSquare.FOOD:
                    brushes = Brushes.Green;
                    break;
                case NameSquare.ACID:
                    brushes = Brushes.Red;
                    break;
                case NameSquare.EMPTY:
                    brushes = Brushes.WhiteSmoke;
                    break;
                case NameSquare.WALL:
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
