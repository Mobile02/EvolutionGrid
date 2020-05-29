using GalaSoft.MvvmLight;
using System;

namespace EvolutionGrid.Model
{
    public class Square : ICloneable
    {
        private int health;
        private TypeSquare typeSquare;
        private double iD;

        private void RaiseTypeSquare(TypeSquare value) => ChangeTypeSquare?.Invoke(this, value);
        private void RaiseHealth(int value) => ChangeHealth?.Invoke(this, value);
        private void RaiseID(double value) => ChangeID?.Invoke(this, value);

        public event EventHandler<TypeSquare> ChangeTypeSquare;
        public event EventHandler<bool> ChangeIsSelected;
        public event EventHandler<int> ChangeHealth;
        public event EventHandler<double> ChangeID;


        public double ID
        {
            get { return iD; }
            set
            {
                iD = value;
                RaiseID(ID);
            }
        }
        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                RaiseHealth(Health);
            }
        }
        public TypeSquare TypeSquare
        {
            get { return typeSquare; }
            set
            {
                typeSquare = value;
                RaiseTypeSquare(TypeSquare);
            }
        }
        public Direction Direction { get; set; }
        public int[] Brain { get; set; }
        public int Pointer { get; set; }
        public int PointX { get; set; }
        public int PointY { get; set; }
        public ColorSquare Fill { get; set; }
        public int EnergyFood { get; set; }
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
