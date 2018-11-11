using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Inputs;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class MedicineInputsViewModel : ListViewModel
    {
        public MedicineInputsViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<MedicineInputsViewModel, MedicineInOutFilter>();
            InitializeBindData();
        }
        public MedicineInputsViewModel(bool withinInitilization) { }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var medicineNames = this.Service.GetMedicineNameBind();
                var manufacturers = this.Service.GetManufactureBind();
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.MedicineNames.Clear();
                    this.Manufacturers.Clear();
                    this.Principals.Clear();
                    this.MedicineNames.Add(new MedicineBind { Name = defaultSelection });
                    medicineNames.ForEach(m => this.MedicineNames.Add(m));
                    this.Manufacturers.Add(new ManufactureBind { Name = defaultSelection });
                    manufacturers.ForEach(m => this.Manufacturers.Add(m));
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public MedicineInOutFilter Filter
        {
            get
            {
                MedicineInOutFilter filter = Mapper.Map<MedicineInOutFilter>(this);
                return filter;
            }
        }

        #region 搜索字段
        private ObservableCollection<MedicineBind> medicineNames = new ObservableCollection<MedicineBind>();
        public ObservableCollection<MedicineBind> MedicineNames { get { return medicineNames; } }

        private ObservableCollection<ManufactureBind> manufacturers = new ObservableCollection<ManufactureBind>();
        public ObservableCollection<ManufactureBind> Manufacturers { get { return manufacturers; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string nameId;
        [EntityProperty]
        public string NameId
        {
            get { return nameId; }
            set
            {
                nameId = value;
                this.Validate("NameId");
            }
        }

        private string manufacturerId;
        [EntityProperty]

        public string ManufacturerId
        {
            get { return manufacturerId; }
            set
            {
                manufacturerId = value;
                this.Validate("ManufacturerId");
            }
        }

        private string maxAmount;
        [EntityProperty]
        public string MaxAmount
        {
            get { return maxAmount; }
            set
            {
                maxAmount = value;
                this.RaisePropertyChanged("MaxAmount");
            }
        }
        private string minAmount;
        [EntityProperty]
        public string MinAmount
        {
            get { return minAmount; }
            set
            {
                minAmount = value;
                this.RaisePropertyChanged("MinAmount");
            }
        }


        private InOutWarehouseDirectionEnum? direction;
        [EntityProperty]
        public InOutWarehouseDirectionEnum? Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                this.RaisePropertyChanged("Direction");
            }
        }



        private DateTime? startExpirationDate;
        [EntityProperty]
        public DateTime? StartExpirationDate
        {
            get { return startExpirationDate; }
            set
            {
                startExpirationDate = value;
                this.RaisePropertyChanged("StartExpirationDate");
            }
        }
        private DateTime? endExpirationDate;
        [EntityProperty]
        public DateTime? EndExpirationDate
        {
            get { return endExpirationDate; }
            set
            {
                endExpirationDate = value;
                this.RaisePropertyChanged("EtartExpirationDate");
            }
        }

        private DateTime? startOperationDate;
        [EntityProperty]
        public DateTime? StartOperationDate
        {
            get { return startOperationDate; }
            set
            {
                startOperationDate = value;
                this.RaisePropertyChanged("StartOperationDate");
            }
        }
        private DateTime? endOperationDate;
        [EntityProperty]
        public DateTime? EndOperationDate
        {
            get { return endOperationDate; }
            set
            {
                endOperationDate = value;
                this.RaisePropertyChanged("EndOperationDate");
            }
        }

        private string principalId;
        [EntityProperty]

        public string PrincipalId
        {
            get { return principalId; }
            set
            {
                principalId = value;
                this.RaisePropertyChanged("PrincipalId");
            }
        }
        #endregion

        #region 列表数据
        public List<MedicineInOut> MedicineList { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action Initialize = () =>
            {

                int count;
                this.MedicineList = this.Service.GetMenicineInOut(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<MedicineInOut>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.MedicineList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
        #endregion

        public DelegateCommand AddInCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddMedicineInOutWarehouseWindow win = new AddMedicineInOutWarehouseWindow(InOutWarehouseDirectionEnum.In) { Owner = CurrentWindow };
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        public DelegateCommand AddOutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddMedicineInOutWarehouseWindow win = new AddMedicineInOutWarehouseWindow(InOutWarehouseDirectionEnum.Out) { Owner = CurrentWindow };
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

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
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetMenicineInOut(this.Filter, rowCount).ToArray();
        }
    }
}
