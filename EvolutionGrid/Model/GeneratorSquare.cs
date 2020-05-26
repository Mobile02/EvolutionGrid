using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EvolutionGrid.Model
{
    public class GeneratorSquare
    {
        private Random random = new Random();
        private Constants constants = new Constants();

        private int[] GeneratorBrain()
        {
            int[] brain = new int[constants.SizeBrain];

            for (int i = 0; i < constants.SizeBrain; i++)
            {
                brain[i] = random.Next(64);
            }

            return (int[])brain.Clone();
        }

        private int[] GeneratorBrainMutant(Square square)
        {
            int[] brainMutant = new int[constants.SizeBrain];

            brainMutant = (int[])square.Brain.Clone();

            if (random.Next(8) == 2 || random.Next(8) == 7)
                brainMutant[random.Next(constants.SizeBrain)] = random.Next(constants.SizeBrain);

            return (int[])brainMutant.Clone();
        }

        public void AddBioSquare(Square[][] worldMap, int count)
        {
            int x, y;

            while (count > 0)
            {
                y = random.Next(1, constants.WorldSizeY - 1);
                x = random.Next(1, constants.WorldSizeX - 1);

                if (worldMap[y][x].Fill == Brushes.WhiteSmoke)
                {
                    worldMap[y][x].Fill = Brushes.Blue;
                    worldMap[y][x].Health = constants.HealthSquare;
                    worldMap[y][x].Direction = (Direction)random.Next(8);
                    worldMap[y][x].Pointer = 0;
                    worldMap[y][x].PointX = x;
                    worldMap[y][x].PointY = y;
                    worldMap[y][x].Brain = GeneratorBrain();

                    count--;
                }
            }
        }

        public void AddAcidSquare(Square[][] worldMap, int count)
        {
            int tmpCount = 0;

            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 1; x < constants.WorldSizeX - 1; x++)
                {
                    if (worldMap[y][x].Fill == Brushes.Red)
                        tmpCount++;
                }
            }

            if (tmpCount >= constants.CountAcid + 10)
                return;
            else
                tmpCount = 0;

            while (tmpCount < count)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (worldMap[y][x].Fill == Brushes.WhiteSmoke)
                {
                    worldMap[y][x].Fill = Brushes.Red;
                    tmpCount++;
                }
            }
        }

        public void AddFoodSquare(Square[][] worldMap, int count)
        {
            int tmpCount = 0;

            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 1; x < constants.WorldSizeX - 1; x++)
                {
                    if (worldMap[y][x].Fill == Brushes.Green)
                        tmpCount++;
                }
            }

            if (tmpCount >= constants.CountFood)
                return;
            else
                tmpCount = 0;

            while (tmpCount < count)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (worldMap[y][x].Fill == Brushes.WhiteSmoke)
                {
                    worldMap[y][x].Fill = Brushes.Green;
                    tmpCount++;
                }
            }
        }

        public void AddChild(Square[][] worldMap)
        {
            //new FileOperation().SaveBrain(worldMap);

            Square[] arraySquares = new Square[constants.CountSquare / 8];

            int count = 0;
            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 1; x < constants.WorldSizeX - 1; x++)
                {
                    if (worldMap[y][x].Fill == Brushes.Blue)
                    {
                        arraySquares[count] = (Square)worldMap[y][x].Clone();

                        count++;
                    }
                }
            }

            for (int i = 0; i < arraySquares.Length; i++)
            {
                int tmpCount = 0;

                while (tmpCount < 7)
                {
                    int x = random.Next(1, constants.WorldSizeX - 1);
                    int y = random.Next(1, constants.WorldSizeY - 1);

                    if (worldMap[y][x].Fill == Brushes.WhiteSmoke)
                    {
                        worldMap[y][x].Fill = Brushes.Blue;
                        worldMap[y][x].Health = constants.HealthSquare;
                        worldMap[y][x].Direction = (Direction)random.Next(8);
                        worldMap[y][x].Pointer = 0;
                        worldMap[y][x].PointX = x;
                        worldMap[y][x].PointY = y;
                        worldMap[y][x].Brain = GeneratorBrainMutant(arraySquares[i]);

                        tmpCount++;
                    }
                }
            }
        }

        public void AddWall(Square[][] worldMap, int count)
        {
            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (worldMap[y][x].Fill == Brushes.WhiteSmoke)
                {
                    worldMap[y][x].Fill = Brushes.Gray;
                    count--;
                }
            }
        }

        public void RefreshSquare(Square[][] worldMap)
        {
            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 1; x < constants.WorldSizeX - 1; x++)
                {
                    if (worldMap[y][x].Fill != Brushes.Gray && worldMap[y][x].Fill != Brushes.Blue)
                    {
                        worldMap[y][x].Fill = Brushes.WhiteSmoke;
                        worldMap[y][x].Pointer = 0;
                        worldMap[y][x].Brain = null;
                    }

                    if (worldMap[y][x].Fill == Brushes.Blue)
                        worldMap[y][x].Health = constants.HealthSquare;
                }
            }
        }
    }
}
