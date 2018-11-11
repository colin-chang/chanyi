using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Group;
using Chanyi.Shepherd.QueryModel.Model.Group;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.GroupManage;

namespace Chanyi.Shepherd.WPF.ViewModels.GroupManage
{
    class DeathManageViewModel : ListViewModel
    {
        public DeathManageViewModel(bool withinInitilization) { }

        public DeathManageViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;
            Mapper.CreateMap<DeathManageViewModel, DeathManageFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var sheeps = this.Service.GetSheepBind(new QueryModel.BindingFilter.SheepBindFilter { Status = SheepStatusEnum.Dead });
                var breeds = this.Service.GetBreedBind();
                var sheepfolds = this.Service.GetSheepfoldBind();
                var principals = this.Service.GetAllEmployeeBind();
                this.UIDispatcher.Invoke(new Action(()=> {
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
                this.Reset();
                }),null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
        public DeathManageFilter Filter
        {
            get
            {
                DeathManageFilter filter = Mapper.Map<DeathManageFilter>(this);
                return filter;
            }
        }

        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }


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

        private ObservableCollection<BreedBind> breeds = new ObservableCollection<BreedBind>();
        public ObservableCollection<BreedBind> Breeds { get { return breeds; } }


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

        private ObservableCollection<SheepfoldBind> sheepfolds = new ObservableCollection<SheepfoldBind>();
        public ObservableCollection<SheepfoldBind> Sheepfolds { get { return sheepfolds; } }

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

        private DeathDisposeEnum? dispose;
        [EntityProperty]
        public DeathDisposeEnum? Dispose
        {
            get { return dispose; }
            set
            {
                dispose = value;
                this.RaisePropertyChanged("Dispose");
            }
        }

        private DateTime? startDeathDate;
        [EntityProperty]
        public DateTime? StartDeathDate
        {
            get { return startDeathDate; }
            set
            {
                startDeathDate = value;
                this.RaisePropertyChanged("StartDeathDate");
            }
        }

        private DateTime? endDeathDate;
        [EntityProperty]
        public DateTime? EndDeathDate
        {
            get { return endDeathDate; }
            set
            {
                endDeathDate = value;
                this.RaisePropertyChanged("EndDeathDate");
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
        public IEnumerable<DeathManage> DeathManageList { get; set; }
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
                this.DeathManageList = this.Service.GetDeathManage(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<DeathManage>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.DeathManageList);
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
                    EditDeathManageWindow win = new EditDeathManageWindow(id);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        this.LoadData();
                });
            }
        }

        public DelegateCommand<string> RemoveCommand
        {
            get
            {
                return this.GetRemoveCommand(id =>
                {
                    this.Service.DeleteDeathManage(id);
                    this.LoadData();
                });
            }
        }
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddDeathManageWindow win = new AddDeathManageWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetDeathManage(this.Filter, rowCount).ToArray();
        }
    }
}
