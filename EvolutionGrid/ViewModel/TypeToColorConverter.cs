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
    public class TypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brushes = Brushes.Transparent;
            
            switch (value)
            {
                case TypeSquare.BIO:
                    brushes = Brushes.Blue;
                    break;
                case TypeSquare.FOOD:
                    brushes = Brushes.Green;
                    break;
                case TypeSquare.ACID:
                    brushes = Brushes.Red;
                    break;
                case TypeSquare.EMPTY:
                    brushes = Brushes.WhiteSmoke;
                    break;
                case TypeSquare.WALL:
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
