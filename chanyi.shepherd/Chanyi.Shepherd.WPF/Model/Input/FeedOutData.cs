using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.Model.Input
{
    public class FeedOutData : FeedData
    {
        private float outAmount;

        public float OutAmount
        {
            get { return outAmount; }
            set
            {
                outAmount = value;
                this.RaisePropertyChanged("OutAmount");
            }
        }
    }
}
