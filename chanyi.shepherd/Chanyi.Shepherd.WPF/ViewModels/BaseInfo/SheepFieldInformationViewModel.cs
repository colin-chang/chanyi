using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;

namespace Chanyi.Shepherd.WPF.ViewModels.BaseInfo
{
    class SheepFieldInformationViewModel : AddViewModel
    {
        public SheepFieldInformationViewModel()
        {
            this.InitializeBindData();
        }

        protected override void InitializeBindData()
        {
            Action initialize = () =>
            {
                var paras = this.Service.GetSheepParameters();
                //var ablactation = paras.Where(p => p.Type == SettingsEnum.PreAblactationRemaindful).FirstOrDefault();
                //var gestation = paras.Where(p => p.Type == SettingsEnum.PreDeliveryRemaindful).FirstOrDefault();

                //if (ablactation != null)
                //{
                //    this.AblactationId = ablactation.Id;
                //    this.AblactationDays = ablactation.Value;
                //    this.IsAblactationRemaindful = ablactation.IsRemaindful;
                //}
                //if (gestation != null)
                //{
                //    this.GestationId = gestation.Id;
                //    this.GestationDays = gestation.Value;
                //    this.IsGestationRemaindful = gestation.IsRemaindful;
                //}
                //this.Range = gestation.Range.ToString();
                //this.Remark = ablactation.Remark;
            };

            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);

        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    
                   var result = this.Service.AddSheepFieldInformation(this.Name,this.OperatingRange,this.Qualifications,this.Remark);
                   if (!this.ValidateFailedServiceResult<string>(result)) return;
                   this.UpdateNotification();
                   MessageBox.Show("羊场信息成功!", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                   this.CurrentWindow.DialogResult = true;
                });
            }
        }


        private string name;
        [Required(ErrorMessage = "羊场名称必填")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.Validate("Name");
            }
        }
        private string operatingRange;
        [Required(ErrorMessage = "经营范围必填")]
        public string OperatingRange
        {
            get { return operatingRange; }
            set
            {
                operatingRange = value;
                this.Validate("OperatingRange");
            }
        }

        private string qualifications;
        [Required(ErrorMessage = "经营范围必填")]
        public string Qualifications
        {
            get { return qualifications; }
            set
            {
                qualifications = value;
                this.Validate("Qualifications");
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

    }
}
