using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class EditOtherViewModel : EditViewModel
    {
        public EditOtherViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }

        protected override object GetEditModel()
        {
            return this.Service.GetOtherByKindId(Id);
        }

        public string Id { get; set; }

        private string name;

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
                    var result = this.Service.UpdateOther(this.Name,this.Unit, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result)) return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
