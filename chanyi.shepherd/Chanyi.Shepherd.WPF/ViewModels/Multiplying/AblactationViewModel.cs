using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Multiplying;
using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Multiplying;
using Chanyi.Shepherd.WPF.Model;
using System.Collections.ObjectModel;

namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class AblactationViewModel : ListViewModel
    {
        public AblactationViewModel(bool withinInitilization) { }

        public AblactationViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;
            Mapper.CreateMap<AblactationViewModel, AblactationFilter>();
            Mapper.CreateMap<Ablactation, AblactationData>();
            InitializeBindData();
        }

        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var sheeps = this.Service.GetMatingSheepSelectBind();
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Sheeps.Clear();
                    this.Principals.Clear();
                    this.Sheeps.Add(new SheepBind { SerialNumber = defaultSelection });
                    sheeps.ForEach(s => this.Sheeps.Add(s));
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public AblactationFilter Filter
        {
            get
            {
                AblactationFilter filter = Mapper.Map<AblactationFilter>(this);
                return filter;
            }
        }

        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }

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
        private DateTime? startAblactationDay;
        [EntityProperty]
        public DateTime? StartAblactationDay
        {
            get { return startAblactationDay; }
            set
            {
                startAblactationDay = value;
                this.RaisePropertyChanged("StartAblactationDay");
            }
        }

        private DateTime? endAblactationDay;
        [EntityProperty]
        public DateTime? EndAblactationDay
        {
            get { return endAblactationDay; }
            set
            {
                endAblactationDay = value;
                this.RaisePropertyChanged("EndAblactationDay");
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

        #region 列表
        public IEnumerable<AblactationData> AblactationList { get; set; }
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
                this.AblactationList = this.Service.GetAblactation(this.Filter, this.PageIndex, this.PageSize, out count).Select(s => Mapper.Map<AblactationData>(s));

                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<AblactationData>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.AblactationList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);

        }
        #endregion

        public DelegateCommand<string> EditCommand
        {
            get
            {
                return this.GetEditCommand(id =>
                {
                    EditAblactationWindow win = new EditAblactationWindow(id);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
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
                    AddAblactationWindow win = new AddAblactationWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            this.AblactationList = this.Service.GetAblactation(this.Filter, rowCount).ToArray().Select(s => Mapper.Map<AblactationData>(s));
            return this.AblactationList.ToArray();
        }
    }
}
