using AutoMapper;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.QueryModel.Model.Finance;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Chanyi.Shepherd.WPF.ViewModels.GroupManage
{
    class SellSheepManageViewModel : ListViewModel
    {
        public SellSheepManageViewModel(bool withinInitilization) { }
        public SellSheepManageViewModel(string header, string icon, string intro)
        {
            Header = header;
            Icon = icon;
            Intro = intro;

            Mapper.CreateMap<SellSheepManageViewModel, SellSheepFilter>();
            InitializeBindData();
        }

        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var sheeps = this.Service.GetSheepBind(new SheepBindFilter { Status = SheepStatusEnum.Selled });
                var breeds = this.Service.GetBreedBind();
                var sheepfolds = this.Service.GetSheepfoldBind();
                var principals = this.Service.GetAllEmployeeBind();
                var operators = this.Service.GetUserBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Sheeps.Clear();
                    this.Sheeps.Add(new SheepBind { SerialNumber = defaultSelection });
                    sheeps.ForEach(s => this.Sheeps.Add(s));
                    this.Breeds.Clear();
                    this.Breeds.Add(new BreedBind { Name = defaultSelection });
                    breeds.ForEach(b => this.Breeds.Add(b));
                    this.Sheepfolds.Clear();
                    this.Sheepfolds.Add(new SheepfoldBind { Name = defaultSelection });
                    sheepfolds.ForEach(sf => this.Sheepfolds.Add(sf));
                    this.Principals.Clear();
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));
                    this.Operators.Clear();
                    this.Operators.Add(new UserBind { UserName = defaultSelection });
                    operators.ForEach(o => this.Operators.Add(o));
                    this.Reset();
                }), DispatcherPriority.Send, null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public SellSheepFilter Filter
        {
            get
            {
                SellSheepFilter filter = Mapper.Map<SellSheepFilter>(this);
                return filter;
            }
        }

        #region 搜索绑定字段

        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }

        private ObservableCollection<BreedBind> breeds = new ObservableCollection<BreedBind>();
        public ObservableCollection<BreedBind> Breeds { get { return breeds; } }

        private ObservableCollection<SheepfoldBind> sheepfolds = new ObservableCollection<SheepfoldBind>();
        public ObservableCollection<SheepfoldBind> Sheepfolds { get { return sheepfolds; } }


        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private ObservableCollection<UserBind> operators = new ObservableCollection<UserBind>();
        public ObservableCollection<UserBind> Operators { get { return operators; } }

        #endregion

        #region 搜索字段

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

        private string breedId;
        [EntityProperty]
        public string BreedId
        {
            get { return breedId; }
            set
            {
                breedId = value;
                this.RaisePropertyChanged("BreedId");
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

        private GrowthStageEnum? growthStage;
        [EntityProperty]
        public GrowthStageEnum? GrowthStage
        {
            get { return growthStage; }
            set
            {
                growthStage = value;
                this.RaisePropertyChanged("GrowthStage");
            }
        }

        private string sheepfoldId;
        [EntityProperty]
        public string SheepfoldId
        {
            get { return sheepfoldId; }
            set
            {
                sheepfoldId = value;
                this.RaisePropertyChanged("SheepfoldId");
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

        private string operatorId;
        [EntityProperty]
        public string OperatorId
        {
            get { return operatorId; }
            set
            {
                operatorId = value;
                this.RaisePropertyChanged("OperatorId");
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

        public IEnumerable<SellSheep> SellSheepList { get; set; }

        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }

            Action InitializeSheeps = () =>
            {
                int count;
                this.SellSheepList = this.Service.GetSellSheep(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<SellSheep>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.SellSheepList);
            };
            this.ProgressRing.Show();
            InitializeSheeps.BeginInvoke(ar => InitializeSheeps.EndInvoke(ar as IAsyncResult), InitializeSheeps);
        }

        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetSellSheep(this.Filter, rowCount).ToArray();
        }
    }
}
