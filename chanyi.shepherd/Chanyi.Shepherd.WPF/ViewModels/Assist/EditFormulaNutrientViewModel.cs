using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;

using Microsoft.Practices.Prism.Commands;

using Chanyi.Shepherd.QueryModel.BindingModel;
using Chanyi.Shepherd.WPF.Expands.ValidateRule;
using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.WPF.ViewModels.Assist
{
    class EditFormulaNutrientViewModel : EditViewModel
    {
        public EditFormulaNutrientViewModel(string editItemId)
        {
            this.Id = editItemId;
            this.InitializeBindData();
        }

        protected override object GetEditModel()
        {
            //return this.Service.GetFormulaNutrientById(this.Id).FirstOrDefault();
            return this.Service.GetFormulaNutrientById(this.Id);
        }

        protected override void InitializeBindItem()
        {
            var pincipals = this.Service.GetAllEmployeeBind();
            this.UIDispatcher.Invoke(new Action(() => pincipals.ForEach(p => this.Principals.Add(p))), null);
        }

        public DelegateCommand<UIElement> SubmitCommand
        {
            get
            {
                return this.GetSubmitCommand<UIElement>(err =>
                {
                    var result = this.Service.UpdateFormulaNutrient(this.Name, this.DailyGain.ToFloat(), this.CP.ToFloat(), this.DMI.ToFloat(), this.EE.ToFloat(), this.CF.ToFloat(), this.NFE.ToFloat(), this.Ash.ToFloat(), this.NDF.ToFloat(), this.ADF.ToFloat(), this.Starch.ToFloat(), this.Ga.ToFloat(), this.AllP.ToFloat(), this.Arg.ToFloat(), this.His.ToFloat(), this.Ile.ToFloat(), this.Leu.ToFloat(), this.Lys.ToFloat(), this.Met.ToFloat(), this.Cys.ToFloat(), this.Phe.ToFloat(), this.Tyr.ToFloat(), this.Thr.ToFloat(), this.Trp.ToFloat(), this.Val.ToFloat(), this.P.ToFloat(), this.Na.ToFloat(), this.Cl.ToFloat(), this.Mg.ToFloat(), this.K.ToFloat(), this.Fe.ToFloat(), this.Cu.ToFloat(), this.Mn.ToFloat(), this.Zn.ToFloat(), this.Se.ToFloat(), this.Carotene.ToFloat(), this.VE.ToFloat(), this.VB1.ToFloat(), this.VB2.ToFloat(), this.PantothenicAcid.ToFloat(), this.Niacin.ToFloat(), this.Biotin.ToFloat(), this.Folic.ToFloat(), this.Choline.ToFloat(), this.VB6.ToFloat(), this.VB12.ToFloat(), this.LinoleicAcid.ToFloat(), this.Salt.ToFloat(), this.PrincipalId, this.Remark, this.Id);

                    if (!ValidateFailedServiceResult<bool>(result)) return;
                    this.CurrentWindow.DialogResult = true;
                });
            }
        }

        #region 属性

        public bool CanEdit { get { return true; } }

        public string Title { get { return "编辑营养标准"; } }

        public string Id { get; set; }

        private string name;

        [Required(ErrorMessage = "名称必填")]
        [EntityProperty]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                this.Validate("Name");
            }
        }

        private string dailyGain;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "日增重不合法")]
        public string DailyGain
        {
            get { return dailyGain; }
            set
            {
                dailyGain = value;
                this.Validate("DailyGain");
            }
        }

        private string _CP;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "蛋白质含量不合法")]
        public string CP
        {
            get { return _CP; }
            set
            {
                _CP = value;
                this.Validate("CP");
            }
        }

        private string _DMI;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "干物质含量不合法")]
        public string DMI
        {
            get { return _DMI; }
            set
            {
                _DMI = value;
                this.Validate("DMI");
            }
        }

        private string _EE;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "粗脂肪含量不合法")]
        public string EE
        {
            get { return _EE; }
            set
            {
                _EE = value;
                this.Validate("EE");
            }
        }

        private string _CF;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "粗纤维含量不合法")]
        public string CF
        {
            get { return _CF; }
            set
            {
                _CF = value;
                this.Validate("CF");
            }
        }

        private string _NFE;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "无氮浸出物含量不合法")]
        public string NFE
        {
            get { return _NFE; }
            set
            {
                _NFE = value;
                this.Validate("NFE");
            }
        }

        private string _Ash;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "粗灰分含量不合法")]
        public string Ash
        {
            get { return _Ash; }
            set
            {
                _Ash = value;
                this.Validate("Ash");
            }
        }

        private string _NDF;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "中性洗涤纤维含量不合法")]
        public string NDF
        {
            get { return _NDF; }
            set
            {
                _NDF = value;
                this.Validate("NDF");
            }
        }

        private string _ADF;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "酸性洗涤纤维含量不合法")]
        public string ADF
        {
            get { return _ADF; }
            set
            {
                _ADF = value;
                this.Validate("ADF");
            }
        }

        private string _Starch;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "淀粉含量不合法")]
        public string Starch
        {
            get { return _Starch; }
            set
            {
                _Starch = value;
                this.Validate("Starch");
            }
        }

        private string _Ga;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "钙含量不合法")]
        public string Ga
        {
            get { return _Ga; }
            set
            {
                _Ga = value;
                this.Validate("Ga");
            }
        }

        private string _AllP;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "总磷含量不合法")]
        public string AllP
        {
            get { return _AllP; }
            set
            {
                _AllP = value;
                this.Validate("AllP");
            }
        }

        private string _Arg;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "精氨酸含量不合法")]
        public string Arg
        {
            get { return _Arg; }
            set
            {
                _Arg = value;
                this.Validate("Arg");
            }
        }

        private string _His;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "组氨酸含量不合法")]
        public string His
        {
            get { return _His; }
            set
            {
                _His = value;
                this.Validate("His");
            }
        }

        private string _Ile;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "异亮氨酸含量不合法")]
        public string Ile
        {
            get { return _Ile; }
            set
            {
                _Ile = value;
                this.Validate("Ile");
            }
        }

        private string _Leu;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "亮氨酸含量不合法")]
        public string Leu
        {
            get { return _Leu; }
            set
            {
                _Leu = value;
                this.Validate("Leu");
            }
        }

        private string _Lys;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "赖氨酸含量不合法")]
        public string Lys
        {
            get { return _Lys; }
            set
            {
                _Lys = value;
                this.Validate("Lys");
            }
        }

        private string _Met;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "蛋氨酸含量不合法")]
        public string Met
        {
            get { return _Met; }
            set
            {
                _Met = value;
                this.Validate("Met");
            }
        }

        private string _Cys;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "胱氨酸含量不合法")]
        public string Cys
        {
            get { return _Cys; }
            set
            {
                _Cys = value;
                this.Validate("Cys");
            }
        }

        private string _Phe;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "苯丙氨酸含量不合法")]
        public string Phe
        {
            get { return _Phe; }
            set
            {
                _Phe = value;
                this.Validate("Phe");
            }
        }

        private string _Tyr;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "酪氨酸含量不合法")]
        public string Tyr
        {
            get { return _Tyr; }
            set
            {
                _Tyr = value;
                this.Validate("Tyr");
            }
        }

        private string _Thr;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "苏氨酸含量不合法")]
        public string Thr
        {
            get { return _Thr; }
            set
            {
                _Thr = value;
                this.Validate("Thr");
            }
        }

        private string _Trp;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "色氨酸含量不合法")]
        public string Trp
        {
            get { return _Trp; }
            set
            {
                _Trp = value;
                this.Validate("Trp");
            }
        }

        private string _Val;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "缬氨酸含量不合法")]
        public string Val
        {
            get { return _Val; }
            set
            {
                _Val = value;
                this.Validate("Val");
            }
        }

        private string _P;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "有效磷含量不合法")]
        public string P
        {
            get { return _P; }
            set
            {
                _P = value;
                this.Validate("P");
            }
        }

        private string _Na;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "钠含量不合法")]
        public string Na
        {
            get { return _Na; }
            set
            {
                _Na = value;
                this.Validate("Na");
            }
        }

        private string _Cl;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "氯含量不合法")]
        public string Cl
        {
            get { return _Cl; }
            set
            {
                _Cl = value;
                this.Validate("Cl");
            }
        }

        private string _Mg;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "镁含量不合法")]
        public string Mg
        {
            get { return _Mg; }
            set
            {
                _Mg = value;
                this.Validate("Mg");
            }
        }

        private string _K;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "钾含量不合法")]
        public string K
        {
            get { return _K; }
            set
            {
                _K = value;
                this.Validate("K");
            }
        }

        private string _Fe;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "铁含量不合法")]
        public string Fe
        {
            get { return _Fe; }
            set
            {
                _Fe = value;
                this.Validate("Fe");
            }
        }

        private string _Cu;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "铜含量不合法")]
        public string Cu
        {
            get { return _Cu; }
            set
            {
                _Cu = value;
                this.Validate("Cu");
            }
        }

        private string _Mn;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "锰含量不合法")]
        public string Mn
        {
            get { return _Mn; }
            set
            {
                _Mn = value;
                this.Validate("Mn");
            }
        }

        private string _Zn;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "锌含量不合法")]
        public string Zn
        {
            get { return _Zn; }
            set
            {
                _Zn = value;
                this.Validate("Zn");
            }
        }

        private string _Se;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "硒含量不合法")]
        public string Se
        {
            get { return _Se; }
            set
            {
                _Se = value;
                this.Validate("Se");
            }
        }

        private string _Carotene;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "胡萝卜素含量不合法")]
        public string Carotene
        {
            get { return _Carotene; }
            set
            {
                _Carotene = value;
                this.Validate("Carotene");
            }
        }

        private string _VE;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "维生素E含量不合法")]
        public string VE
        {
            get { return _VE; }
            set
            {
                _VE = value;
                this.Validate("VE");
            }
        }

        private string _VB1;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "维生素B1含量不合法")]
        public string VB1
        {
            get { return _VB1; }
            set
            {
                _VB1 = value;
                this.Validate("VB1");
            }
        }

        private string _VB2;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "维生素B2含量不合法")]
        public string VB2
        {
            get { return _VB2; }
            set
            {
                _VB2 = value;
                this.Validate("VB2");
            }
        }

        private string _PantothenicAcid;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "泛酸含量不合法")]
        public string PantothenicAcid
        {
            get { return _PantothenicAcid; }
            set
            {
                _PantothenicAcid = value;
                this.Validate("PantothenicAcid");
            }
        }

        private string _Niacin;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "烟酸含量不合法")]
        public string Niacin
        {
            get { return _Niacin; }
            set
            {
                _Niacin = value;
                this.Validate("Niacin");
            }
        }

        private string _Biotin;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "生物素含量不合法")]
        public string Biotin
        {
            get { return _Biotin; }
            set
            {
                _Biotin = value;
                this.Validate("Biotin");
            }
        }

        private string _Folic;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "叶酸含量不合法")]
        public string Folic
        {
            get { return _Folic; }
            set
            {
                _Folic = value;
                this.Validate("Folic");
            }
        }

        private string _Choline;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "胆碱含量不合法")]
        public string Choline
        {
            get { return _Choline; }
            set
            {
                _Choline = value;
                this.Validate("Choline");
            }
        }

        private string _VB6;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "维生素B6含量不合法")]
        public string VB6
        {
            get { return _VB6; }
            set
            {
                _VB6 = value;
                this.Validate("VB6");
            }
        }

        private string _VB12;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "维生素B12含量不合法")]
        public string VB12
        {
            get { return _VB12; }
            set
            {
                _VB12 = value;
                this.Validate("VB12");
            }
        }

        private string _LinoleicAcid;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "亚油酸含量不合法")]
        public string LinoleicAcid
        {
            get { return _LinoleicAcid; }
            set
            {
                _LinoleicAcid = value;
                this.Validate("LinoleicAcid");
            }
        }

        private string _Salt;
        [EntityProperty]
        [FloatNumber(0, 10, IsNullable = true, ErrorMessage = "盐含量不合法")]
        public string Salt
        {
            get { return _Salt; }
            set
            {
                _Salt = value;
                this.Validate("Salt");
            }
        }

        private ObservableCollection<EmployeeBind> principals = new ObservableCollection<EmployeeBind>();
        public ObservableCollection<EmployeeBind> Principals { get { return principals; } }

        private string principalId;
        [EntityProperty]
        public string PrincipalId
        {
            get { return principalId; }
            set
            {
                principalId = value;
                this.RaisePropertyChanged("PrincipalId");
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
    }
}
