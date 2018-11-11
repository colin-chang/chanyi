using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Collections.ObjectModel;


using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class AddWaterRateViewModel : AddViewModel
    {
        public AddWaterRateViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddWaterRateViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            if (this.isContinue)
                return;
            var principals = this.Service.GetEmployeeBind();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = this.defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }
        private string price;
        [EntityProperty]
        [Required(ErrorMessage = "水费必填")]
        [FloatNumber(0, int.MaxValue, ErrorMessage = "水费输入不合法")]
        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                this.Validate("Price");


            }
        }

        private string amount;
        [EntityProperty]
        [FloatNumber(0, int.MaxValue, ErrorMessage = "用水量输入不合法")]
        [Required(ErrorMessage = "用水量必填")]
        public string Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                this.Validate("Amount");
            }
        }


        private DateTime? operationDate;
        [EntityProperty]
        [Required(ErrorMessage = "日期必填")]
        public DateTime? OperationDate
        {
            get { return operationDate; }
            set
            {
                operationDate = value;
                this.Validate("OperationDate");
            }
        }
        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "操作人必填")]
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
        [EntityProperty]
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                this.RaisePropertyChanged("Remark");
            }
        }
        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.AddWaterRate(float.Parse(this.Amount), decimal.Parse(this.Price), (DateTime)this.OperationDate, this.PrincipalId, this.UserId, this.Remark);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("水费记录添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
