using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Chanyi.Shepherd.QueryModel.BindingModel;

namespace Chanyi.Shepherd.WPF.Model.Finance
{
    class SellOtherData : SellOtherBind, INotifyPropertyChanged
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

    class SellOtherChargeData : SellOtherBind, INotifyPropertyChanged
    {
        private decimal price;
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Price"));
                }
            }
        }

        private string purchaserId;
        public string PurchaserId
        {
            get { return purchaserId; }
            set
            {
                purchaserId = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("PurchaserId"));
                }
            }
        }

        private DateTime? sellDate;
        public DateTime? SellDate
        {
            get { return sellDate; }
            set
            {
                sellDate = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("SellDate"));
                }
            }
        }
        private string principalId;
        public string PrincipalId
        {
            get { return principalId; }
            set
            {
                principalId = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("PrincipalId"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
