using Chanyi.Shepherd.QueryModel.BindingModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.Model.Input
{
    class FormulaData : FormulaBind, INotifyPropertyChanged
    {
        private bool isCheck;

        public bool IsCheck
        {
            get { return isCheck; }
            set
            {
                isCheck = value;
                this.RaisePropertyChanged("IsCheck");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
