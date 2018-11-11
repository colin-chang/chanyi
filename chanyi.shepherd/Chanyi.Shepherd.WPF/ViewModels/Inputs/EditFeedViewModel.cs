using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Configuration;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.WPF.ViewModels.Inputs
{
    class EditFeedViewModel : EditViewModel
    {
        public EditFeedViewModel(string id)
        {
            this.Id = id;
            this.InitializeBindData();
        }
        protected override object GetEditModel()
        {
            return this.Service.GetFeedByKindId(this.Id);
        }
        protected override void InitializeBindItem()
        {
            this.FeedNames = this.Service.GetFeedNameBind();
            this.FeedNames.Insert(0, new FeedNameBind { Name = ConfigurationManager.AppSettings["formDefaultSelection"] });
            this.Types = this.Service.GetFeedTypeBind();
            this.AreaNames = this.Service.GetAreaBind();
            this.AreaNames.Insert(0, new AreaBind { Name = ConfigurationManager.AppSettings["formDefaultSelection"] });
        }
        public string Id { get; set; }
        #region 饲料成分
        private string cp = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "蛋白质含量输入不合法")]
        public string CP
        {
            get { return cp; }
            set
            {
                cp = value;
                this.Validate("CP");
            }
        }

        private string dmi = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "干物质含量输入不合法")]
        public string DMI
        {
            get { return dmi; }
            set
            {
                dmi = value;
                this.Validate("DMI");
            }
        }

        private string ee = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "粗脂肪含量输入不合法")]
        public string EE
        {
            get { return ee; }
            set
            {
                ee = value;
                this.Validate("EE");
            }
        }

        private string cf = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "粗纤维含量输入不合法")]
        public string CF
        {
            get { return cf; }
            set
            {
                cf = value;
                this.Validate("CF");
            }
        }

        private string nfe = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "无氮浸出物含量输入不合法")]

        public string NFE
        {
            get { return nfe; }
            set
            {
                nfe = value;
                this.Validate("NFE");
            }
        }

        private string ash = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "粗灰分含量输入不合法")]
        public string Ash
        {
            get { return ash; }
            set
            {
                ash = value;
                this.Validate("Ash");
            }
        }

        private string ndf = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "中性洗涤纤维输入不合法")]
        public string NDF
        {
            get { return ndf; }
            set
            {
                ndf = value;
                this.Validate("NDF");
            }
        }

        private string adf = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "酸性洗涤纤维输入不合法")]
        public string ADF
        {
            get { return adf; }
            set
            {
                adf = value;
                this.Validate("ADF");
            }
        }


        private string starch = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "酸性洗涤纤维输入不合法")]
        public string Starch
        {
            get { return starch; }
            set
            {
                starch = value;
                this.Validate("Starch");
            }
        }

        private string ga = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "钙输入不合法")]
        public string Ga
        {
            get { return ga; }
            set
            {
                ga = value;
                this.Validate("Ga");
            }
        }

        private string niacin = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "烟酸输入不合法")]
        public string Niacin
        {
            get { return niacin; }
            set
            {
                niacin = value;
                this.Validate("Niacin");
            }
        }

        private string arg = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "精氨酸输入不合法")]
        public string Arg
        {
            get { return arg; }
            set
            {
                arg = value;
                this.Validate("Arg");
            }
        }

        private string his = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "组氨酸输入不合法")]
        public string His
        {
            get { return his; }
            set
            {
                his = value;
                this.Validate("His");
            }
        }

        private string ile = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "异亮氨酸输入不合法")]
        public string Ile
        {
            get { return ile; }
            set
            {
                ile = value;
                this.Validate("Ile");
            }
        }

        private string leu = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "亮氨酸输入不合法")]
        public string Leu
        {
            get { return leu; }
            set
            {
                leu = value;
                this.Validate("Leu");
            }
        }

        private string lys = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "赖氨酸输入不合法")]
        public string Lys
        {
            get { return lys; }
            set
            {
                lys = value;
                this.Validate("Lys");
            }
        }

        private string met = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "蛋氨酸输入不合法")]
        public string Met
        {
            get { return met; }
            set
            {
                met = value;
                this.Validate("Met");
            }
        }

        private string cys = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "胱氨酸输入不合法")]
        public string Cys
        {
            get { return cys; }
            set
            {
                cys = value;
                this.Validate("Cys");
            }
        }

        private string phe = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "苯丙氨酸输入不合法")]
        public string Phe
        {
            get { return phe; }
            set
            {
                phe = value;
                this.Validate("Phe");
            }
        }

        private string folic = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "叶酸输入不合法")]
        public string Folic
        {
            get { return folic; }
            set
            {
                folic = value;
                this.Validate("Folic");
            }
        }


        private string choline = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "胆碱输入不合法")]
        public string Choline
        {
            get { return choline; }
            set
            {
                choline = value;
                this.Validate("Choline");
            }
        }

        private string linoleicAcid = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "亚油酸输入不合法")]
        public string LinoleicAcid
        {
            get { return linoleicAcid; }
            set
            {
                linoleicAcid = value;
                this.Validate("LinoleicAcid");
            }
        }

        private string tyr = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "酪氨酸输入不合法")]
        public string Tyr
        {
            get { return tyr; }
            set
            {
                tyr = value;
                this.Validate("Tyr");
            }
        }

        private string thr = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "苏氨酸输入不合法")]
        public string Thr
        {
            get { return thr; }
            set
            {
                thr = value;
                this.Validate("Thr");
            }
        }
        private string trp = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "色氨酸输入不合法")]
        public string Trp
        {
            get { return trp; }
            set
            {
                trp = value;
                this.Validate("Trp");
            }
        }

        private string val = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "色氨酸输入不合法")]
        public string Val
        {
            get { return val; }
            set
            {
                val = value;
                this.Validate("Val");
            }
        }

        private string p = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "有效磷含量输入不合法")]
        public string P
        {
            get { return p; }
            set
            {
                p = value;
                this.Validate("P");
            }
        }

        private string na = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "钠含量输入不合法")]
        public string Na
        {
            get { return na; }
            set
            {
                na = value;
                this.Validate("Na");
            }
        }

        private string cl = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "氯含量输入不合法")]
        public string Cl
        {
            get { return cl; }
            set
            {
                cl = value;
                this.Validate("Cl");
            }
        }

        private string mg = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "镁含量输入不合法")]
        public string Mg
        {
            get { return mg; }
            set
            {
                mg = value;
                this.Validate("Mg");
            }
        }
        private string k = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "钾含量输入不合法")]
        public string K
        {
            get { return k; }
            set
            {
                k = value;
                this.Validate("K");
            }
        }
        private string pantothenicAcid = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "泛酸含量输入不合法")]
        public string PantothenicAcid
        {
            get { return pantothenicAcid; }
            set
            {
                pantothenicAcid = value;
                this.Validate("PantothenicAcid");
            }
        }

        private string biotin = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "生物素含量输入不合法")]
        public string Biotin
        {
            get { return biotin; }
            set
            {
                biotin = value;
                this.Validate("Biotin");
            }
        }

        private string fe = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "铁含量输入不合法")]
        public string Fe
        {
            get { return fe; }
            set
            {
                fe = value;
                this.Validate("Fe");
            }
        }

        private string cu = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "铜含量输入不合法")]
        public string Cu
        {
            get { return cu; }
            set
            {
                cu = value;
                this.Validate("Cu");
            }
        }

        private string mn = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "锰含量输入不合法")]
        public string Mn
        {
            get { return mn; }
            set
            {
                mn = value;
                this.Validate("Mn");
            }
        }

        private string zn = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "锌含量输入不合法")]
        public string Zn
        {
            get { return zn; }
            set
            {
                zn = value;
                this.Validate("Zn");
            }
        }

        private string se = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "硒含量输入不合法")]
        public string Se
        {
            get { return se; }
            set
            {
                se = value;
                this.Validate("Se");
            }
        }
        private string carotene = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "胡萝卜素含量输入不合法")]
        public string Carotene
        {
            get { return carotene; }
            set
            {
                carotene = value;
                this.Validate("Carotene");
            }
        }

        private string ve = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "维生素E含量输入不合法")]
        public string VE
        {
            get { return ve; }
            set
            {
                ve = value;
                this.Validate("VE");
            }
        }

        private string vb1 = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "维生素B1含量输入不合法")]
        public string VB1
        {
            get { return vb1; }
            set
            {
                vb1 = value;
                this.Validate("VB1");
            }
        }

        private string vb2 = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "维生素B2含量输入不合法")]
        public string VB2
        {
            get { return vb2; }
            set
            {
                vb2 = value;
                this.Validate("VB2");
            }
        }


        private string vb6 = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "维生素B6含量输入不合法")]
        public string VB6
        {
            get { return vb6; }
            set
            {
                vb6 = value;
                this.Validate("VB6");
            }
        }

        private string vb12 = "0.00";
        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "维生素B12含量输入不合法")]
        public string VB12
        {
            get { return vb12; }
            set
            {
                vb12 = value;
                this.Validate("VB12");
            }
        }
        private string allp = "0.00";

        [EntityProperty]
        [FloatNumber(0, 1000, ErrorMessage = "总磷含量输入不合法")]
        public string AllP
        {
            get { return allp; }
            set
            {
                allp = value;
                this.Validate("AllP");
            }
        }
        #endregion

        private List<FeedNameBind> feedNames;
        public List<FeedNameBind> FeedNames
        {
            get { return feedNames; }
            set
            {
                feedNames = value;
                this.RaisePropertyChanged("FeedNames");
            }
        }

        private string nameId;
        [EntityProperty]

        [Required(ErrorMessage = "饲料名称必填")]
        public string NameId
        {
            get { return nameId; }
            set
            {
                nameId = value;
                this.Validate("NameId");
            }
        }


        private string description;
        [EntityProperty]
        [Required(ErrorMessage = "饲料描述必填")]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                this.Validate("Description");
            }
        }

        private List<FeedTypeBind> types;
        public List<FeedTypeBind> Types
        {
            get { return types; ; }
            set
            {
                types = value;
                this.RaisePropertyChanged("Types");
            }
        }

        private string typeId;
        [EntityProperty]
        [Required(ErrorMessage = "饲料类型必填")]
        public string TypeId
        {
            get { return typeId; ; }
            set
            {
                typeId = value;
                this.Validate("TypeId");
            }
        }

        private List<AreaBind> areaNames;

        public List<AreaBind> AreaNames
        {
            get { return areaNames; }
            set
            {
                areaNames = value;
                this.RaisePropertyChanged("AreaNames");
            }
        }
        private string areaId;
        [EntityProperty]
        [Required(ErrorMessage = "饲料产地必填")]
        public string AreaId
        {
            get { return areaId; }
            set
            {
                areaId = value;
                this.Validate("AreaId");
            }
        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.UpdateFeed(this.NameId, this.TypeId, this.AreaId, this.Description, this.UserId, this.CP.ToFloat(), this.DMI.ToFloat(), this.EE.ToFloat(), this.CF.ToFloat(), this.NFE.ToFloat(), this.Ash.ToFloat(), this.NDF.ToFloat(), this.ADF.ToFloat(), this.Starch.ToFloat(), this.Ga.ToFloat(), this.Arg.ToFloat(), this.His.ToFloat(), this.Ile.ToFloat(), this.Leu.ToFloat(), this.Lys.ToFloat(), this.Met.ToFloat(), this.Cys.ToFloat(), this.Phe.ToFloat(), this.Tyr.ToFloat(), this.Thr.ToFloat(), this.Trp.ToFloat(), this.Val.ToFloat(), this.P.ToFloat(), this.Na.ToFloat(), this.Cl.ToFloat(), this.Mg.ToFloat(), this.K.ToFloat(), this.Fe.ToFloat(), this.Cu.ToFloat(), this.Mn.ToFloat(), this.Zn.ToFloat(), this.Se.ToFloat(), this.Carotene.ToFloat(), this.VE.ToFloat(), this.VB1.ToFloat(), this.VB2.ToFloat(), this.PantothenicAcid.ToFloat(), this.Niacin.ToFloat(), this.Biotin.ToFloat(), this.Folic.ToFloat(), this.Choline.ToFloat(), this.VB6.ToFloat(), this.VB12.ToFloat(), this.LinoleicAcid.ToFloat(), this.AllP.ToFloat(), this.Id);

                    if (!ValidateFailedServiceResult<bool>(result)) return;
                    this.CurrentWindow.DialogResult = true;

                });
            }
        }

    }
}
