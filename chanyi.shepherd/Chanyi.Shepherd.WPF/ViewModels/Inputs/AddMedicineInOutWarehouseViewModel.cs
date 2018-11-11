using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Collections.ObjectModel;
using Chanyi.Shepherd.WPF.Views.Inputs;
using System.Windows.Controls;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class AddMedicineInOutWarehouseViewModel : AddViewModel
    {
        public AddMedicineInOutWarehouseViewModel(UIElement error, InOutWarehouseDirectionEnum direction)
        {
            this.errorControl = error;
            this.InitializeBindData();
            this.direction = direction;
        }

        public AddMedicineInOutWarehouseViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            if (isContinue)
                return;

            var medicineNames = this.Service.GetMedicineNameBind();
            var principals = this.Service.GetEmployeeBind();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.MedicineNames.Add(new MedicineBind { Name = defaultSelection });
                medicineNames.ForEach(m => this.MedicineNames.Add(m));
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }

        InOutWarehouseDirectionEnum direction;

        public string Title { get { return this.direction == InOutWarehouseDirectionEnum.In ? "药品入库" : "药品出库"; } }


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

        private ObservableCollection<MedicineTypeBind> types = new ObservableCollection<MedicineTypeBind>();
        public ObservableCollection<MedicineTypeBind> Types
        {
            get { return types; ; }
        }

        private string typeId;
        [EntityProperty]
        [Required(ErrorMessage = "饲料类型必选")]
        public string TypeId
        {
            get { return typeId; ; }
            set
            {
                typeId = value;
                this.Validate("TypeId");
            }
        }

      

        private ObservableCollection<CooperaterBind> manufacturers = new ObservableCollection<CooperaterBind>();
        public ObservableCollection<CooperaterBind> Manufacturers { get { return manufacturers; } }

        private string manufacturerId;
        [EntityProperty]
        [Required(ErrorMessage = "生产商必选")]
        public string ManufacturerId
        {
            get { return manufacturerId; }
            set
            {
                manufacturerId = value;
                this.Validate("ManufacturerId");
            }
        }


        private string amount;
        [EntityProperty]
        [Required(ErrorMessage = "药品数量必填")]
        [FloatNumber(0, 1000000, ErrorMessage = "药品数量不合法")]
        public string Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                this.Validate("Amount");
            }
        }



        private DateTime? expirationDate;
        [EntityProperty]
        [AfterToday(ErrorMessage = "有效期需大于当前日期")]
        [Required(ErrorMessage = "有效期必填")]
        public DateTime? ExpirationDate
        {
            get { return expirationDate; }
            set
            {
                expirationDate = value;
                this.Validate("ExpirationDate");
            }
        }

        private DateTime? operationDate;
        [EntityProperty]
        [BeforeToday(ErrorMessage = "日期需小于当前日期")]
        [Required(ErrorMessage = "时间必填")]
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
        [Required(ErrorMessage = "操作人必选")]
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

     
        private bool typeEnable;

        public bool TypeEnable
        {
            get { return typeEnable; }
            set
            {
                typeEnable = value;
                this.RaisePropertyChanged("TypeEnable");
            }
        }
      private bool manufacturerEnable;
        public bool ManufacturerEnable
        {
            get { return manufacturerEnable; }
            set
            {
                manufacturerEnable = value;
                this.RaisePropertyChanged("ManufacturerEnable");
            }
        }

        public DelegateCommand SelectMedicineNameChanged
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (this.MedicineNameId == null)
                    {
                        ResetSelected();
                        return;
                    }

                    var typeNames = this.Service.GetMedicineTypeBind(this.MedicineNameId);
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        this.Types.Clear();
                        this.Types.Add(new MedicineTypeBind { Name = defaultSelection });
                        typeNames.ForEach(t => this.Types.Add(t));
                        this.TypeId = this.Types.FirstOrDefault().Id;
                        this.ManufacturerId = this.Manufacturers.FirstOrDefault() == null ? null : this.Manufacturers.FirstOrDefault().Id;
                    }), null);

                    if (Types.Count() <= 1)
                    {
                        this.errors["TypeNameId"] = "无对应的药品类型，请添加新药品";
                        this.RaisePropertyChanged("Error");
                        this.TypeEnable = false;
                    }
                    else
                    {
                        this.errors.Remove("TypeNameId");
                        this.TypeEnable = true;
                    }
                });
            }
        }

        public DelegateCommand SelectTypeNameChanged
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (this.TypeId == null)
                    {
                        this.UIDispatcher.Invoke(new Action(() => { this.Manufacturers.Clear(); this.ManufacturerEnable = false; }), null);
                        return;
                    }

                    var manufacturers = this.Service.GetManufacturerBind(this.MedicineNameId, this.TypeId);
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        this.Manufacturers.Clear();
                        this.Manufacturers.Add(new CooperaterBind { Name = defaultSelection });
                        manufacturers.ForEach(a => this.Manufacturers.Add(a));
                        this.ManufacturerId = this.Manufacturers.FirstOrDefault().Id;
                    }), null);

                    if (Manufacturers.Count() <= 1)
                    {
                        this.errors["AreaId"] = "无对应的产地,请添加新饲料";
                        this.RaisePropertyChanged("Error");
                        this.ManufacturerEnable = false;
                    }
                    else
                    {
                        this.errors.Remove("AreaId");
                        this.ManufacturerEnable = true;
                    }
                });
            }
        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.AddMedicineInOutWarehouse(this.MedicineNameId, this.ManufacturerId,this.TypeId, (DateTime)this.ExpirationDate, float.Parse(this.Amount), this.direction, (DateTime)this.OperationDate, this.PrincipalId, this.UserId, this.Remark);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    this.UpdateNotification();
                    if (this.Continue2Add("药品入(出)库添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
        /// <summary>
        /// 添加新药品
        /// </summary>
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddMedicineWindow win = new AddMedicineWindow();
                    win.Owner = CurrentWindow;
                    win.ShowDialog();
                });
            }
        }
        protected override void Reset()
        {
            base.Reset();
            ResetSelected();
        }

        void ResetSelected()
        {
            if (this.MedicineNameId == null)
            {
                this.ManufacturerEnable = false;
                this.UIDispatcher.Invoke(new Action(() => this.Manufacturers.Clear()), null);
            }
        }
    }
}
