using EvolutionGrid.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionGrid.ViewModel
{
    public class SquareViewModel : ViewModelBase
    {
        private readonly Square model;
        private bool isSelected;

        public SquareViewModel(Square model)
        {
            this.model = model;
            this.model.ChangeTypeSquare += (sender, square) => RaisePropertyChanged(nameof(Type));
            this.model.ChangeHealth += (sender, square) => RaisePropertyChanged(nameof(Health));
            this.model.ChangeID += (sender, square) => RaisePropertyChanged(nameof(ID));
        }

        public TypeSquare Type => model.TypeSquare;

        public double ID => model.ID;

        public bool IsSelected 
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        public string Health => model.Health == 0 ? "" : model.Health.ToString();
    }
}
