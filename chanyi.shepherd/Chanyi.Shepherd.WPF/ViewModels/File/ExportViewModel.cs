using Microsoft.Win32;
using System;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Commands;
using Chanyi.Shepherd.WPF.Views.File;
using System.Windows;

namespace Chanyi.Shepherd.WPF.ViewModels.File
{
    class ExportViewModel : FormViewModel
    {
        private int rowCount;

        public int RowCount
        {
            get { return rowCount; }
            set
            {
                rowCount = value;
                this.RaisePropertyChanged("RowCount");
            }
        }

        public DelegateCommand<ProgressBar> SubmitCommand
        {
            get
            {
                return new DelegateCommand<ProgressBar>(pb =>
                {
                    SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel 97-2003 工作簿(*.xls)|*.xls|Excel 工作簿(*.xlsx)|*.xlsx" };
                    if (sfd.ShowDialog() != true)
                        return;
                    var win = CurrentWindow as ExportWindow;
                    if (win == null)
                        return;

                    win.RowCount = this.RowCount;
                    win.FileName = sfd.FileName;
                    win.CanExport = true;
                    pb.Visibility = Visibility.Visible;

                    Action listenExport = () =>
                    {
                        bool continueListen = true;
                        while (continueListen)
                        {
                            if (win.Exported)
                            {
                                this.UIDispatcher.Invoke(new Action(() =>
                                {
                                    pb.Visibility = Visibility.Collapsed;
                                    if (MessageBox.Show(this.CurrentWindow,"数据导出成功!", "成功", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                                    {
                                        win.Close();
                                        continueListen = false;
                                    }
                                }), null);
                            }
                        }
                    };
                    listenExport.BeginInvoke(ar => listenExport.EndInvoke(ar as IAsyncResult), listenExport);
                });
            }
        }
    }
}
