using EvolutionGrid.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EvolutionGrid.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ManualResetEventSlim eventSlim;

        private ICommand cSpeedPlus;
        private ICommand cSpeedMinus;
        private ICommand cStart;
        private ICommand cStop;

        Action DelegateGrafLife;

        private Square[][] worldMap;
        private Constants constants;
        private int generation;
        private int speed = 980;
        private bool enableStartButton = true;
        private int timeLife;
        private int maxTimeLife = 0;
        private PointCollection graf;
        private int pointX = 0;
        private int[] pointY;
        private int offsetX = 0;
        private int scale = 700;


        public PointCollection Graf
        {
            get { return graf; }
            set
            {
                graf = value;
                RaisePropertyChanged("Graf");
            }
        }

        public int TimeLife
        {
            get { return timeLife; }
            set
            {
                timeLife = value;
                RaisePropertyChanged("TimeLife");
            }
        }

        public int MaxTimeLife
        {
            get { return maxTimeLife; }
            set
            {
                maxTimeLife = value;
                RaisePropertyChanged("MaxTimeLife");
            }
        }

        public int Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                RaisePropertyChanged("Speed");
            }
        }

        public bool EnableStartButton
        {
            get { return enableStartButton; }
            set
            {
                enableStartButton = value;
                RaisePropertyChanged("EnableStartButton");
            }
        }

        public int Generation
        {
            get { return generation; }
            set
            {
                generation = value;
                RaisePropertyChanged("Generation");
            }
        }

        public Square[][] WorldMap
        {
            get { return worldMap; }
            set
            {
                worldMap = value;
                RaisePropertyChanged("WorldMap");
            }
        }

        public MainWindowViewModel()
        {
            DelegateGrafLife = GrafLife;
            constants = new Constants();
            pointY = new int[constants.CountCicle];
            WorldMap = new Square[constants.WorldSizeY][];

            for (int y = 0; y < constants.WorldSizeY; y++)
            {
                WorldMap[y] = new Square[constants.WorldSizeX];

                for (int x = 0; x < constants.WorldSizeX; x++)
                {
                    WorldMap[y][x] = new Square
                    {
                        PointX = x,
                        PointY = y,
                        Fill = Brushes.WhiteSmoke,
                    };

                    if (y == 0 || y == constants.WorldSizeY - 1)
                        WorldMap[y][x] = new Square
                        {
                            PointX = x,
                            PointY = y,
                            Fill = Brushes.Gray,
                        };
                    if (x == 0 || x == constants.WorldSizeX - 1)
                        WorldMap[y][x] = new Square
                        {
                            PointX = x,
                            PointY = y,
                            Fill = Brushes.Gray,
                        };
                }
            }

            CountOfLive.CountLiveBio = constants.CountSquare;

            eventSlim = new ManualResetEventSlim(false);

            new GeneratorSquare().AddWall(WorldMap, 60);
            new GeneratorSquare().AddAcidSquare(WorldMap, constants.CountAcid);
            new GeneratorSquare().AddFoodSquare(WorldMap, constants.CountFood);
            new GeneratorSquare().AddBioSquare(WorldMap, constants.CountSquare);

            //new FileOperation().SaveBrain(WorldMap);

            MainAsync();
        }

        private async void MainAsync()
        {
            
            await Task.Run(() => Main());

            MessageBox.Show(Generation.ToString(), "End");
        }

        private void Main()
        {
            for (int gen = 0; gen < constants.CountCicle; gen++)
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    eventSlim.Wait();

                    new BehaviorSquare(WorldMap);
                    
                    Thread.Sleep(1000 - Speed);

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
                            offsetX++;

                        pointY[pointX] = i;
                        App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, DelegateGrafLife);

                        Generation = gen;
                        i = int.MaxValue - 1;
                        pointX++;
                    }
                }
            }
        }

        private void GrafLife()
        {
            Graf = new PointCollection();

            for (int i = 0; i < pointY.Length; i++)
            {
                if (pointY[i + offsetX] == 0)
                    break;
                Graf.Add(new Point(i, pointY[i + offsetX]));
            }
        }

        #region Commands
        public ICommand ComSpeedPlus
        {
            get
            {
                if (cSpeedPlus == null)
                {
                    return cSpeedPlus = new RelayCommand(execute: SpeedPlus);
                }
                return cSpeedPlus;
            }
        }

        public ICommand ComSpeedMinus
        {
            get
            {
                if (cSpeedMinus == null)
                {
                    return cSpeedMinus = new RelayCommand(execute: SpeedMinus);
                }
                return cSpeedMinus;
            }
        }

        public ICommand ComStart
        {
            get
            {
                if (cStart == null)
                {
                    return cStart = new RelayCommand(execute: Start);
                }
                return cStart;
            }
        }

        public ICommand ComStop
        {
            get
            {
                if (cStop == null)
                {
                    return cStop = new RelayCommand(execute: Stop);
                }
                return cStop;
            }
        }

        #endregion

        private void SpeedPlus()
        {
            Speed += 10;

            if (Speed >= 1000)
            {
                Speed = 1000;
                EnableStartButton = false;
            }
        }

        private void SpeedMinus()
        {
            Speed -= 10;

            if (Speed < 1000)
                EnableStartButton = true;
        }

        private void Start()
        {
            eventSlim.Set();
        }

        private void Stop()
        {
            eventSlim.Reset();
            new FileOperation().SaveTimeLife(pointY);
        }
    }
}
