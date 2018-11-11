using Common.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Chanyi.Shepherd.WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public App()
        {
            Application.Current.DispatcherUnhandledException += (sender, e) =>
            {
                Logger.Debug(e.Exception.Message, e.Exception);
                e.Handled = true;
                MessageBox.Show(e.Exception.Message);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                try
                {
                    Exception ex = e.ExceptionObject as Exception;
                    Logger.Debug(ex.Message, ex);
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    Logger.Debug(ex.Message, ex);
                    MessageBox.Show(ex.Message);
                }
            };
        }
    }
}
