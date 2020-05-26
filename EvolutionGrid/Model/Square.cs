using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EvolutionGrid.Model
{
    public class Square : ViewModelBase, ICloneable
    {
        private int pointx;
        private int pointy;
        private int width;
        private int height;
        private Brush fill;
        private int energyFood;
        private int health;
        private Direction direction;
        private int[] brain;
        private int pointer;

        public bool IsSelected;

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
        public Brush Fill
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

        public object Clone()
        {
            return this.MemberwiseClone();
            //return new Square
            //{
            //    PointX = this.PointX,
            //    PointY = this.PointY,
            //    Width = this.Width,
            //    Height = this.Height,
            //    Fill = this.Fill,
            //    EnefgyFood = this.EnefgyFood,
            //    Health = this.Health,
            //    Direction = this.Direction,
            //    Brain = this.Brain,
            //    Pointer = this.Pointer
            //};
        }
    }
}
