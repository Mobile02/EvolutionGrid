using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EvolutionGrid.Model
{
    public class BehaviorSquare
    {
        private delegate void DelegateCalcPointer();

        private Square[][] worldMap;
        private Constants constants = new Constants();
        private Point currentPoint = new Point();
        private bool minCountLive = false;
        private Dictionary<Brush, DelegateCalcPointer> colorSwitch;

        public BehaviorSquare(Square[][] worldMap)
        {
            this.worldMap = worldMap;

            colorSwitch = new Dictionary<Brush, DelegateCalcPointer>
            {
                { Brushes.Red, () => worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer++ },
                { Brushes.Green, () => worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 2 },
                { Brushes.Blue, () => worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 3 },
                { Brushes.WhiteSmoke, () => worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 4 },
                { Brushes.Gray, () => worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 5 }
            };

            StartAction();
        }

        private void StartAction()
        {
            Square[] tmpArraySquares = new Square[constants.CountSquare + 100];
            int count = 0;

            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 0; x < constants.WorldSizeX - 1; x++)
                {
                    if (worldMap[y][x].Fill == Brushes.Blue)
                    {
                        tmpArraySquares[count] = worldMap[y][x];
                        
                        count++;
                    }
                }
            }


            for (int i = 0; i < tmpArraySquares.Length; i++)
            {
                if (tmpArraySquares[i] == null)
                    return;

                currentPoint.X = tmpArraySquares[i].PointX;
                currentPoint.Y = tmpArraySquares[i].PointY;

                ActionSquare();

                if (minCountLive)
                    return;
            }
        }

        private void ActionSquare()
        {
            for (int i = 0; i < 10; i++)
            {
                if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] < 8)
                {
                    Move();
                    break;
                }

                if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] > 7
                    && worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] < 16)
                    Turn();

                if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] > 15
                    && worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] < 24)
                {
                    Convert();
                    break;
                }

                if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] > 23
                    && worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] < 32)
                    Check(worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] - 24);

                if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] > 31)
                {
                    worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer];
                    if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer >= constants.SizeBrain)
                        worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer -= constants.SizeBrain;
                }
            }

            worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health--;

            if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health < 0)
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health = 0;
            if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health > 99)
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health = 99;

            if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health <= 0 && worldMap[(int)currentPoint.Y][(int)currentPoint.X].Fill == Brushes.Blue)
            {
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health = 0;
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Fill = Brushes.WhiteSmoke;
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain = null;

                CountOfLive.CountLiveBio--;

                if (CountOfLive.CountLiveBio == (constants.CountSquare / 8))
                    minCountLive = true;
            }
        }

        private Point Check(int direction)
        {
            Point newPoint = new Point();
            Direction oldDirection = new Direction();
            oldDirection = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction;
            int x, y;

            x = (int)currentPoint.X;
            y = (int)currentPoint.Y;

            worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction + direction;

            if ((int)worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction > 7)
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction - 8;

            //if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction == Direction.UP)
            //    y--;
            //if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction == Direction.UPRIGHT)
            //{
            //    y--;
            //    x++;
            //}
            //if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction == Direction.RIGHT)
            //    x++;
            //if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction == Direction.RIGHTDOWN)
            //{
            //    y++;
            //    x++;
            //}
            //if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction == Direction.DOWN)
            //    y++;
            //if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction == Direction.LEFTDOWN)
            //{
            //    y++;
            //    x--;
            //}
            //if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction == Direction.LEFT)
            //    x--;
            //if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction == Direction.UPLEFT)
            //{
            //    y--;
            //    x--;
            //}

            switch (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction)
            {
                case Direction.UP:
                    y--;
                    break;
                case Direction.UPRIGHT:
                    y--;
                    x++;
                    break;
                case Direction.RIGHT:
                    x++;
                    break;
                case Direction.RIGHTDOWN:
                    y++;
                    x++;
                    break;
                case Direction.DOWN:
                    y++;
                    break;
                case Direction.LEFTDOWN:
                    y++;
                    x--;
                    break;
                case Direction.LEFT:
                    x--;
                    break;
                case Direction.UPLEFT:
                    y--;
                    x--;
                    break;
            }

            newPoint.X = x;
            newPoint.Y = y;

            colorSwitch[worldMap[(int)newPoint.Y][(int)newPoint.X].Fill]();

            //if (worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.Red)
            //    worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer++;
            //if (worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.Green)
            //    worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 2;
            //if (worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.Blue)
            //    worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 3;
            //if (worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.WhiteSmoke)
            //    worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 4;
            //if (worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.Gray)
            //    worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 5;

            if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer >= constants.SizeBrain)
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer -= constants.SizeBrain;

            worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction = oldDirection;
            
            return newPoint;
        }

        private void Move()
        {
            Point newPoint = new Point();

            newPoint = Check(worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer]);

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.Green)
            {
                worldMap[(int)newPoint.Y][(int)newPoint.X].Brain = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain;
                worldMap[(int)newPoint.Y][(int)newPoint.X].Fill = Brushes.Blue;
                worldMap[(int)newPoint.Y][(int)newPoint.X].Pointer = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer;
                worldMap[(int)newPoint.Y][(int)newPoint.X].Health = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health + constants.EnergyFood;
                worldMap[(int)newPoint.Y][(int)newPoint.X].Direction = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction;

                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Fill = Brushes.WhiteSmoke;
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain = null;
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].EnergyFood = 0;
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health = 0;

                currentPoint.X = newPoint.X;
                currentPoint.Y = newPoint.Y;

                new GeneratorSquare().AddFoodSquare(worldMap, 1);
            }

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.WhiteSmoke)
            {
                worldMap[(int)newPoint.Y][(int)newPoint.X].Brain = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain;
                worldMap[(int)newPoint.Y][(int)newPoint.X].Fill = Brushes.Blue;
                worldMap[(int)newPoint.Y][(int)newPoint.X].Pointer = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer;
                worldMap[(int)newPoint.Y][(int)newPoint.X].Health = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health;
                worldMap[(int)newPoint.Y][(int)newPoint.X].Direction = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction;

                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Fill = Brushes.WhiteSmoke;
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain = null;
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].EnergyFood = 0;
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health = 0;

                currentPoint.X = newPoint.X;
                currentPoint.Y = newPoint.Y;
            }

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.Red)
            {
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Fill = Brushes.Red;
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health = 0;
                CountOfLive.CountLiveBio--;

                if (CountOfLive.CountLiveBio == (constants.CountSquare / 8))
                    minCountLive = true;
            }
        }

        private void Turn()
        {
            int direction = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] - 8;

            worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction + direction;

            if ((int)worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction > 7)
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction - 8;

            worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer++;

            if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer >= constants.SizeBrain)
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer -= constants.SizeBrain;
        }

        private void Convert()
        {
            Point newPoint = new Point();
            newPoint = Check(worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] - 16);

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.WhiteSmoke || worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.Gray)
                return;

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.Green)
            {
                worldMap[(int)newPoint.Y][(int)newPoint.X].Fill = Brushes.WhiteSmoke;
                worldMap[(int)newPoint.Y][(int)newPoint.X].EnergyFood = 0;

                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health += constants.EnergyFood;

                new GeneratorSquare().AddFoodSquare(worldMap, 1);
            }

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].Fill == Brushes.Red)
            {
                worldMap[(int)newPoint.Y][(int)newPoint.X].Fill = Brushes.Green;
                new GeneratorSquare().AddAcidSquare(worldMap, 1);
            }
        }
    }
}
