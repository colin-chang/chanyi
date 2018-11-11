
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.AddModel.Finance;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Model;
using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class AddSellFeedViewModel : AddViewModel
    {
        public AddSellFeedViewModel()
        {
            Mapper.CreateMap<SellFeedBind, SellFeedData>();
            Mapper.CreateMap<SellFeedBind, SellFeedChargeData>();
            Mapper.CreateMap<SellFeedChargeData, AddSellInput>();
            this.InitializeBindData();
        }
        public AddSellFeedViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            if (this.isContinue)
                return;

            var principals = this.Service.GetEmployeeBind();
            var purchasers = this.Service.GetPurchaserBind();
            var sellFeeds = this.Service.GetSellFeedBind4Add();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = this.defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
                this.Purchasers.Insert(0, new PurchaserBind { Name = defaultSelection });
                purchasers.ForEach(p => this.Purchasers.Add(p));
                sellFeeds.ForEach(bf => this.SellFeeds.Add(Mapper.Map<SellFeedData>(bf)));
            }), null);
        }

        private ObservableCollection<SellFeedData> sellFeeds = new ObservableCollection<SellFeedData>();
        public ObservableCollection<SellFeedData> SellFeeds { get { return sellFeeds; } }

        private ObservableCollection<SellFeedChargeData> sellFeedsCharge = new ObservableCollection<SellFeedChargeData>();
        public ObservableCollection<SellFeedChargeData> SellFeedsCharge { get { return sellFeedsCharge; } }

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
                    var selectItem = this.SellFeeds.Where(bf => bf.Id == id).FirstOrDefault();
                    if (selectItem == null)
                        return;
                    if (selectItem.IsChecked)
                    {
                        this.SellFeedsCharge.Add(Mapper.Map<SellFeedChargeData>(selectItem));
                    }
                    else
                    {
                        var current = this.SellFeedsCharge.Where(bf => bf.Id == id).FirstOrDefault();
                        if (current == null) return;
                        this.SellFeedsCharge.Remove(current);
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
                    var current = this.SellFeedsCharge.Where(bf => bf.Id == id).FirstOrDefault();
                    if (current == null) return;
                    this.SellFeedsCharge.Remove(current);
                    var partner = this.SellFeeds.Where(bf => bf.Id == id).FirstOrDefault();
                    if (partner == null) return;
                    partner.IsChecked = false;
                });
            }
        }
        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return new DelegateCommand<UIElement>(btn =>
                {

                    if (this.SellFeedsCharge.Count <= 0)
                    {
                        MessageBox.Show("需要至少一种饲料!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (this.SellFeedsCharge.Where(sf => !(sf.Price > 0 && sf.Price.ToString().IsDecimal())).Count() > 0)
                    {
                        this.errors["Price"] = "价格输入不合法";
                        this.RaisePropertyChanged("Error");
                    }

                    if (!this.IsValid)
                    {
                        MessageBox.Show(Application.Current.MainWindow, this.Error, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        if (this.errors.Keys.Select(k => k == "Price").Count() > 0)
                        {
                            this.errors.Remove("Price");
                        }
                        return;
                    }


                    List<AddSellInput> list = new List<AddSellInput>();

                    this.SellFeedsCharge.Each(s => list.Add(Mapper.Map<AddSellInput>(s)));
                    var result = this.Service.AddSellFeed(list, this.UserId, this.Remark);

                    if (!ValidateFailedServiceResult<int>(result))
                        return;

                    MessageBox.Show(result.Result + "条记录添加成功", "成功", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }
    }
}
