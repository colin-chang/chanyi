using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Multiplying
{
    class AddMatingViewModel : AddViewModel
    {
        public AddMatingViewModel(UIElement error)
        {
            this.errorControl = error;
            this.InitializeBindData();
        }
        public AddMatingViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            if (this.isContinue)
                return;

            List<SheepBind> studsheeps = this.Service.GetStudSheepBind();
            var males = studsheeps.Where(s => s.Gender == GenderEnum.Male).ToList();
            var females = studsheeps.Where(s => s.Gender == GenderEnum.Female).ToList();
            var principals = this.Service.GetEmployeeBind();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Males.Add(new SheepBind { SerialNumber = defaultSelection });
                males.ForEach(m => this.Males.Add(m));
                this.Females.Add(new SheepBind { SerialNumber = defaultSelection });
                females.ForEach(f => this.Females.Add(f));

                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
            }), null);

        }

        private ObservableCollection<SheepBind> males = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Males { get { return males; } }

        private ObservableCollection<SheepBind> females = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Females { get { return females; } }

        private string maleId;
        [EntityProperty]
        [Required(ErrorMessage = "配种公羊必填")]
        public string MaleId
        {
            get { return maleId; }
            set
            {
                maleId = value;
                this.Validate("MaleId");
            }
        }

        private string femaleId;
        [EntityProperty]
        [Required(ErrorMessage = "配种母羊必填")]
        public string FemaleId
        {
            get { return femaleId; }
            set
            {
                femaleId = value;
                this.Validate("FemaleId");
            }
        }

        private DateTime? matingDate;
        [EntityProperty]
        [Required(ErrorMessage = "配种日期必填")]
        [BeforeToday(ErrorMessage = "配种日期需小于当前日期")]
        public DateTime? MatingDate
        {
            get { return matingDate; }
            set
            {
                matingDate = value;
                this.Validate("MatingDate");
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

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.AddMating(this.FemaleId, this.MaleId, (DateTime)this.MatingDate, this.PrincipalId, this.UserId, this.Remark);

                    this.errorControl = err;
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("配种记录添加成功"))
                        return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
