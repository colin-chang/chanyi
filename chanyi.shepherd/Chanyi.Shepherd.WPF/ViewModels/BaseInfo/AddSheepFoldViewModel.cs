
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Collections.ObjectModel;
using System;


namespace Chanyi.Shepherd.WPF.ViewModels.BaseInfo
{
    class AddSheepFoldViewModel : AddViewModel
    {
        public AddSheepFoldViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }
        public AddSheepFoldViewModel(bool withinInitilization)
        {
        }
        protected override void InitializeBindItem()
        {
            if (this.isContinue) return;
            var administrators = this.Service.GetEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Administrators.Add(new EmployeeBind { Name = defaultSelection });
                administrators.ForEach(a => this.Administrators.Add(a));
            }), null);
            
        }

        private string name;
        [EntityProperty]
        [Required(ErrorMessage = "羊圈舍必填")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.Validate("Name");
            }
        }
        private ObservableCollection<EmployeeBind> administrators = new ObservableCollection<EmployeeBind>();

        public ObservableCollection<EmployeeBind> Administrators
        {
            get { return administrators; }
        }

        private string administratorId;
        [EntityProperty]
        [Required(ErrorMessage = "羊圈管理员必填")]
        public string AdministratorId
        {
            get { return administratorId; }
            set
            {
                administratorId = value;
                this.Validate("AdministratorId");
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
                    var result = this.Service.AddSheepFold(this.Name, this.AdministratorId, this.UserId, this.Remark);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("圈舍添加成功"))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
