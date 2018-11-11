using System.Windows;

using Microsoft.Practices.Prism.Commands;
using System.ComponentModel.DataAnnotations;

namespace Chanyi.Shepherd.WPF.ViewModels.SystemService
{
    class EditPermissionViewModel : EditViewModel
    {
        public EditPermissionViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }

        protected override object GetEditModel()
        {
            return this.Service.GetPermissionById(this.Id);
        }

        public string Id { get; set; }

        private string name;

        [Required(ErrorMessage = "角色名称必填")]
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

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                this.RaisePropertyChanged("Description");
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

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.UpdatePermission(this.name, this.Description, this.Remark, this.Id);
                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
