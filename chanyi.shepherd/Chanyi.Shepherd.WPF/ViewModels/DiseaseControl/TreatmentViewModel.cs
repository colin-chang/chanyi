using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.DiseaseControl;
using Chanyi.Shepherd.QueryModel.Model.DiseaseControl;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.DiseaseControl;
using System.Collections.ObjectModel;

namespace Chanyi.Shepherd.WPF.ViewModels.DiseaseControl
{
    /// <summary>
    /// 疾病治疗
    /// </summary>
    public class TreatmentViewModel : ListViewModel
    {
        public TreatmentViewModel(bool withinInitilization) { }

        public TreatmentViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;
            Mapper.CreateMap<TreatmentViewModel, TreatmentFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var sheeps = this.Service.GetTreatmentSheepBind();
                var principals = this.Service.GetAllEmployeeBind();

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Sheeps.Clear();
                    this.Sheeps.Add(new SheepBind { SerialNumber = defaultSelection });
                    sheeps.ForEach(s => this.Sheeps.Add(s));
                    this.Principals.Clear();
                    this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Principals.Add(p));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
        public TreatmentFilter Filter
        {
            get
            {
                TreatmentFilter filter = Mapper.Map<TreatmentFilter>(this);
                return filter;
            }
        }

        #region 搜索条件


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

        private string symptom;
        [EntityProperty]
        public string Symptom
        {
            get { return symptom; }
            set
            {
                symptom = value;
                this.RaisePropertyChanged("Symptom");
            }
        }

        private string disease;
        [EntityProperty]
        public string Disease
        {
            get { return disease; }
            set
            {
                disease = value;
                this.RaisePropertyChanged("Disease");
            }
        }

        private string effect;
        [EntityProperty]
        public string Effect
        {
            get { return effect; }
            set
            {
                effect = value;
                this.RaisePropertyChanged("Effect");
            }
        }


        private DateTime? startStartDate;
        [EntityProperty]
        public DateTime? StartStartDate
        {
            get { return startStartDate; }
            set
            {
                startStartDate = value;
                this.RaisePropertyChanged("StartStartDate");
            }
        }

        private DateTime? endStartDate;
        [EntityProperty]
        public DateTime? EndStartDate
        {
            get { return endStartDate; }
            set
            {
                endStartDate = value;
                this.RaisePropertyChanged("EndStartDate");
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

        public IEnumerable<Treatment> TreatmentList { get; set; }
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
                this.TreatmentList = this.Service.GetTreatment(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                UIDispatcher.Invoke(new Action<IEnumerable<Treatment>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.TreatmentList);
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
                    EditTreatmentWindow win = new EditTreatmentWindow(id);
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
                    AddTreatmentWindow win = new AddTreatmentWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }
        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetTreatment(this.Filter, rowCount).ToArray();
        }
    }
}
