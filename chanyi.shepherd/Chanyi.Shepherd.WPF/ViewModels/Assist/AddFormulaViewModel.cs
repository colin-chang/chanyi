using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.Model.Formula;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Model;
using Chanyi.Shepherd.WPF.UserControls;
using System.Windows;

namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    class AddFormulaViewModel : FormViewModel
    {
        string nutrientId;
        ProgressRing pg;
        public ActionResultEnum ActionResult { get; set; }

        public AddFormulaViewModel(string nutrientId, ProgressRing pg)
        {
            this.nutrientId = nutrientId;
            this.pg = pg;

            Mapper.CreateMap<FeedWithAllFileds, FeedWithAllFiledsData>();
            this.InitializeBindData();
        }

        protected override void InitializeBindData()
        {
            Action initialize = () =>
            {
                var nutrient = this.Service.GetFormulaNutrientById(this.nutrientId);
                if (nutrient != null)
                    this.Nutrient.Add(nutrient);

                var feeds = this.Service.GetFeedWithAllFileds();
                this.UIDispatcher.Invoke(new Action(() => feeds.ForEach(f => this.Feeds.Add(Mapper.Map<FeedWithAllFiledsData>(f)))), null);
                this.UIDispatcher.Invoke(new Action(() => this.pg.Hide()), null);
            };
            this.pg.Show();
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);
        }

        private List<FormulaNutrient> nutrient = new List<FormulaNutrient>();
        public List<FormulaNutrient> Nutrient
        {
            get { return nutrient; }
            set
            {
                nutrient = value;
                this.RaisePropertyChanged("Nutrient");
            }
        }

        private ObservableCollection<FeedWithAllFiledsData> feeds = new ObservableCollection<FeedWithAllFiledsData>();
        public ObservableCollection<FeedWithAllFiledsData> Feeds { get { return feeds; } }

        private ObservableCollection<FeedWithAllFiledsData> selectedFeeds = new ObservableCollection<FeedWithAllFiledsData>();
        public ObservableCollection<FeedWithAllFiledsData> SelectedFeeds { get { return selectedFeeds; } }

        private ObservableCollection<Nutrient> differentces = new ObservableCollection<Nutrient>();
        public ObservableCollection<Nutrient> Differentces { get { return differentces; } }

        public DelegateCommand<string> SourceSelectCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var feed = this.Feeds.Where(f => f.Id == id).FirstOrDefault();
                    feed.Dosage = 0;
                    if (feed == null)
                        return;
                    this.UIDispatcher.Invoke(new Action(() =>
                    {
                        bool isChecked = this.SelectedFeeds.Where(f => f.Id == id).Count() > 0;
                        if (feed.IsChecked)
                        {
                            if (!isChecked)
                                this.SelectedFeeds.Add(feed);
                        }
                        else
                        {
                            if (isChecked)
                                this.SelectedFeeds.Remove(feed);
                        }
                    }), null);
                    this.ValidateFeed();
                });
            }
        }

        public DelegateCommand<string> DosageChangeCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    Action calc = () =>
                    {
                        var nutrient = this.Nutrient.FirstOrDefault();
                        if (nutrient == null)
                            return;
                        var dif = new Nutrient();
                        foreach (var p in dif.GetType().GetProperties())
                        {
                            object target = nutrient.GetType().GetProperty(p.Name).GetValue(nutrient, null);
                            if (target == null)
                                continue;
                            float targetVal = float.Parse(target.ToString());

                            float sum = 0;
                            foreach (var feed in this.SelectedFeeds)
                            {
                                var prop = feed.GetType().GetProperty(p.Name);
                                if (prop == null)
                                    continue;
                                object purity = prop.GetValue(feed, null);
                                if (purity == null)
                                    continue;

                                object dosage = feed.GetType().GetProperty("Dosage").GetValue(feed, null);
                                if (dosage == null)
                                    continue;
                                float num = float.Parse(dosage.ToString());

                                sum += num * float.Parse(purity.ToString());
                            }

                            p.SetValue(dif, targetVal - sum, null);
                        }
                        this.UIDispatcher.Invoke(new Action(() =>
                        {
                            this.Differentces.Clear();
                            this.Differentces.Add(dif);
                        }), null);
                    };
                    calc.BeginInvoke(ar => calc.EndInvoke(ar as IAsyncResult), calc);
                    this.ValidateFeed();
                });
            }
        }

        private string name;
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

        public string SideEffect
        {
            get { return sideEffect; }
            set
            {
                sideEffect = value;
                this.RaisePropertyChanged("SideEffect");
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

        public DelegateCommand SubmitCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (!ValidateFeed())
                        return;

                    if (!IsValid)
                        return;

                    Dictionary<string, float> dict = new Dictionary<string, float>();
                    SelectedFeeds.ToList().ForEach(f => dict.Add(f.Id, f.Dosage));
                    var result = this.Service.AddFormula(dict, this.Name, this.Apply2, this.SideEffect, this.UserId, this.UserId, this.Remark);
                    if (result.Status == IServices.ResultStatus.WANING)
                    {
                        this.SetError("Waring", result.Message, true);
                        return;
                    }
                    else
                        this.SetError("Waring", null, true);

                    this.ActionResult = result.Status == IServices.ResultStatus.OK ? ActionResultEnum.OK : ActionResultEnum.Error;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

        public override DelegateCommand CancelCommand
        {
            get
            {
                this.ActionResult = ActionResultEnum.Cancel;
                return base.CancelCommand;
            }
        }

        bool ValidateFeed()
        {
            bool result = true;

            if (this.SelectedFeeds.Count() <= 0)
            {
                this.SetError("NoFeed", "至少需要选择一种饲料");
                result = false;
            }
            else
                this.SetError("NoFeed", null);

            if (this.SelectedFeeds.Select(f => f.Dosage).Where(d => d <= 0).Count() > 0)
            {
                this.SetError("InvalidNumber", "饲料用量必须大于零");
                result = false;
            }
            else
                this.SetError("InvalidNumber", null);

            return result;
        }
    }
}
