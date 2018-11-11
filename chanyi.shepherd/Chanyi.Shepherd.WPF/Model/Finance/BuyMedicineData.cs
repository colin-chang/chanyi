using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Chanyi.Shepherd.QueryModel.BindingModel;

namespace Chanyi.Shepherd.WPF.Model
{
    class BuyMedicineData : BuyMedicineBind, INotifyPropertyChanged
    {
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                if (this.PropertyChanged == null)
                    return;
                this.PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    class BuyMedicineChargeDate : BuyMedicineBind, INotifyPropertyChanged
    {
        private decimal money;

        public decimal Money
        {
            get { return money; }
            set
            {
                money = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Money"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
