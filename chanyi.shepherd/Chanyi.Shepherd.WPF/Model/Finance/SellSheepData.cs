using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.Model
{
    class SellSheepData : INotifyPropertyChanged
    {
        public string SheepId { get; set; }
        
        public string SerialNumber { get; set; }

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

        private float weight;
        public float Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                this.RaisePropertyChanged("Weight");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
