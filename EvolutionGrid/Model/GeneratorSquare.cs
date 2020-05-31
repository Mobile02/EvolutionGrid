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
        private Random random;
        private Constants constants = new Constants();

        public GeneratorSquare()
        {
            DateTimeOffset timeOffset = new DateTimeOffset(DateTime.Now);
            random = new Random((int)timeOffset.Ticks);
        }

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

            if (random.Next(8) == 2 || random.Next(8) == 7 || random.Next(8) == 4)
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

                if (worldMap[y][x].TypeSquare == TypeSquare.EMPTY)
                {
                    worldMap[y][x].ID = CountSquare.ID;
                    worldMap[y][x].TypeSquare = TypeSquare.BIO;
                    worldMap[y][x].Health = constants.HealthSquare;
                    worldMap[y][x].Direction = (Direction)random.Next(8);
                    worldMap[y][x].Pointer = 0;
                    worldMap[y][x].PointX = x;
                    worldMap[y][x].PointY = y;
                    worldMap[y][x].Brain = GeneratorBrain();

                    CountSquare.CountLiveBio++;
                    CountSquare.ID++;
                    count--;
                }
            }
        }

        public void AddAcidSquare(Square[][] worldMap, int count)
        {
            if (CountSquare.CountAcid > constants.CountAcid + 10)
                return;

            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (worldMap[y][x].TypeSquare == TypeSquare.EMPTY)
                {
                    worldMap[y][x].TypeSquare = TypeSquare.ACID;
                    CountSquare.CountAcid++;
                    count--;
                }
            }
        }

        public void AddFoodSquare(Square[][] worldMap, int count)
        {
            if (CountSquare.CountFood > constants.CountFood)
                return;

            while (count > 0)
            {
                int y = random.Next(1, constants.WorldSizeY - 1);
                int x = random.Next(1, constants.WorldSizeX - 1);

                if (worldMap[y][x].TypeSquare == TypeSquare.EMPTY)
                {
                    worldMap[y][x].TypeSquare = TypeSquare.FOOD;
                    CountSquare.CountFood++;
                    count--;
                }
            }
        }

        public void AddChild(Square[][] worldMap)
        {
            new FileOperation().SaveBrain(worldMap);

            var oo = CountSquare.CountLiveBio;

            Square[] arraySurvivorSquares = new Square[constants.CountSquare / 8];

            int count = 0;
            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 1; x < constants.WorldSizeX - 1; x++)
                {
                    if (worldMap[y][x].TypeSquare == TypeSquare.BIO)
                    {
                        arraySurvivorSquares[count] = (Square)worldMap[y][x].Clone();

                        count++;
                    }
                }
            }

            for (int i = 0; i < arraySurvivorSquares.Length; i++)
            {
                int tmpCount = 0;

                while (tmpCount < 7)
                {
                    int x = random.Next(1, constants.WorldSizeX - 1);
                    int y = random.Next(1, constants.WorldSizeY - 1);

                    if (worldMap[y][x].TypeSquare == TypeSquare.EMPTY)
                    {
                        worldMap[y][x].ID = CountSquare.ID;
                        worldMap[y][x].TypeSquare = TypeSquare.BIO;
                        worldMap[y][x].Health = constants.HealthSquare;
                        worldMap[y][x].Direction = (Direction)random.Next(8);
                        worldMap[y][x].Pointer = 0;
                        worldMap[y][x].PointX = x;
                        worldMap[y][x].PointY = y;
                        worldMap[y][x].Brain = GeneratorBrainMutant(arraySurvivorSquares[i]);

                        CountSquare.ID++;
                        CountSquare.CountLiveBio++;
                        tmpCount++;
                    }
                }
            }
        }

        public void Reproduction(Square[][] worldMap, Square parent)
        {
            Square[] parents = new Square[1]
            {
                parent
            };

            for (int x = parent.PointX - 1; x < parent.PointX + 1; x++)
            {
                for (int y = parent.PointY - 1; y < parent.PointY + 1; y++)
                {
                    if (worldMap[y][x].TypeSquare == TypeSquare.EMPTY)
                    {
                        CountSquare.ID++;

                        worldMap[y][x].ID = CountSquare.ID;
                        worldMap[y][x].TypeSquare = TypeSquare.BIO;
                        worldMap[y][x].Health = constants.HealthSquare;
                        worldMap[y][x].Direction = (Direction)random.Next(8);
                        worldMap[y][x].Pointer = 0;
                        worldMap[y][x].PointX = x;
                        worldMap[y][x].PointY = y;
                        worldMap[y][x].Brain = GeneratorBrainMutant(parents[0]);

                        CountSquare.CountLiveBio++;
                        return;
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

                if (worldMap[y][x].TypeSquare == TypeSquare.EMPTY)
                {
                    worldMap[y][x].TypeSquare = TypeSquare.WALL;
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
                    if (worldMap[y][x].TypeSquare != TypeSquare.WALL && worldMap[y][x].TypeSquare != TypeSquare.BIO)
                    {
                        worldMap[y][x].TypeSquare = TypeSquare.EMPTY;
                        worldMap[y][x].Pointer = 0;
                        worldMap[y][x].Brain = null;
                    }

                    if (worldMap[y][x].TypeSquare == TypeSquare.BIO)
                        worldMap[y][x].Health = constants.HealthSquare;
                }
            }

            CountSquare.CountFood = 0;
            CountSquare.CountAcid = 0;
        }

        public void FillWorldMap(Square[][] worldMap)
        {
            for (int y = 0; y < constants.WorldSizeY; y++)
            {
                worldMap[y] = new Square[constants.WorldSizeX];

                for (int x = 0; x < constants.WorldSizeX; x++)
                {
                    worldMap[y][x] = new Square
                    {
                        PointX = x,
                        PointY = y,
                        TypeSquare = TypeSquare.EMPTY
                    };

                    if (y == 0 || y == constants.WorldSizeY - 1)
                        worldMap[y][x] = new Square
                        {
                            PointX = x,
                            PointY = y,
                            TypeSquare = TypeSquare.WALL
                        };
                    if (x == 0 || x == constants.WorldSizeX - 1)
                        worldMap[y][x] = new Square
                        {
                            PointX = x,
                            PointY = y,
                            TypeSquare = TypeSquare.WALL
                        };
                }
            }
        }
    }
}
