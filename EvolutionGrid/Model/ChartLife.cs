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
        public ObservableCollection<int[]> UpdateChart(int[] pointY, int offsetX)
        {
            ObservableCollection<int[]> points = new ObservableCollection<int[]>();

            int[] point = new int[2];

            for (int pointX = 0; pointX < pointY.Length; pointX++)
            {
                if (pointY[pointX + offsetX] == 0)
                    break;
                point[0] = pointX ;
                point[1] = pointY[pointX + offsetX];
                points.Add((int[])point.Clone());

            }

            return points;
        }
    }
}
