using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chanyi.Shepherd.QueryModel.Model.Breeding;

namespace Chanyi.Shepherd.WPF.Model.AssessData
{
    class FirstAssessData : FirstAssess
    {
        public string Age { get; set; }
        

        private DateTime assessDate;
        public new DateTime AssessDate
        {
            get { return assessDate; }
            set
            {
                assessDate = value;
                SetAge();
            }
        }

        private DateTime? birthday;
        public new DateTime? Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                SetAge();
            }
        }
        void SetAge()
        {
            if (this.AssessDate != null && this.Birthday != null)
            {
                this.Age = ((int)(this.AssessDate - (DateTime)this.Birthday).TotalDays).ToString();
            }
            else
            {
                this.Age = "-";
            }
        }
    }
}
