using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.DiseaseControl;
using Chanyi.Shepherd.QueryModel.Model.DiseaseControl;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.DiseaseControl;

namespace Chanyi.Shepherd.WPF.ViewModels.DiseaseControl
{
    /// <summary>
    /// 防疫实施情况
    /// </summary>
    public class AntiepidemicViewModel : ListViewModel
    {
        public AntiepidemicViewModel(bool withinInitilization) { }

        public AntiepidemicViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;
            Mapper.CreateMap<AntiepidemicViewModel, AntiepidemicFilter>();
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
        public AntiepidemicFilter Filter
        {
            get
            {
                AntiepidemicFilter filter = Mapper.Map<AntiepidemicFilter>(this);
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

        private DateTime? startExecuteDate;
        [EntityProperty]
        public DateTime? StartExecuteDate
        {
            get { return startExecuteDate; }
            set
            {
                startExecuteDate = value;
                this.RaisePropertyChanged("StartExecuteDate");
            }
        }

        private DateTime? endExecuteDate;
        [EntityProperty]
        public DateTime? EndExecuteDate
        {
            get { return endExecuteDate; }
            set
            {
                endExecuteDate = value;
                this.RaisePropertyChanged("EndExecuteDate");
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

        public IEnumerable<Antiepidemic> AntiepidemicList { get; set; }
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
                this.AntiepidemicList = this.Service.GetAntiepidemic(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<Antiepidemic>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.AntiepidemicList);
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
                    EditAntiepidemicWindow win = new EditAntiepidemicWindow(id);
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
                    AddAntiepidemicWindow win = new AddAntiepidemicWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetAntiepidemic(this.Filter, rowCount).ToArray();
        }
    }
}
