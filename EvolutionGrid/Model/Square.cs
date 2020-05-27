using GalaSoft.MvvmLight;
using System;

namespace EvolutionGrid.Model
{
    public class Square : ViewModelBase, ICloneable
    {
        private int pointx;
        private int pointy;
        private int width;
        private int height;
        private ColorSquare fill;
        private int energyFood;
        private int health;
        private Direction direction;
        private int[] brain;
        private int pointer;
        private NameSquare nameSquare;
        private bool isSelected;


        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
        public int PointX
        {
            get { return pointx; }
            set
            {
                pointx = value;
                RaisePropertyChanged("PointX");
            }
        }
        public int PointY
        {
            get { return pointy; }
            set
            {
                pointy = value;
                RaisePropertyChanged("PointY");
            }
        }
        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                RaisePropertyChanged("Width");
            }
        }
        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                RaisePropertyChanged("Height");
            }
        }
        public ColorSquare Fill
        {
            get { return fill; }
            set
            {
                fill = value;
                RaisePropertyChanged("Fill");
            }
        }
        public int EnergyFood
        {
            get { return energyFood; }
            set
            {
                energyFood = value;
                RaisePropertyChanged("EnergyFood");
            }
        }
        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                RaisePropertyChanged("Health");
            }
        }
        public Direction Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                RaisePropertyChanged("Direction");
            }
        }
        public int[] Brain
        {
            get { return brain; }
            set
            {
                brain = value;
                RaisePropertyChanged("Brain");
            }
        }
        public int Pointer
        {
            get { return pointer; }
            set
            {
                pointer = value;
                RaisePropertyChanged("Pointer");
            }
        }
        public NameSquare NameSquare
        {
            get { return nameSquare; }
            set
            {
                nameSquare = value;
                RaisePropertyChanged("NameSquare");
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
