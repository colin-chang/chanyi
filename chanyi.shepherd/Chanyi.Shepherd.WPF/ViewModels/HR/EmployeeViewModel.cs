using System;
using System.Collections.Generic;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;


using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.HR;
using Chanyi.Shepherd.QueryModel.Model.HR;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.HR;
using System.Collections.ObjectModel;


namespace Chanyi.Shepherd.WPF.ViewModels.HR
{
    class EmployeeViewModel : ListViewModel
    {
        public EmployeeViewModel(bool withinInitilization) { }
        public EmployeeViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;

            Mapper.CreateMap<EmployeeViewModel, EmployeeFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                List<EmployeeBind> list = this.Service.GetAllEmployeeBind();
                list.Insert(0, new EmployeeBind { Name = defaultSelection, SerialNum = defaultSelection });
                var dutys = this.Service.GetDutyBind(); ;
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Names.Clear();
                    this.SerialNums.Clear();
                    this.Principals.Clear();
                    this.Dutys.Clear();
                    list.ForEach(l => this.Names.Add(l));
                    list.ForEach(l => this.SerialNums.Add(l));
                    list.ForEach(l => this.Principals.Add(l));
                    this.Dutys.Add(new DutyBind { Name = defaultSelection });
                    dutys.ForEach(d => this.Dutys.Add(d));
                    this.Reset();
                }), null);
                
               
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);

        }

        public EmployeeFilter Filter
        {
            get
            {
                EmployeeFilter filter = Mapper.Map<EmployeeFilter>(this);
                filter.Name = filter.Name == defaultSelection ? null : filter.Name;
                filter.SerialNum = filter.SerialNum == defaultSelection ? null : filter.SerialNum;
                return filter;
            }
        }
        protected override void Reset()
        {
            base.Reset();
            this.Name = defaultSelection;
            this.SerialNum = defaultSelection;
        }

        #region 数据绑定搜索

        private ObservableCollection<EmployeeBind> names = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Names { get { return names; } }

        private ObservableCollection<DutyBind> dutys = new ObservableCollection<DutyBind>();
        public ObservableCollection<DutyBind> Dutys { get { return dutys; } }

        private ObservableCollection<EmployeeBind> serialNums = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> SerialNums { get { return serialNums; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals
        {
            get { return principals; }
        }

        private string name;
        [EntityProperty]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        private GenderEnum? gender;
        [EntityProperty]
        public GenderEnum? Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                this.RaisePropertyChanged("Gender");
            }
        }

        private EmployeeStatusEnum? status;
        [EntityProperty]
        public EmployeeStatusEnum? Status
        {
            get { return status; }
            set
            {
                status = value;
                this.RaisePropertyChanged("Status");
            }
        }


        private string dutyId;
        [EntityProperty]
        public string DutyId
        {
            get { return dutyId; }
            set
            {
                dutyId = value;
                this.RaisePropertyChanged("DutyId");
            }
        }


        private string serialNum;
        [EntityProperty]
        public string SerialNum
        {
            get { return serialNum; }
            set
            {
                serialNum = value;
                this.RaisePropertyChanged("SerialNum");
            }
        }

        private DateTime? startEntryDate;
        [EntityProperty]
        public DateTime? StartEntryDate
        {
            get { return startEntryDate; }
            set
            {
                startEntryDate = value;
                this.RaisePropertyChanged("StartEntryDate");
            }
        }
        private DateTime? endEntryDate;
        [EntityProperty]
        public DateTime? EndEntryDate
        {
            get { return endEntryDate; }
            set
            {
                endEntryDate = value;
                this.RaisePropertyChanged("EndEntryDate");
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
        public IEnumerable<Employee> EmployeeList { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action initialize = () =>
            {
                int count;
                this.EmployeeList = this.Service.GetEmployee(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<Employee>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.EmployeeList);
            };
            this.ProgressRing.Show();
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);

        }
        #endregion


        public DelegateCommand<string> EditCommand
        {
            get
            {
                return this.GetEditCommand(id =>
                {
                    EditEmployeeWindow win = new EditEmployeeWindow(id);
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
                    AddEmployeeWindow win = new AddEmployeeWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }


        public DelegateCommand<string> DepartureCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    AddDepartureEmployeeWindow win = new AddDepartureEmployeeWindow(id);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetEmployee(this.Filter, rowCount).ToArray();
        }
    }
}
