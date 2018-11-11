using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

using Chanyi.Shepherd.QueryModel.Model.Formula;


namespace Chanyi.Shepherd.WPF.Model.Input
{
    class FormulaDetailsData : FormulaFeed, INotifyPropertyChanged
    {
        private SolidColorBrush bgColor;
        public SolidColorBrush BGColor
        {
            get { return bgColor; }
            set
            {
                bgColor = value;
                this.RaisePropertyChanged("BGColor");
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
