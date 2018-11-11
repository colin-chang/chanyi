﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Breeding
{
    class EditSecondAssessViewModel : EditViewModel
    {

        public EditSecondAssessViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }

        protected override object GetEditModel()
        {
            //return this.Service.GetSecondAssess(new SecondAssessFilter { Id = this.Id }).FirstOrDefault();
            return this.Service.GetSecondAssessById(this.Id);
        }

        protected override void InitializeBindItem()
        {
            this.Principals = this.Service.GetAllEmployeeBind();
        }
        public string Id { get; set; }
        public string SheepId { get; set; }

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

        private string breedFeatureScore;

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
        [Required(ErrorMessage = "种羊重量必填")]
        [FloatNumber(0, 250, ErrorMessage = "种羊重量输入不合法")]
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
        [Required(ErrorMessage = "体型评分必填")]
        [FloatNumber(0, 10, ErrorMessage = "体型评分输入不合法")]
        public string HabitusScore
        {
            get { return habitusScore; }
            set
            {
                habitusScore = value;
                this.Validate("HabitusScore");
            }
        }


        private DateTime assessDate;
        [Required(ErrorMessage = "鉴定日期必填")]
        [BeforeToday(ErrorMessage = "鉴定日期大于当前日期")]
        public DateTime AssessDate
        {
            get { return assessDate; }
            set
            {
                assessDate = value;
                this.Validate("AssessDate");
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
                    var result = this.Service.UpdateSecondAssess(float.Parse(this.BreedFeatureScore), float.Parse(this.GenitaliaScore), float.Parse(this.Weight), float.Parse(this.HabitusScore), (DateTime)this.AssessDate, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result)) return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
