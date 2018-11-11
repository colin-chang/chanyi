using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.Model
{
    class DeliveryData : Delivery
    {
        public int? GestationDays { get; set; }

        private DateTime deliveryDate;
        public new DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                deliveryDate = value;
                SetDeliveryDays();
            }
        }

        private DateTime matingDate;
        public new DateTime MatingDate
        {
            get { return matingDate; }
            set
            {
                matingDate = value;
                SetDeliveryDays();
            }
        }

        public int LiveTotalCount
        {
            get
            {
                int mc = this.LiveMaleCount ?? 0;
                int mf = this.LiveFemaleCount ?? 0;
                return mc + mf;
            }
        }

        void SetDeliveryDays()
        {
            if (this.DeliveryDate != null && this.MatingDate != null)
            {
                this.GestationDays = (int?)(this.DeliveryDate - this.MatingDate).TotalDays;
            }
        }
    }
}
