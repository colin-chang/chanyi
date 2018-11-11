using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Breeding;
using Chanyi.Shepherd.QueryModel.Model.Breeding;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Breeding;
using Chanyi.Shepherd.WPF.Model.AssessData;

namespace Chanyi.Shepherd.WPF.ViewModels.Breeding
{
    class FirstAssessViewModel:ListViewModel
    {
        public FirstAssessViewModel(bool withinInitilization) { }

        public FirstAssessViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<FirstAssessViewModel, FirstAssessFilter>();
            Mapper.CreateMap<FirstAssess, FirstAssessData>();
            InitializeBindData();
        }
        public FirstAssessViewModel(string header, string icon, string intro, string _editPermUrl)
            :this(header,icon,intro)
        {
            this.editPermUrl = _editPermUrl;
        }

        protected override void InitializeBindData()
        {

            Action Initialize = () =>
            {
                var sheeps = this.Service.GetFirstAssessSheepSelectBind();
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
        public FirstAssessFilter Filter
        {
            get
            {
                FirstAssessFilter filter = Mapper.Map<FirstAssessFilter>(this);
                return filter;
            }
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

        private string maxWeight;
        [EntityProperty]
        [FloatNumber(0, 100, IsNullable = true, ErrorMessage = "最大重量不合法")]
        public string MaxWeight
        {
            get { return maxWeight; }
            set
            {
                maxWeight = value;
                this.Validate("MaxWeight");
            }
        }

        private string minWeight;
        [EntityProperty]
        [FloatNumber(0, 100, IsNullable = true, ErrorMessage = "最小重量不合法")]
        public string MinWeight
        {
            get { return minWeight; }
            set
            {
                minWeight = value;
                this.Validate("MinWeight");
            }
        }

        private string maxHabitusScore;

        [EntityProperty]
        [FloatNumber(0, 100, IsNullable = true, ErrorMessage = "最大体型评分不合法")]

        public string MaxHabitusScore
        {
            get { return maxHabitusScore; }
            set
            {
                maxHabitusScore = value;
                this.Validate("MaxHabitusScore");
            }
        }

        private string minHabitusScore;
        [EntityProperty]
        [FloatNumber(0, 100, IsNullable = true, ErrorMessage = "最小体评分型不合法")]
        public string MinHabitusScore
        {
            get { return minHabitusScore; }
            set
            {
                minHabitusScore = value;
                this.Validate("MinHabitusScore");
            }
        }

        private DateTime? startAssessDate;
        [EntityProperty]
        public DateTime? StartAssessDate
        {
            get { return startAssessDate; }
            set
            {
                startAssessDate = value;
                this.RaisePropertyChanged("StartAssessDate");
            }
        }
        private DateTime? endAssessDate;
        [EntityProperty]
        public DateTime? EndAssessDate
        {
            get { return endAssessDate; }
            set
            {
                endAssessDate = value;
                this.RaisePropertyChanged("EndAssessDate");
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

        #region 列表数据
        public IEnumerable<FirstAssessData> FirstAssessList { get; set; }
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
                this.FirstAssessList = this.Service.GetFirstAssess(this.Filter, this.PageIndex, this.PageSize, out count).Select(a => Mapper.Map<FirstAssessData>(a));
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<FirstAssess>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), FirstAssessList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);

        } 
        #endregion

        //public DelegateCommand<string> EditCommand
        //{
        //    get
        //    {
        //        return this.GetEditCommand(id =>
        //        {
        //            EditFirstAssessWindow win = new EditFirstAssessWindow(id);
        //            win.Owner = CurrentWindow;
        //            if (win.ShowDialog()==true)
        //            {
        //                this.LoadData();
        //            }
        //        });
        //    }
        //}

        //public DelegateCommand AddCommand
        //{
        //    get
        //    {
        //        return new DelegateCommand(() =>
        //        {
        //            AddFirstAssessWindow win = new AddFirstAssessWindow();
        //            win.Owner = CurrentWindow;
        //            if (win.ShowDialog() == true)
        //                LoadData();
        //        });
        //    }
        //}
        protected override Array GetExportData(int rowCount)
        {
            this.FirstAssessList = this.Service.GetFirstAssess(this.Filter, rowCount).Select(a => Mapper.Map<FirstAssessData>(a));
            return this.FirstAssessList.ToArray();
        }
    }
}
