using EvolutionGrid.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
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

        private SquareViewModel[][] worldMap;
        private Constants constants;
        private int generation;
        private int speed;
        private int timeLife;
        private int maxTimeLife = 0;
        private PointCollection graf;
        private int[] pointY;
        private int offsetX = 0;
        private int widthGraf;
        private SquareViewModel infoSelectSquare;
        private double iDSelected;


        public int WidthGraf
        {
            get { return widthGraf; }
            set
            {
                widthGraf = value;
                RaisePropertyChanged("WidthGraf");
            }
        }

        public SquareViewModel InfoSelectSquare
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

        public SquareViewModel[][] WorldMap
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
            WorldMap = new SquareViewModel[constants.WorldSizeY][];

            engine = new EngineNaturalSelection();
            WorldMap = engine.WorldMap.Select(squares => squares.Select(square => new SquareViewModel(square)).ToArray()).ToArray(); //TODO: Разобраться, какая то магия, что делает понятно, а как нет.

            WidthGraf = (int)(constants.WorldSizeX * 15 + (constants.WorldSizeX * 1.5));
            pointY = engine.ArrayTimeLife;

            engine.ChangeGenerationProperty += (sender, e) =>
            {
                Generation = e;
                GrafLife();
            };
            engine.ChangeMaxTimeLifeProperty += (sender, e) => MaxTimeLife = e;
            engine.ChangeOffsetXProperty += (sender, e) => offsetX = e;
            engine.ChangeTimeLifeProperty += (sender, e) => { TimeLife = e; if (iDSelected != 0) RefreshSelectedSquare(); };
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

            Graf.Freeze();
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
                    return selectItemCommand = new RelayCommand<SquareViewModel>(SelectItemCommand_Execute);

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

        private void SelectItemCommand_Execute(SquareViewModel parameter)     //TODO: Вынести, если можно в модель. Оба метода
        {
            if (InfoSelectSquare != null)
            {
                InfoSelectSquare.IsSelected = false;
                iDSelected = 0;
            } 

            if (parameter.Type == TypeSquare.BIO)
            {
                parameter.IsSelected = true;
                InfoSelectSquare = parameter;
                iDSelected = parameter.ID;
            }
            else
            {
                InfoSelectSquare = null;
                parameter = null;
                iDSelected = 0;
            }
        }

        private void RefreshSelectedSquare()
        {
            bool die = true;

            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 1; x < constants.WorldSizeX - 1; x++)
                {
                    WorldMap[y][x].IsSelected = false;

                    if (WorldMap[y][x].ID == iDSelected && WorldMap[y][x].ID != 0)
                    {
                        WorldMap[y][x].IsSelected = true;
                        InfoSelectSquare = WorldMap[y][x];
                        die = false;
                    }     
                }
            }

            if (die) iDSelected = 0;
        }
    }
}
