using Chanyi.Shepherd.IServices;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.BindingModel;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    class TwoMatingViewModel : FormViewModel
    {
        /// <summary>
        /// CoboBox默认文本
        /// </summary>
        protected string defaultSelection = ConfigurationManager.AppSettings["formDefaultSelection"];

        public TwoMatingViewModel()
        {
            this.InitializeBindItem();
        }
        public TwoMatingViewModel(bool withinInitilization) { }

        protected override void InitializeBindItem()
        {
            List<SheepBind> studsheeps = this.Service.GetStudSheepBind();
            var males = studsheeps.Where(s => s.Gender == GenderEnum.Male).ToList();
            var females = studsheeps.Where(s => s.Gender == GenderEnum.Female).ToList();

            this.UIDispatcher.Invoke(new Action(() =>
            {
                this.Males.Add(new SheepBind { SerialNumber = defaultSelection });
                males.ForEach(m => this.Males.Add(m));
                this.Females.Add(new SheepBind { SerialNumber = defaultSelection });
                females.ForEach(f => this.Females.Add(f));
            }), null);

        }

        private ObservableCollection<SheepBind> males = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Males { get { return males; } }

        private ObservableCollection<SheepBind> females = new ObservableCollection<SheepBind>();
        public ObservableCollection<SheepBind> Females { get { return females; } }

        private string maleId;

        [Required(ErrorMessage = "配种公羊必填")]
        public string MaleId
        {
            get { return maleId; }
            set
            {
                maleId = value;
                this.Validate("MaleId");
                this.IsOK = false;
            }
        }

        private string femaleId;
        [Required(ErrorMessage = "配种母羊必填")]
        public string FemaleId
        {
            get { return femaleId; }
            set
            {
                femaleId = value;
                this.Validate("FemaleId");
                this.IsOK = false;
            }
        }

        private bool isOK;

        public bool IsOK
        {
            get { return isOK; }
            set
            {
                isOK = value;
                this.RaisePropertyChanged("IsOK");
            }
        }


        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(btn =>
                {
                    ServiceResult<bool> result = this.Service.VarifyTwoSheepsCanMating(this.MaleId, this.FemaleId);
                    if (result.Result)
                    {
                        this.IsOK = true;
                        this.errors["smsg"] = "✔ 这两只羊可以配种";
                    }
                    else if (result.Code == "1")
                    {
                        this.IsOK = false;
                        this.errors["smsg"] = "✘ 两只羊属于"+result.Message+"代近亲不能配种";
                    }
                    else
                    {
                        this.IsOK = false;
                        this.errors["smsg"] = "✘ " + result.Message;
                    }
                    this.RaisePropertyChanged("Error");
                    this.errors.Remove("smsg");
                });
            }
        }
    }
}
