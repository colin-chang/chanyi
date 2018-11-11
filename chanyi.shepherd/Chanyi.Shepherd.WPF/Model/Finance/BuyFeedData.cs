using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Chanyi.Shepherd.QueryModel.BindingModel;
namespace Chanyi.Shepherd.WPF.Model
{
    class BuyFeedData : BuyFeedBind, INotifyPropertyChanged
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

    class BuyFeedChargeData : BuyFeedBind, INotifyPropertyChanged
    {
        private decimal price;

        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                if (this.PropertyChanged!=null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Price"));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
