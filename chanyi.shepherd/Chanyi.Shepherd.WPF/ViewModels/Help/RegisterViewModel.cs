using System;
using System.Windows;
using System.Configuration;
using System.Diagnostics;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.WPF.Properties;
using Chanyi.Utility.Common;
using Chanyi.Shepherd.WPF.Views;
using Chanyi.SecurityUtility;

namespace Chanyi.Shepherd.WPF.ViewModels.Help
{
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel()
        {
            LoaddingWindow win = new LoaddingWindow();
            Action initilize = () =>
            {
                this.SerialNumber = Settings.Default.SerialNumber;
                this.Registered = LicenseHelper.ValidateLicense(Resources.ProductId, this.SerialNumber, Resources.PubKey);
                this.UIDispatcher.Invoke(new Action(() => win.Close()), null);
            };
            initilize.BeginInvoke(ar => initilize.EndInvoke(ar as IAsyncResult), initilize);
            win.ShowDialog();
        }

        private bool registered;

        public bool Registered
        {
            get { return registered; }
            set
            {
                registered = value;
                this.RaisePropertyChanged("Registered");
            }
        }


        private string serialNumber;

        public string SerialNumber
        {
            get { return serialNumber; }
            set
            {
                serialNumber = value;
                this.RaisePropertyChanged("SerialNumber");
            }
        }

        public string Url
        {
            get { return ConfigurationManager.AppSettings["instructionUrl"]; }
        }

        public DelegateCommand VisitCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Process.Start(this.Url);
                });
            }
        }

        public DelegateCommand ActiveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (LicenseHelper.ValidateLicense(Resources.SerialId, this.SerialNumber, Resources.PubKey))
                    {
                        Settings.Default.SerialNumber = this.SerialNumber;
                        Settings.Default.Save();
                        var reg = new RegistryHelper("Shepherd");
                        reg["SerialId"] = Resources.SerialId;
                        reg["SerialNumber"] = this.SerialNumber;
                        this.Registered = true;
                        MessageBox.Show("恭喜您，软件激活成功，感谢购买正版！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.CurrentWindow.DialogResult = true;
                        return;
                    }
                    MessageBox.Show("序列号不合法，请购买正版！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        public DelegateCommand ExitCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Application.Current.Shutdown();
                });
            }
        }
    }
}
