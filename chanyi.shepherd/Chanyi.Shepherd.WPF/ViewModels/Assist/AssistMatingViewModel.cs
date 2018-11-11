using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Configuration;
using System.IO;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.Model.Assist;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Assist;

namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    class AssistMatingViewModel : ListViewModel
    {
        public AssistMatingViewModel(bool withinInitilization) { }

        public AssistMatingViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            InitializeBindData();
        }

        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                this.Sheeps = this.Service.GetStudSheepBind();
                this.Sheeps.Insert(0, new SheepBind { SerialNumber = ConfigurationManager.AppSettings["formDefaultSelection"] });
                this.Reset();
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        private List<SheepBind> sheeps;
        public List<SheepBind> Sheeps
        {
            get { return sheeps; }
            set
            {
                sheeps = value;
                this.RaisePropertyChanged("Sheeps");
            }
        }

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

        //private int generations;
        //[EntityProperty]
        //public int Generations
        //{
        //    get { return generations; }
        //    set
        //    {
        //        generations = value;
        //        this.RaisePropertyChanged("Generations");
        //    }
        //}

        private List<object> generations = new List<object>();
        public List<object> Generations
        {
            get
            {
                if (generations.Count() <= 0)
                {
                    for (int i = 3; i < 6; i++)
                        generations.Add(new { Id = i, Name = i });
                }
                return generations;
            }
        }

        private int generation = 3;
        [EntityProperty]
        public int Generation
        {
            get { return generation; }
            set
            {
                generation = value;
                this.Validate("Generation");
            }
        }


        public bool HasAnyRecord
        {
            get
            {
                if (this.AssistMatingList == null)
                    return false;
                return this.AssistMatingList.Count() > 0;
            }
        }

        #region 列表数据
        public IEnumerable<AssistMating> AssistMatingList { get; set; }
        protected override void LoadData()
        {
            if (string.IsNullOrWhiteSpace(this.SheepId) || this.Sheeps.Where(s => s.Id == this.SheepId).Count() <= 0)
            {
                this.AssistMatingList = null;
                this.RaisePropertyChanged("HasAnyRecord");
                return;
            }

            Action InitializeMatings = () =>
            {
                this.AssistMatingList = this.Service.GetAssistMating(this.SheepId, this.Generation);
                this.RaisePropertyChanged("HasAnyRecord");
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Table.ItemsSource = AssistMatingList;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }));
            };
            this.ProgressRing.Show();
            InitializeMatings.BeginInvoke(ar => InitializeMatings.EndInvoke(ar as IAsyncResult), InitializeMatings);
        }
        #endregion

        public DelegateCommand MatingTestCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    TwoMatingWindow win = new TwoMatingWindow() { Owner = Application.Current.MainWindow };
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        public override DelegateCommand ExportCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (this.AssistMatingList == null || this.AssistMatingList.Count() <= 0)
                    {
                        MessageBox.Show(Application.Current.MainWindow, "没有任何数据，无法进行数据导出！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel 97-2003 工作簿(*.xls)|*.xls|Excel 工作簿(*.xlsx)|*.xlsx" };
                    if (sfd.ShowDialog() != true)
                        return;
                    Action export = () =>
                    {
                        var workbook = this.CreateExportSheet(this.AssistMatingList.Count(), System.IO.Path.GetExtension(sfd.FileName).ToLower() == ".xls" ? ExcelExtension.XLS : ExcelExtension.XLSX);
                        using (FileStream stream = System.IO.File.OpenWrite(sfd.FileName))
                        {
                            workbook.Write(stream);
                        }
                        this.UIDispatcher.Invoke(new Action(() => MessageBox.Show(Application.Current.MainWindow, "数据导出成功，成功导出" + this.AssistMatingList.Count() + "条数据！", "消息", MessageBoxButton.OK, MessageBoxImage.Information)), null);
                    };
                    export.BeginInvoke(ar => export.EndInvoke(ar as IAsyncResult), export);
                });
            }
        }

        protected override Array GetExportData(int rowCount)
        {
            return this.AssistMatingList.ToArray();
        }
    }
}
