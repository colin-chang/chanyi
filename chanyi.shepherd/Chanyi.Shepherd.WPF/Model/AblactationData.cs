using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.Model
{
    class AblactationData : Ablactation
    {
        public int? AblactationDays { get; set; }

        private DateTime? birthday;
        public new DateTime? Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                SetAblactationDays();
            }
        }

        private DateTime ablactationDate;
        public new DateTime AblactationDate
        {
            get { return ablactationDate; }
            set
            {
                ablactationDate = value;
                SetAblactationDays();
            }
        }

        void SetAblactationDays()
        {
            if (this.Birthday != null && this.AblactationDate != null)
            {
                this.AblactationDays = (int?)(this.AblactationDate - (DateTime)this.Birthday).TotalDays;
            }
        }
    }
}
