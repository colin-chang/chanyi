using System;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using AutoMapper;
using Chanyi.Shepherd.WPF.Helper;
using Chanyi.Shepherd.WPF.Views.Assist;
using Chanyi.Shepherd.WPF.Model;
using System.Configuration;
using System.Windows.Controls;
using Chanyi.Shepherd.QueryModel.BindingModel;

namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    class DiseaseViewModel : SicknessViewModel
    {
        string defaultDisease = ConfigurationManager.AppSettings["defaultDiseaseDesc"];

        public DiseaseViewModel()
        {
            this.InitializeBindData();
            Mapper.CreateMap<DiseaseTypeBind, Node>();
            Mapper.CreateMap<DiseaseBind, Node>();
        }

        private string symptoms;

        public string Symptoms
        {
            get { return symptoms; }
            set
            {
                symptoms = value;
                this.RaisePropertyChanged("Symptoms");
            }
        }

        protected override void InitializeBindData()
        {
            if (this.IsInDesignMode) return;
            Action initialize = () =>
            {
                var treeData = new ObservableCollection<Node>();
                var dts = this.Service.GetDiseaseTypeBind();
                dts.Keys.ToList().ForEach(t =>
                {
                    var node = Mapper.Map<Node>(t);
                    dts[t].ForEach(d => node.Children.Add(Mapper.Map<Node>(d)));
                    treeData.Add(node);
                });
                this.TreeData = treeData;
            };
            initialize.BeginInvoke(ar => initialize.EndInvoke(ar as IAsyncResult), initialize);
        }

        public override DelegateCommand<Button> SearchCommand
        {
            get
            {
                return new DelegateCommand<Button>(btn =>
                {
                    btn.Focus();
                    this.DiseaseDesc = null;
                    this.Symptoms = null;
                    if (string.IsNullOrWhiteSpace(this.KeyWord))
                        this.InitializeBindData();
                    else
                    {
                        Action search = () => this.TreeData = new ObservableCollection<Node>(this.Service.GetDiseaseBindByName(this.KeyWord.Trim()).Select(d => Mapper.Map<Node>(d)));
                        search.BeginInvoke(ar => search.EndInvoke(ar as IAsyncResult), search);
                    }
                });
            }
        }

        public DelegateCommand MoreConditionCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SymptomWindow win = new SymptomWindow { Owner=this.CurrentWindow};
                    if (win.ShowDialog() == true)
                    {
                        this.KeyWord = null;
                        this.DiseaseDesc = null;
                        Action search = () =>
                        {
                            this.Symptoms = string.Join("，", win.Symptoms.Select(s => s.Name));
                            this.TreeData = new ObservableCollection<Node>(this.Service.GetDiseaseBindBySymptomIds(win.Symptoms.Select(s => s.Id).ToArray()).Select(d => Mapper.Map<Node>(d)));
                        };
                        search.BeginInvoke(ar => search.EndInvoke(ar as IAsyncResult), search);
                    }
                });
            }
        }

        private string diseaseDesc;

        public string DiseaseDesc
        {
            get
            {
                if (string.IsNullOrWhiteSpace(diseaseDesc))
                    diseaseDesc = this.defaultDisease;
                return diseaseDesc;
            }
            set
            {
                diseaseDesc = value;
                this.RaisePropertyChanged("DiseaseDesc");
            }
        }

        public override DelegateCommand<string> NodeClickCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    var node = GetNodeById(id);
                    if (node == null)
                        return;

                    node.IsSelected = true;

                    if (node.Children.Count() <= 0)
                        this.DiseaseDesc = XmlHelper.GetDiseaseDesc(id);
                });
            }
        }
    }
}
