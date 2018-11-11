using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Multiplying;
using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Multiplying;


namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class MatingViewModel : ListViewModel
    {
        public MatingViewModel(bool withinInitilization) { }

        public MatingViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;

            Mapper.CreateMap<MatingViewModel, MatingFilter>();
            InitializeBindData();
        }

        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var allsheeps = this.Service.GetMatingSheepSelectBind();
                var males = allsheeps.Where(s => s.Gender == GenderEnum.Male).ToList();
                var females = allsheeps.Where(s => s.Gender == GenderEnum.Female).ToList();
                var breeds = this.Service.GetBreedBind();
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Males.Clear();
                    this.Females.Clear();
                    this.Breeds.Clear();
                    this.Principals.Clear();

                    this.Males.Add(new SheepBind { SerialNumber = defaultSelection });
                    males.ForEach(m => this.Males.Add(m));
                    this.Females.Add(new SheepBind { SerialNumber = defaultSelection });
                    females.ForEach(f => this.Females.Add(f));
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

        public MatingFilter Filter
        {
            get
            {
                MatingFilter filter = Mapper.Map<MatingFilter>(this);
                return filter;
            }
        }

        #region 数据源
        private ObservableCollection<SheepBind> males = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Males { get { return males; } }

        private ObservableCollection<SheepBind> females = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Females { get { return females; } }

        private ObservableCollection<BreedBind> breeds = new ObservableCollection<BreedBind>();
        public ObservableCollection<BreedBind> Breeds { get { return breeds; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }


        private string maleId;
        [EntityProperty]
        public string MaleId
        {
            get { return maleId; }
            set
            {
                maleId = value;
                this.RaisePropertyChanged("MaleId");
            }
        }

        private string femaleId;
        [EntityProperty]
        public string FemaleId
        {
            get { return femaleId; }
            set
            {
                femaleId = value;
                this.RaisePropertyChanged("FemaleId");
            }
        }

        private string mBreedId;

        public string MBreedId
        {
            get { return mBreedId; }
            set
            {
                mBreedId = value;
                this.RaisePropertyChanged("MBreedId");
            }
        }

        private string fBreedId;

        public string FBreedId
        {
            get { return fBreedId; }
            set
            {
                fBreedId = value;
                this.RaisePropertyChanged("FBreedId");
            }
        }

        private DateTime? startMatingDate;
        [EntityProperty]
        public DateTime? StartMatingDate
        {
            get { return startMatingDate; }
            set
            {
                startMatingDate = value;
                this.RaisePropertyChanged("StartMatingDate");
            }
        }

        private DateTime? endMatingDate;
        [EntityProperty]
        public DateTime? EndMatingDate
        {
            get { return endMatingDate; }
            set
            {
                endMatingDate = value;
                this.RaisePropertyChanged("EndMatingDate");
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
        public IEnumerable<Mating> MatingList { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }

            Action InitializeMatings = () =>
            {
                int count;
                this.MatingList = this.Service.GetMating(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<Mating>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.MatingList);
            };
            this.ProgressRing.Show();
            InitializeMatings.BeginInvoke(ar => InitializeMatings.EndInvoke(ar as IAsyncResult), InitializeMatings);
        }
        #endregion

        public DelegateCommand<string> EditCommand
        {
            get
            {
                return this.GetEditCommand(id =>
                {
                    EditMatingWindow win = new EditMatingWindow(id);
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
                    this.Service.DeleteMating(id);
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
                    AddMatingWindow win = new AddMatingWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetMating(this.Filter, rowCount).ToArray();
        }
    }
}
