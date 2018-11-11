using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.HR
{
    class AddDutyViewModel:AddViewModel
    {
        public AddDutyViewModel(UIElement error)
        {
            this.errorControl = error;
        }
        public AddDutyViewModel(bool withinInitilization) { }
    
        private string name;
        [EntityProperty]
        [Required(ErrorMessage = "职务名称必填")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.Validate("Name");
            }
        }
        private string description;
        [EntityProperty]
        [Required(ErrorMessage = "职务描述信息必填")]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                this.Validate("Description");
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
                    var result = this.Service.AddDuty(this.Name, this.Description, this.UserId, this.Remark);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("职务添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
