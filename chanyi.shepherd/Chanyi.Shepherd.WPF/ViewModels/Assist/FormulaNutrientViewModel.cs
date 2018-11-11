using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.Filter.Formula;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.QueryModel.Model.Formula;
using Chanyi.Shepherd.WPF.Views.Assist;
using Chanyi.Shepherd.WPF.Model;

namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    class FormulaNutrientViewModel : ListViewModel
    {
        public FormulaNutrientViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<FormulaNutrientViewModel, FormulaNutrientFilter>();
            InitializeBindData();
        }

        public FormulaNutrientViewModel(bool withinInitilization) { }

        protected override void InitializeBindData()
        {
            this.Reset();
        }

        public FormulaNutrientFilter Filter
        {
            get
            {
                var filter = Mapper.Map<FormulaNutrientFilter>(this);
                return filter;
            }
        }

        #region 搜索字段

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

        private string dailyGain;
        [EntityProperty]
        [FloatNumber(0, 5, ErrorMessage = "日增重不合法", IsNullable = true)]
        public string DailyGain
        {
            get { return dailyGain; }
            set
            {
                dailyGain = value;
                this.Validate("DailyGain");
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

        public override int PageSize
        {
            get
            {
                return 28;
            }
        }

        public ObservableCollection<FormulaNutrient> FormulaNutrients { get; set; }

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
                this.FormulaNutrients = new ObservableCollection<FormulaNutrient>(this.Service.GetFormulaNutrient(this.Filter, this.PageIndex, this.PageSize, out count));
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<FormulaNutrient>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.FormulaNutrients);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        //AddCommand与EditCommand共用一个View,无法应用原始的Add与Edit
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    FormulaNutrientWindow win = new FormulaNutrientWindow(false) { Owner = Application.Current.MainWindow };
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        public DelegateCommand<string> EditCommand
        {
            get
            {
                return this.GetEditCommand(id =>
                {
                    FormulaNutrientWindow win = new FormulaNutrientWindow(true, id) { Owner = Application.Current.MainWindow };
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        public DelegateCommand<string> AddFormulaCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    AddFormulaWindow win = new AddFormulaWindow(id) { Owner = Application.Current.MainWindow };
                    win.ShowDialog();
                    var result = (win.DataContext as AddFormulaViewModel).ActionResult;
                    if (result == ActionResultEnum.Error)
                        MessageBox.Show(this.CurrentWindow, "抱歉，配方添加失败，请稍后重试或联系技术人员！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (result == ActionResultEnum.OK)
                        MessageBox.Show(this.CurrentWindow, "配方添加成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        public DelegateCommand AddCustomFormulaCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddCustomFormulaWindow win = new AddCustomFormulaWindow { Owner = Application.Current.MainWindow };
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetFormulaNutrient(this.Filter, rowCount).ToArray();
        }
    }
}
