using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Model;


namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class AddBuyOtherViewModel : AddViewModel
    {
        public AddBuyOtherViewModel()
        {
            Mapper.CreateMap<BuyOtherBind, BuyOtherData>();
            Mapper.CreateMap<BuyOtherBind, BuyOtherChargeData>();
            this.InitializeBindData();
        }
        public AddBuyOtherViewModel(bool withinInitilization) { }
        protected override void InitializeBindItem()
        {
            if (this.isContinue)
                return;

            var principals = this.Service.GetEmployeeBind();
            var buyMedicines = this.Service.GetBuyOtherBind4Add();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = this.defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
                buyMedicines.ForEach(bf => this.buyOthers.Add(Mapper.Map<BuyOtherData>(bf)));
            }), null);
        }
        private DateTime? operationDate;
        [EntityProperty]
        [Required(ErrorMessage = "购买时间必选")]
        public DateTime? OperationDate
        {
            get { return operationDate; }
            set
            {
                operationDate = value;
                this.Validate("OperationDate");
            }
        }
        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "购买人必选")]
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
                this.Validate("Remark");
            }
        }

        private ObservableCollection<BuyOtherData> buyOthers = new ObservableCollection<BuyOtherData>();
        public ObservableCollection<BuyOtherData> BuyOthers { get { return buyOthers; } }

        private ObservableCollection<BuyOtherChargeData> buyOthersCharge = new ObservableCollection<BuyOtherChargeData>();
        public ObservableCollection<BuyOtherChargeData> BuyOthersCharge { get { return buyOthersCharge; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }
        public DelegateCommand<string> SourceSelectCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var selectItem = this.BuyOthers.Where(bf => bf.Id == id).FirstOrDefault();
                    if (selectItem == null)
                        return;
                    if (selectItem.IsChecked)
                    {
                        this.BuyOthersCharge.Add(Mapper.Map<BuyOtherChargeData>(selectItem));
                    }
                    else
                    {
                        var current = this.BuyOthersCharge.Where(bf => bf.Id == id).FirstOrDefault();
                        if (current == null) return;
                        this.BuyOthersCharge.Remove(current);
                    }
                });
            }
        }
        public DelegateCommand<string> RemoveCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var current = this.BuyOthersCharge.Where(bf => bf.Id == id).FirstOrDefault();
                    if (current == null) return;
                    this.BuyOthersCharge.Remove(current);
                    var partner = this.BuyOthers.Where(bf => bf.Id == id).FirstOrDefault();
                    if (partner == null) return;
                    partner.IsChecked = false;
                });
            }
        }
        public DelegateCommand SubmitCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (!this.IsValid)
                    {
                        MessageBox.Show(this.Error, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (this.BuyOthersCharge.Count <= 0)
                    {
                        MessageBox.Show("需要至少一种饲料!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (this.BuyOthersCharge.Where(bf => bf.Money <= 0).Count() > 0)
                    {
                        MessageBox.Show("饲料用量不合法!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
                    this.BuyOthersCharge.Each(bf => dict[bf.Id] = bf.Money);
                    var result = this.Service.AddBuyOther(dict, (DateTime)this.OperationDate, this.PrincipalId, this.UserId, this.Remark);

                    if (!ValidateFailedServiceResult<int>(result))
                        return;
                    MessageBox.Show(result.Result + "条记录添加成功", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

    }
}

