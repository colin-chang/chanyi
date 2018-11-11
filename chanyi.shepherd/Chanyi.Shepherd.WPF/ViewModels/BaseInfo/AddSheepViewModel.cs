using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Configuration;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.BaseInfo;
using Chanyi.Shepherd.WPF.Views.HR;
using Chanyi.Utility.Common;

using Chanyi.Shepherd.IServices;
using Chanyi.Shepherd.WPF.Helper;

namespace Chanyi.Shepherd.WPF.ViewModels.BaseInfo
{
    class AddSheepViewModel : AddViewModel
    {
        public AddSheepViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddSheepViewModel(bool withinInitilization)
        {
        }

        protected override void InitializeBindItem()
        {
            this.studSheeps = this.Service.GetStudSheepBindWithOuter();
            List<BreedBind> breeds = null;
            List<SheepfoldBind> sheepfolds = null;
            List<EmployeeBind> principals = null;
            if (!this.isContinue)
            {
                breeds = this.Service.GetBreedBind();
                sheepfolds = this.Service.GetSheepfoldBind();
                principals = this.Service.GetEmployeeBind();
            }
            this.UIDispatcher.Invoke(new Action(() =>
            {
                SheepBind defaultSheep = new SheepBind { SerialNumber = defaultSelection };
                this.Fathers.Clear();
                this.Mothers.Clear();
                this.Fathers.Add(defaultSheep);
                this.Mothers.Add(defaultSheep);
                this.studSheeps.ForEach(s =>
                {
                    if (s.Gender == GenderEnum.Male)
                        this.Fathers.Add(s);
                    else
                        this.Mothers.Add(s);
                });
                if (this.isContinue)
                    return;

                this.Breeds.Add(new BreedBind { Name = defaultSelection });
                breeds.ForEach(b => this.Breeds.Add(b));
                this.Sheepfolds.Add(new SheepfoldBind { Name = defaultSelection });
                sheepfolds.ForEach(sf => this.Sheepfolds.Add(sf));
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }

        private string serialNumber;
        [EntityProperty]
        [Required(ErrorMessage = "编号必填")]
        public string SerialNumber
        {
            get { return serialNumber; }
            set
            {
                serialNumber = value;
                this.Validate("SerialNumber");
            }
        }


        private ObservableCollection<BreedBind> breeds = new ObservableCollection<BreedBind>();
        public ObservableCollection<BreedBind> Breeds { get { return breeds; } }

        private string breedId;
        [EntityProperty]
        [Required(ErrorMessage = "品种必选")]
        public string BreedId
        {
            get { return breedId; }
            set
            {
                breedId = value;
                this.Validate("BreedId");
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

        private GrowthStageEnum growthStage;
        [EntityProperty]
        public GrowthStageEnum GrowthStage
        {
            get { return growthStage; }
            set
            {
                growthStage = value;
                this.RaisePropertyChanged("GrowthStage");
            }
        }

        private OriginEnum origin;
        [EntityProperty]
        public OriginEnum Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                this.RaisePropertyChanged("Origin");
                if (value == OriginEnum.HomeBred)
                {
                    this.GetType().GetProperties().Where(p => p.IsDefined(typeof(ConditionalVerifyAttribute), true)).Select(p => p.Name).ToList().ForEach(p => this.errors.Remove(p));
                    this.RaisePropertyChanged("Error");
                }
            }
        }


        private string brithWeight;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "初生重不合法")]
        public string BirthWeight
        {
            get { return brithWeight; }
            set
            {
                brithWeight = value;
                this.Validate("BirthWeight");
            }
        }

        private List<int> compatriotNumbers = new List<int>();
        public List<int> CompatriotNumbers
        {
            get
            {
                if (compatriotNumbers.Count() <= 0)
                {
                    for (int i = 0; i < 10; i++)
                        compatriotNumbers.Add(i);
                }
                return compatriotNumbers;
            }
        }

        private int compatriotNumber;
        [EntityProperty]
        public int CompatriotNumber
        {
            get { return compatriotNumber; }
            set
            {
                compatriotNumber = value;
                this.RaisePropertyChanged("CompatriotNumber");
            }
        }

        private DateTime? birthday;
        [EntityProperty]
        [BeforeToday(ErrorMessage = "出生日期须小于当前日期")]
        public DateTime? Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                this.Validate("Birthday");
            }
        }

        ///<summary>
        ///所有种羊
        ///</summary>
        private List<SheepBind> studSheeps;

        private ObservableCollection<SheepBind> fathers = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Fathers { get { return fathers; } }

        private ObservableCollection<SheepBind> mathers = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Mothers { get { return mathers; } }

        private string fatherId;
        [EntityProperty]
        public string FatherId
        {
            get { return fatherId; }
            set
            {
                fatherId = value;
                this.RaisePropertyChanged("FatherId");
            }
        }

        private string motherId;
        [EntityProperty]
        public string MotherId
        {
            get { return motherId; }
            set
            {
                motherId = value;
                this.RaisePropertyChanged("MotherId");
            }
        }

        public string FatherSerialNumber { get; set; }
        public string MotherSerialNumber { get; set; }

        private ObservableCollection<SheepfoldBind> sheepfolds = new ObservableCollection<SheepfoldBind>();
        public ObservableCollection<SheepfoldBind> Sheepfolds { get { return sheepfolds; } }

        private string sheepfoldId;
        [EntityProperty]
        [Required(ErrorMessage = "圈舍必选")]
        public string SheepfoldId
        {
            get { return sheepfoldId; }
            set
            {
                sheepfoldId = value;
                this.Validate("SheepfoldId");
            }
        }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "技术员必选")]
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

        /// <summary>
        /// 生成编号
        /// </summary>
        public DelegateCommand GenerateSerialNumberCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var farm = this.Service.GetFarm();
                    string prefix = farm == null ? string.Empty : ChineseToPinYin.ToPinYin((farm.Code ?? "").ToUpper());
                    string breed = ChineseToPinYin.ToPinYin(this.Breeds.Where(b => b.Id == this.BreedId).FirstOrDefault().Name);
                    string year = DateTime.Today.Year.ToString().Substring(2);
                    string month = DateTime.Today.Month.ToString().PadLeft(2, '0');
                    string ser = (this.Service.GetCurMonthBirthCount() + 1).ToString("X4");
                    this.SerialNumber = string.Format("{0}{1}{2}{3}{4}", prefix, breed, year, month, ser);
                });
            }
        }

        #region BuySheep

        private string money;
        [ConditionalVerify]
        [EntityProperty]
        [Required(ErrorMessage = "购买价格必填")]
        [FloatNumber(0, int.MaxValue, ErrorMessage = "购买价格输入不合法")]
        public string Money
        {
            get { return money; }
            set
            {
                money = value;
                this.Validate("Money");
            }
        }

        private string source;
        [ConditionalVerify]
        [EntityProperty]
        [Required(ErrorMessage = "羊来源内容必填")]
        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                this.Validate("Source");
            }
        }

        private DateTime? operationDate;
        [ConditionalVerify]
        [EntityProperty]
        [Required(ErrorMessage = "购买日期必填")]
        [BeforeToday(ErrorMessage = "购买日期需小于当前日期")]
        public DateTime? OperationDate
        {
            get { return operationDate; }
            set
            {
                operationDate = value;
                this.Validate("OperationDate");
            }
        }

        private string buyPrincipalId;
        [ConditionalVerify]
        [EntityProperty]
        [Required(ErrorMessage = "购买人必填")]
        public string BuyPrincipalId
        {
            get { return buyPrincipalId; }
            set
            {
                buyPrincipalId = value;
                this.Validate("BuyPrincipalId");
            }
        }

        private string buyWeight;
        [ConditionalVerify]
        [EntityProperty]
        [FloatNumber(0, 500, IsNullable = true, ErrorMessage = "购入重量不合法")]
        public string BuyWeight
        {
            get { return buyWeight; }
            set
            {
                buyWeight = value;
                this.Validate("BuyWeight");
            }
        }

        private string buyRemark;
        [EntityProperty]
        public string BuyRemark
        {
            get { return buyRemark; }
            set
            {
                buyRemark = value;
                this.RaisePropertyChanged("BuyRemark");
            }
        }

        #endregion

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(() => this.Origin == OriginEnum.Purchase,
                    err =>
                    {
                        ServiceResult<string> result = this.Origin == OriginEnum.Purchase ?
                        this.Service.AddSheep(this.SerialNumber, this.BreedId, this.Gender, this.GrowthStage, this.Origin, this.BirthWeight.ToFloat(), this.CompatriotNumber, this.Birthday, this.FatherId, this.FatherSerialNumber, this.MotherId, this.MotherSerialNumber, this.SheepfoldId, this.PrincipalId, this.UserId, this.Remark, this.Source, Convert.ToDecimal(this.Money), this.BuyWeight.ToFloat(), (DateTime)this.OperationDate, this.BuyPrincipalId, this.BuyRemark)
                        :
                         this.Service.AddSheep(this.SerialNumber, this.BreedId, this.Gender, this.GrowthStage, this.Origin, this.BirthWeight.ToFloat(), this.CompatriotNumber, this.Birthday, this.FatherId, this.MotherId, this.SheepfoldId, this.PrincipalId, this.UserId, this.Remark);

                        if (!ValidateFailedServiceResult<string>(result))
                            return;
                        if (this.Continue2Add("羊只添加成功"))
                            return;

                        this.CurrentWindow.DialogResult = true;
                    });
            }
        }

        public DelegateCommand AddBreed
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddBreedWindow win = new AddBreedWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        this.Breeds.Clear();
                        this.Breeds.Add(new BreedBind { Name = defaultSelection });
                        this.Service.GetBreedBind().ForEach(b => this.Breeds.Add(b));
                        this.BreedId = this.Breeds.FirstOrDefault() == null ? null : this.Breeds.FirstOrDefault().Id; ;
                    }
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

        public DelegateCommand AddEmployee
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddEmployeeWindow win = new AddEmployeeWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        this.principals.Clear();
                        this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                        this.Service.GetEmployeeBind().ForEach(p => this.Principals.Add(p));
                        this.PrincipalId = this.Principals.FirstOrDefault() == null ? null : this.Principals.FirstOrDefault().Id;
                    }
                });
            }
        }
    }
}