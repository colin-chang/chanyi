using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Multiplying;
using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Multiplying;
using Chanyi.Shepherd.WPF.Model;


namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class AbortionViewModel : ListViewModel
    {
        public AbortionViewModel(bool withinInitilization)
        {
        }

        public AbortionViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;

            Mapper.CreateMap<AbortionViewModel, AbortionFilter>();
            Mapper.CreateMap<Abortion, AbortionData>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var females = this.Service.GetAbortionSheepSelectBind();
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Females.Clear();
                    this.Principals.Clear();
                    this.Females.Add(new SheepBind { SerialNumber = defaultSelection });
                    females.ForEach(f => this.Females.Add(f));
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public AbortionFilter Filter
        {
            get
            {
                AbortionFilter filter = Mapper.Map<AbortionFilter>(this);
                return filter;
            }
        }

        private ObservableCollection<SheepBind> females = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Females { get { return females; } }

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
                this.Validate("SheepId");
            }
        }

        private DateTime? startAbortionDate;
        [EntityProperty]
        public DateTime? StartAbortionDate
        {
            get { return startAbortionDate; }
            set
            {
                startAbortionDate = value;
                this.RaisePropertyChanged("StartAbortionDate");
            }
        }

        private DateTime? endAbortionDate;
        [EntityProperty]
        public DateTime? EndAbortionDate
        {
            get { return endAbortionDate; }
            set
            {
                endAbortionDate = value;
                this.RaisePropertyChanged("EndAbortionDate");
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
        public IEnumerable<AbortionData> AbortionList { get; set; }
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
                this.AbortionList = this.Service.GetAbortion(this.Filter, this.PageIndex, this.PageSize, out count).Select(s => Mapper.Map<AbortionData>(s));

                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<AbortionData>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.AbortionList);
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
                    EditAbortionWindow win = new EditAbortionWindow(id);
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
                    this.Service.DeleteAbortion(id);
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
                    AddAbortionWindow win = new AddAbortionWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            this.AbortionList = this.Service.GetAbortion(this.Filter, rowCount).Select(s => Mapper.Map<AbortionData>(s));
            return this.AbortionList.ToArray();
        }
    }
}
