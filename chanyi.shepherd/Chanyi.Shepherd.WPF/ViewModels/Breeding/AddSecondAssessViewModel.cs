using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;


namespace Chanyi.Shepherd.WPF.ViewModels.Breeding
{
    class AddSecondAssessViewModel : AddViewModel
    {
        public AddSecondAssessViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }
        public AddSecondAssessViewModel(bool withinInitilization) { }
        protected override void InitializeBindItem()
        {
             var sheeps = this.Service.GetSecondAssessSheepAddBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Sheeps.Clear();
                this.Sheeps.Add(new SheepBind { SerialNumber = defaultSelection });
                sheeps.ForEach(s => this.Sheeps.Add(s));
            }), null);

            if (this.isContinue) return;
            var principals = this.Service.GetEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);
        }

        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }

        private string sheepId;
        [EntityProperty]
        [Required(ErrorMessage = "羊编号输入有误")]
        public string SheepId
        {
            get { return sheepId; }
            set
            {
                sheepId = value;
                this.Validate("SheepId");
            }
        }

        private string breedFeatureScore;
        [EntityProperty]
        [Required(ErrorMessage = "特征评分必填")]
        [FloatNumber(0, 10, ErrorMessage = "特征评分输入不合法")]
        public string BreedFeatureScore
        {
            get { return breedFeatureScore; }
            set
            {
                breedFeatureScore = value;
                this.Validate("BreedFeatureScore");
            }
        }

        private string genitaliaScore;
        [EntityProperty]
        [Required(ErrorMessage = "生殖器官评分必填")]
        [FloatNumber(0, 10, ErrorMessage = "生殖器官评分输入不合法")]
        public string GenitaliaScore
        {
            get { return genitaliaScore; }
            set
            {
                genitaliaScore = value;
                this.Validate("GenitaliaScore");
            }
        }

        private string weight;
        [EntityProperty]
        [Required(ErrorMessage = "羊体重必填")]
        [FloatNumber(0, 250, ErrorMessage = "羊体重输入不合法")]
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
                    var result = this.Service.AddSecondAssess(this.SheepId, float.Parse(this.BreedFeatureScore), float.Parse(this.GenitaliaScore), float.Parse(this.Weight), float.Parse(this.HabitusScore), (DateTime)this.AssessDate, this.PrincipalId, this.UserId, this.Remark);

                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("第二次鉴定添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
