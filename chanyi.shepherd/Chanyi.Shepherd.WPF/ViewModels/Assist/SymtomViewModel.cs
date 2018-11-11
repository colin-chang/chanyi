using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Commands;
using AutoMapper;

using Chanyi.Shepherd.QueryModel.Model.Assist;
using Chanyi.Shepherd.WPF.Model;
using Chanyi.Shepherd.QueryModel.Filter.Assist;
using Chanyi.Shepherd.WPF.Views.Assist;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.QueryModel.BindingModel;

namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    class SymtomViewModel : SicknessViewModel
    {
        public SymtomViewModel()
        {
            this.InitializeBindData();
            Mapper.CreateMap<SymptomTypeBind, Node>();
            Mapper.CreateMap<SymptomBind, Node>();
        }

        private ObservableCollection<Symptom> symptoms;

        public ObservableCollection<Symptom> Symptoms
        {
            get
            {
                if (symptoms == null)
                    symptoms = new ObservableCollection<Symptom>();
                return symptoms;
            }
            set
            {
                symptoms = value;
                this.RaisePropertyChanged("Symptoms");
            }
        }


        protected override void InitializeBindData()
        {
            if (this.IsInDesignMode) return;
            Action initilize = () =>
            {
                var treeData = new ObservableCollection<Node>();
                var sts = this.Service.GetSymptomTypeBind();
                sts.Keys.ToList().ForEach(st =>
                {
                    var node = Mapper.Map<Node>(st);
                    sts[st].ForEach(s => node.Children.Add(Mapper.Map<Node>(s)));
                    treeData.Add(node);
                });
                this.TreeData = treeData;
            };
            initilize.BeginInvoke(ar => initilize.EndInvoke(ar as IAsyncResult), initilize);
        }

        public override DelegateCommand<Button> SearchCommand
        {
            get
            {
                return new DelegateCommand<Button>(btn =>
                {
                    btn.Focus();
                    if (string.IsNullOrWhiteSpace(this.KeyWord))
                        this.InitializeBindData();
                    else
                        this.TreeData = new ObservableCollection<Node>(this.Service.GetSymptomBind(new SymptomBindFilter { Name = this.KeyWord }).Select(s => Mapper.Map<Node>(s)));
                });
            }
        }

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
                    this.SetNodeChecked(node);
                });
            }
        }

        /// <summary>
        /// 设置节点选中状态
        /// </summary>
        /// <param name="node"></param>
        void SetNodeChecked(Node node)
        {
            TreeData.Each(n =>
            {
                if (n.Children.Count() > 0 && n.Children.Contains(node))
                {
                    int count = n.Children.Where(c => c.IsChecked == true).Count();
                    if (count <= 0)
                        n.IsChecked = false;
                    else if (count < n.Children.Count())
                        n.IsChecked = null;
                    else
                        n.IsChecked = true;
                }
            });
            if (node.Children.Count() <= 0)
            {
                if (node.IsChecked == true)
                {
                    if (this.Symptoms.Where(s => s.Id == node.Id).Count() <= 0)
                        this.Symptoms.Add(new Symptom { Id = node.Id, Name = node.Name });
                }
                else
                    this.Symptoms.Remove(this.Symptoms.Where(s => s.Id == node.Id).FirstOrDefault());
            }
            else
            {
                if (node.IsChecked == true)
                    node.Children.Each(n =>
                    {
                        if (this.Symptoms.Where(s => s.Id == n.Id).Count() <= 0)
                            this.Symptoms.Add(new Symptom { Id = n.Id, Name = n.Name });
                    });
                else
                    node.Children.Each(n => this.Symptoms.Remove(this.Symptoms.Where(s => s.Id == n.Id).FirstOrDefault()));
            }
        }

        public DelegateCommand<string> RemoveSymptopmCommand
        {
            get
            {
                return new DelegateCommand<string>(id =>
                {
                    this.Symptoms.Remove(this.Symptoms.Where(s => s.Id == id).FirstOrDefault());
                    var node = this.GetNodeById(id);
                    if (node == null)
                        return;
                    node.IsChecked = false;
                    this.SetNodeChecked(node);
                });
            }
        }

        public DelegateCommand<SymptomWindow> SubmitCommand
        {
            get
            {
                return new DelegateCommand<SymptomWindow>(win =>
                {
                    win.Symptoms = this.Symptoms;
                    win.DialogResult = true;
                });
            }
        }


        public new DelegateCommand<Window> CancelCommand { get { return new DelegateCommand<Window>(win => win.DialogResult = false); } }
    }
}
