using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows;
using System.Reflection;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;
using Chanyi.Shepherd.WPF.Views.BaseInfo;
using Chanyi.Shepherd.QueryModel.Filter.BaseInfo;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;


namespace Chanyi.Shepherd.WPF.ViewModels.BaseInfo
{
    class SheepViewModel : ListViewModel
    {
        public SheepViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;

            Mapper.CreateMap<SheepViewModel, SheepFilter>();
            InitializeBindData();
        }

        public SheepViewModel(bool withinInitilization) { }

        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var allsheeps = this.Service.GetSheepBind(null);
                var studSheeps = this.Service.GetSheepParentBind(null);
                SheepBind defaultSheep = new SheepBind { SerialNumber = defaultSelection };
                var breeds = this.Service.GetBreedBind();
                var sheepfolds = this.Service.GetSheepfoldBind();
                var principals = this.Service.GetAllEmployeeBind();
                var operators = this.Service.GetUserBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Sheeps.Clear();
                    this.Sheeps.Add(defaultSheep);
                    this.Fathers.Clear();
                    this.Fathers.Add(defaultSheep);
                    this.Mothers.Clear();
                    this.Mothers.Add(defaultSheep);
                    allsheeps.ForEach(s =>
                    {
                        this.Sheeps.Add(s);
                    });
                    studSheeps.ForEach(s =>
                    {
                        if (s.Gender == GenderEnum.Male)
                            this.Fathers.Add(s);
                        else
                            this.Mothers.Add(s);
                    });
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

        public SheepFilter Filter
        {
            get
            {
                SheepFilter filter = Mapper.Map<SheepFilter>(this);
                return filter;
            }
        }

        #region 搜索绑定字段

        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }

        private ObservableCollection<BreedBind> breeds = new ObservableCollection<BreedBind>();
        public ObservableCollection<BreedBind> Breeds { get { return breeds; } }

        private List<object> compatriotNumbers = new List<object>();
        public List<object> CompatriotNumbers
        {
            get
            {
                if (compatriotNumbers.Count() <= 0)
                {
                    compatriotNumbers.Add(new { Name = defaultSelection });
                    for (int i = 0; i < 10; i++)
                        compatriotNumbers.Add(new { Id = i, Name = i });
                }
                return compatriotNumbers;
            }
        }

        private ObservableCollection<SheepfoldBind> sheepfolds = new ObservableCollection<SheepfoldBind>();
        public ObservableCollection<SheepfoldBind> Sheepfolds { get { return sheepfolds; } }

        private ObservableCollection<SheepBind> mothers = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Mothers { get { return mothers; } }

        private ObservableCollection<SheepBind> fathers = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Fathers { get { return fathers; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private ObservableCollection<UserBind> operators = new ObservableCollection<UserBind>();
        public ObservableCollection<UserBind> Operators { get { return operators; } }

        #endregion

        #region 搜索字段

        private string id;
        [EntityProperty]
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                this.RaisePropertyChanged("Id");
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

        private OriginEnum? origin;
        [EntityProperty]
        public OriginEnum? Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                this.RaisePropertyChanged("Origin");
            }
        }

        private string minBirthWeight;
        [EntityProperty]
        [FloatNumber(0, 100, IsNullable = true, ErrorMessage = "最小初生重不合法")]
        public string MinBirthWeight
        {
            get { return minBirthWeight; }
            set
            {
                minBirthWeight = value;
                this.Validate("MinBirthWeight");
            }
        }

        private string maxBirthWeight;
        [EntityProperty]
        [FloatNumber(0, 100, IsNullable = true, ErrorMessage = "最大初生重不合法")]
        public string MaxBirthWeight
        {
            get { return maxBirthWeight; }
            set
            {
                maxBirthWeight = value;
                this.Validate("MaxBirthWeight");
            }
        }

        private int? compatriotNumber;
        [EntityProperty]
        public int? CompatriotNumber
        {
            get { return compatriotNumber; }
            set
            {
                compatriotNumber = value;
                this.RaisePropertyChanged("CompatriotNumber");
            }
        }

        private DateTime? startBirthDay;
        [EntityProperty]
        public DateTime? StartBirthDay
        {
            get { return startBirthDay; }
            set
            {
                startBirthDay = value;
                this.RaisePropertyChanged("StartBirthDay");
            }
        }

        private DateTime? endBirthDay;
        [EntityProperty]
        public DateTime? EndBirthDay
        {
            get { return endBirthDay; }
            set
            {
                endBirthDay = value;
                this.RaisePropertyChanged("EndBirthDay");
            }
        }

        private string minAblactationWeight;
        [EntityProperty]
        [FloatNumber(0, 100, IsNullable = true, ErrorMessage = "最小断奶重不合法")]
        public string MinAblactationWeight
        {
            get { return minAblactationWeight; }
            set
            {
                minAblactationWeight = value;
                this.Validate("MinAblactationWeight");
            }
        }

        private string maxAblactationWeight;
        [EntityProperty]
        [FloatNumber(0, 100, IsNullable = true, ErrorMessage = "最大断奶重不合法")]
        public string MaxAblactationWeight
        {
            get { return maxAblactationWeight; }
            set
            {
                maxAblactationWeight = value;
                this.Validate("MaxAblactationWeight");
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

        private SheepStatusEnum? status;
        [EntityProperty]
        public SheepStatusEnum? Status
        {
            get { return status; }
            set
            {
                status = value;
                this.RaisePropertyChanged("Status");
            }
        }

        private string fatherId;
        [EntityProperty]
        public string FatherId
        {
            get { return fatherId == string.Empty ? null : fatherId; }
            set
            {
                fatherId = value;
                this.RaisePropertyChanged("FatherId");
            }
        }

        private string motherId;
        [EntityProperty]
        public string MotherId
        {
            get { return motherId == string.Empty ? null : motherId; }
            set
            {
                motherId = value;
                this.RaisePropertyChanged("MotherId");
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

        public IEnumerable<Sheep> SheepList { get; set; }

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
                this.SheepList = this.Service.GetSheep(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<Sheep>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.SheepList);
            };
            this.ProgressRing.Show();
            InitializeSheeps.BeginInvoke(ar => InitializeSheeps.EndInvoke(ar as IAsyncResult), InitializeSheeps);
        }

        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetSheep(this.Filter, rowCount).ToArray();
        }
    }
}