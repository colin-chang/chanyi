
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Chanyi.Shepherd.QueryModel.Model.ReportForms;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.ReportForms
{
    /// <summary>
    /// 繁殖报表
    /// </summary>
    public class MultiplyReportViewModel : BaseReportViewModel
    {
        public MultiplyReportViewModel(bool withinInitilization) { }

        public MultiplyReportViewModel(string header, string icon, string intro)
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
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }


        private DateTime? startDate;
        [EntityProperty]
        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                this.RaisePropertyChanged("StartDate");
            }
        }
        private DateTime? endDate;
        [EntityProperty]
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                this.RaisePropertyChanged("EndDate");
            }
        }

        public IEnumerable<MultiplyReport> Report { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action Initialize = () =>
            {
                this.Report = this.Service.GetMultiplyReport(this.StartDate, this.EndDate);

                this.UIDispatcher.Invoke(new Action<IEnumerable<MultiplyReport>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), Report);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
    }
}
