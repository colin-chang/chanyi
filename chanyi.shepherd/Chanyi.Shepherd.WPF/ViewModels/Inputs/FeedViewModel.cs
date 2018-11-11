using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using Chanyi.Shepherd.QueryModel.Model.Input;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Shepherd.WPF.Views.Inputs;
using System.Collections.ObjectModel;


namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class FeedViewModel : ListViewModel
    {
        public FeedViewModel(bool withinInitilization) { }

        public FeedViewModel(string header, string icon, string intro, string editPermUrl)
        {
            this.Header = header;
            this.Icon = icon;
            this.Intro = intro;
            this.editPermUrl = editPermUrl;
            Mapper.CreateMap<FeedViewModel, FeedFilter>();
            InitializeBindData();
        }
        protected override void InitializeBindData()
        {
            Action Initialize = () =>
            {
                var feedNames = this.Service.GetFeedNameBind();
                var typeNames = this.Service.GetFeedTypeBind();
                var areaNames = this.Service.GetAreaBind();
                this.UIDispatcher.Invoke(new Action(() =>
                {
                    this.FeedNames.Clear();
                    this.TypeNames.Clear();
                    this.AreaNames.Clear();
                    this.FeedNames.Add(new FeedNameBind { Name = defaultSelection });
                    feedNames.ForEach(f => this.FeedNames.Add(f));
                    this.TypeNames.Add(new FeedTypeBind { Name = defaultSelection });
                    typeNames.ForEach(t => this.TypeNames.Add(t));
                    this.AreaNames.Add(new AreaBind { Name = defaultSelection });
                    areaNames.ForEach(a => this.AreaNames.Add(a));
                    this.Reset();
                }), null);
            };
            if (!this.IsInDesignMode)
                Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }

        public override int PageSize
        {
            get
            {
                return 20;
            }
        }

        public FeedFilter Filter
        {
            get
            {
                FeedFilter filter = Mapper.Map<FeedFilter>(this);
                return filter;
            }
        }

        #region 绑定搜索字段
        private ObservableCollection<FeedNameBind> feedNames = new ObservableCollection<FeedNameBind>();
        public ObservableCollection<FeedNameBind> FeedNames { get { return feedNames; } }

        private ObservableCollection<FeedTypeBind> typeNames = new ObservableCollection<FeedTypeBind>();
        public ObservableCollection<FeedTypeBind> TypeNames { get { return typeNames; ; } }

        private ObservableCollection<AreaBind> areaNames = new ObservableCollection<AreaBind>();

        public ObservableCollection<AreaBind> AreaNames { get { return areaNames; } }

        private string nameId;
        [EntityProperty]
        public string NameId
        {
            get { return nameId; }
            set
            {
                nameId = value;
                this.RaisePropertyChanged("NameId");
            }
        }


        private string description;
        [EntityProperty]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                this.RaisePropertyChanged("Description");
            }
        }



        private string typeId;
        [EntityProperty]
        public string TypeId
        {
            get { return typeId; ; }
            set
            {
                typeId = value;
                this.RaisePropertyChanged("TypeId");
            }
        }


        private string areaId;
        [EntityProperty]
        public string AreaId
        {
            get { return areaId; }
            set
            {
                areaId = value;
                this.RaisePropertyChanged("AreaId");
            }
        }
        #endregion

        #region 列表数据
        public IEnumerable<Feed> FeedList { get; set; }
        protected override void LoadData()
        {
            if (!this.IsValid)
            {
                this.Table.ItemsSource = null;
                return;
            }
            Action Initialize = () =>
            {
                int count;
                this.FeedList = this.Service.GetFeed(this.Filter, this.PageIndex, this.PageSize, out count);
                this.TotalCount = count;
                this.UIDispatcher.Invoke(new Action<IEnumerable<Feed>>(d =>
                {
                    this.Table.ItemsSource = d;
                    this.Table.SelectedIndex = 0;
                    this.ProgressRing.Hide();
                }), this.FeedList);
            };
            this.ProgressRing.Show();
            Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
        }
        #endregion

        protected override Array GetExportData(int rowCount)
        {
            return this.Service.GetFeed(this.Filter, rowCount).ToArray();
        }

        public DelegateCommand<string> DataGridSelectionChangedCommand
        {
            get
            {
                return new DelegateCommand<string>(Id =>
                {
                    Action Initialize = () =>
                    {
                        FeedDetail model = this.Service.GetFeedDetail(Id);
                        if (model == null) return;
                        var ps = this.GetType().GetProperties();
                        model.GetType().GetProperties().ToList().ForEach(p =>
                        {
                            var prop = ps.Where(pi => pi.Name == p.Name).FirstOrDefault();
                            if (prop != null)
                            {
                                var modelType = p.PropertyType;
                                var thisType = prop.PropertyType;
                                var value = p.GetValue(model, null);
                                if (modelType == thisType)
                                    prop.SetValue(this, value, null);
                                if (thisType == typeof(string) && (value is ValueType))
                                    prop.SetValue(this, value.ToString(), null);
                            }
                        });
                    };
                    Initialize.BeginInvoke(ar => Initialize.EndInvoke(ar as IAsyncResult), Initialize);
                });
            }
        }

        #region 饲料成分
        private string cp;
        [EntityProperty]
        public string CP
        {
            get { return cp; }
            set
            {
                cp = value;
                this.RaisePropertyChanged("CP");
            }
        }

        private string dmi;
        [EntityProperty]
        public string DMI
        {
            get { return dmi; }
            set
            {
                dmi = value;
                this.RaisePropertyChanged("DMI");
            }
        }

        private string ee;

        [EntityProperty]
        public string EE
        {
            get { return ee; }
            set
            {
                ee = value;
                this.RaisePropertyChanged("EE");
            }
        }

        private string cf;

        [EntityProperty]
        public string CF
        {
            get { return cf; }
            set
            {
                cf = value;
                this.RaisePropertyChanged("CF");
            }
        }

        private string nfe;
        [EntityProperty]

        public string NFE
        {
            get { return nfe; }
            set
            {
                nfe = value;
                this.RaisePropertyChanged("NFE");
            }
        }

        private string ash;
        [EntityProperty]
        public string Ash
        {
            get { return ash; }
            set
            {
                ash = value;
                this.RaisePropertyChanged("Ash");
            }
        }

        private string ndf;

        [EntityProperty]
        public string NDF
        {
            get { return ndf; }
            set
            {
                ndf = value;
                this.RaisePropertyChanged("NDF");
            }
        }

        private string adf;

        [EntityProperty]
        public string ADF
        {
            get { return adf; }
            set
            {
                adf = value;
                this.RaisePropertyChanged("ADF");
            }
        }


        private string starch;

        [EntityProperty]
        public string Starch
        {
            get { return starch; }
            set
            {
                starch = value;
                this.RaisePropertyChanged("Starch");
            }
        }

        private string ga;

        [EntityProperty]
        public string Ga
        {
            get { return ga; }
            set
            {
                ga = value;
                this.RaisePropertyChanged("Ga");
            }
        }

        private string niacin;

        [EntityProperty]
        public string Niacin
        {
            get { return niacin; }
            set
            {
                niacin = value;
                this.RaisePropertyChanged("Niacin");
            }
        }

        private string arg;

        [EntityProperty]
        public string Arg
        {
            get { return arg; }
            set
            {
                arg = value;
                this.RaisePropertyChanged("Arg");
            }
        }

        private string his;

        [EntityProperty]
        public string His
        {
            get { return his; }
            set
            {
                his = value;
                this.RaisePropertyChanged("His");
            }
        }

        private string ile;

        [EntityProperty]
        public string Ile
        {
            get { return ile; }
            set
            {
                ile = value;
                this.RaisePropertyChanged("Ile");
            }
        }

        private string leu;

        [EntityProperty]
        public string Leu
        {
            get { return leu; }
            set
            {
                leu = value;
                this.RaisePropertyChanged("Leu");
            }
        }

        private string lys;
        [EntityProperty]
        public string Lys
        {
            get { return lys; }
            set
            {
                lys = value;
                this.RaisePropertyChanged("Lys");
            }
        }

        private string met;

        [EntityProperty]
        public string Met
        {
            get { return met; }
            set
            {
                met = value;
                this.RaisePropertyChanged("Met");
            }
        }

        private string cys;
        [EntityProperty]
        public string Cys
        {
            get { return cys; }
            set
            {
                cys = value;
                this.RaisePropertyChanged("Cys");
            }
        }

        private string phe;
        [EntityProperty]
        public string Phe
        {
            get { return phe; }
            set
            {
                phe = value;
                this.RaisePropertyChanged("Phe");
            }
        }

        private string folic;

        [EntityProperty]
        public string Folic
        {
            get { return folic; }
            set
            {
                folic = value;
                this.RaisePropertyChanged("Folic");
            }
        }


        private string choline;

        [EntityProperty]
        public string Choline
        {
            get { return choline; }
            set
            {
                choline = value;
                this.RaisePropertyChanged("Choline");
            }
        }

        private string linoleicAcid;

        [EntityProperty]
        public string LinoleicAcid
        {
            get { return linoleicAcid; }
            set
            {
                linoleicAcid = value;
                this.RaisePropertyChanged("LinoleicAcid");
            }
        }

        private string tyr;

        [EntityProperty]
        public string Tyr
        {
            get { return tyr; }
            set
            {
                tyr = value;
                this.RaisePropertyChanged("Tyr");
            }
        }

        private string thr;

        [EntityProperty]
        public string Thr
        {
            get { return thr; }
            set
            {
                thr = value;
                this.RaisePropertyChanged("Thr");
            }
        }
        private string trp;

        [EntityProperty]
        public string Trp
        {
            get { return trp; }
            set
            {
                trp = value;
                this.RaisePropertyChanged("Trp");
            }
        }

        private string val;

        [EntityProperty]
        public string Val
        {
            get { return val; }
            set
            {
                val = value;
                this.RaisePropertyChanged("Val");
            }
        }

        private string p;

        [EntityProperty]
        public string P
        {
            get { return p; }
            set
            {
                p = value;
                this.RaisePropertyChanged("P");
            }
        }

        private string na;

        [EntityProperty]
        public string Na
        {
            get { return na; }
            set
            {
                na = value;
                this.RaisePropertyChanged("Na");
            }
        }

        private string cl;

        [EntityProperty]
        public string Cl
        {
            get { return cl; }
            set
            {
                cl = value;
                this.RaisePropertyChanged("Cl");
            }
        }

        private string mg;

        [EntityProperty]
        public string Mg
        {
            get { return mg; }
            set
            {
                mg = value;
                this.RaisePropertyChanged("Mg");
            }
        }
        private string k;

        [EntityProperty]
        public string K
        {
            get { return k; }
            set
            {
                k = value;
                this.RaisePropertyChanged("K");
            }
        }
        private string pantothenicAcid;

        [EntityProperty]
        public string PantothenicAcid
        {
            get { return pantothenicAcid; }
            set
            {
                pantothenicAcid = value;
                this.RaisePropertyChanged("PantothenicAcid");
            }
        }

        private string biotin;

        [EntityProperty]
        public string Biotin
        {
            get { return biotin; }
            set
            {
                biotin = value;
                this.RaisePropertyChanged("Biotin");
            }
        }

        private string fe;

        [EntityProperty]
        public string Fe
        {
            get { return fe; }
            set
            {
                fe = value;
                this.RaisePropertyChanged("Fe");
            }
        }

        private string cu;

        [EntityProperty]
        public string Cu
        {
            get { return cu; }
            set
            {
                cu = value;
                this.RaisePropertyChanged("Cu");
            }
        }

        private string mn;

        [EntityProperty]
        public string Mn
        {
            get { return mn; }
            set
            {
                mn = value;
                this.RaisePropertyChanged("Mn");
            }
        }

        private string zn;
        [EntityProperty]
        public string Zn
        {
            get { return zn; }
            set
            {
                zn = value;
                this.RaisePropertyChanged("Zn");
            }
        }

        private string se;
        [EntityProperty]
        public string Se
        {
            get { return se; }
            set
            {
                se = value;
                this.RaisePropertyChanged("Se");
            }
        }
        private string carotene;
        [EntityProperty]
        public string Carotene
        {
            get { return carotene; }
            set
            {
                carotene = value;
                this.RaisePropertyChanged("Carotene");
            }
        }

        private string ve;
        [EntityProperty]
        public string VE
        {
            get { return ve; }
            set
            {
                ve = value;
                this.RaisePropertyChanged("VE");
            }
        }

        private string vb1;
        [EntityProperty]
        public string VB1
        {
            get { return vb1; }
            set
            {
                vb1 = value;
                this.RaisePropertyChanged("VB1");
            }
        }

        private string vb2;
        [EntityProperty]
        public string VB2
        {
            get { return vb2; }
            set
            {
                vb2 = value;
                this.RaisePropertyChanged("VB2");
            }
        }


        private string vb6;
        [EntityProperty]
        public string VB6
        {
            get { return vb6; }
            set
            {
                vb6 = value;
                this.RaisePropertyChanged("VB6");
            }
        }

        private string vb12;
        [EntityProperty]
        public string VB12
        {
            get { return vb12; }
            set
            {
                vb12 = value;
                this.RaisePropertyChanged("VB12");
            }
        }
        private string allp;

        [EntityProperty]
        public string AllP
        {
            get { return allp; }
            set
            {
                allp = value;
                this.RaisePropertyChanged("AllP");
            }
        }
        #endregion
    }
}
