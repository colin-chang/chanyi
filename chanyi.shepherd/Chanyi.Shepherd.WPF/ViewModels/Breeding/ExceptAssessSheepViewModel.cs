using AutoMapper;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.BaseInfo;
using Chanyi.Shepherd.QueryModel.Filter.Breeding;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;
using Chanyi.Shepherd.QueryModel.Model.Breeding;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Breeding;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.ViewModels.Breeding
{
    /// <summary>
    /// 意见否决羊只
    /// </summary>
    public class ExceptAssessSheepViewModel : ListViewModel
    {
        public ExceptAssessSheepViewModel(bool withinInitilization) { }

        public ExceptAssessSheepViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            Mapper.CreateMap<ExceptAssessSheepViewModel, ExceptAssessFilter>();
            InitializeBindData();
        }

        #region 搜索相关
        protected override void InitializeBindData()
        {

            Action Initialize = () =>
            {
                var sheeps = this.Service.GetExceptAssessSheepSelectBind();
                var breeds = this.Service.GetBreedBind();
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Sheeps.Clear();
                    this.Breeds.Clear();
                    this.Principals.Clear();

                    this.Sheeps.Add(new SheepBind() { SerialNumber = defaultSelection });
                    sheeps.ForEach(s => this.Sheeps.Add(s));
                    this.Breeds.Add(new BreedBind { Name = defaultSelection });
                    breeds.ForEach(b => this.Breeds.Add(b));
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }

        private ObservableCollection<BreedBind> breeds = new ObservableCollection<BreedBind>();
        public ObservableCollection<BreedBind> Breeds { get { return breeds; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string sheepId;
        [EntityProperty]
        public string SheepId
        {
            get { return sheepId; }
            set
            {
                sheepId = value;
                this.RaisePropertyChanged("SheepId");
            }
        }

        private string reason;
        [EntityProperty]
        public string Reason
        {
            get { return reason; }
            set
            {
                reason = value;
                this.RaisePropertyChanged("Reason");
            }
        }
        private DateTime? startCreateTime;
        [EntityProperty]
        public DateTime? StartCreateTime
        {
            get { return startCreateTime; }
            set
            {
                startCreateTime = value;
                this.RaisePropertyChanged("StartCreateTime");
            }
        }

        private DateTime? endCreateTime;
        [EntityProperty]
        public DateTime? EndCreateTime
        {
            get { return endCreateTime; }
            set
            {
                endCreateTime = value;
                this.RaisePropertyChanged("EndCreateTime");
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
        public ExceptAssessFilter Filter
        {
            get
            {
                ExceptAssessFilter filter = Mapper.Map<ExceptAssessFilter>(this);
                return filter;
            }
        }
        public IEnumerable<ExceptAssess> ExceptSheep { get; set; }
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
                this.ExceptSheep = this.Service.GetExceptAssess(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<ExceptAssess>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), ExceptSheep);
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
                    ExceptAssessSheepWindow win = new ExceptAssessSheepWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        public DelegateCommand<string> RemoveCommand
        {
            get
            {
                return this.GetRemoveCommand(id =>
                {
                    this.Service.DeleteExceptAssessSheep(id);
                    this.LoadData();
                });
            }
        }

        protected override Array GetExportData(int rowCount)
        {
            this.ExceptSheep = this.Service.GetExceptAssess(new ExceptAssessFilter(), rowCount);
            return this.ExceptSheep.ToArray();
        }
    }
}
