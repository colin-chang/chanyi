using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Microsoft.Practices.Prism.Commands;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.WPF.ViewModels.BaseInfo
{
    class EditSheepViewModel : EditViewModel
    {
        public EditSheepViewModel(string editItemId)
        {
            this.Id = editItemId;
            this.InitializeBindData();
        }

        protected override object GetEditModel()
        {
            return this.Service.GetSheepById(this.Id);
        }

        protected override void InitializeBindItem()
        {
            this.studSheeps = this.Service.GetSheepParentBind(this.Id);
            this.studSheeps.Insert(0, new SheepBind { SerialNumber = ConfigurationManager.AppSettings["formDefaultSelection"] });
            this.Fathers = this.studSheeps.Where(s => (s.Gender == GenderEnum.Male || s.Id == null)).ToList();
            this.Mothers = this.studSheeps.Where(s => s.Gender == GenderEnum.Female || s.Id == null).ToList();

            this.Breeds = this.Service.GetBreedBind();
            this.Sheepfolds = this.Service.GetSheepfoldBind();
            this.Principals = this.Service.GetAllEmployeeBind();
        }

        public string Id { get; set; }


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

        private List<BreedBind> breeds;

        public List<BreedBind> Breeds
        {
            get { return breeds; }
            set
            {
                breeds = value;
                this.RaisePropertyChanged("Breeds");
            }
        }

        private string breedId;

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

        public OriginEnum Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                this.RaisePropertyChanged("Origin");
            }
        }


        private string brithWeight;

        [FloatNumber(0, 50, IsNullable = true, ErrorMessage = "初生重不合法")]
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

        private string ablactationWeight;
        [FloatNumber(0, 100, IsNullable = true, ErrorMessage = "断奶重不合法")]
        public string AblactationWeight
        {
            get { return ablactationWeight; }
            set
            {
                ablactationWeight = value;
                this.Validate("AblactationWeight");
            }
        }

        private DateTime? ablactationDate;

        [BeforeToday(ErrorMessage = "断奶日期须小于当前日期")]
        public DateTime? AblactationDate
        {
            get { return ablactationDate; }
            set
            {
                ablactationDate = value;
                this.Validate("AblactationDate");
            }
        }


        ///<summary>
        ///所有种羊
        ///</summary>
        private List<SheepBind> studSheeps;

        /// <summary>
        /// 所有种公羊
        /// </summary>
        private List<SheepBind> fathers;

        public List<SheepBind> Fathers
        {
            get { return fathers; }
            set
            {
                fathers = value;
                this.RaisePropertyChanged("Fathers");
            }
        }

        /// <summary>
        /// 所有种母羊
        /// </summary>
        private List<SheepBind> mothers;

        public List<SheepBind> Mothers
        {
            get { return mothers; }
            set
            {
                mothers = value;
                this.RaisePropertyChanged("Mothers");
            }
        }

        private string fatherId;

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

        public string MotherId
        {
            get { return motherId; }
            set
            {
                motherId = value;
                this.RaisePropertyChanged("MotherId");
            }
        }

        private List<SheepfoldBind> sheepfolds;

        public List<SheepfoldBind> Sheepfolds
        {
            get { return sheepfolds; }
            set
            {
                sheepfolds = value;
                this.RaisePropertyChanged("Sheepfolds");
            }
        }

        private string sheepfoldId;

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

        private List<EmployeeBind> principals;

        public List<EmployeeBind> Principals
        {
            get { return principals; }
            set
            {
                principals = value;
                this.RaisePropertyChanged("Principals");
            }
        }

        private string principalId;

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

        public string Remark
        {
            get { return remark; }
            set
            {
                remark = value;
                this.RaisePropertyChanged("Remark");
            }
        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.UpdateSheep(this.BreedId, this.Gender, this.GrowthStage, this.Origin, this.BirthWeight.ToFloat(), this.CompatriotNumber, this.Birthday, this.AblactationWeight.ToFloat(), this.AblactationDate, this.FatherId, this.MotherId, this.SheepfoldId, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result)) return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}