using System;
using System.Collections.Generic;

using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Collections.ObjectModel;


namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class MedicineWarehouseViewModel : ListViewModel
    {
        public MedicineWarehouseViewModel(bool withinInitilization) { }

        public MedicineWarehouseViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<MedicineWarehouseViewModel, MedicineInventoryFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var medicineNames = this.Service.GetMedicineNameBind();
                var manufacturers = this.Service.GetManufactureBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.MedicineNames.Clear();
                    this.Manufacturers.Clear();
                    this.MedicineNames.Add(new MedicineBind { Name = defaultSelection });
                    medicineNames.ForEach(m => this.MedicineNames.Add(m));
                    this.Manufacturers.Add(new ManufactureBind { Name = defaultSelection });
                    manufacturers.ForEach(m => this.Manufacturers.Add(m));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public MedicineInventoryFilter Filter
        {
            get
            {
                MedicineInventoryFilter filter = Mapper.Map<MedicineInventoryFilter>(this);
                return filter;
            }
        }

        #region 搜索字段
        private ObservableCollection<MedicineBind> medicineNames = new ObservableCollection<MedicineBind>();
        public ObservableCollection<MedicineBind> MedicineNames { get { return medicineNames; } }

        private ObservableCollection<ManufactureBind> manufacturers = new ObservableCollection<ManufactureBind>();
        public ObservableCollection<ManufactureBind> Manufacturers { get { return manufacturers; } }

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

        #endregion

        #region 列表数据
        public List<MedicineInventory> MedicineInventoryList { get; set; }
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
                this.MedicineInventoryList = this.Service.GetMedicineInventory(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<MedicineInventory>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.MedicineInventoryList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
        #endregion
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetMedicineInventory(this.Filter, rowCount).ToArray();
        }
    }
}


