using System;
using System.Collections.Generic;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Inputs;
using System.Windows;
using System.Collections.ObjectModel;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class OtherInputsViewModel : ListViewModel
    {
        public OtherInputsViewModel(bool withinInitilization) { }

        public OtherInputsViewModel(string header, string icon, string intro,InOutWarehouseDirectionEnum direction)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.Direction = direction;
            Mapper.CreateMap<OtherInputsViewModel, OtherInOutFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var names = this.Service.GetOtherBind();
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Principals.Clear();
                    this.Names.Clear();
                    this.Names.Add(new OtherBind { Name = defaultSelection });
                    names.ForEach(n => this.Names.Add(n));
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public OtherInOutFilter Filter
        {
            get
            {
                OtherInOutFilter filter = Mapper.Map<OtherInOutFilter>(this);
                return filter;
            }
        }

        #region 搜索字段
        private ObservableCollection<OtherBind> names = new ObservableCollection<OtherBind>();
        public ObservableCollection<OtherBind> Names { get { return names; } }

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
                this.RaisePropertyChanged("NameId");
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
        public InOutWarehouseDirectionEnum? Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                this.RaisePropertyChanged("Direction");
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
        public List<OtherInOut> OtherList { get; set; }
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
                this.OtherList = this.Service.GetOtherInOut(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<OtherInOut>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.OtherList);
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
                    AddOtherInOutWarehouseWindow win = new AddOtherInOutWarehouseWindow(InOutWarehouseDirectionEnum.In) { Owner = Application.Current.MainWindow };
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
                    AddOtherInOutWarehouseWindow win = new AddOtherInOutWarehouseWindow(InOutWarehouseDirectionEnum.Out) { Owner = Application.Current.MainWindow };
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetOtherInOut(this.Filter, rowCount).ToArray();
        }
    }
}

