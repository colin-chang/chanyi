using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.QueryModel.Filter.Formula;
using Chanyi.Shepherd.QueryModel.Model.Formula;
using Chanyi.Shepherd.WPF.Views.Assist;
using Chanyi.Shepherd.QueryModel.BindingModel;
using NPOI.HSSF.UserModel;
using Microsoft.Win32;
using System.IO;
using NPOI.SS.Util;

namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    class FormulaViewModel : ListViewModel
    {
        public FormulaViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            Mapper.CreateMap<FormulaViewModel, FormulaFilter>();
            InitializeBindData();
        }

        public FormulaViewModel(bool withinInitilization) { }

        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                this.Principals = this.Service.GetEmployeeBind();
                this.Principals.Insert(0, new EmployeeBind { Name = defaultSelection });

                this.Reset();
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public FormulaFilter Filter
        {
            get
            {
                FormulaFilter filter = Mapper.Map<FormulaFilter>(this);
                return filter;
            }
        }

        #region 搜索绑定字段

        private List<EmployeeBind> principals;

        public List<EmployeeBind> Principals
        {
            get { return principals; }
            set
            {
                principals = value;
                this.RaisePropertyChanged("Principals");
            }
        }


        #endregion

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

        private string applyTo;
        [EntityProperty]
        public string ApplyTo
        {
            get { return applyTo; }
            set
            {
                applyTo = value;
                this.RaisePropertyChanged("ApplyTo");
            }
        }

        private string sideEffect;
        [EntityProperty]
        public string SideEffect
        {
            get { return sideEffect; }
            set
            {
                sideEffect = value;
                this.RaisePropertyChanged("SideEffect");
            }
        }

        private bool? isEnable;
        [EntityProperty]
        public bool? IsEnable
        {
            get { return isEnable; }
            set
            {
                isEnable = value;
                this.RaisePropertyChanged("IsEnable");
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

        public override int PageSize
        {
            get
            {
                return 20;
            }
        }

        public ObservableCollection<Formula> Formulas { get; set; }

        private List<FormulaFeed> formulaDetails;

        public List<FormulaFeed> FormulaDetails
        {
            get { return formulaDetails; }
            set
            {
                formulaDetails = value;
                this.RaisePropertyChanged("FormulaDetails");
            }
        }

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
                this.Formulas = new ObservableCollection<Formula>(this.Service.GetFormula(this.Filter, this.PageIndex, this.PageSize, out count));
                this.TotalCount = count;
                if (this.TotalCount > 0)
                    //this.FormulaDetails =this.Service.GetFormulaFeed(this.Formulas.FirstOrDefault().Id);
                    this.FormulaDetails = this.Service.GetFormulaFeedById(this.Formulas.FirstOrDefault().Id);
                this.UIDispatcher.Invoke(new Action<IEnumerable<Formula>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.ProgressRing.Hide();
                }), this.Formulas);
            };
            this.ProgressRing.Show();
            InitializeSheeps.BeginInvoke(ar => InitializeSheeps.EndInvoke(ar as IAsyncResult), InitializeSheeps);
        }

        public DelegateCommand<string> EditCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    this.ProgressRing.Show();
                    var current = this.Formulas.Where(f => f.Id == id).FirstOrDefault();
                    if (current == null)
                        return;

                    current.IsEnable = !current.IsEnable;
                    var result = this.Service.UpdateFormulaStatus(current.IsEnable, id);
                    if (!result.Result)
                    {
                        MessageBox.Show(result.Message, "服务异常", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                    this.Table.ItemsSource = from f in this.Formulas orderby f.IsEnable descending, f.CreateTime descending select f;
                    this.ProgressRing.Hide();
                });
            }
        }

        public DelegateCommand<string> SelectFormulaCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    this.FormulaDetails = this.Service.GetFormulaFeedById(id);
                });
            }
        }

        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddCustomFormulaWindow win = new AddCustomFormulaWindow { Owner = Application.Current.MainWindow };
                    if (win.ShowDialog() == true)
                        this.LoadData();
                });
            }
        }

        public override DelegateCommand ExportCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (this.Table.SelectedItem == null)
                    {
                        MessageBox.Show(Application.Current.MainWindow, "没有任何选中数据，无法进行数据导出！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel 97-2003 工作簿(*.xls)|*.xls|Excel 工作簿(*.xlsx)|*.xlsx" };
                    if (sfd.ShowDialog() != true)
                        return;
                    Action export = () =>
                    {
                        var workbook = this.CreateExportSheet(1, System.IO.Path.GetExtension(sfd.FileName).ToLower() == ".xls" ? ExcelExtension.XLS : ExcelExtension.XLSX);
                        using (FileStream stream = System.IO.File.OpenWrite(sfd.FileName))
                        {
                            workbook.Write(stream);
                        }
                        this.UIDispatcher.Invoke(new Action(() => MessageBox.Show(Application.Current.MainWindow, "配方数据导出成功", "消息", MessageBoxButton.OK, MessageBoxImage.Information)), null);
                    };
                    export.BeginInvoke(ar => export.EndInvoke(ar as IAsyncResult), export);
                });
            }
        }

        protected override IWorkbook CreateExportSheet(int rowCount, ExcelExtension ext)
        {
            return this.UIDispatcher.Invoke(new Func<IWorkbook>(() =>
            {
                IWorkbook workbook = ext == ExcelExtension.XLS ? (IWorkbook)new HSSFWorkbook() : new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(this.Header);
                var firstRow = sheet.CreateRow(0);
                firstRow.CreateCell(0, CellType.String).SetCellValue("名称");
                firstRow.CreateCell(1, CellType.String).SetCellValue("适用于");
                firstRow.CreateCell(2, CellType.String).SetCellValue("不良反应");
                firstRow.CreateCell(3, CellType.String).SetCellValue("制作人");
                firstRow.CreateCell(4, CellType.String).SetCellValue("饲料名称");
                firstRow.CreateCell(5, CellType.String).SetCellValue("饲料产地");
                firstRow.CreateCell(6, CellType.String).SetCellValue("饲料类型");
                firstRow.CreateCell(7, CellType.String).SetCellValue("用量");

                for (int i = 0; i < this.FormulaDetails.Count(); i++)
                {
                    var ff = this.FormulaDetails[i];
                    var row = sheet.CreateRow(i + 1);
                    row.CreateCell(4, CellType.String).SetCellValue(ff.Name);
                    row.CreateCell(5, CellType.String).SetCellValue(ff.Area);
                    row.CreateCell(6, CellType.String).SetCellValue(ff.Type);
                    row.CreateCell(7, CellType.Numeric).SetCellValue(ff.Amount);
                }
                Formula formula = (this.Table.SelectedItem as Formula);
                var srow = sheet.GetRow(1);
                ICellStyle style = workbook.CreateCellStyle();
                style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                srow.CreateCell(0, CellType.String).SetCellValue(formula.Name);
                srow.CreateCell(1, CellType.String).SetCellValue(formula.ApplyTo);
                srow.CreateCell(2, CellType.String).SetCellValue(formula.SideEffect);
                srow.CreateCell(3, CellType.String).SetCellValue(formula.PrincipalName);
                srow.Cells.ForEach(c => c.CellStyle = style);
                sheet.AddMergedRegion(new CellRangeAddress(1, this.FormulaDetails.Count(), 0, 0));
                sheet.AddMergedRegion(new CellRangeAddress(1, this.FormulaDetails.Count(), 1, 1));
                sheet.AddMergedRegion(new CellRangeAddress(1, this.FormulaDetails.Count(), 2, 2));
                sheet.AddMergedRegion(new CellRangeAddress(1, this.FormulaDetails.Count(), 3, 3));


                return workbook;
            }), null) as IWorkbook;
        }

        public static void SetCellRangeAddress(ISheet sheet, int rowstart, int rowend, int colstart, int colend)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
            sheet.AddMergedRegion(cellRangeAddress);
        }
    }
}
