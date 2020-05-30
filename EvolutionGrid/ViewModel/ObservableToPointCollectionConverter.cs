using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace EvolutionGrid.ViewModel
{
    public class ObservableToPointCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<int[]> points = (ObservableCollection<int[]>)value;

            if (points == null)
                return null;

            PointCollection pointCollection = new PointCollection();

            for (int i = 0; i < points.Count(); i++)
            {
                pointCollection.Add(new Point(points.ElementAt(i)[0], points.ElementAt(i)[1]));
            }

            return pointCollection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
