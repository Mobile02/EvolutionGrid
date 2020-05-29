using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EvolutionGrid.Model
{
    public class BehaviorSquare
    {
        private delegate void DelegateCalcPointer();

        private Square[][] worldMap;
        private Constants constants = new Constants();
        private Point currentPoint = new Point();
        private bool minCountLive = false;
        private Dictionary<TypeSquare, DelegateCalcPointer> pointerOffset;

        public BehaviorSquare(Square[][] worldMap)
        {
            this.worldMap = worldMap;

            pointerOffset = new Dictionary<TypeSquare, DelegateCalcPointer>
            {
                { TypeSquare.ACID, () => worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer++ },
                { TypeSquare.FOOD, () => worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 2 },
                { TypeSquare.BIO, () => worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 3 },
                { TypeSquare.EMPTY, () => worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 4 },
                { TypeSquare.WALL, () => worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += 5 }
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
                    if (worldMap[y][x].TypeSquare == TypeSquare.BIO)
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
                {
                    Turn();
                    continue;
                }
                    
                if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] > 15
                    && worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] < 24)
                {
                    Convert();
                    break;
                }

                if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] > 23
                    && worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] < 32)
                {
                    Check(worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] - 24);
                    continue;
                }
                    
                if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer] > 31)
                {
                    worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer += worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer];
                    if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer >= constants.SizeBrain)
                        worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer -= constants.SizeBrain;
                    continue;
                }
            }

            worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health--;

            if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health < 0)
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health = 0;
            if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health > 99)
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health = 99;

            if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health <= 0 && worldMap[(int)currentPoint.Y][(int)currentPoint.X].TypeSquare == TypeSquare.BIO)
            {
                DeleteBio();
                CountSquare.CountLiveBio--;

                if (CountSquare.CountLiveBio == (constants.CountSquare / 8))
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

            pointerOffset[worldMap[(int)newPoint.Y][(int)newPoint.X].TypeSquare]();

            if (worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer >= constants.SizeBrain)
                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer -= constants.SizeBrain;

            worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction = oldDirection;
            
            return newPoint;
        }

        private void Move()
        {
            Point newPoint = new Point();

            newPoint = Check(worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain[worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer]);

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].TypeSquare == TypeSquare.FOOD)
            {
                StepBio((int)newPoint.Y, (int)newPoint.X);

                CountSquare.CountFood--;

                new GeneratorSquare().AddFoodSquare(worldMap, 1);
            }

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].TypeSquare == TypeSquare.EMPTY)
            {
                StepBio((int)newPoint.Y, (int)newPoint.X);
            }

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].TypeSquare == TypeSquare.ACID)
            {
                DeleteBio();

                worldMap[(int)currentPoint.Y][(int)currentPoint.X].TypeSquare = TypeSquare.ACID;

                CountSquare.CountLiveBio--;
                CountSquare.CountAcid++;

                if (CountSquare.CountLiveBio == (constants.CountSquare / 8))
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

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].TypeSquare == TypeSquare.EMPTY || worldMap[(int)newPoint.Y][(int)newPoint.X].TypeSquare == TypeSquare.WALL)
                return;

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].TypeSquare == TypeSquare.FOOD)
            {
                worldMap[(int)newPoint.Y][(int)newPoint.X].TypeSquare = TypeSquare.EMPTY;
                worldMap[(int)newPoint.Y][(int)newPoint.X].EnergyFood = 0;

                worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health += constants.EnergyFood;

                CountSquare.CountFood--;

                new GeneratorSquare().AddFoodSquare(worldMap, 1);
            }

            if (worldMap[(int)newPoint.Y][(int)newPoint.X].TypeSquare == TypeSquare.ACID)
            {
                worldMap[(int)newPoint.Y][(int)newPoint.X].TypeSquare = TypeSquare.FOOD;

                CountSquare.CountAcid--;
                CountSquare.CountFood++;

                new GeneratorSquare().AddAcidSquare(worldMap, 1);
            }
        }

        private void DeleteBio()
        {
            worldMap[(int)currentPoint.Y][(int)currentPoint.X].ID = 0;
            worldMap[(int)currentPoint.Y][(int)currentPoint.X].TypeSquare = TypeSquare.EMPTY;
            worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain = null;
            worldMap[(int)currentPoint.Y][(int)currentPoint.X].EnergyFood = 0;
            worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health = 0;
            //worldMap[(int)currentPoint.Y][(int)currentPoint.X].IsSelected = false;
        }

        private void StepBio(int pointY, int pointX)
        {
            worldMap[pointY][pointX].ID = worldMap[(int)currentPoint.Y][(int)currentPoint.X].ID;
            worldMap[pointY][pointX].Brain = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Brain;
            worldMap[pointY][pointX].TypeSquare = TypeSquare.BIO;
            worldMap[pointY][pointX].Pointer = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Pointer;
            worldMap[pointY][pointX].Health = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Health;
            worldMap[pointY][pointX].Direction = worldMap[(int)currentPoint.Y][(int)currentPoint.X].Direction;
            //worldMap[pointY][pointX].IsSelected = worldMap[(int)currentPoint.Y][(int)currentPoint.X].IsSelected;

            DeleteBio();

            currentPoint.X = pointX;
            currentPoint.Y = pointY;
        }
    }
}
