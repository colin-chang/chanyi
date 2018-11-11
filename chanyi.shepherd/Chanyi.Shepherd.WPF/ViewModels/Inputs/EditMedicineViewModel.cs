using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Configuration;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class EditMedicineViewModel : EditViewModel
    {
        public EditMedicineViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            return this.Service.GetMedicineByKindId(Id);
        }

        protected override void InitializeBindItem()
        {
            this.MedicineNames = this.Service.GetMedicineNameBind();
            this.MedicineNames.Insert(0, new MedicineBind { Name = ConfigurationManager.AppSettings["formDefaultSelection"] });
            this.Manufacturers = this.Service.GetManufactureBind();
            this.Manufacturers.Insert(0, new ManufactureBind { Name = ConfigurationManager.AppSettings["formDefaultSelection"] });
        }

        public string Id { get; set; }

        private List<MedicineBind> medicineNames;
        public List<MedicineBind> MedicineNames
        {
            get { return medicineNames; }
            set
            {
                medicineNames = value;
                this.RaisePropertyChanged("MedicineNames");
            }
        }

        private string nameId;
        [EntityProperty]
        [Required(ErrorMessage = "内容必填")]
        public string NameId
        {
            get { return nameId; }
            set
            {
                nameId = value;
                this.Validate("NameId");
            }
        }

        private List<ManufactureBind> manufacturers;

        public List<ManufactureBind> Manufacturers
        {
            get { return manufacturers; }
            set
            {
                manufacturers = value;
                this.RaisePropertyChanged("Manufacturers");
            }
        }
        private string manufacturerId;
        [EntityProperty]
        [Required(ErrorMessage = "生产商内容必填")]
        public string ManufacturerId
        {
            get { return manufacturerId; }
            set
            {
                manufacturerId = value;
                this.Validate("ManufacturerId");
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
                //    var result = this.Service.UpdateMedicine(this.NameId, this.ManufacturerId, this.Remark, this.Id);

                  //  if (!ValidateFailedServiceResult<bool>(result)) return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
