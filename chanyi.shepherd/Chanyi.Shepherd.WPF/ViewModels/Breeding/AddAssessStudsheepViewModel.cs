using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;


namespace Chanyi.Shepherd.WPF.ViewModels.Breeding
{
    class AddAssessStudsheepViewModel : AddViewModel
    {
        public AddAssessStudsheepViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }

        public AddAssessStudsheepViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            this.AllSheeps = this.Service.GetSheepBind(new QueryModel.BindingFilter.SheepBindFilter { GrowthStage = GrowthStageEnum.StudSheep });
          
            if (this.isContinue) return;
            var principals = this.Service.GetEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }

        List<SheepBind> AllSheeps = new List<SheepBind>();

        private GenderEnum gender;
        [EntityProperty]
        public GenderEnum Gender
        {
            get { return gender; }
            set
            {
                gender = value;

                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.StudSheeps.Clear();
                    this.StudSheeps.Add(new SheepBind { SerialNumber = defaultSelection });
                    this.AllSheeps.ForEach(p => this.StudSheeps.Add(p));
                    this.StudSheepId = null;

                }), null);

                this.RaisePropertyChanged("Gender");
            }
        }

        private ObservableCollection<SheepBind> studSheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> StudSheeps
        {
            get { return studSheeps; }
        }

        private string studSheepId;
        [EntityProperty]
        [Required(ErrorMessage = "鉴定种羊输入有误")]
        public string StudSheepId
        {
            get { return studSheepId; }
            set
            {
                studSheepId = value;
                this.Validate("StudSheepId");
            }
        }

        private string matingAbility;
        [EntityProperty]
        [Required(ErrorMessage = "配种能力输入必填")]
        [FloatNumber(0, 10, ErrorMessage = "配种能力输入不合法")]
        public string MatingAbility
        {
            get { return matingAbility; }
            set
            {
                matingAbility = value;
                this.Validate("MatingAbility");
            }
        }

        private string weight;
        [EntityProperty]
        [Required(ErrorMessage = "种羊体重必填")]
        [FloatNumber(0, 250, ErrorMessage = "种羊体重输入不合法")]
        public string Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                this.Validate("Weight");
            }
        }

        private string habitusScore;
        [EntityProperty]
        [Required(ErrorMessage = "体况评分必填")]
        [FloatNumber(0, 10, ErrorMessage = "体况评分输入不合法")]
        public string HabitusScore
        {
            get { return habitusScore; }
            set
            {
                habitusScore = value;
                this.Validate("HabitusScore");
            }
        }

        private DateTime? assessDate;
        [EntityProperty]
        [Required(ErrorMessage = "鉴定日期必填")]
        [BeforeToday(ErrorMessage = "鉴定日期大于当前日期")]
        public DateTime? AssessDate
        {
            get { return assessDate; }
            set
            {
                assessDate = value;
                this.Validate("AssessDate");
            }
        }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "测评人必选")]
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

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.AddAssessStudsheep(this.StudSheepId, float.Parse(this.MatingAbility), float.Parse(this.Weight), float.Parse(this.HabitusScore), (DateTime)this.AssessDate, this.PrincipalId, this.UserId, this.Remark);
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("种羊鉴定记录添加成功"))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
