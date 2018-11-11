
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;
using Chanyi.Shepherd.QueryModel.Filter.BaseInfo;
using AutoMapper;
using Microsoft.Practices.Prism.Commands;
using Chanyi.Shepherd.WPF.Views.BaseInfo;
using Chanyi.Shepherd.QueryModel.BindingModel;
using System.ComponentModel.DataAnnotations;

namespace Chanyi.Shepherd.WPF.ViewModels.BaseInfo
{
    public class SheepfoldViewModel : ListViewModel
    {
        public SheepfoldViewModel(bool withinInitilization) { }
        public SheepfoldViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;

            Mapper.CreateMap<SheepfoldViewModel, SheepfoldFilter>();

            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            var principals = this.Service.GetAllEmployeeBind();
            Action Initialize = () =>
            {
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.Administrators.Add(new EmployeeBind { Name = defaultSelection });
                    principals.ForEach(p => this.Administrators.Add(p));

                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        private List<EmployeeBind> administrators = new List<EmployeeBind>();

        public List<EmployeeBind> Administrators
        {
            get { return administrators; }
            set
            {
                this.administrators = value;
                this.RaisePropertyChanged("Administrators");
            }
        }

        private string administrator;
        [EntityProperty]
        public string Administrator
        {
            get { return administrator; }
            set
            {
                administrator = value;
                this.Validate("Administrator");
            }
        }

        private string searchSheepfoldName;
        [EntityProperty]
        public string SearchSheepfoldName
        {
            get { return searchSheepfoldName; }
            set
            {
                this.searchSheepfoldName = value;
                this.RaisePropertyChanged("SearchSheepfoldName");
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

        public SheepfoldFilter Filter
        {
            get
            {
                SheepfoldFilter filter = Mapper.Map<SheepfoldFilter>(this);
                return filter;
            }
        }
        public IEnumerable<Sheepfold> Report { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action Initialize = () =>
            {
                this.Report = this.Service.GetSheepfold(this.Filter);

                this.UIDispatcher.Invoke(new Action<IEnumerable<Sheepfold>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), Report);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public DelegateCommand<string> EditCommand
        {
            get
            {
                return this.GetEditCommand(id =>
                {
                    EditSheepfoldWindow win = new EditSheepfoldWindow(id);
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        this.LoadData();
                });
            }
        }

        public DelegateCommand AddCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AddSheepFoldWindow win = new AddSheepFoldWindow();
                    win.Owner = CurrentWindow;
                    if (win.ShowDialog() == true)
                        LoadData();
                });
            }
        }

        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetSheepfold(this.Filter, rowCount).ToArray();
        }
    }
}
