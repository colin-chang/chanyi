using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Win32;

using Microsoft.Practices.Prism.Commands;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


using Chanyi.Shepherd.WPF.UserControls;
using System.IO;
using Chanyi.Shepherd.WPF.Views.File;
using Chanyi.Shepherd.WPF.Expands.AttcahProperty;
using System.Windows.Data;
using NPOI.HSSF.UserModel;

namespace Chanyi.Shepherd.WPF.ViewModels.ReportForms
{
    /// <summary>
    /// 报表List
    /// </summary>
    public abstract class BaseReportViewModel : BaseViewModel
    {
        protected string defaultSelection = ConfigurationManager.AppSettings["listDefaultSelection"];
        private int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);

        #region 标题栏
        public string Icon { get; set; }

        public string Header { get; set; }

        public string Intro { get; set; }
        #endregion

        /// <summary>
        /// 列表控件
        /// </summary>
        public DataGrid Table { get; set; }

        /// <summary>
        /// ProgressRing
        /// </summary>
        public ProgressRing ProgressRing { get { return this.Table.Tag as ProgressRing; } }

        /// <summary>
        /// 加载列表数据
        /// </summary>
        protected abstract void LoadData();

        protected abstract override void InitializeBindData();

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

    }

}
