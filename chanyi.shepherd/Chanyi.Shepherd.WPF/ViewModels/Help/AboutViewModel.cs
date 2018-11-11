using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Microsoft.Practices.Prism.Commands;
using Chanyi.Shepherd.WPF.Properties;

namespace Chanyi.Shepherd.WPF.ViewModels.Help
{
    class AboutViewModel : BaseViewModel
    {
        public string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string ProductId { get { return Resources.ProductId.ToUpper(); } }

        public bool AutoUpdate
        {
            get { return Settings.Default.AutoUpdate; }
            set
            {
                Settings.Default.AutoUpdate = value;
                Settings.Default.Save();
                this.RaisePropertyChanged("AutoUpdate");
            }
        }


        public DelegateCommand CancelCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.CurrentWindow.Close();
                });
            }
        }
    }
}
