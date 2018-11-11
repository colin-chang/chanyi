using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Model;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.WPF.UserControls;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;

namespace Chanyi.Shepherd.WPF.ViewModels.GroupManage
{
    class AddMoveSheepfoldViewModel : AddWithTreeViewModel
    {
        public AddMoveSheepfoldViewModel()
        {
            this.InitializeBindData();
        }

        public AddMoveSheepfoldViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            InitializeTreeData(true);
            if (this.isContinue)
                return;

            var principals = this.Service.GetEmployeeBind();
            var sheepfoldBinds = this.Service.GetSheepfoldBind();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
                this.TargetSheepfoldBind.Add(new SheepfoldBind { Name = defaultSelection });
                sheepfoldBinds.ForEach(sf => this.TargetSheepfoldBind.Add(sf));
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

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }



        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "技术员必填")]
        public string PrincipalId
        {
            get { return principalId; }
            set
            {
                principalId = value;
                this.Validate("PrincipalId");
            }
        }


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

        private ObservableCollection<SheepfoldBind> targetSheepfoldBind = new ObservableCollection<SheepfoldBind>();
        public ObservableCollection<SheepfoldBind> TargetSheepfoldBind { get { return targetSheepfoldBind; } }


        private string targetSheepfoldId;
        [EntityProperty]
        [Required(ErrorMessage = "目标圈舍必填")]
        public string TargetSheepfoldId
        {
            get { return targetSheepfoldId; }
            set
            {
                targetSheepfoldId = value;
                this.Validate("TargetSheepfoldId");
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


        private ObservableCollection<SheepBind> sheeps = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Sheeps { get { return sheeps; } }

        public override DelegateCommand<string> NodeClickCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var node = this.GetNodeById(id);
                    if (node == null)
                        return;
                    node.IsSelected = true;
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
                    this.Sheeps.Remove(this.Sheeps.Where(s => s.Id == id).FirstOrDefault());
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
                    if (this.Sheeps.Where(s => s.Id == node.Id).Count() <= 0 && this.Sheeepfolds.Where(sf => sf.Id == node.Id).Count() <= 0)
                        this.Sheeps.Add(new SheepBind { Id = node.Id, SerialNumber = node.Name });
                }
                else
                    this.Sheeps.Remove(this.Sheeps.Where(s => s.Id == node.Id).FirstOrDefault());
            }
            else
            {
                if (node.IsChecked == true)
                    node.Children.Each(n =>
                    {
                        if (this.Sheeps.Where(s => s.Id == n.Id).Count() <= 0)
                            this.Sheeps.Add(new SheepBind { Id = n.Id, SerialNumber = n.Name });
                    });
                else
                    node.Children.Each(n => this.Sheeps.Remove(this.Sheeps.Where(s => s.Id == n.Id).FirstOrDefault()));
            }
        }

        public DelegateCommand SubmitCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (Sheeps.Count() <= 0)
                    {
                        MessageBox.Show(Application.Current.MainWindow, "没有选中任何羊只!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (!this.IsValid)
                    {
                        MessageBox.Show(Application.Current.MainWindow,this.Error, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    Action moveSheepfold = () =>
                    {
                        var result = this.Service.AddMoveSheepfold(this.Sheeps.Select(s => s.Id), this.TargetSheepfoldId, this.PrincipalId, this.UserId,this.Remark);
                        bool isValid = ValidateFailedServiceResult<bool>(result);
                        this.UIDispatcher.Invoke(new Action(() => this.ProgressRing.Hide()), null);
                        if (!isValid)
                        {
                            MessageBox.Show(this.Error, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (this.Continue2Add("转圈操作成功"))
                            return;

                        this.UIDispatcher.Invoke(new Action(() => this.CurrentWindow.DialogResult = true), null);
                    };
                    this.ProgressRing.Show();
                    moveSheepfold.BeginInvoke(ar => moveSheepfold.EndInvoke(ar as IAsyncResult), moveSheepfold);
                });
            }
        }

        protected override void Reset()
        {
            base.Reset();
            this.UIDispatcher.Invoke(new Action(()=>this.Sheeps.Clear()),null);
        }
    }
}
