using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using System.Collections.ObjectModel;
using Chanyi.Shepherd.WPF.Views.Inputs;


namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class AddFeedInOutWarehouseViewModel : AddViewModel
    {
        public AddFeedInOutWarehouseViewModel(UIElement error, InOutWarehouseDirectionEnum direction)
        {
            this.errorControl = error;
            this.InitializeBindData();
            this.direction = direction;
        }

        InOutWarehouseDirectionEnum direction;
        public string Title { get { return this.direction == InOutWarehouseDirectionEnum.In ? "饲料入库" : "饲料出库"; } }
        public AddFeedInOutWarehouseViewModel(bool withinInitilization) { }
        protected override void InitializeBindItem()
        {
            if (this.isContinue)
                return;

            var feedNames = this.Service.GetFeedNameBind();
            var principals = this.Service.GetEmployeeBind();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.FeedNames.Add(new FeedNameBind { Name = defaultSelection });
                feedNames.ForEach(f => this.feedNames.Add(f));
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }

        private ObservableCollection<FeedNameBind> feedNames = new ObservableCollection<FeedNameBind>();
        public ObservableCollection<FeedNameBind> FeedNames { get { return feedNames; } }

        private string feednameId;
        [EntityProperty]
        [Required(ErrorMessage = "饲料名称必选")]
        public string FeedNameId
        {
            get { return feednameId; }
            set
            {
                feednameId = value;
                this.Validate("FeedNameId");
            }
        }

        private string typeNameId;
        [EntityProperty]
        [Required(ErrorMessage = "饲料类型必选")]
        public string TypeNameId
        {
            get { return typeNameId; ; }
            set
            {
                typeNameId = value;
                this.Validate("TypeNameId");
            }
        }

        private OutWarehouseDispositonEnum dispositon;
        [EntityProperty]
        [Required(ErrorMessage = "饲料去向必选")]
        public OutWarehouseDispositonEnum Dispositon
        {
            get { return dispositon; }
            set
            {
                dispositon = value;
                this.Validate("Dispositon");
            }
        }

        private string amount;
        [EntityProperty]
        [Required(ErrorMessage = "数量名称必选")]
        [FloatNumber(0, int.MaxValue, ErrorMessage = "饲料数量输入不合法")]
        public string Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                this.Validate("Amount");
            }
        }

        private ObservableCollection<FeedTypeBind> typeNames = new ObservableCollection<FeedTypeBind>();
        public ObservableCollection<FeedTypeBind> TypeNames
        {
            get { return typeNames; ; }
        }

        private ObservableCollection<AreaBind> areas = new ObservableCollection<AreaBind>();
        public ObservableCollection<AreaBind> Areas
        {
            get { return areas; }
        }

        private string areaId;
        [EntityProperty]
        [Required(ErrorMessage = "产地名称必填")]
        public string AreaId
        {
            get { return areaId; }
            set
            {
                areaId = value;
                this.Validate("AreaId");
            }
        }
        private DateTime? operationDate;
        [EntityProperty]
        [Required(ErrorMessage = "时间必填")]
        [BeforeToday(ErrorMessage = "日期需小于当前日期")]

        public DateTime? OperationDate
        {
            get { return operationDate; }
            set
            {
                operationDate = value;
                this.Validate("OperationDate");
            }
        }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

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
                this.RaisePropertyChanged("Remark");
            }
        }

        private bool typeEnable;

        public bool TypeEnable
        {
            get { return typeEnable; }
            set
            {
                typeEnable = value;
                this.RaisePropertyChanged("TypeEnable");
            }
        }

        private bool areaEnable;
        public bool AreaEnable
        {
            get { return areaEnable; }
            set
            {
                areaEnable = value;
                this.RaisePropertyChanged("AreaEnable");
            }
        }


        public DelegateCommand SelectFeedNameChanged
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (this.FeedNameId == null)
                    {
                        ResetSelected();
                        return;
                    }

                    var typeNames = this.Service.GetFeedTypeBind(this.FeedNameId);
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        this.TypeNames.Clear();
                        this.TypeNames.Add(new FeedTypeBind { Name = defaultSelection });
                        typeNames.ForEach(t => this.TypeNames.Add(t));
                        this.TypeNameId = this.TypeNames.FirstOrDefault().Id;
                        this.AreaId = this.Areas.FirstOrDefault() == null ? null : this.Areas.FirstOrDefault().Id;
                    }), null);

                    if (TypeNames.Count() <= 1)
                    {
                        this.errors["TypeNameId"] = "无对应的饲料类型，请添加新饲料";
                        this.RaisePropertyChanged("Error");
                        this.TypeEnable = false;
                    }
                    else
                    {
                        this.errors.Remove("TypeNameId");
                        this.TypeEnable = true;
                    }
                });
            }
        }

        public DelegateCommand SelectTypeNameChanged
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (this.TypeNameId == null)
                    {
                        this.UIDispatcher.Invoke(new Action(() => { this.Areas.Clear(); this.AreaEnable = false; }), null);
                        return;
                    }

                    var areas = this.Service.GetAreaBind(this.FeedNameId, this.TypeNameId);
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        this.Areas.Clear();
                        this.Areas.Add(new AreaBind { Name = defaultSelection });
                        areas.ForEach(a => this.Areas.Add(a));
                        this.AreaId = this.Areas.FirstOrDefault().Id;
                    }), null);

                    if (Areas.Count() <= 1)
                    {
                        this.errors["AreaId"] = "无对应的产地,请添加新饲料";
                        this.RaisePropertyChanged("Error");
                        this.AreaEnable = false;
                    }
                    else
                    {
                        this.errors.Remove("AreaId");
                        this.AreaEnable = true;
                    }
                });
            }
        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.AddFeedInOutWarehouse(this.FeedNameId, this.TypeNameId, this.AreaId, float.Parse(this.Amount), (DateTime)this.OperationDate, this.direction, this.PrincipalId, this.UserId, this.Remark, this.Dispositon);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    this.UpdateNotification();
                    if (this.Continue2Add("饲料入(出)库成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

        /// <summary>
        /// 添加新饲料
        /// </summary>
        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddFeedWindow win = new AddFeedWindow();
                    win.Owner = CurrentWindow;
                    win.ShowDialog();
                });
            }
        }

        protected override void Reset()
        {
            base.Reset();
            ResetSelected();
        }

        void ResetSelected()
        {
            if (this.FeedNameId == null)
            {
                this.TypeEnable = false;
                this.AreaEnable = false;
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.TypeNames.Clear();
                    this.Areas.Clear();
                }), null);
            }
        }
    }
}
