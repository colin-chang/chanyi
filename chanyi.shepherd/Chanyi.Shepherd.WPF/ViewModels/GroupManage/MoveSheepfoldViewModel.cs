using System;
using System.Collections.Generic;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Group;
using Chanyi.Shepherd.QueryModel.Model.Group;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.GroupManage;
using System.Windows;
using System.Collections.ObjectModel;

namespace Chanyi.Shepherd.WPF.ViewModels.GroupManage
{
    class MoveSheepfoldViewModel : ListViewModel
    {
        public MoveSheepfoldViewModel(bool withinInitilization) { }

        public MoveSheepfoldViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<MoveSheepfoldViewModel, MoveSheepfoldFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var sheeps = this.Service.GetSheepBind(null);
                var sheepfolds = this.Service.GetSheepfoldBind();
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() => {
                    this.Sheeps.Clear();
                    this.Sheeps.Add(new SheepBind { SerialNumber = defaultSelection });
                    sheeps.ForEach(s => this.Sheeps.Add(s));
                    this.Sheepfolds.Clear();
                    this.Sheepfolds.Add(new SheepfoldBind { Name = defaultSelection });
                    sheepfolds.ForEach(sf => this.Sheepfolds.Add(sf));
                    this.Principals.Clear();
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public MoveSheepfoldFilter Filter
        {
            get
            {
                MoveSheepfoldFilter filter = Mapper.Map<MoveSheepfoldFilter>(this);
                return filter;
            }
        }

        private ObservableCollection<SheepfoldBind> sheepfolds = new ObservableCollection<SheepfoldBind>();
        public ObservableCollection<SheepfoldBind> Sheepfolds { get { return sheepfolds; } }

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

        private string sourceSheepfoldId;
        [EntityProperty]
        public string SourceSheepfoldId
        {
            get { return sourceSheepfoldId; }
            set
            {
                sourceSheepfoldId = value;
                this.RaisePropertyChanged("SourceSheepfoldId");
            }
        }

        private string destinationSheepfoldId;
        [EntityProperty]
        public string DestinationSheepfoldId
        {
            get { return destinationSheepfoldId; }
            set
            {
                destinationSheepfoldId = value;
                this.RaisePropertyChanged("DestinationSheepfoldId");
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
        public IEnumerable<MoveSheepfold> MoveSheepfoldList { get; set; }
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
                this.MoveSheepfoldList = this.Service.GetMoveSheepfold(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<MoveSheepfold>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.MoveSheepfoldList);
            };

            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddMoveSheepfoldWindow win = new AddMoveSheepfoldWindow();
                    win.Owner = Application.Current.MainWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetMoveSheepfold(this.Filter, rowCount).ToArray();
        }
    }
}
