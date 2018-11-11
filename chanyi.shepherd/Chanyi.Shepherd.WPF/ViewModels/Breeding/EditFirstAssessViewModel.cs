﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.Breeding
{
    class EditFirstAssessViewModel:EditViewModel
    {
        public EditFirstAssessViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }  

        protected override object GetEditModel()
        {
            return this.Service.GetFirstAssessById(this.Id);
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

        private string weight;
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
                    var result = this.Service.UpdateFirstAssess(float.Parse(this.Weight), float.Parse(this.HabitusScore), this.assessDate, this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result)) return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}