using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.DiseaseControl;
using Chanyi.Shepherd.QueryModel.Model.DiseaseControl;
using Chanyi.Shepherd.WPF.Views.DiseaseControl;
using System.Collections.ObjectModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Windows;


namespace Chanyi.Shepherd.WPF.ViewModels.DiseaseControl
{
    /// <summary>
    /// 防疫计划
    /// </summary>
    public class AntiepidemicPlanViewModel : ListViewModel
    {
        public AntiepidemicPlanViewModel(bool withinInitilization) { }

        public AntiepidemicPlanViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;

            Mapper.CreateMap<AntiepidemicPlanViewModel, AntiepidemicPlanFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var principals = this.Service.GetAllEmployeeBind();
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Principals.Clear();
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
        public AntiepidemicPlanFilter Filter
        {
            get
            {
                AntiepidemicPlanFilter filter = Mapper.Map<AntiepidemicPlanFilter>(this);
                return filter;
            }
        }

        #region 搜索条件

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

        private string vaccine;
        [EntityProperty]
        public string Vaccine
        {
            get { return vaccine; }
            set
            {
                vaccine = value;
                this.RaisePropertyChanged("Vaccine");
            }
        }

        private DateTime? startPlanExecuteDate;
        [EntityProperty]
        public DateTime? StartPlanExecuteDate
        {
            get { return startPlanExecuteDate; }
            set
            {
                startPlanExecuteDate = value;
                this.RaisePropertyChanged("StartPlanExecuteDate");
            }
        }

        private DateTime? endPlanExecuteDate;
        [EntityProperty]
        public DateTime? EndPlanExecuteDate
        {
            get { return endPlanExecuteDate; }
            set
            {
                endPlanExecuteDate = value;
                this.RaisePropertyChanged("EndPlanExecuteDate");
            }
        }

        private string sheepFlock;

        [EntityProperty]
        public string SheepFlock
        {
            get { return sheepFlock; }
            set
            {
                sheepFlock = value;
                this.RaisePropertyChanged("SheepFlock");
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

        public IEnumerable<AntiepidemicPlan> AntiepidemicPlanList { get; set; }
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
                this.AntiepidemicPlanList = this.Service.GetAntiepidemicPlan(this.Filter, this.PageIndex, this.PageSize, out count);
                this.AntiepidemicPlanList.Each(a => a.IsExcuted = !a.IsExcuted);
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<AntiepidemicPlan>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.AntiepidemicPlanList);
            };

            this.ProgressRing.Show();
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);
        }
        public DelegateCommand<string> EditCommand
        {
            get
            {
                return this.GetEditCommand(id =>
                {
                    EditAntiepidemicPlanWindow win = new EditAntiepidemicPlanWindow(id);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        this.LoadData();
                });
            }
        }

        /// <summary>
        /// 执行防御计划
        /// </summary>
        public DelegateCommand<string> ExecuteColumn
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        MessageBox.Show("请选择要执行的计划！", "警告", MessageBoxButton.OK, MessageBoxImage.Question);
                        return;
                    }
                    ExecuteAntiepidemicPlanWindow win = new ExecuteAntiepidemicPlanWindow(id);
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
                    AddAntiepidemicPlanWindow win = new AddAntiepidemicPlanWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetAntiepidemicPlan(this.Filter, rowCount).ToArray();
        }
    }
}
