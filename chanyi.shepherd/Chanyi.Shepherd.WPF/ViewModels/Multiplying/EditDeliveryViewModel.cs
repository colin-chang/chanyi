using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Windows.Controls.Primitives;


namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class EditDeliveryViewModel : EditViewModel
    {
        public EditDeliveryViewModel(string editItemId)
        {
            this.Id = editItemId;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            return this.Service.GetDeliveryById(this.Id);
        }

        protected override void InitializeBindItem()
        {
            this.Principals = this.Service.GetAllEmployeeBind(); ;
        }

        public string Id { get; set; }


        private string femaleNumber;
        public string FemaleNumber
        {
            get { return femaleNumber; }
            set
            {
                femaleNumber = value;
                this.RaisePropertyChanged("FemaleNumber");
            }
        }

        private DeliveryWayEnum deliveryWay;
        public DeliveryWayEnum DeliveryWay
        {
            get { return deliveryWay; }
            set
            {
                deliveryWay = value;
                this.RaisePropertyChanged("DeliveryWay");
            }
        }


        private List<object> numbers = new List<object>();
        public List<object> Numbers
        {
            get
            {
                if (numbers.Count() <= 0)
                {
                    for (int i = 0; i < 10; i++)
                        numbers.Add(new { Id = i, Name = i });
                }
                return numbers;
            }
        }

        public List<object> LiveMaleNumbers
        {
            get
            {
                List<object> list = new List<object>() { new { Name = "公" } };
                list.AddRange(Numbers);
                return list;
            }
        }

        public List<object> LiveFemaleNumbers
        {
            get
            {
                List<object> list = new List<object>() { new { Name = "母" } };
                list.AddRange(Numbers);
                return list;
            }
        }

        public List<object> LiveTotalNumbers
        {
            get
            {
                List<object> list = new List<object>() { new { Name = "总" } };
                for (int i = 0; i < 18; i++)
                    list.Add(new { Id = i, Name = i });
                return list;
            }
        }


        private int? liveMaleCount;

        public int? LiveMaleCount
        {
            get { return liveMaleCount; }
            set
            {
                liveMaleCount = value;
                ChangeLiveTotalCount();
                this.RaisePropertyChanged("LiveMaleCount");
            }
        }

        private int? liveFemaleCount;

        public int? LiveFemaleCount
        {
            get { return liveFemaleCount; }
            set
            {
                liveFemaleCount = value;
                ChangeLiveTotalCount();
                this.RaisePropertyChanged("LiveFemaleCount");
            }
        }

        void ChangeLiveTotalCount()
        {
            int fc = this.LiveMaleCount ?? 0;
            int mc = this.LiveFemaleCount ?? 0;
            this.LiveTotalCount = fc + mc;
        }

        /// <summary>
        /// 产活羔总数
        /// </summary>
        private int? liveTotalCount;

        public int? LiveTotalCount
        {
            get { return liveTotalCount; }
            set
            {
                liveTotalCount = value;
                if (this.TotalCount < this.LiveTotalCount)
                    this.errors["Error"] = "产活羊数大于总产羊数";
                else this.errors.Remove("Error");
                this.RaisePropertyChanged("Error");
                this.RaisePropertyChanged("LiveTotalCount");
            }
        }
        /// <summary>
        /// 总产羔数
        /// </summary>
        private int totalCount;

        public int TotalCount
        {
            get { return totalCount; }
            set
            {
                totalCount = value;
                if (this.totalCount < this.LiveTotalCount)
                    this.errors["Error"] = "产活羊数大于总产羊数";
                else this.errors.Remove("Error");
                this.RaisePropertyChanged("Error");
                this.RaisePropertyChanged("TotalCount");
            }
        }

        private MidwiferyReasonEnum deliverReason;
        public MidwiferyReasonEnum DeliverReason
        {
            get { return deliverReason; }
            set
            {
                deliverReason = value;
                this.RaisePropertyChanged("DeliverReason");
            }
        }

        private string deliverReasonOtherDetail;
        public string DeliverReasonOtherDetail
        {
            get { return deliverReasonOtherDetail; }
            set
            {
                deliverReasonOtherDetail = value;
                this.Validate("DeliverReasonOtherDetail");
            }
        }

        private DateTime deliveryDate;

        [BeforeToday(ErrorMessage = "分娩日期需小于当前日期")]
        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                deliveryDate = value;
                this.Validate("DeliveryDate");
            }
        }

        private List<EmployeeBind> principals;
        public List<EmployeeBind> Principals
        {
            get { return principals; }
            set
            {
                principals = value;
                this.RaisePropertyChanged("Principals");
            }
        }

        private string principalId;

        public string PrincipalId
        {
            get { return principalId; }
            set
            {
                principalId = value;
                this.Validate("PrincipalId");
            }
        }

        private string remark;
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                this.RaisePropertyChanged("Remark");
            }
        }

        /// <summary>
        /// 关闭pop窗口
        /// </summary>
        public DelegateCommand<Popup> ClosePopCommand
        {
            get
            {
                return new DelegateCommand<Popup>(p =>
                {
                    p.IsOpen = false;
                });
            }
        }

        public DelegateCommand<Popup> OpenPopCommand
        {
            get
            {
                return new DelegateCommand<Popup>(p =>
                {
                    p.IsOpen = true;
                });
            }
        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.UpdateDelivery(this.DeliveryWay, this.DeliverReason, this.DeliverReasonOtherDetail, this.LiveMaleCount, this.LiveFemaleCount, this.TotalCount, this.DeliveryDate, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
