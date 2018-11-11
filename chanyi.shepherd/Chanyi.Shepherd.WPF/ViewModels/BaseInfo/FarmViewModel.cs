using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.Model.BaseInfo;

namespace Chanyi.Shepherd.WPF.ViewModels.BaseInfo
{
    class FarmViewModel : AddViewModel
    {
        public FarmViewModel()
        {
            this.InitializeBindData();
        }

        protected override void InitializeBindData()
        {
            Action initialize = () =>
            {
                var farm = this.Service.GetFarm();
                if (farm == null) return;
                this.Name = farm.Name;
                this.Contacts = farm.Contacts;
                this.Phone = farm.Phone;
                this.Address = farm.Address;
                this.Code = farm.Code;
                this.BusinessScope = farm.BusinessScope;
                this.Qualifications = farm.Qualifications;
                this.Remark = farm.Remark;
            };
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);
        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.EditFarm(this.Name, this.Contacts, this.Phone, this.Address, this.Code, this.BusinessScope, this.Qualifications, this.Remark);
                    if (!this.ValidateFailedServiceResult<bool>(result)) return;
                    this.UpdateNotification();
                    MessageBox.Show("羊场信息修改成功!", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private string contacts;
        [Required(ErrorMessage = "联系人必填")]
        public string Contacts
        {
            get { return contacts; }
            set
            {
                contacts = value;
                this.RaisePropertyChanged("Contacts");
            }
        }

        private string phone;
        [Required(ErrorMessage = "手机号必填")]
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                this.RaisePropertyChanged("Phone");
            }
        }

        private string address;
        [Required(ErrorMessage = "地址必填")]
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                this.RaisePropertyChanged("Address");
            }
        }

        private string code;
        [Required(ErrorMessage = "编码规范必填")]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                this.RaisePropertyChanged("Code");
            }
        }

        private string businessScope;
        [Required(ErrorMessage = "经营范围必填")]
        public string BusinessScope
        {
            get { return businessScope; }
            set
            {
                businessScope = value;
                this.Validate("BusinessScope");
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
