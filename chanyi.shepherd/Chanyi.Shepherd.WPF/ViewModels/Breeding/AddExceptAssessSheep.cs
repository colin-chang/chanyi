
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Breeding
{
    public class AddExceptAssessSheep : AddViewModel
    {
        public AddExceptAssessSheep(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddExceptAssessSheep(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            var Sheeps = this.Service.GetExceptAssessSheepAddBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.sheeps.Clear();
                this.Sheeps.Add(new SheepBind { SerialNumber = defaultSelection });
                Sheeps.ForEach(s => this.Sheeps.Add(s));
            }), null);

            if (this.isContinue) return;
            var principals = this.Service.GetEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }

        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }

        private string exceptSheepId;
        [EntityProperty]
        [Required(ErrorMessage = "羊只编号输入有误")]
        public string ExceptSheepId
        {
            get { return exceptSheepId; }
            set
            {
                exceptSheepId = value;
                this.Validate("ExceptSheepId");
            }
        }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string reason;
        [EntityProperty]
        public string Reason
        {
            get { return reason; }
            set
            {
                reason = value;
                this.RaisePropertyChanged("Reason");
            }
        }

        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "测评人必选")]
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
                    var result = this.Service.AddExceptAssessSheep(this.ExceptSheepId, this.Reason, this.PrincipalId, this.UserId, this.Remark);
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("一键排除羊只鉴定成功"))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
