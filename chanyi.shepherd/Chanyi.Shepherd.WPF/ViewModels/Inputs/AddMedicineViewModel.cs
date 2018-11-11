using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Inputs;




namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class AddMedicineViewModel : AddViewModel
    {
        public AddMedicineViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }
        public AddMedicineViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            if (this.isContinue) return;
            var medicineNames = this.Service.GetMedicineNameBind();
            var manufacturers = this.Service.GetManufactureBind();
            var types = this.Service.GetMedicineTypeBind();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.MedicineNames.Add(new MedicineBind { Name = defaultSelection });
                medicineNames.ForEach(m => this.MedicineNames.Add(m));
                this.Manufacturers.Add(new ManufactureBind { Name = defaultSelection });
                manufacturers.ForEach(m => this.Manufacturers.Add(m));
                this.Types.Add(new MedicineTypeBind { Name = defaultSelection });
                types.ForEach(t => this.Types.Add(t));
            }), null);
        }

        private ObservableCollection<MedicineBind> medicineNames = new ObservableCollection<MedicineBind>();
        public ObservableCollection<MedicineBind> MedicineNames { get { return medicineNames; } }

        private string medicineNameId;
        [EntityProperty]
        [Required(ErrorMessage = "药品名称必填")]
        public string MedicineNameId
        {
            get { return medicineNameId; }
            set
            {
                medicineNameId = value;
                this.Validate("MedicineNameId");
            }
        }

        private ObservableCollection<ManufactureBind> manufacturers = new ObservableCollection<ManufactureBind>();
        public ObservableCollection<ManufactureBind> Manufacturers { get { return manufacturers; } }


        private string manufacturerId;
        [EntityProperty]
        [Required(ErrorMessage = "生产商必填")]
        public string ManufacturerId
        {
            get { return manufacturerId; }
            set
            {
                manufacturerId = value;
                this.Validate("ManufacturerId");
            }
        }


        private ObservableCollection<MedicineTypeBind> types = new ObservableCollection<MedicineTypeBind>();
        public ObservableCollection<MedicineTypeBind> Types { get { return types; } }


        private string typeId;
        [EntityProperty]
        [Required(ErrorMessage = "药品类型必填")]
        public string TypeId
        {
            get { return typeId; }
            set
            {
                typeId = value;
                this.Validate("TypeId");
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
                this.RaisePropertyChanged("Unit");
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
                    var result = this.Service.AddMedicine(this.MedicineNameId, this.ManufacturerId,this.TypeId,this.Unit, this.UserId, this.Remark);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("新药品添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
        public DelegateCommand AddMedicineName
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddMedicineNameWindow win = new AddMedicineNameWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        var medicineNames = this.Service.GetMedicineNameBind();
                        this.UIDispatcher.Invoke(new Action(() =>
                        {
                            this.MedicineNames.Clear();
                            this.MedicineNames.Add(new MedicineBind { Name = defaultSelection });
                            medicineNames.ForEach(m => this.MedicineNames.Add(m));
                            this.MedicineNameId = this.MedicineNames.FirstOrDefault() == null ? null : this.MedicineNames.FirstOrDefault().Id;
                        }), null);
                    }
                });
            }
        }
        public DelegateCommand AddManufacturer
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddManufacturerWindow win = new AddManufacturerWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        var manufacturers = this.Service.GetManufactureBind();
                        this.UIDispatcher.Invoke(new Action(() =>
                        {
                            this.Manufacturers.Clear();
                            this.Manufacturers.Add(new ManufactureBind { Name = defaultSelection });
                            manufacturers.ForEach(m => this.Manufacturers.Add(m));
                            this.ManufacturerId = this.Manufacturers.FirstOrDefault() == null ? null : this.Manufacturers.FirstOrDefault().Id;
                        }), null);
                    }
                });
            }
        }
    }
}