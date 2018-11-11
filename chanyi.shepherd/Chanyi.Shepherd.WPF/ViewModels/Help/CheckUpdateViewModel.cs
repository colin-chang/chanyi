using Chanyi.Shepherd.WPF.Properties;
using Microsoft.Practices.Prism.Commands;
using System.Reflection;

namespace Chanyi.Shepherd.WPF.ViewModels.Help
{
    class CheckUpdateViewModel:BaseViewModel
    {
        public CheckUpdateViewModel(string newVersion)
        {
            this.NewVersion = newVersion;
        }

        public string Version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        public string NewVersion { get; set; }

        public DelegateCommand SkipCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Settings.Default.SkipVersion = this.NewVersion;
                    Settings.Default.Save();
                    this.CurrentWindow.DialogResult = false;
                });
            }
        }

        public DelegateCommand RemindLaterCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.CurrentWindow.DialogResult = false;
                });
            }
        }

        public DelegateCommand UpdateNowCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
