using System.Windows;

using Microsoft.Practices.Prism.Commands;

namespace Chanyi.Shepherd.WPF.ViewModels.SystemService
{
    class EditUserViewModel : EditViewModel
    {
        public EditUserViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }

        protected override void InitializeBindData()
        {
            this.CanEdit = this.Id != this.UserId;
            base.InitializeBindData();
        }

        private bool canEdit;

        public bool CanEdit
        {
            get { return canEdit; }
            set
            {
                canEdit = value;
                this.RaisePropertyChanged("CanEdit");
            }
        }
        protected override object GetEditModel()
        {
            return this.Service.GetUserById(this.Id);
        }
        public string Id { get; set; }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                this.RaisePropertyChanged("UserName");
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                this.RaisePropertyChanged("IsEnabled");
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
                    var result = this.Service.UpdateUser(this.IsEnabled, this.Remark, this.Id);
                    if (!ValidateFailedServiceResult<bool>(result))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
