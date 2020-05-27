using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EvolutionGrid.Model
{
    public class EngineNaturalSelection
    {
        private ManualResetEventSlim eventSlim;
        private Constants constants;
        private int pointX = 0;
        private int offsetX = 0;
        private int scale = 700;
        private int timeLife;
        private int generation;
        private int maxTimeLife;
        private Square infoSelectSquare;

        
        private void RaiseOffsetXProperty(int value) => ChangeOffsetXProperty?.Invoke(this, value);
        private void RaiseTimeLifeProperty(int value) => ChangeTimeLifeProperty?.Invoke(this, value);
        private void RaiseGenerationProperty(int value) => ChangeGenerationProperty?.Invoke(this, value);
        private void RaiseMaxTimeLifeProperty(int value) => ChangeMaxTimeLifeProperty?.Invoke(this, value);

        private void RaiseMyProperty(Square value) => ChangeSquareProperty?.Invoke(this, value);


        public event EventHandler<int> ChangeOffsetXProperty;
        public event EventHandler<int> ChangeTimeLifeProperty;
        public event EventHandler<int> ChangeGenerationProperty;
        public event EventHandler<int> ChangeMaxTimeLifeProperty;
        public event EventHandler<Square> ChangeSquareProperty;

        public int[] ArrayTimeLife;
        public int OffsetX
        {
            get { return offsetX; }
            set
            {
                offsetX = value;
                RaiseOffsetXProperty(OffsetX);
            }
        }
        public int TimeLife
        {
            get { return timeLife; }
            set
            {
                timeLife = value;
                RaiseTimeLifeProperty(TimeLife);
            }
        }
        public int Generation
        {
            get { return generation; }
            set
            {
                generation = value;
                RaiseGenerationProperty(Generation);
            }
        }
        public int MaxTimeLife
        {
            get { return maxTimeLife; }
            set
            {
                maxTimeLife = value;
                RaiseMaxTimeLifeProperty(MaxTimeLife);
            }
        }
        public Square InfoSelectSquare
        {
            get { return infoSelectSquare; }
            set
            {
                infoSelectSquare = value;
                RaiseMyProperty(InfoSelectSquare);
            }
        }
        public Square[][] WorldMap { get; set; }
        public int Speed { get; set; }

        public EngineNaturalSelection()
        {
            constants = new Constants();
            WorldMap = new Square[constants.WorldSizeY][];
            CountOfLive.CountLiveBio = constants.CountSquare;
            eventSlim = new ManualResetEventSlim(false);

            new GeneratorSquare().FillWorldMap(WorldMap);
            new GeneratorSquare().AddWall(WorldMap, 60);
            new GeneratorSquare().AddAcidSquare(WorldMap, constants.CountAcid);
            new GeneratorSquare().AddFoodSquare(WorldMap, constants.CountFood);
            new GeneratorSquare().AddBioSquare(WorldMap, constants.CountSquare);

            ArrayTimeLife = new int[constants.CountCicle];

            Speed = 20;

            MainAsync();
        }

        private async void MainAsync()
        {
            await Task.Run(() => Main());
        }

        private void Main()
        {
            for (int gen = 0; gen < constants.CountCicle; gen++)
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    eventSlim.Wait();

                    new BehaviorSquare(WorldMap);

                    RefreshInfoSelectSquare();

                    Thread.Sleep(Speed);

                    TimeLife = i;
                    if (MaxTimeLife < TimeLife)
                        MaxTimeLife = TimeLife;

                    if (CountOfLive.CountLiveBio <= (constants.CountSquare / 8))
                    {
                        new GeneratorSquare().RefreshSquare(WorldMap);
                        new GeneratorSquare().AddAcidSquare(WorldMap, constants.CountAcid);
                        new GeneratorSquare().AddFoodSquare(WorldMap, constants.CountFood);
                        new GeneratorSquare().AddChild(WorldMap);
                        CountOfLive.CountLiveBio = constants.CountSquare;

                        if (pointX > scale)
                            OffsetX++;

                        ArrayTimeLife[pointX] = i;

                        Generation = gen;
                        i = int.MaxValue - 1;
                        pointX++;
                    }
                }
            }
        }

        private void RefreshInfoSelectSquare()
        {
            InfoSelectSquare = null;

            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 1; x < constants.WorldSizeX - 1; x++)
                {
                    if (WorldMap[y][x].IsSelected)
                        InfoSelectSquare = WorldMap[y][x];
                }
            }
        }

        public void Start()
        {
            eventSlim.Set();
        }

        public void Stop()
        {
            eventSlim.Reset();
            new FileOperation().SaveTimeLife(ArrayTimeLife);
        }

        public void SelectItemCommand_Execute(Square parameter)
        {
            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 1; x < constants.WorldSizeX - 1; x++)
                {
                    if (WorldMap[y][x].IsSelected)
                        WorldMap[y][x].IsSelected = false;
                }
            }

            if (parameter.NameSquare == NameSquare.BIO)
            {
                parameter.IsSelected = true;
                InfoSelectSquare = parameter;
            }
            else
            {
                InfoSelectSquare = null;
                parameter = null;
            }
        }
    }
}
