using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Utility.Common;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Model.Input;
using Chanyi.Shepherd.WPF.UserControls;
using Chanyi.Shepherd.QueryModel.Model.Formula;
using System.Windows.Media;
namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class AddFeedOutWarehouseByCompoundViewModel : AddWithTreeViewModel
    {
        public AddFeedOutWarehouseByCompoundViewModel(ProgressRing progressRing)
        {
            this.InitializeBindData();
            this.ProgressRing = progressRing;
            Mapper.CreateMap<FormulaFeed, FormulaDetailsData>();
            Mapper.CreateMap<FormulaBind, FormulaData>();
            Mapper.CreateMap<FeedInventory, FeedData>();
            Mapper.CreateMap<FeedData, FeedOutData>();
            Mapper.CreateMap<SheepfoldSheepCountBind, SheepfoldData>();
        }
        public AddFeedOutWarehouseByCompoundViewModel(bool withinInitilization) { }
        protected override void InitializeBindItem()
        {
            InitializeData(true);
            if (this.isContinue) return;
            var principals = this.Service.GetEmployeeBind();
            var formulas = this.Service.GetFormulaBind();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
                formulas.ForEach(f => this.AllFormulas.Add(Mapper.Map<FormulaData>(f)));

            }), null);
        }

        void InitializeData(bool initializeData)
        {
            var feeds = this.Service.GetFeedInventory();
            var sheepflods = this.Service.GetSheepfoldSheepCountBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.AllFormulas.Each(f => f.IsCheck = false);
                this.AllFeeds.Clear();
                this.AllSheepfold.Clear();
                this.FormulaDetails.Clear();
                this.FeedsOut.Clear();
                this.Sheepfolds.Clear();
                feeds.ForEach(f => this.AllFeeds.Add(Mapper.Map<FeedData>(f)));
                sheepflods.Each(s => this.AllSheepfold.Add(Mapper.Map<SheepfoldData>(s)));
            }), null);
        }

        public ProgressRing ProgressRing { get; set; }

        private ObservableCollection<FormulaData> allFormulas = new ObservableCollection<FormulaData>();
        public ObservableCollection<FormulaData> AllFormulas { get { return allFormulas; } }

        private ObservableCollection<FormulaDetailsData> formulaDetails = new ObservableCollection<FormulaDetailsData>();
        public ObservableCollection<FormulaDetailsData> FormulaDetails { get { return formulaDetails; } }

        private ObservableCollection<FeedData> allFeeds = new ObservableCollection<FeedData>();
        public ObservableCollection<FeedData> AllFeeds { get { return allFeeds; } }

        private ObservableCollection<FeedOutData> feedsOut = new ObservableCollection<FeedOutData>();
        public ObservableCollection<FeedOutData> FeedsOut { get { return feedsOut; } }

        private ObservableCollection<SheepfoldData> allSheepfold = new ObservableCollection<SheepfoldData>();
        public ObservableCollection<SheepfoldData> AllSheepfold { get { return allSheepfold; } }

        private ObservableCollection<SheepfoldData> sheepfolds = new ObservableCollection<SheepfoldData>();
        public ObservableCollection<SheepfoldData> Sheepfolds { get { return sheepfolds; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private DateTime? operationDate;
        [EntityProperty]
        [Required(ErrorMessage = "出库时间必选")]
        [BeforeToday(ErrorMessage = "出库时间需小于当前时间")]
        public DateTime? OperationDate
        {
            get { return operationDate; }
            set
            {
                operationDate = value;
                this.Validate("OperationDate");
            }
        }
        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "操作人必选")]
        public string PrincipalId
        {
            get { return principalId; }
            set
            {
                principalId = value;
                this.Validate("PrincipalId");
            }
        }

        private string remark;
        [EntityProperty]
        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                this.Validate("Remark");
            }
        }

        public DelegateCommand<string> FormulaSourceSelectCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    this.FormulaDetails.Clear();
                    var formula = this.AllFormulas.Where(f => f.Id == id).FirstOrDefault();
                    if (formula == null) return;
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        var formulaDetails = this.Service.GetFormulaFeedById(id);
                        if (formula.IsCheck)
                        {
                            formulaDetails.ForEach(f => this.FormulaDetails.Add(Mapper.Map<FormulaDetailsData>(f)));

                            foreach (var item in this.FormulaDetails)
                            {
                                if (this.AllFeeds.Where(f => f.KindId == item.KindId).FirstOrDefault() == null)
                                {
                                    item.BGColor = Brushes.Red;
                                }
                            }
                        }
                    }));
                });
            }
        }

        #region 饲料
        /// <summary>
        /// 选择饲料
        /// </summary>
        public DelegateCommand<string> FeedSourceSelectCommand
        {
            get
            {
                return new DelegateCommand<string>(kindId =>
                {
                    var feed = this.AllFeeds.Where(f => f.KindId == kindId).FirstOrDefault();
                    if (feed == null) return;
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        var feedOut = this.FeedsOut.Where(f => f.KindId == kindId).FirstOrDefault();
                        if (feed.IsCheck)
                        {
                            if (feedOut == null) this.FeedsOut.Add(Mapper.Map<FeedOutData>(feed));
                        }
                        else
                        {
                            if (feedOut != null) this.FeedsOut.Remove(feedOut);
                        }
                    }));
                });
            }
        }
        /// <summary>
        /// 移除饲料
        /// </summary>
        public DelegateCommand<string> FeedSourceRemoveCommand
        {
            get
            {
                return new DelegateCommand<string>(kindId =>
                {
                    var feed = this.FeedsOut.Where(f => f.KindId == kindId).FirstOrDefault();
                    var allfeed = this.AllFeeds.Where(f => f.KindId == kindId).FirstOrDefault();
                    if (feed == null || allfeed == null) return;
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        this.FeedsOut.Remove(feed);
                        allfeed.IsCheck = false;
                    }));
                });
            }
        }
        #endregion

        #region 圈舍
        public DelegateCommand<string> CheckFoldCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var fold = this.AllSheepfold.Where(s => s.Id == id).FirstOrDefault();
                    if (fold == null) return;
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        var foldOut = this.Sheepfolds.Where(s => s.Id == id).FirstOrDefault();
                        if (fold.IsCheck)
                        {
                            if (foldOut == null) this.Sheepfolds.Add(fold);
                        }
                        else
                        {
                            if (foldOut != null) this.Sheepfolds.Remove(foldOut);
                        }
                    }));
                });
            }
        }

        public DelegateCommand<string> RemoveFoldCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var fold = this.AllSheepfold.Where(s => s.Id == id).FirstOrDefault();
                    var foldOut = this.Sheepfolds.Where(s => s.Id == id).FirstOrDefault();
                    if (fold == null || foldOut == null) return;
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        this.Sheepfolds.Remove(foldOut);
                        fold.IsCheck = false;
                    }));
                });
            }
        }
        #endregion
        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return new DelegateCommand<UIElement>(btn =>
                {
                    if (this.FeedsOut.Count() <= 0)
                    {
                        MessageBox.Show(Application.Current.MainWindow, "没有选中任何饲料!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (this.Sheepfolds.Count() <= 0)
                    {
                        MessageBox.Show(Application.Current.MainWindow, "没有选中任何圈舍!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (this.FeedsOut.Where(s => !(s.OutAmount > 0 && s.OutAmount.ToString().IsFloat())).Count() > 0)
                    {
                        this.errors["OutAmount"] = "饲料出库量输入不合法";
                        this.RaisePropertyChanged("OutAmount");
                    }
                    if (this.FeedsOut.Where(f => f.OutAmount > f.Amount).Count() > 0)
                    {
                        this.errors["AmountOver"] = "饲料出库量不足";
                        this.RaisePropertyChanged("AmountOver");
                    }

                    if (!this.IsValid)
                    {
                        MessageBox.Show(Application.Current.MainWindow, this.Error, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        if (this.errors.Keys.Select(k => k == "OutAmount").Count() > 0)
                        {
                            this.errors.Remove("OutAmount");
                        }
                        if (this.errors.Keys.Select(k => k == "AmountOver").Count() > 0)
                        {
                            this.errors.Remove("AmountOver");
                        }
                        return;
                    }


                    Action outFeed = () =>
                    {
                        Dictionary<string, float> dicFeed = new Dictionary<string, float>();
                        this.FeedsOut.Each(f => dicFeed.Add(f.KindId, f.OutAmount));
                        List<string> listFold = new List<string>();
                        this.Sheepfolds.Each(s => listFold.Add(s.Id));
                        var result = this.Service.AddFeedBatchOutWarehouse(dicFeed, listFold, (DateTime)this.OperationDate, this.PrincipalId, this.UserId, this.Remark);

                        this.UIDispatcher.Invoke(new Action(() => this.ProgressRing.Hide()), null);
                        if (!ValidateFailedServiceResult<int>(result))
                        {
                            MessageBox.Show(this.Error, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (this.Continue2Add("添加饲料出库记录成功"))
                            return;
                        this.UIDispatcher.Invoke(new Action(() => this.CurrentWindow.DialogResult = true), null);
                    };
                    this.ProgressRing.Show();
                    outFeed.BeginInvoke(ar => outFeed.EndInvoke(ar as IAsyncResult), outFeed);
                    this.UpdateNotification();
                });
            }
        }

        public override DelegateCommand<string> NodeClickCommand
        {
            get { throw new NotImplementedException(); }
        }
    }
}

