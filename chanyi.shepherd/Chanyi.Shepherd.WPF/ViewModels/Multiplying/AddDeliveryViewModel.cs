using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Configuration;
using System.Windows.Controls;
using System.Text.RegularExpressions;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.BaseInfo;
using Chanyi.Shepherd.QueryModel.AddModel.BaseInfo;
using Chanyi.Shepherd.WPF.Model;

namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class AddDeliveryViewModel : AddViewModel
    {
        public AddDeliveryViewModel(UIElement error)
        {
            this.errorControl = error;

            this.InitializeBindData();
        }

        public AddDeliveryViewModel(bool withinInitilization) { }
        protected override void InitializeBindItem()
        {
            var females = this.Service.GetDeliverySheepBind();
            List<SheepfoldBind> sheepfolds = null;
            //if (!this.isContinue)
            sheepfolds = this.Service.GetSheepfoldBind();

            this.LambList = new List<Sheep>();
            this.LambMsg = new ObservableCollection<DeliveryLambData>();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Females.Clear();
                this.Females.Add(new SheepBind { SerialNumber = defaultSelection });
                females.ForEach(f => this.Females.Add(f));
                this.Sheepfolds.Add(new SheepfoldBind { Name = defaultSelection });
                sheepfolds.ForEach(sf => this.Sheepfolds.Add(sf));
            }), DispatcherPriority.DataBind, null);

            if (this.isContinue) return;
            var principals = this.Service.GetEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }


        #region 母羊产羔信息

        private ObservableCollection<SheepBind> females = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Females { get { return females; } }

        private string femaleId;
        [EntityProperty]
        [Required(ErrorMessage = "产羔母羊必填")]
        public string FemaleId
        {
            get { return femaleId; }
            set
            {
                femaleId = value;
                this.Validate("FemaleId");
            }
        }

        private DeliveryWayEnum deliveryWay;
        [EntityProperty]
        public DeliveryWayEnum DeliveryWay
        {
            get
            {
                return deliveryWay;
            }
            set
            {
                deliveryWay = value;
                this.RaisePropertyChanged("DeliveryWay");
            }
        }

        private List<object> numbers = new List<object>();
        public List<object> Numbers
        {
            get
            {
                if (numbers.Count() <= 0)
                {
                    for (int i = 0; i < 10; i++)
                        numbers.Add(new { Id = i, Name = i });
                }
                return numbers;
            }
        }

        public List<object> LiveMaleNumbers
        {
            get
            {
                List<object> list = new List<object>() { new { Id = 0, Name = "公" } };
                list.AddRange(Numbers);
                return list;
            }
        }

        public List<object> LiveFemaleNumbers
        {
            get
            {
                List<object> list = new List<object>() { new { Id = 0, Name = "母" } };
                list.AddRange(Numbers);
                return list;
            }
        }

        public List<object> LiveTotalNumbers
        {
            get
            {
                List<object> list = new List<object>() { new { Id = 0, Name = "总" } };
                for (int i = 0; i < 18; i++)
                    list.Add(new { Id = i, Name = i });
                return list;
            }
        }


        private int? liveMaleCount;
        [EntityProperty]
        public int? LiveMaleCount
        {
            get
            {
                return liveMaleCount == null ? 0 : liveMaleCount;
            }
            set
            {
                liveMaleCount = value;
                ChangeLiveTotalCount();
                this.RaisePropertyChanged("LiveMaleCount");
            }
        }

        private int? liveFemaleCount;
        [EntityProperty]
        public int? LiveFemaleCount
        {
            get { return liveFemaleCount == null ? 0 : liveFemaleCount; }
            set
            {
                liveFemaleCount = value;
                ChangeLiveTotalCount();
                this.RaisePropertyChanged("LiveFemaleCount");
            }
        }
        /// <summary>
        /// 产活羔总数
        /// </summary>
        private int? liveTotalCount;
        [EntityProperty]
        public int? LiveTotalCount
        {
            get { return liveTotalCount == null ? 0 : liveTotalCount; }
            set
            {
                liveTotalCount = value;
                if (this.TotalCount < this.LiveTotalCount)
                    this.errors["Error"] = "产活羊数大于总产羊数";
                else this.errors.Remove("Error");
                this.RaisePropertyChanged("Error");
                this.RaisePropertyChanged("LiveTotalCount");
            }
        }

        void ChangeLiveTotalCount()
        {
            int fc = this.LiveMaleCount ?? 0;
            int mc = this.LiveFemaleCount ?? 0;
            this.LiveTotalCount = fc + mc;
        }

        /// <summary>
        /// 总产羔数
        /// </summary>
        private int totalCount;
        [EntityProperty]
        public int TotalCount
        {
            get { return totalCount; }
            set
            {
                totalCount = value;
                if (this.totalCount < this.LiveTotalCount)
                    this.errors["Error"] = "产活羊数大于总产羊数";
                else this.errors.Remove("Error");
                this.RaisePropertyChanged("Error");
                this.RaisePropertyChanged("TotalCount");
            }
        }

        private DateTime? deliveryDate;
        [EntityProperty]
        [Required(ErrorMessage = "分娩日期必填")]
        [BeforeToday(ErrorMessage = "分娩日期需小于当前日期")]
        public DateTime? DeliveryDate
        {
            get { return deliveryDate; }
            set
            {
                deliveryDate = value;
                this.Validate("DeliveryDate");
            }
        }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        /// <summary>
        /// 助产原因
        /// </summary>
        private MidwiferyReasonEnum midwiferyReason;
        [EntityProperty]
        public MidwiferyReasonEnum MidwiferyReason
        {
            get
            {
                return midwiferyReason;
            }
            set
            {
                midwiferyReason = value;
                this.Validate("MidwiferyReason");
            }
        }

        private string deliverReasonOtherDetail;
        [EntityProperty]
        public string DeliverReasonOtherDetail
        {
            get { return deliverReasonOtherDetail; }
            set
            {
                deliverReasonOtherDetail = value;
                this.Validate("DeliverReasonOtherDetail");
            }
        }

        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "接生员必选")]
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
        #endregion

        #region 添加羔羊

        /// <summary>
        /// 添加羔羊时的错误提示
        /// </summary>
        private string lambError;
        public string LambError
        {
            get { return lambError; }
            set
            {
                lambError = value;
                this.RaisePropertyChanged("LambError");
            }
        }

        private string serialNumber;
        [EntityProperty]
        public string SerialNumber
        {
            get { return serialNumber; }
            set
            {
                serialNumber = value;
                this.Validate("SerialNumber");
            }
        }

        private GenderEnum gender;
        [EntityProperty]
        public GenderEnum Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                this.RaisePropertyChanged("Gender");
            }
        }

        private string brithWeight;
        [EntityProperty]
        public string BirthWeight
        {
            get { return brithWeight; }
            set
            {
                brithWeight = value;
                this.Validate("BirthWeight");
            }
        }

        private ObservableCollection<SheepfoldBind> sheepfolds = new ObservableCollection<SheepfoldBind>();
        public ObservableCollection<SheepfoldBind> Sheepfolds { get { return sheepfolds; } }

        private string sheepfoldId;
        [EntityProperty]
        public string SheepfoldId
        {
            get { return sheepfoldId; }
            set
            {
                sheepfoldId = value;
                this.Validate("SheepfoldId");
            }
        }

        private string remarkLamb;
        [EntityProperty]
        public string RemarkLamb
        {
            get { return remarkLamb; }
            set
            {
                remarkLamb = value;
                this.RaisePropertyChanged("RemarkLamb");
            }
        }

        #endregion

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.AddDelivery(this.FemaleId, this.DeliveryWay, this.MidwiferyReason, this.DeliverReasonOtherDetail, this.LiveMaleCount, this.LiveFemaleCount, this.TotalCount, (DateTime)this.DeliveryDate, this.PrincipalId, this.UserId, this.Remark, this.LambList);
                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    this.UpdateNotification();
                    if (this.Continue2Add("分娩记录添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
        /// <summary>
        /// 关闭pop窗口
        /// </summary>
        public DelegateCommand<Popup> ClosePopCommand
        {
            get
            {
                return new DelegateCommand<Popup>(p =>
                {
                    p.IsOpen = false;
                });
            }
        }
        /// <summary>
        /// 打开pop窗口
        /// </summary>
        public DelegateCommand<Popup> OpenPopCommand
        {
            get
            {
                return new DelegateCommand<Popup>(p =>
                {
                    p.IsOpen = true;
                });
            }
        }

        /// <summary>
        /// 生成编号
        /// </summary>
        public DelegateCommand GenerateSerialNumberCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    string add = ConfigurationManager.AppSettings["farmAbb"];
                    string year = DateTime.Today.Year.ToString().Substring(2);
                    string month = DateTime.Today.Month.ToString().PadLeft(2, '0');
                    string ser = (this.Service.GetCurMonthBirthCount() + 1 + this.LambList.Count).ToString("X4");
                    this.SerialNumber = string.Format("{0}{1}{2}{3}", add, year, month, ser);
                });
            }
        }

        public DelegateCommand AddSheepFold
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddSheepFoldWindow win = new AddSheepFoldWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        this.Sheepfolds.Clear();
                        this.Sheepfolds.Add(new SheepfoldBind { Name = defaultSelection });
                        this.Service.GetSheepfoldBind().ForEach(sf => this.Sheepfolds.Add(sf));
                        this.SheepfoldId = this.Sheepfolds.FirstOrDefault() == null ? null : this.Sheepfolds.FirstOrDefault().Id;
                    }
                });
            }
        }

        /// <summary>
        /// 新添加的羔羊列表
        /// </summary>
        private List<Sheep> lambList;
        public List<Sheep> LambList
        {
            get
            {
                if (lambList == null)
                    lambList = new List<Sheep>();

                //if (this.lambList.Count != this.LiveCount)
                //    this.errors["Error"] = "产活数量与已录入羔羊数量不同";
                //else this.errors.Remove("Error");
                //this.RaisePropertyChanged("Error");

                return lambList;
            }
            set
            {
                lambList = value;
            }
        }
        /// <summary>
        /// 羔羊添加显示
        /// </summary>
        private ObservableCollection<DeliveryLambData> lambMsg;
        public ObservableCollection<DeliveryLambData> LambMsg
        {
            get
            {
                if (lambMsg == null)
                    lambMsg = new ObservableCollection<DeliveryLambData>();
                return lambMsg;
            }
            set
            {
                lambMsg = value;
                this.RaisePropertyChanged("LambMsg");
            }
        }

        /// <summary>
        /// 添加羔羊
        /// </summary>
        public DelegateCommand<Popup> AddLambCommand
        {
            get
            {
                return new DelegateCommand<Popup>(pop =>
                {
                    this.LambError = string.Empty;
                    if (string.IsNullOrEmpty(this.SerialNumber))
                    {
                        this.LambError = "羊只编号必填";
                        return;
                    }
                    if (this.BirthWeight != null && this.BirthWeight != "" && !Regex.IsMatch(this.BirthWeight, @"^\d\.\d{1,}$|^\d$"))
                    {
                        this.LambError = "初生重不合法";
                        return;
                    }
                    if (string.IsNullOrEmpty(this.SheepfoldId))
                    {
                        this.LambError = "圈舍编号必选";
                        return;
                    }
                    if (this.LambList.Where(l => l.SerialNumber == this.SerialNumber).Count() > 0)
                    {
                        this.LambError = "存在相同的羔羊编号，请重新输入";
                        return;
                    }

                    //添加的羔羊性别数量应该与成活的数量相同
                    int curGenderLambCnt = lambList.Count(l => l.Gender == this.Gender);
                    bool isMale = this.Gender == GenderEnum.Male;
                    if (curGenderLambCnt >= (isMale ? this.LiveMaleCount : this.LiveFemaleCount))
                    {
                        this.LambError = string.Format("{0}羔数量已经达到成活{0}羔数量最大值", isMale ? "公" : "母");
                        return;
                    }


                    this.LambList.Add(new Sheep() { SerialNumber = this.SerialNumber, Gender = this.Gender, BirthWeight = string.IsNullOrEmpty(this.BirthWeight) ? 0 : float.Parse(this.BirthWeight), SheepfoldId = this.SheepfoldId, Remark = this.RemarkLamb });

                    this.LambMsg.Add(new DeliveryLambData()
                    {
                        IsChecked = false,
                        SerialNumber = this.SerialNumber,
                        Gender = this.Gender == GenderEnum.Male ? "公" : "母",
                        BirthWeight = this.BirthWeight,
                        Sheepfold = this.Sheepfolds.Where(t => t.Id == this.SheepfoldId).FirstOrDefault().Name,
                        Remark = this.RemarkLamb
                    });

                    this.SerialNumber = string.Empty;
                    this.Gender = GenderEnum.Male;
                    this.BirthWeight = string.Empty;
                    this.SheepfoldId = null;
                    this.RemarkLamb = string.Empty;

                    this.UpdateNotification();

                    if (this.LambList.Count == this.LiveTotalCount)
                        pop.IsOpen = false;
                });
            }
        }

        public DelegateCommand RemoveLambCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.LambMsg.Where(l => l.IsChecked).ToList().ForEach(item =>
                    {
                        this.LambMsg.Remove(item);
                        this.LambList.Remove(this.LambList.Where(lamb => lamb.SerialNumber == item.SerialNumber).FirstOrDefault());
                    });
                });
            }
        }
    }

}
