using System;
using System.Collections.Generic;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.QueryModel.Model.Finance;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Finance;
using System.Collections.ObjectModel;



namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class PayoffViewModel : ListViewModel
    {
        public PayoffViewModel(bool withinInitilization) { }

        public PayoffViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<PayoffViewModel, PayoffFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {

            Action Initialize = () =>
            {
                List<EmployeeBind> list = this.Service.GetAllEmployeeBind();
                list.Insert(0, new EmployeeBind { Name = defaultSelection });

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Principals.Clear();
                    this.Employees.Clear();

                    list.ForEach(p => this.Principals.Add(p));
                    list.ForEach(p => this.Employees.Add(p));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public PayoffFilter Filter
        {
            get
            {
                PayoffFilter filter = Mapper.Map<PayoffFilter>(this);
                return filter;
            }
        }

        #region 绑定搜索字段
        private ObservableCollection<EmployeeBind> employees = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Employees { get { return employees; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

     

        private string employeeId;
        [EntityProperty]
        public string EmployeeId
        {
            get { return employeeId; }
            set
            {
                employeeId = value;
                this.Validate("EmployeeId");
            }
        }

        private string maxMoney;
        [EntityProperty]
        [FloatNumber(0, int.MaxValue, IsNullable = true, ErrorMessage = "最大价格不合法")]
        public string MaxMoney
        {
            get { return maxMoney; }
            set
            {
                maxMoney = value;
                this.Validate("MaxMoney");
            }
        }

        private string minMoney;
        [EntityProperty]
        [FloatNumber(0, int.MaxValue, IsNullable = true, ErrorMessage = "最小价格不合法")]
        public string MinMoney
        {
            get { return minMoney; }
            set
            {
                minMoney = value;
                this.Validate("MinMoney");
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
        public IEnumerable<Payoff> PayoffList { get; set; }
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
                this.PayoffList = this.Service.GetPayoff(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<Payoff>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.PayoffList);
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
                    AddPayoffWindow win = new AddPayoffWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetPayoff(this.Filter, rowCount).ToArray();
        }
    }
}

