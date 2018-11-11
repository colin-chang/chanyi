using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.Model
{
    class AbortionData : Abortion
    {
        public int AbortionDays { get; set; }

        private DateTime abortionDate;

        public new DateTime AbortionDate
        {
            get { return abortionDate; }
            set { abortionDate = value; SetAbortionDays(); }
        }

        private DateTime matingDate;

        public new DateTime MatingDate
        {
            get { return matingDate; }
            set { matingDate = value; SetAbortionDays(); }
        }

        void SetAbortionDays()
        {
            if (this.AbortionDate != null && this.MatingDate != null)
            {
                this.AbortionDays = (int)(this.AbortionDate - this.MatingDate).TotalDays;
            }
        }
    }
}
