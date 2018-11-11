using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Spring.Data.Common;
using Spring.Data.Generic;
using Spring.Transaction.Interceptor;

using Chanyi.Shepherd.QueryModel.Model.Formula;
using Chanyi.Shepherd.QueryModel.Filter.Formula;
using Chanyi.Shepherd.QueryModel.Model.Input;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region Formula

        public List<Formula> GetFormula(FormulaFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY f.\"IsEnable\" desc,f.\"CreateTime\" desc) as \"rownum\",f.\"Id\",f.\"Name\",f.\"ApplyTo\",f.\"SideEffect\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",f.\"IsEnable\",f.\"Remark\",f.\"CreateTime\" from \"T_Formula\" f join \"T_Employee\" e on e.\"Id\"=f.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=f.\"OperatorId\"";

            string countSql = "select count(f.\"Id\") from \"T_Formula\" f join \"T_Employee\" e on e.\"Id\"=f.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=f.\"OperatorId\"";

            return GetPagedData<Formula, FormulaFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<Formula> GetFormula(FormulaFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY f.\"IsEnable\" desc,f.\"CreateTime\" desc) as \"rownum\",f.\"Id\",f.\"Name\",f.\"ApplyTo\",f.\"SideEffect\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",f.\"IsEnable\",f.\"Remark\",f.\"CreateTime\" from \"T_Formula\" f join \"T_Employee\" e on e.\"Id\"=f.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=f.\"OperatorId\"";
            return GetRuledRowsData<Formula, FormulaFilter>(rowsCount, querySql, filter);
        }

        public List<FormulaFeed> GetFormulaFeedById(string formulaId)
        {
            string sql = "select f.\"Id\" as \"KindId\",n.\"Name\",a.\"Name\" as \"Area\",d.\"Value\" as \"Type\",ff.\"Amount\" from \"T_FormulaFeed\" ff join \"T_Feed\" f on f.\"Id\"=ff.\"FeedId\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" where ff.\"FormulaId\"=:formulaId and d.\"Category\"=:category";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("formulaId", formulaId);
            pms.AddWithValue("category", feedTypeCategory);

            return GetData<FormulaFeed>(sql, pms);
        }

        public List<SimpleFeed> GetSimpleFeed()
        {
            string sql = "select f.\"Id\",n.\"Name\",a.\"Name\" as \"Area\",d.\"Value\" as \"Type\" from \"T_Feed\" f join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" where d.\"Category\"=:category";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("category", feedTypeCategory);
            return GetData<SimpleFeed>(sql, pms);
        }

        public List<FormulaNutrient> GetFormulaNutrient(FormulaNutrientFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY fn.\"Name\") as \"rownum\",fn.\"Id\",fn.\"Name\",fn.\"DailyGain\",fn.\"CP\",fn.\"DMI\",fn.\"EE\",fn.\"CF\",fn.\"NFE\",fn.\"Ash\",fn.\"NDF\",fn.\"ADF\",fn.\"Starch\",fn.\"Ga\",fn.\"AllP\",fn.\"Arg\",fn.\"His\",fn.\"Ile\",fn.\"Leu\",fn.\"Lys\",fn.\"Met\",fn.\"Cys\",fn.\"Phe\",fn.\"Tyr\",fn.\"Thr\",fn.\"Trp\",fn.\"Val\",fn.\"P\",fn.\"Na\",fn.\"Cl\",fn.\"Mg\",fn.\"K\",fn.\"Fe\",fn.\"Cu\",fn.\"Mn\",fn.\"Zn\",fn.\"Se\",fn.\"Carotene\",fn.\"VE\",fn.\"VB1\",fn.\"VB2\",fn.\"PantothenicAcid\",fn.\"Niacin\",fn.\"Biotin\",fn.\"Folic\",fn.\"Choline\",fn.\"VB6\",fn.\"VB12\",fn.\"LinoleicAcid\",u.\"UserName\" as \"OperatorName\",fn.\"CreateTime\",fn.\"Remark\",fn.\"Salt\",fn.\"IsEditable\" from \"T_FormulaNutrient\" fn  join \"T_User\" u on u.\"Id\"=fn.\"OperatorId\"";

            string countSql = "select count(fn.\"Id\") from \"T_FormulaNutrient\" fn join \"T_User\" u on u.\"Id\"=fn.\"OperatorId\"";

            return GetPagedData<FormulaNutrient, FormulaNutrientFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<FormulaNutrient> GetFormulaNutrient(FormulaNutrientFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY fn.\"Name\") as \"rownum\",fn.\"Id\",fn.\"Name\",fn.\"DailyGain\",fn.\"CP\",fn.\"DMI\",fn.\"EE\",fn.\"CF\",fn.\"NFE\",fn.\"Ash\",fn.\"NDF\",fn.\"ADF\",fn.\"Starch\",fn.\"Ga\",fn.\"AllP\",fn.\"Arg\",fn.\"His\",fn.\"Ile\",fn.\"Leu\",fn.\"Lys\",fn.\"Met\",fn.\"Cys\",fn.\"Phe\",fn.\"Tyr\",fn.\"Thr\",fn.\"Trp\",fn.\"Val\",fn.\"P\",fn.\"Na\",fn.\"Cl\",fn.\"Mg\",fn.\"K\",fn.\"Fe\",fn.\"Cu\",fn.\"Mn\",fn.\"Zn\",fn.\"Se\",fn.\"Carotene\",fn.\"VE\",fn.\"VB1\",fn.\"VB2\",fn.\"PantothenicAcid\",fn.\"Niacin\",fn.\"Biotin\",fn.\"Folic\",fn.\"Choline\",fn.\"VB6\",fn.\"VB12\",fn.\"LinoleicAcid\",u.\"UserName\" as \"OperatorName\",fn.\"CreateTime\",fn.\"Remark\",fn.\"Salt\",fn.\"IsEditable\" from \"T_FormulaNutrient\" fn  join \"T_User\" u on u.\"Id\"=fn.\"OperatorId\"";
            return GetRuledRowsData<FormulaNutrient, FormulaNutrientFilter>(rowsCount, querySql, filter);
        }
        public FormulaNutrient GetFormulaNutrientById(string id)
        {
            string sql = "select \"Id\",\"Name\",\"DailyGain\",\"CP\",\"DMI\",\"EE\",\"CF\",\"NFE\",\"Ash\",\"NDF\",\"ADF\",\"Starch\",\"Ga\",\"AllP\",\"Arg\",\"His\",\"Ile\",\"Leu\",\"Lys\",\"Met\",\"Cys\",\"Phe\",\"Tyr\",\"Thr\",\"Trp\",\"Val\",\"P\",\"Na\",\"Cl\",\"Mg\",\"K\",\"Fe\",\"Cu\",\"Mn\",\"Zn\",\"Se\",\"Carotene\",\"VE\",\"VB1\",\"VB2\",\"PantothenicAcid\",\"Niacin\",\"Biotin\",\"Folic\",\"Choline\",\"VB6\",\"VB12\",\"LinoleicAcid\",\"Salt\",\"IsEditable\",\"PrincipalId\",\"Remark\" from \"T_FormulaNutrient\" where \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            return GetData<FormulaNutrient>(sql, pms).FirstOrDefault();
        }

        public void UpdateFormulaStatus(bool isEnable, string id)
        {
            string sql = "UPDATE \"T_Formula\" SET \"IsEnable\"=:isEnable where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("isEnable", isEnable);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public void AddFormulaNutrient(string id, string name, float? dailyGain, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? AllP, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? Salt, bool isEditable, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_FormulaNutrient\" (\"Id\",\"Name\",\"DailyGain\",\"CP\",\"DMI\",\"EE\",\"CF\",\"NFE\",\"Ash\",\"NDF\",\"ADF\",\"Starch\",\"Ga\",\"AllP\",\"Arg\",\"His\",\"Ile\",\"Leu\",\"Lys\",\"Met\",\"Cys\",\"Phe\",\"Tyr\",\"Thr\",\"Trp\",\"Val\",\"P\",\"Na\",\"Cl\",\"Mg\",\"K\",\"Fe\",\"Cu\",\"Mn\",\"Zn\",\"Se\",\"Carotene\",\"VE\",\"VB1\",\"VB2\",\"PantothenicAcid\",\"Niacin\",\"Biotin\",\"Folic\",\"Choline\",\"VB6\",\"VB12\",\"LinoleicAcid\",\"Salt\",\"IsEditable\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:name,:dailyGain,:cP,:dMI,:eE,:cF,:nFE,:ash,:nDF,:aDF,:starch,:ga,:allP,:arg,:his,:ile,:leu,:lys,:met,:cys,:phe,:tyr,:thr,:trp,:val,:p,:na,:cl,:mg,:k,:fe,:cu,:mn,:zn,:se,:carotene,:vE,:vB1,:vB2,:pantothenicAcid,:niacin,:biotin,:folic,:choline,:vB6,:vB12,:linoleicAcid,:salt,:isEditable,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("dailyGain", dailyGain);
            pms.AddWithValue("cP", CP);
            pms.AddWithValue("dMI", DMI);
            pms.AddWithValue("eE", EE);
            pms.AddWithValue("cF", CF);
            pms.AddWithValue("nFE", NFE);
            pms.AddWithValue("ash", Ash);
            pms.AddWithValue("nDF", NDF);
            pms.AddWithValue("aDF", ADF);
            pms.AddWithValue("starch", Starch);
            pms.AddWithValue("ga", Ga);
            pms.AddWithValue("allP", AllP);
            pms.AddWithValue("arg", Arg);
            pms.AddWithValue("his", His);
            pms.AddWithValue("ile", Ile);
            pms.AddWithValue("leu", Leu);
            pms.AddWithValue("lys", Lys);
            pms.AddWithValue("met", Met);
            pms.AddWithValue("cys", Cys);
            pms.AddWithValue("phe", Phe);
            pms.AddWithValue("tyr", Tyr);
            pms.AddWithValue("thr", Thr);
            pms.AddWithValue("trp", Trp);
            pms.AddWithValue("val", Val);
            pms.AddWithValue("p", P);
            pms.AddWithValue("na", Na);
            pms.AddWithValue("cl", Cl);
            pms.AddWithValue("mg", Mg);
            pms.AddWithValue("k", K);
            pms.AddWithValue("fe", Fe);
            pms.AddWithValue("cu", Cu);
            pms.AddWithValue("mn", Mn);
            pms.AddWithValue("zn", Zn);
            pms.AddWithValue("se", Se);
            pms.AddWithValue("carotene", Carotene);
            pms.AddWithValue("vE", VE);
            pms.AddWithValue("vB1", VB1);
            pms.AddWithValue("vB2", VB2);
            pms.AddWithValue("pantothenicAcid", PantothenicAcid);
            pms.AddWithValue("niacin", Niacin);
            pms.AddWithValue("biotin", Biotin);
            pms.AddWithValue("folic", Folic);
            pms.AddWithValue("choline", Choline);
            pms.AddWithValue("vB6", VB6);
            pms.AddWithValue("vB12", VB12);
            pms.AddWithValue("linoleicAcid", LinoleicAcid);
            pms.AddWithValue("salt", Salt);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("isEditable", isEditable);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public void UpdateFormulaNutrient(string name, float? dailyGain, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? AllP, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? Salt, string principalId, string remark, string id)
        {
            string sql = "UPDATE \"T_FormulaNutrient\" SET \"Name\"=:name,\"DailyGain\"=:dailyGain,\"CP\"=:cP,\"DMI\"=:dMI,\"EE\"=:eE,\"CF\"=:cF,\"NFE\"=:nFE,\"Ash\"=:ash,\"NDF\"=:nDF,\"ADF\"=:aDF,\"Starch\"=:starch,\"Ga\"=:ga,\"AllP\"=:allP,\"Arg\"=:arg,\"His\"=:his,\"Ile\"=:ile,\"Leu\"=:leu,\"Lys\"=:lys,\"Met\"=:met,\"Cys\"=:cys,\"Phe\"=:phe,\"Tyr\"=:tyr,\"Thr\"=:thr,\"Trp\"=:trp,\"Val\"=:val,\"P\"=:p,\"Na\"=:na,\"Cl\"=:cl,\"Mg\"=:mg,\"K\"=:k,\"Fe\"=:fe,\"Cu\"=:cu,\"Mn\"=:mn,\"Zn\"=:zn,\"Se\"=:se,\"Carotene\"=:carotene,\"VE\"=:vE,\"VB1\"=:vB1,\"VB2\"=:vB2,\"PantothenicAcid\"=:pantothenicAcid,\"Niacin\"=:niacin,\"Biotin\"=:biotin,\"Folic\"=:folic,\"Choline\"=:choline,\"VB6\"=:vB6,\"VB12\"=:vB12,\"LinoleicAcid\"=:linoleicAcid,\"Salt\"=:salt,\"PrincipalId\"=:principalId,\"Remark\"=:remark where \"Id\"=:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            pms.AddWithValue("dailyGain", dailyGain);
            pms.AddWithValue("cP", CP);
            pms.AddWithValue("dMI", DMI);
            pms.AddWithValue("eE", EE);
            pms.AddWithValue("cF", CF);
            pms.AddWithValue("nFE", NFE);
            pms.AddWithValue("ash", Ash);
            pms.AddWithValue("nDF", NDF);
            pms.AddWithValue("aDF", ADF);
            pms.AddWithValue("starch", Starch);
            pms.AddWithValue("ga", Ga);
            pms.AddWithValue("allP", AllP);
            pms.AddWithValue("arg", Arg);
            pms.AddWithValue("his", His);
            pms.AddWithValue("ile", Ile);
            pms.AddWithValue("leu", Leu);
            pms.AddWithValue("lys", Lys);
            pms.AddWithValue("met", Met);
            pms.AddWithValue("cys", Cys);
            pms.AddWithValue("phe", Phe);
            pms.AddWithValue("tyr", Tyr);
            pms.AddWithValue("thr", Thr);
            pms.AddWithValue("trp", Trp);
            pms.AddWithValue("val", Val);
            pms.AddWithValue("p", P);
            pms.AddWithValue("na", Na);
            pms.AddWithValue("cl", Cl);
            pms.AddWithValue("mg", Mg);
            pms.AddWithValue("k", K);
            pms.AddWithValue("fe", Fe);
            pms.AddWithValue("cu", Cu);
            pms.AddWithValue("mn", Mn);
            pms.AddWithValue("zn", Zn);
            pms.AddWithValue("se", Se);
            pms.AddWithValue("carotene", Carotene);
            pms.AddWithValue("vE", VE);
            pms.AddWithValue("vB1", VB1);
            pms.AddWithValue("vB2", VB2);
            pms.AddWithValue("pantothenicAcid", PantothenicAcid);
            pms.AddWithValue("niacin", Niacin);
            pms.AddWithValue("biotin", Biotin);
            pms.AddWithValue("folic", Folic);
            pms.AddWithValue("choline", Choline);
            pms.AddWithValue("vB6", VB6);
            pms.AddWithValue("vB12", VB12);
            pms.AddWithValue("linoleicAcid", LinoleicAcid);
            pms.AddWithValue("salt", Salt);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("id", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public bool IsFormulaNutrientEditable(string id)
        {
            string sql = "select \"IsEditable\" from \"T_FormulaNutrient\" where \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            object obj = AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
            return obj == null ? false : bool.Parse(obj.ToString());
        }

        [Transaction]
        public void AddFormula(string id, Dictionary<string, float> formulaFeed, string name, string applyTo, string sideEffect, bool isEnable, string principalId, string operatorId, DateTime createTime, string remark)
        {
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT into \"T_Formula\" (\"Id\",\"Name\",\"ApplyTo\",\"SideEffect\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"IsEnable\",\"Remark\")VALUES(:id,:name,:applyTo,:sideEffect,:principalId,:operatorId,:createTime,:isEnable,:remark);");

            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("applyTo", applyTo);
            pms.AddWithValue("sideEffect", sideEffect);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("isEnable", isEnable);
            pms.AddWithValue("remark", remark);

            List<string> keys = formulaFeed.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                sb.AppendFormat("INSERT into \"T_FormulaFeed\" (\"Id\",\"FormulaId\",\"FeedId\",\"Amount\")VALUES(:id{0},:formulaId{0},:feedId{0},:amount{0});", i);

                pms.AddWithValue("id" + i, Guid.NewGuid());
                pms.AddWithValue("formulaId" + i, id);
                pms.AddWithValue("feedId" + i, keys[i]);
                pms.AddWithValue("amount" + i, formulaFeed[keys[i]]);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);


        }

        public List<Formula> GetFormulabyName(string name)
        {
            string sql = "select \"Id\" from \"T_Formula\" where \"Name\"=:name";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);

            return GetData<Formula>(sql, pms);
        }

        #endregion
    }
}
