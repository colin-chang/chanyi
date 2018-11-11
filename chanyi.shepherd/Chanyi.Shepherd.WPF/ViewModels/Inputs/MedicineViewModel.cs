using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Inputs;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class MedicineViewModel : ListViewModel
    {
        public MedicineViewModel(bool withinInitilization) { }

        public MedicineViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;
            Mapper.CreateMap<MedicineViewModel, MedicineFilter>();
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

        public MedicineFilter Filter
        {
            get
            {
                MedicineFilter filter = Mapper.Map<MedicineFilter>(this);
                return filter;
            }
        }

        #region 绑定搜索数据
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
        #endregion

        #region 列表数据
        public IEnumerable<Medicine> MedicineList { get; set; }
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
                this.MedicineList = this.Service.GetMedicine(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<Medicine>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.MedicineList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);

        }
        #endregion

        public DelegateCommand<string> EditCommand
        {
            get
            {
                return this.GetEditCommand(id =>
                {
                    EditMedicineWindow win = new EditMedicineWindow(id);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        this.LoadData();
                    }
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
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetMedicine(this.Filter, rowCount).ToArray();
        }
    }
}
