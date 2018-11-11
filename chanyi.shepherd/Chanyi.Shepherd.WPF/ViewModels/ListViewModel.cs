using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Data;
using System.IO;

using System.Reflection;

using Microsoft.Practices.Prism.Commands;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

using Chanyi.Shepherd.WPF.UserControls;
using Chanyi.Shepherd.WPF.Views.File;
using Chanyi.Shepherd.WPF.Expands.AttcahProperty;


namespace Chanyi.Shepherd.WPF.ViewModels
{
    /// <summary>
    /// 列表页面ViewModel
    /// </summary>
    public abstract class ListViewModel : BaseViewModel
    {
        protected string defaultSelection = ConfigurationManager.AppSettings["listDefaultSelection"];
        private int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);

        #region 标题栏
        public string Icon { get; set; }

        public string Header { get; set; }

        public string Intro { get; set; }
        #endregion

        /// <summary>
        /// 编辑权限路径
        /// </summary>
        protected string editPermUrl;

        /// <summary>
        /// 列表控件
        /// </summary>
        public DataGrid Table { get; set; }

        /// <summary>
        /// ProgressRing
        /// </summary>
        public ProgressRing ProgressRing { get { return this.Table.Tag as ProgressRing; } }

        private int pageIndex;

        public int PageIndex
        {
            get
            {
                return pageIndex < 1 ? 1 : pageIndex;
            }
            set
            {
                pageIndex = value;
                this.RaisePropertyChanged("PageIndex");
            }
        }

        public virtual int PageSize { get { return pageSize; } }

        protected int totalCount;

        public int TotalCount
        {
            get { return totalCount; }
            set
            {
                totalCount = value;
                this.RaisePropertyChanged("TotalCount");
            }
        }

        /// <summary>
        /// 加载列表数据
        /// </summary>
        protected abstract void LoadData();

        protected abstract override void InitializeBindData();

        /// <summary>
        /// 编辑数据命令
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected DelegateCommand<string> GetEditCommand(Action<string> action)
        {
            return new DelegateCommand<string>(id =>
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    MessageBox.Show("请选择要编辑的数据！", "警告", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;
                }
                action(id);
            });
        }

        /// <summary>
        /// 分配权限命令
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected DelegateCommand<string> GetGrantCommand(Action<string> action)
        {
            return new DelegateCommand<string>(id =>
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    MessageBox.Show("请选择要分配权限的用户或角色！", "警告", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;
                }
                action(id);
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public DelegateCommand<string> GetRemoveCommand(Action<string> action)
        {
            return new DelegateCommand<string>(id =>
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    MessageBox.Show("请选择要删除的数据！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (MessageBox.Show("确定要删除此条数据？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    action(id);
                }
            });
        }

        /// <summary>
        /// 搜索命令
        /// </summary>
        public virtual DelegateCommand<UIElement> SearchCommand
        {
            get
            {
                return new DelegateCommand<UIElement>(ui =>
                {
                    ui.Focus();
                    this.PageIndex = 1;
                    LoadData();
                });
            }
        }

        /// <summary>
        /// 重置命令
        /// </summary>
        public DelegateCommand ResetCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    InitializeBindData();
                });
            }
        }

        /// <summary>
        /// 列表加载完毕命令
        /// </summary>
        public DelegateCommand<DataGrid> DataGridLoadedCommand
        {
            get
            {
                return new DelegateCommand<DataGrid>(dg =>
                {
                    this.Table = dg;
                    this.LoadData();
                });
            }
        }

        /// <summary>
        /// 翻页命令
        /// </summary>
        public DelegateCommand PageChangedCommand { get { return new DelegateCommand(() => LoadData()); } }

        protected void ShowOperatoionWindow(Window win)
        {
            if (win == null)
            {
                MessageBox.Show(this.CurrentWindow, "程序异常,请重启或联系软件供应商", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            win.Owner = this.CurrentWindow;
            if (win.ShowDialog() == true)
                LoadData();
        }

        public virtual DelegateCommand<string> AddCommand { get { return new DelegateCommand<string>(url => this.ShowOperatoionWindow(Activator.CreateInstance(this.ConverPermUrl2Type(url), false) as Window)); } }

        public virtual DelegateCommand<string> EditCommand { get { return this.GetEditCommand(id => this.ShowOperatoionWindow(Activator.CreateInstance(this.ConverPermUrl2Type(this.editPermUrl), id) as Window)); } }

        //public DelegateCommand PrintCommand
        //{
        //    get
        //    {
        //        return new DelegateCommand(() =>
        //        {
        //        });
        //    }
        //}

        public virtual DelegateCommand ExportCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ExportWindow win = new ExportWindow() { Owner = Application.Current.MainWindow };
                    Action export = () =>
                    {
                        while (!win.Exported)
                        {
                            if (win.CanExport)
                            {
                                var workbook = this.CreateExportSheet(win.RowCount, Path.GetExtension(win.FileName).ToLower() == ".xls" ? ExcelExtension.XLS : ExcelExtension.XLSX);
                                using (FileStream stream = System.IO.File.OpenWrite(win.FileName))
                                {
                                    workbook.Write(stream);
                                }
                                win.Exported = true;
                            }
                        }
                    };
                    export.BeginInvoke(ar => export.EndInvoke(ar as IAsyncResult), export);
                    win.ShowDialog();
                });
            }
        }

        protected virtual IWorkbook CreateExportSheet(int rowCount, ExcelExtension ext)
        {
            return this.UIDispatcher.Invoke(new Func<IWorkbook>(() =>
             {
                 IWorkbook workbook = ext == ExcelExtension.XLS ? (IWorkbook)new HSSFWorkbook() : new XSSFWorkbook();
                 ISheet sheet = workbook.CreateSheet(this.Header);
                 var firstRow = sheet.CreateRow(0);

                 var bindPropCount = this.Table.Columns.Count() - this.Table.Columns.Where(c => string.IsNullOrWhiteSpace(GridColumn.GetBindProp(c))).Count();
                 for (int i = 0; i < bindPropCount; i++)
                 {
                     DataGridColumn col = this.Table.Columns[i];
                     firstRow.CreateCell(i, CellType.String).SetCellValue(col.Header.ToString());
                 }

                 Array context = this.GetExportData(rowCount);
                 for (int i = 0; i < context.Length; i++)
                 {
                     IRow row = sheet.CreateRow(i + 1);
                     object rowData = context.GetValue(i);
                     for (int j = 0; j < bindPropCount; j++)
                     {
                         DataGridColumn col = this.Table.Columns[j];
                         string bindProp = GridColumn.GetBindProp(col);
                         IValueConverter cvt = GridColumn.GetConverter(col);
                         object cellValue = rowData.GetType().GetProperty(bindProp).GetValue(rowData, null);
                         if (cvt != null)
                             cellValue = cvt.Convert(cellValue, typeof(string), null, null);
                         row.CreateCell(j, CellType.String).SetCellValue((cellValue ?? string.Empty).ToString());
                     }
                 }
                 return workbook;
             }), null) as IWorkbook;
        }

        protected virtual Array GetExportData(int rowCount) { return new object[0]; }

        protected Type ConverPermUrl2Type(string permUrl)
        {
            return Assembly.GetExecutingAssembly().GetType(string.Join(".", Assembly.GetExecutingAssembly().GetName().Name, "Views", permUrl));
        }
    }

    public enum ExcelExtension
    {
        XLS,
        XLSX
    }
}
