using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.WPF.Model;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.WPF.UserControls;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Utility.Common;
using Chanyi.Shepherd.WPF.Views.HR;
using Chanyi.Shepherd.QueryModel.AddModel.Finance;
using Chanyi.Shepherd.WPF.Model.Finance;
using Chanyi.Shepherd.QueryModel.BindingModel;

namespace Chanyi.Shepherd.WPF.ViewModels.Finance
{
    class AddSellSheepAllViewModel : AddWithTreeViewModel
    {
        public AddSellSheepAllViewModel()
        {
            Mapper.CreateMap<SellAllSheepData, AddSellSheep>();
            this.InitializeBindData();
        }
        public AddSellSheepAllViewModel(bool withinInitilization) { }
        protected override void InitializeBindItem()
        {
            InitializeTreeData(true);
            if (this.isContinue)
                return;
            var purchasers = this.Service.GetPurchaserBind();
            var principals = this.Service.GetEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = this.defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
                this.Purchasers.Add(new PurchaserBind { Name = defaultSelection });
                purchasers.ForEach(p => this.Purchasers.Add(p));
            }), null);
        }
        /// <summary>
        /// 初始化Sheeepfolds和TreeData
        /// </summary>
        void InitializeTreeData(bool initializeSheepfolds)
        {
            var msfs = this.Service.GetMoveSheepfoldBind();
            if (initializeSheepfolds)
                this.Sheeepfolds = new ObservableCollection<SheepfoldBind>(msfs.Keys);
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.TreeData.Clear();
                msfs.Keys.ToList().ForEach(k =>
                {
                    if (msfs[k].Count() > 0)
                    {
                        var node = new NodeData { Id = k.Id, Name = k.Name };
                        msfs[k].ForEach(s => node.Children.Add(new NodeData { Id = s.Id, Name = s.SerialNumber }));
                        this.TreeData.Add(node);
                    }
                });
            }), null);
        }
        public ProgressRing ProgressRing { get; set; }

        #region 绑定字段必填字段
        private ObservableCollection<PurchaserBind> purchasers = new ObservableCollection<PurchaserBind>();
        public ObservableCollection<PurchaserBind> Purchasers { get { return purchasers; } }

        private string purchaserId;
        [EntityProperty]
        [Required(ErrorMessage = "购买者内容必填")]
        public string PurchaserId
        {
            get { return purchaserId; }
            set
            {
                purchaserId = value;
                this.Validate("PurchaserId");
            }
        }
        private DateTime? operationDate;
        [EntityProperty]
        [Required(ErrorMessage = "出售日期必填")]
        public DateTime? OperationDate
        {
            get { return operationDate; }
            set
            {
                operationDate = value;
                this.Validate("OperationDate");
            }
        }

        private string totalPrice;
        [EntityProperty]
        [Required(ErrorMessage = "总价格必填")]
        [StringToDecimal(ErrorMessage = "总价格输入有误！")]
        public string TotalPrice
        {
            get { return totalPrice; }
            set
            {
                totalPrice = value;
                this.Validate("TotalPrice");
            }
        }

        private string totalWeight;
        [EntityProperty]
        [StringToFloat(ErrorMessage = "总重量输入有误！")]
        [Required(ErrorMessage = "总价格必填")]
        public string TotalWeight
        {
            get { return totalWeight; }
            set
            {
                totalWeight = value;
                this.Validate("TotalWeight");
            }
        }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "操作人必填")]
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
        #endregion

        #region 搜索字段
        private string sheepfoldId;

        [EntityProperty]
        public string SheepfoldId
        {
            get { return sheepfoldId; }
            set
            {
                sheepfoldId = value;
                this.RaisePropertyChanged("SheepfoldId");
            }
        }

        private string serialNumber;
        [EntityProperty]
        public string SerialNumber
        {
            get { return serialNumber; }
            set
            {
                serialNumber = value;
                this.RaisePropertyChanged("SerialNumber");
            }
        }
        #endregion

        private ObservableCollection<SheepfoldBind> sheepfolds;
        public ObservableCollection<SheepfoldBind> Sheeepfolds
        {
            get
            {
                return sheepfolds;
            }
            set
            {
                sheepfolds = value;
                this.RaisePropertyChanged("Sheepfolds");
                this.RaisePropertyChanged("SheepfoldsBind");
            }
        }
        public ObservableCollection<SheepfoldBind> SheepfoldsBind
        {
            get
            {
                var sfs = this.Sheeepfolds;
                if (sfs != null)
                {
                    sfs = new ObservableCollection<SheepfoldBind>(sfs);
                    sfs.Insert(0, new SheepfoldBind { Name = this.defaultSelection });
                }
                return sfs;
            }
        }
        public DelegateCommand<Button> SearchCommand
        {
            get
            {
                return new DelegateCommand<Button>(btn =>
                {
                    btn.Focus();
                    if (string.IsNullOrWhiteSpace(this.SerialNumber) && this.SheepfoldId == null)
                        InitializeTreeData(false);
                    else
                        this.TreeData = new ObservableCollection<NodeData>(this.Service.GetSheepBind(new SheepBindFilter { SheepfoldId = this.SheepfoldId, SerialNumber = this.SerialNumber }).Select(s => new NodeData { Id = s.Id, Name = s.SerialNumber }));
                });
            }
        }

        private ObservableCollection<SellAllSheepData> sheeps = new ObservableCollection<SellAllSheepData>();
        public ObservableCollection<SellAllSheepData> Sheeps { get { return sheeps; } }
        public override DelegateCommand<string> NodeClickCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var node = this.GetNodeById(id);
                    if (node == null)
                        return;

                    this.SetNodeChecked(node, SetSelectedItems);
                });
            }
        }

        public override DelegateCommand<string> RemoveSymptopmCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    this.Sheeps.Remove(this.Sheeps.Where(s => s.SheepId == id).FirstOrDefault());
                    var node = this.GetNodeById(id);
                    if (node == null) return;
                    node.IsChecked = false;
                    this.SetNodeChecked(node, SetSelectedItems);
                });
            }
        }

        //同步选中羊只
        void SetSelectedItems(NodeData node)
        {
            if (node.Children.Count() <= 0)
            {
                if (node.IsChecked == true)
                {
                    if (this.Sheeps.Where(s => s.SheepId == node.Id).Count() <= 0 && this.Sheeepfolds.Where(sf => sf.Id == node.Id).Count() <= 0)
                    {
                        this.Sheeps.Add(new SellAllSheepData { SheepId = node.Id, SerialNumber = node.Name });
                    }
                }
                else
                {
                    this.Sheeps.Remove(this.Sheeps.Where(s => s.SheepId == node.Id).FirstOrDefault());
                }
            }
            else
            {
                if (node.IsChecked == true)
                    node.Children.Each(n =>
                    {
                        if (this.Sheeps.Where(s => s.SheepId == n.Id).Count() <= 0)
                        {
                            this.Sheeps.Add(new SellAllSheepData { SheepId = n.Id, SerialNumber = n.Name });
                        }

                    });
                else
                {
                    node.Children.Each(n => this.Sheeps.Remove(this.Sheeps.Where(s => s.SheepId == n.Id).FirstOrDefault()));
                }
            }
        }
        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return new DelegateCommand<UIElement>(btn =>
                {
                    if (this.Sheeps.Count() <= 0)
                    {
                        MessageBox.Show(Application.Current.MainWindow, "没有选中任何羊只!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (!this.IsValid)
                    {
                        MessageBox.Show(Application.Current.MainWindow, this.Error, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Action sellSheep = () =>
                    {
                        List<AddSellSheep> list = new List<AddSellSheep>();
                        this.Sheeps.Each(s => list.Add(Mapper.Map<AddSellSheep>(s)));
                        list.Each(s =>
                        {
                            s.Price = Decimal.Parse(this.TotalPrice) / list.Count();
                            s.Weight = float.Parse(this.TotalWeight) / list.Count();
                        });
                        var result = this.Service.AddSellSheep(list, decimal.Parse(this.TotalPrice), float.Parse(this.TotalWeight), this.PurchaserId, (DateTime)this.OperationDate, this.PrincipalId, this.UserId, this.Remark);

                        this.UIDispatcher.Invoke(new Action(() => this.ProgressRing.Hide()), null);
                        if (!ValidateFailedServiceResult<int>(result))
                        {
                            MessageBox.Show(this.Error, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (this.Continue2Add("添加出售羊记录成功"))
                        {
                            return;
                        }
                        this.UIDispatcher.Invoke(new Action(() => this.CurrentWindow.DialogResult = true), null);
                    };
                    this.ProgressRing.Show();
                    sellSheep.BeginInvoke(ar => sellSheep.EndInvoke(ar as IAsyncResult), sellSheep);
                    this.UpdateNotification();
                });
            }
        }
        protected override void Reset()
        {
            base.Reset();
            this.UIDispatcher.Invoke(new Action(() => this.Sheeps.Clear()), null);
        }
        /// <summary>
        /// 添加购买者
        /// </summary>
        public DelegateCommand AddPurchaserCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddPurchaserWindow win = new AddPurchaserWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                    {
                        var purchasers = this.Service.GetPurchaserBind();
                        this.UIDispatcher.Invoke(new Action(() =>
                        {
                            this.Purchasers.Clear();
                            this.Purchasers.Add(new PurchaserBind { Name = defaultSelection });
                            purchasers.ForEach(m => this.Purchasers.Add(m));
                            this.PurchaserId = this.Purchasers.FirstOrDefault() == null ? null : this.Purchasers.FirstOrDefault().Id;
                        }), null);
                    }
                });
            }
        }
    }
}
