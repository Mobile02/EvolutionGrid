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
        private int offsetX = 0;
        private int maxPointX;
        private Constants constants = new Constants();

        public ObservableCollection<int[]> UpdateChart(int[] pointY, int generation)
        {
            maxPointX = generation;

            if (generation > constants.ScaleChart)
            {
                offsetX = generation - constants.ScaleChart;
                maxPointX = constants.ScaleChart;
            }

            ObservableCollection<int[]> points = new ObservableCollection<int[]>();

            int[] point = new int[2];

            for (int pointX = 0; pointX < maxPointX; pointX++)
            {
                point[0] = pointX ;
                point[1] = pointY[pointX + offsetX];
                points.Add((int[])point.Clone());
            }

            return points;
        }
    }
}
