using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class AddOtherViewModel : AddViewModel
    {
        public AddOtherViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddOtherViewModel(bool withinInitilization) { }

        private string name;
        [EntityProperty]
        [Required(ErrorMessage = "物品名称必填")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.Validate("Name");
            }
        }

        private string unit;
        [EntityProperty]
        [Required(ErrorMessage = "计量单位必填")]
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                this.Validate("Unit");
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
                    var result = this.Service.AddOther(this.Name, this.Unit, this.UserId, this.Remark);
                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("新物品添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}