using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EvolutionGrid.Model
{
    public class ChartLife
    {
        public ObservableCollection<Point> UpdateChart(int[] pointY, int offsetX)
        {
            ObservableCollection<Point> points = new ObservableCollection<Point>();

            for (int pointX = 0; pointX < pointY.Length; pointX++)
            {
                if (pointY[pointX + offsetX] == 0)
                    break;
                points.Add(new Point(pointX, pointY[pointX + offsetX]));

            }

            return points;
        }
    }
}
