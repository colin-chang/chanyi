using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.QueryModel.UpdateModel.System;

namespace Chanyi.Shepherd.WPF.ViewModels.Tool
{
    class SheepParameterViewModel : AddViewModel
    {
        public SheepParameterViewModel()
        {
            this.InitializeBindData();
        }

        protected override void InitializeBindData()
        {
            Action initialize = () =>
            {
                var paras = this.Service.GetSheepParameters();
                var ablactation = paras.Where(p => p.Type == SettingsEnum.PreAblactationRemaindful).FirstOrDefault();
                var gestation = paras.Where(p => p.Type == SettingsEnum.PreDeliveryRemaindful).FirstOrDefault();

                if (ablactation != null)
                {
                    this.AblactationId = ablactation.Id;
                    this.AblactationDays = ablactation.Value;
                    this.IsAblactationRemaindful = ablactation.IsRemaindful;
                }
                if (gestation != null)
                {
                    this.GestationId = gestation.Id;
                    this.GestationDays = gestation.Value;
                    this.IsGestationRemaindful = gestation.IsRemaindful;
                }
                this.Range = gestation.Range.ToString();
                this.Remark = ablactation.Remark;
            };

            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);

        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.UpdateSheepParameters(new List<UpdateSheepParameter> { 
                        new UpdateSheepParameter { Id = this.AblactationId, Value = this.AblactationDays, IsRemaindful = this.IsAblactationRemaindful,OperatorId=this.UserId, Type=SettingsEnum.PreAblactationRemaindful, Remark=this.Remark },
                        new UpdateSheepParameter{ Id=this.GestationId,Value=this.GestationDays,IsRemaindful=this.IsGestationRemaindful, OperatorId=this.UserId, Type=SettingsEnum.PreDeliveryRemaindful, Remark=this.remark}
                    }, Convert.ToInt32(this.Range));
                    if (!this.ValidateFailedServiceResult<bool>(result)) return;
                    this.UpdateNotification();
                    MessageBox.Show("参数修改成功!","成功",MessageBoxButton.OK,MessageBoxImage.Information);
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
        public string AblactationId { get; set; }


        private string ablactationDays;
        [IntNumber(10, 100, IsNullable = true, ErrorMessage = "断奶日龄不合法")]
        public string AblactationDays
        {
            get { return ablactationDays; }
            set
            {
                ablactationDays = value;
                this.Validate("AblactationDays");
            }
        }

        private bool isAblactationRemaindful;

        public bool IsAblactationRemaindful
        {
            get { return isAblactationRemaindful; }
            set
            {
                isAblactationRemaindful = value;
                this.RaisePropertyChanged("IsAblactationRemaindful");
            }
        }


        public string GestationId { get; set; }

        private string gestationDays;
        [IntNumber(100, 200, IsNullable = true, ErrorMessage = "妊娠天数不合法")]
        public string GestationDays
        {
            get { return gestationDays; }
            set
            {
                gestationDays = value;
                this.Validate("GestationDays");
            }
        }

        private string range;

        [IntNumber(0, 30, IsNullable = true, ErrorMessage = "妊娠误差天数不合法")]
        public string Range
        {
            get { return range; }
            set
            {
                range = value;
                this.Validate("Range");
            }
        }


        private bool isGestationRemaindful;

        public bool IsGestationRemaindful
        {
            get { return isGestationRemaindful; }
            set
            {
                isGestationRemaindful = value;
                this.RaisePropertyChanged("IsGestationRemaindful");
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
