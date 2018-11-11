using System;
using System.Collections.Generic;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.QueryModel.Model.Finance;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Finance;
using System.Collections.ObjectModel;

namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class BuyMedicineViewModel : ListViewModel
    {
        public BuyMedicineViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<BuyMedicineViewModel, BuyMedicineFilter>();
            InitializeBindData();
        }
        public BuyMedicineViewModel(bool withinInitilization) { }
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
                    this.Manufacturers.Add(new ManufactureBind { Name = defaultSelection });
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });

                    medicineNames.ForEach(f => this.MedicineNames.Add(f));
                    manufacturers.ForEach(t => this.Manufacturers.Add(t));
                    principals.ForEach(p => this.Principals.Add(p));

                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public BuyMedicineFilter Filter
        {
            get
            {
                BuyMedicineFilter filter = Mapper.Map<BuyMedicineFilter>(this);
                return filter;
            }
        }

        #region 搜索字段
        //private List<MedicineBind> medicineNames;
        //public List<MedicineBind> MedicineNames
        //{
        //    get { return medicineNames; }
        //    set
        //    {
        //        medicineNames = value;
        //        this.RaisePropertyChanged("MedicineNames");
        //    }
        //}

        private ObservableCollection<MedicineBind> medicineNames = new ObservableCollection<MedicineBind>();
        public ObservableCollection<MedicineBind> MedicineNames { get { return medicineNames; } }


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

        //private List<ManufactureBind> manufacturers;

        //public List<ManufactureBind> Manufacturers
        //{
        //    get { return manufacturers; }
        //    set
        //    {
        //        manufacturers = value;
        //        this.RaisePropertyChanged("Manufacturers");
        //    }
        //}

        private ObservableCollection<ManufactureBind> manufacturers = new ObservableCollection<ManufactureBind>();
        public ObservableCollection<ManufactureBind> Manufacturers { get { return manufacturers; } }


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

        private string maxMoney;
        [EntityProperty]
        public string MaxMoney
        {
            get { return maxMoney; }
            set
            {
                maxMoney = value;
                this.RaisePropertyChanged("MaxMoney");
            }
        }
        private string minMoney;
        [EntityProperty]
        public string MinMoney
        {
            get { return minMoney; }
            set
            {
                minMoney = value;
                this.RaisePropertyChanged("MinMoney");
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

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

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
        public List<BuyMedicine> MedicineList { get; set; }
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
                this.MedicineList = this.Service.GetBuyMedicine(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<BuyMedicine>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.MedicineList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
        #endregion
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddBuyMedicineWindow win = new AddBuyMedicineWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        LoadData();
                    }
                });
            }
        }


        #region 展示选中的药品
        private MedicineInOut medicine;

        public MedicineInOut Medicine
        {
            get { return medicine; }
            set
            {
                medicine = value;
                this.RaisePropertyChanged("Medicine");
            }
        }

        public DelegateCommand<string> SelectCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    //this.Medicine = this.Service.GetMenicineInOutDetail(id);
                    this.Medicine = this.Service.GetMenicineInOutDetailById(id);
                });
            }
        } 
        #endregion
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetBuyMedicine(this.Filter, rowCount).ToArray();
        }
    }
}

