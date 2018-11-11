using Chanyi.Shepherd.QueryModel.Model.ReportForms;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Chanyi.Shepherd.WPF.ViewModels.ReportForms
{
    public class FeedReportViewModel : BaseReportViewModel
    {
        public FeedReportViewModel(bool withinInitilization) { }

        public FeedReportViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;

            InitializeBindData();
        }
        public FeedReportViewModel(string header, string icon, string intro, StackPanel spMain)
            : this(header, icon, intro)
        {
            this.SPMain = spMain;
            LoadData();
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

        #region 搜索条件
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
        #endregion

        /// <summary>
        /// 用于加载用户控件的容器
        /// </summary>
        public StackPanel SPMain { get; set; }

        public IEnumerable<FeedReport> Report { get; set; }

        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                return;
            }
            Action Initialize = () =>
            {
                this.Report = this.Service.GetFeedInventoryReport(this.StartDate, this.EndDate);

                this.UIDispatcher.Invoke(new Action<IEnumerable<FeedReport>>(d =>
                {
                    this.SPMain.Children.Clear();

                    //处理数据
                    this.Report.Select(t => t.FullName).Distinct().ToList().ForEach(t =>
                    {
                        this.SPMain.Children.Add(new FeedChartReport(this.Report.Where(f => f.FullName == t), t));
                    });


                    //this.ProgressRing.Hide();
                }), Report);
            };
            //this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }


    }
}
