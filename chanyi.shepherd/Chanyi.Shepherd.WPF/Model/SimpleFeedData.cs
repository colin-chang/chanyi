using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Chanyi.Shepherd.QueryModel.Model.Input;

namespace Chanyi.Shepherd.WPF.Model
{
    public class SimpleFeedData : SimpleFeed, INotifyPropertyChanged
    {
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class FormulaFeedData : SimpleFeed, INotifyPropertyChanged
    {
        private float amount;

        public float Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Amount"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
