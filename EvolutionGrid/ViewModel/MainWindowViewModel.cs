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
        private EngineNaturalSelection engine;

        private ICommand cStart;
        private ICommand cStop;
        private ICommand selectItemCommand;

        Action DelegateGrafLife;

        private Square[][] worldMap;
        private Constants constants;
        private int generation;
        private int speed = 20;
        private int timeLife;
        private int maxTimeLife = 0;
        private PointCollection graf;
        private int[] pointY;
        private int offsetX = 0;
        private int scale = 700;
        private int widthGraf;
        private Square infoSelectSquare;


        public int WidthGraf
        {
            get { return widthGraf; }
            set
            {
                widthGraf = value;
                RaisePropertyChanged("WidthGraf");
            }
        }

        public Square InfoSelectSquare
        {
            get { return infoSelectSquare; }
            set
            {
                infoSelectSquare = value;
                RaisePropertyChanged("InfoSelectSquare");
            }
        }

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
                engine.Speed = value;
                RaisePropertyChanged("Speed");
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

            engine = new EngineNaturalSelection();
            WorldMap = engine.WorldMap;

            WidthGraf = (int)(constants.WorldSizeX * 15 + (constants.WorldSizeX * 1.5));
            pointY = engine.ArrayTimeLife;
            
            engine.ChangeGenerationProperty += Engine_ChangeGenerationProperty;
            engine.ChangeMaxTimeLifeProperty += Engine_ChangeMaxTimeLifeProperty;
            engine.ChangeOffsetXProperty += Engine_ChangeOffsetXProperty;
            engine.ChangeSquareProperty += Engine_ChangeSquareProperty;
            engine.ChangeTimeLifeProperty += Engine_ChangeTimeLifeProperty;
        }

        private void Engine_ChangeTimeLifeProperty(object sender, int e)
        {
            TimeLife = e;
        }

        private void Engine_ChangeSquareProperty(object sender, Square e)
        {
            InfoSelectSquare = e;
        }

        private void Engine_ChangeOffsetXProperty(object sender, int e)
        {
            offsetX = e;
        }

        private void Engine_ChangeMaxTimeLifeProperty(object sender, int e)
        {
            MaxTimeLife = e;
        }

        private void Engine_ChangeGenerationProperty(object sender, int e)
        {
            Generation = e;
            App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, DelegateGrafLife);
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

        public ICommand SelectItemCommand
        {
            get
            {
                if (selectItemCommand == null)
                    return selectItemCommand = new RelayCommand<Square>(SelectItemCommand_Execute);

                return selectItemCommand;
            }
        }

        #endregion

        private void Start()
        {
            engine.Start();
        }

        private void Stop()
        {
            engine.Stop();
        }

        private void SelectItemCommand_Execute(Square parameter)
        {
            engine.SelectItemCommand_Execute(parameter);
        }
    }
}
