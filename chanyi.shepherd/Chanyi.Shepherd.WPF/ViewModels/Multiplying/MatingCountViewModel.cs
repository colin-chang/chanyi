using AutoMapper;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Multiplying;
using Chanyi.Shepherd.QueryModel.Model.Multiplying;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    /// <summary>
    /// 交配次数统计
    /// </summary>
    public class MatingCountViewModel : ListViewModel
    {
        public MatingCountViewModel(bool withinInitilization) { }

        public MatingCountViewModel(string header, string icon, string intro)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            Mapper.CreateMap<MatingCountViewModel, MatingCountFilter>();
            InitializeBindData();
        }

        #region 搜索相关
        protected override void InitializeBindData()
        {

            Action Initialize = () =>
            {
                var sheeps = this.Service.GetMatingSheepSelectBind().Where(t => t.Gender == GenderEnum.Female).ToList();
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Sheeps.Clear();

                    this.Sheeps.Add(new SheepBind() { SerialNumber = defaultSelection });
                    sheeps.ForEach(s => this.Sheeps.Add(s));

                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }

        private string sheepId;
        [EntityProperty]
        public string SheepId
        {
            get { return sheepId; }
            set
            {
                sheepId = value;
                this.RaisePropertyChanged("SheepId");
            }
        }

        private List<object> years = new List<object>();
        public List<object> Years
        {
            get
            {
                if (years.Count() <= 0)
                {
                    years.Add(new { Name = defaultSelection });

                    for (int i = 2014; i <= DateTime.Now.Year; i++)
                        years.Add(new { Id = i, Name = i });
                }
                return years;
            }
        }

        private int? year;
        [EntityProperty]
        public int? Year
        {
            get { return year; }
            set
            {
                year = value;
                this.RaisePropertyChanged("Year");
            }
        }

        private SeasonEnum? season;
        [EntityProperty]
        public SeasonEnum? Season
        {
            get { return season; }
            set
            {
                season = value;
                this.RaisePropertyChanged("Season");
            }
        }

        private int? count;
        [EntityProperty]
        public int? Count
        {
            get { return count; }
            set
            {
                count = value;
                this.RaisePropertyChanged("Count");
            }
        }

        #endregion

        #region 列表数据
        //public MatingCountFilter Filter
        //{
        //    get
        //    {
        //        MatingCountFilter filter = Mapper.Map<MatingCountFilter>(this);
        //        return filter;
        //    }
        //}
        public IEnumerable<MatingCount> ExceptSheep { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action Initialize = () =>
            {
                int count;
                this.ExceptSheep = this.Service.GetMatingCount(this.SheepId, this.Count, this.Year, this.Season, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<MatingCount>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), ExceptSheep);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);

        }
        #endregion

        protected override Array GetExportData(int rowCount)
        {
            this.ExceptSheep = this.Service.GetMatingCount(this.SheepId, this.Count, this.Year, this.Season, rowCount);
            return this.ExceptSheep.ToArray();
        }
    }
}
