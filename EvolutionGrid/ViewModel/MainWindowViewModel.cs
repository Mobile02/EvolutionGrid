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
using System.Windows.Input;

namespace EvolutionGrid.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private EngineNaturalSelection engine;

        private ICommand cStart;
        private ICommand cStop;
        private ICommand selectItemCommand;

        private SquareViewModel[][] worldMap;
        private Constants constants;
        private int generation;
        private int speed;
        private int timeLife;
        private int maxTimeLife = 0;
        private ObservableCollection<int[]> chartTimeLife;
        private int[] pointY;
        private int widthGraf;
        private SquareViewModel selectedSquare;
        private double iDSelected;



        public double IDSelected
        {
            get { return iDSelected; }
            set
            {
                iDSelected = value;
                RaisePropertyChanged("IDSelected");
            }
        }

        public int WidthChart
        {
            get { return widthGraf; }
            set
            {
                widthGraf = value;
                RaisePropertyChanged("WidthGraf");
            }
        }

        public SquareViewModel SelectedSquare
        {
            get { return selectedSquare; }
            set
            {
                selectedSquare = value;
                RaisePropertyChanged("SelectedSquare");
            }
        }

        public ObservableCollection<int[]> ChartTimeLife
        {
            get { return chartTimeLife; }
            set
            {
                chartTimeLife = value;
                RaisePropertyChanged("ChartTimeLife");
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
            constants = new Constants();
            pointY = new int[constants.CountCicle];
            WorldMap = new SquareViewModel[constants.WorldSizeY][];

            engine = new EngineNaturalSelection();
            WorldMap = engine.WorldMap.Select(squares => squares.Select(square => new SquareViewModel(square)).ToArray()).ToArray(); //TODO: Разобраться, какая то магия, что делает понятно, а как нет.

            WidthChart = (int)(constants.WorldSizeX * 15 + (constants.WorldSizeX * 1.5));
            pointY = engine.ArrayTimeLife;

            engine.ChangeGenerationProperty += (sender, e) => { Generation = e; UpdateChartLife(); };
            engine.ChangeMaxTimeLifeProperty += (sender, e) => MaxTimeLife = e;
            engine.ChangeTimeLifeProperty += (sender, e) => { TimeLife = e; if (iDSelected != 0) RefreshSelectedSquare(); };

            ChartTimeLife = new ObservableCollection<int[]>();
        }

        private void UpdateChartLife()
        {
            ChartTimeLife.Clear();
            ChartTimeLife = new ChartLife().UpdateChart(pointY, Generation);
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
            if (SelectedSquare != null)
            {
                SelectedSquare.IsSelected = false;
                IDSelected = 0;
            }

            if (parameter.Type == TypeSquare.BIO)
            {
                parameter.IsSelected = true;
                SelectedSquare = parameter;
                IDSelected = parameter.ID;
            }
            else
            {
                SelectedSquare = null;
                parameter = null;
                IDSelected = 0;
            }
        }

        private void RefreshSelectedSquare()
        {
            bool isDie = true;

            for (int y = 1; y < constants.WorldSizeY - 1; y++)
            {
                for (int x = 1; x < constants.WorldSizeX - 1; x++)
                {
                    WorldMap[y][x].IsSelected = false;

                    if (WorldMap[y][x].ID == IDSelected && WorldMap[y][x].ID != 0)
                    {
                        WorldMap[y][x].IsSelected = true;
                        SelectedSquare = WorldMap[y][x];
                        isDie = false;
                    }
                }
            }

            if (isDie) IDSelected = 0;
        }
    }
}
