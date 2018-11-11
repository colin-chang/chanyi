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
using Chanyi.Shepherd.QueryModel.Model.Input;



namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    class AddCustomFormulaViewModel : AddViewModel
    {
        public AddCustomFormulaViewModel()
        {
            Mapper.CreateMap<SimpleFeed, SimpleFeedData>();
            Mapper.CreateMap<SimpleFeedData, FormulaFeedData>();
            this.InitializeBindData();
        }

        public AddCustomFormulaViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            if (this.isContinue)
                return;

            var principals = this.Service.GetEmployeeBind();
            var feeds = this.Service.GetSimpleFeed();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Principals.Add(new EmployeeBind { Name = this.defaultSelection });
                principals.ForEach(p => this.Principals.Add(p));
                feeds.ForEach(f => this.Feeds.Add(Mapper.Map<SimpleFeedData>(f)));
            }), null);
        }

        private string name;
        [EntityProperty]
        [Required(ErrorMessage = "名称必填")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.Validate("Name");
            }
        }

        private string apply2;
        [EntityProperty]
        [Required(ErrorMessage = "适用于必填")]
        public string Apply2
        {
            get { return apply2; }
            set
            {
                apply2 = value;
                this.Validate("Apply2");
            }
        }

        private string sideEffect;
        [EntityProperty]
        public string SideEffect
        {
            get { return sideEffect; }
            set
            {
                sideEffect = value;
                this.Validate("SideEffect");
            }
        }

        private string principalId;
        [EntityProperty]
        [Required(ErrorMessage = "制作人必填")]
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

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private ObservableCollection<SimpleFeedData> feeds = new ObservableCollection<SimpleFeedData>();
        public ObservableCollection<SimpleFeedData> Feeds { get { return feeds; } }

        private ObservableCollection<FormulaFeedData> formulaFeeds = new ObservableCollection<FormulaFeedData>();
        [EntityProperty]
        public ObservableCollection<FormulaFeedData> FormulaFeeds { get { return formulaFeeds; } }

        public DelegateCommand<string> SourceSelectCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var selectedItem = this.Feeds.Where(f => f.Id == id).FirstOrDefault();
                    if (selectedItem == null)
                        return;
                    if (selectedItem.IsChecked)
                        this.FormulaFeeds.Add(Mapper.Map<FormulaFeedData>(selectedItem));
                    else
                    {
                        var current = this.FormulaFeeds.Where(f => f.Id == id).FirstOrDefault();
                        if (current == null)
                            return;
                        this.FormulaFeeds.Remove(current);
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
                    var current = this.FormulaFeeds.Where(f => f.Id == id).FirstOrDefault();
                    if (current == null)
                        return;
                    this.FormulaFeeds.Remove(current);
                    var partner = this.Feeds.Where(f => f.Id == id).FirstOrDefault();
                    if (partner == null)
                        return;
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
                    if (this.FormulaFeeds.Count() <= 0)
                    {
                        MessageBox.Show("需要至少一种饲料!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (this.formulaFeeds.Where(f => f.Amount <= 0).Count() > 0)
                    {
                        MessageBox.Show("饲料用量不合法!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    Dictionary<string, float> dict = new Dictionary<string, float>();
                    this.FormulaFeeds.Each(f => dict[f.Id] = f.Amount);
                    var result = this.Service.AddFormula(dict, this.Name, this.Apply2, this.SideEffect, this.PrincipalId, this.UserId, this.Remark);

                    if (result.Status == IServices.ResultStatus.WANING)
                    {
                        MessageBox.Show(result.Message,"错误",MessageBoxButton.OK,MessageBoxImage.Warning);
                        return;
                    }
                    if (!ValidateFailedServiceResult<string>(result))
                        return;
                    if (this.Continue2Add("自定义配方添加成功"))
                        return;

                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

        protected override void Reset()
        {
            base.Reset();
            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.formulaFeeds.Clear();
                this.Feeds.Where(f => f.IsChecked).ToList().ForEach(f => f.IsChecked = false);
            }), null);
        }
    }
}
