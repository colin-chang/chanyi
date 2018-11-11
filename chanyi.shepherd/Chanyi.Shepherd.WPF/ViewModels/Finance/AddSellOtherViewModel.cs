
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.AddModel.Finance;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Model.Finance;
using Chanyi.Utility.Common;
using Chanyi.Shepherd.WPF.Views.HR;
namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class AddSellOtherViewModel: AddViewModel
    {
        public AddSellOtherViewModel()
        {
            Mapper.CreateMap<SellOtherBind, SellOtherData>();
            Mapper.CreateMap<SellOtherBind, SellOtherChargeData>();
            Mapper.CreateMap<SellOtherChargeData, AddSellInput>();
            this.InitializeBindData();
        }
        public AddSellOtherViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            if (this.isContinue)
                return;

            var principals = this.Service.GetEmployeeBind();
            var purchasers = this.Service.GetPurchaserBind();
            var sellOthers = this.Service.GetSellOtherBind4Add();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = this.defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
                this.Purchasers.Insert(0, new PurchaserBind { Name = defaultSelection });
                purchasers.ForEach(p => this.Purchasers.Add(p));
                sellOthers.ForEach(bf => this.SellOthers.Add(Mapper.Map<SellOtherData>(bf)));
            }), null);
        }

        private ObservableCollection<SellOtherData> sellOthers = new ObservableCollection<SellOtherData>();
        public ObservableCollection<SellOtherData> SellOthers { get { return sellOthers; } }

        private ObservableCollection<SellOtherChargeData> sellOthersCharge = new ObservableCollection<SellOtherChargeData>();
        public ObservableCollection<SellOtherChargeData> SellOthersCharge { get { return sellOthersCharge; } }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private ObservableCollection<PurchaserBind> purchasers = new ObservableCollection<PurchaserBind>();
        public ObservableCollection<PurchaserBind> Purchasers { get { return purchasers; } }


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
        
        public DelegateCommand<string> SourceSelectCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var selectItem = this.SellOthers.Where(bf => bf.Id == id).FirstOrDefault();
                    if (selectItem == null)
                        return;
                    if (selectItem.IsChecked)
                    {
                        this.SellOthersCharge.Add(Mapper.Map<SellOtherChargeData>(selectItem));
                    }
                    else
                    {
                        var current = this.SellOthersCharge.Where(bf => bf.Id == id).FirstOrDefault();
                        if (current == null) return;
                        this.SellOthersCharge.Remove(current);
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
                    var current = this.SellOthersCharge.Where(bf => bf.Id == id).FirstOrDefault();
                    if (current == null) return;
                    this.SellOthersCharge.Remove(current);
                    var partner = this.SellOthers.Where(bf => bf.Id == id).FirstOrDefault();
                    if (partner == null) return;
                    partner.IsChecked = false;
                });
            }
        }
        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(btn =>
                {
                    if (this.SellOthersCharge.Count <= 0)
                    {
                        MessageBox.Show("需要至少一种物品!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (this.SellOthersCharge.Where(sf => !(sf.Price>0&&sf.Price.ToString().IsDecimal())).Count() > 0)
                    {
                        this.errors["Price"] = "价格输入不合法";
                        this.RaisePropertyChanged("Error");
                        return;
                    }

                    if (!this.IsValid)
                    {
                        MessageBox.Show(Application.Current.MainWindow,this.Error, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        if (this.errors.Keys.Select(k => k == "Price").Count() > 0)
                        {
                            this.errors.Remove("Price");
                        }
                        return;
                    }
                    List<AddSellInput> list = new List<AddSellInput>();

                    this.SellOthersCharge.Each(s =>list.Add(Mapper.Map<AddSellInput>(s)));
                    var result = this.Service.AddSellOther(list, this.UserId, this.Remark);

                    if (!ValidateFailedServiceResult<int>(result))
                        return;

                    MessageBox.Show(result.Result + "条记录添加成功", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
 
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

    }
}

