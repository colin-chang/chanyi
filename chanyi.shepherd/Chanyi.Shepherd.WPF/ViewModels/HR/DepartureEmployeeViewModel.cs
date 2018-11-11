using AutoMapper;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.HR;
using Chanyi.Shepherd.QueryModel.Model.HR;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.ViewModels.HR
{
    class DepartureEmployeeViewModel : ListViewModel
    {
        public DepartureEmployeeViewModel(bool withinInitilization) { }
        public DepartureEmployeeViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<DepartureEmployeeViewModel, QuitFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                List<EmployeeBind> list = this.Service.GetAllEmployeeBind();
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Employees.Clear();
                    this.Principals.Clear();
                    list.Insert(0, new EmployeeBind { Name = defaultSelection, SerialNum = defaultSelection });
                    list.ForEach(l => this.Employees.Add(l));
                    list.ForEach(l => this.Principals.Add(l));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);

        }

        public QuitFilter Filter
        {
            get
            {
                QuitFilter filter = Mapper.Map<QuitFilter>(this);
                return filter;
            }
        }

        #region 数据绑定搜索

        private ObservableCollection<EmployeeBind> employees = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Employees { get { return employees; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals
        {
            get { return principals; }
        }

        private string employeeId;
        [EntityProperty]
        public string EmployeeId
        {
            get { return employeeId; }
            set
            {
                employeeId = value;
                this.RaisePropertyChanged("EmployeeId");
            }
        }

        private DateTime? startQuitDate;
        [EntityProperty]
        public DateTime? StartQuitDate
        {
            get { return startQuitDate; }
            set
            {
                startQuitDate = value;
                this.RaisePropertyChanged("StartQuitDate");
            }
        }
        private DateTime? endQuitDate;
        [EntityProperty]
        public DateTime? EndQuitDate
        {
            get { return endQuitDate; }
            set
            {
                endQuitDate = value;
                this.RaisePropertyChanged("EndQuitDate");
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
        public IEnumerable<Quit> EmployeeList { get; set; }
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
                this.EmployeeList = this.Service.GetQuit(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<Quit>>(d =>
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

        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetQuit(this.Filter, rowCount).ToArray();
        }
    }
}
