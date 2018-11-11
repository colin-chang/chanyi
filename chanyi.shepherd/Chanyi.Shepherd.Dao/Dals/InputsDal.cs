using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Spring.Data.Common;
using Spring.Transaction.Interceptor;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;
using Chanyi.Shepherd.QueryModel.Model.Finance;
using Chanyi.Shepherd.QueryModel.Model.Input;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region Inputs

        #region 饲料
        public List<Feed> GetFeedKindId(string nameId, string typeId, string areaId)
        {
            string sql = "select f.\"Id\" from \"T_Feed\" f where f.\"NameId\"=:nameId and f.\"TypeId\"=:typeId and f.\"AreaId\"=:areaId";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("nameId", nameId);
            pms.AddWithValue("typeId", typeId);
            pms.AddWithValue("areaId", areaId);

            return GetData<Feed>(sql, pms);
        }

        public object GetFeedInventoryAmount(string kindId)
        {
            string sql = "select \"Amount\" from \"T_FeedInventory\" where \"KindId\"=:kindId";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("kindId", kindId);

            return AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
        }

        public List<InOutAmount> GetFeedInOutAmount(string kindId, DateTime operationDate)
        {
            string sql = "select  \"sum\"(w.\"Amount\") as \"Amount\",\"Direction\" from \"T_FeedInOut\" fin  join \"T_InOutWarehouse\" w on fin.\"InOutWarehouseId\"=w.\"Id\" where w.\"KindId\"=:kindId and \"OperationDate\">:operationDate GROUP BY w.\"Direction\"";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("kindId", kindId);
            pms.AddWithValue("operationDate", operationDate);

            return GetData<InOutAmount>(sql, pms);
        }


        [Transaction]
        public void AddFeedInOutWarehouse(string id, string kindId, float amount, DateTime operationDate, InOutWarehouseDirectionEnum direction, OutWarehouseDispositonEnum dispositon, string principalId, string operatorId, DateTime createTime, string remark, bool hasInventory)
        {
            //T_InOutWarehouse
            //T_FeedInOut
            //T_FeedInventory
            StringBuilder sb = new StringBuilder();

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            //不存在该类型饲料库存，先创建
            if (hasInventory)
                sb.Append("update \"T_FeedInventory\" set \"Amount\"=\"Amount\"" + (direction == InOutWarehouseDirectionEnum.In ? "+" : "-") + ":inventoryAmount where \"KindId\"=:inventoryKindId;");
            else
            {
                sb.Append("INSERT into \"T_FeedInventory\" (\"Id\",\"KindId\",\"Amount\")VALUES(:inventoryId,:inventoryKindId,:inventoryAmount);");
                pms.AddWithValue("inventoryId", Guid.NewGuid());
            }


            sb.Append("INSERT into \"T_InOutWarehouse\" (\"Id\",\"KindId\",\"Amount\",\"Direction\",\"Dispositon\",\"SurplusInventory\",\"PrincipalId\",\"OperatorId\",\"OperationDate\",\"CreateTime\",\"Remark\")VALUES(:id,:kindId,:amount,:direction,:dispositon,(select \"Amount\" from \"T_FeedInventory\" where \"KindId\"=:selectInventoryKindId),:principalId,:operatorId,:operationDate,:createTime,:remark);INSERT into \"T_FeedInOut\" (\"InOutWarehouseId\")VALUES(:inOutWarehouseId);");


            pms.AddWithValue("inventoryAmount", amount);
            pms.AddWithValue("inventoryKindId", kindId);

            pms.AddWithValue("id", id);
            pms.AddWithValue("kindId", kindId);
            pms.AddWithValue("amount", amount);
            pms.AddWithValue("direction", (int)direction);
            pms.AddWithValue("dispositon", (int)dispositon);

            pms.AddWithValue("selectInventoryKindId", kindId);

            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("inOutWarehouseId", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }

        [Transaction]
        public void AddFeedBatchOutWarehouse(List<Sheep4FeedOutWarehouse> listSheeps, List<FeedPrice> listPrices, Dictionary<string, float> dictFeedAmount, DateTime operationDate, InOutWarehouseDirectionEnum inOutWarehouseDirectionEnum, OutWarehouseDispositonEnum outWarehouseDispositonEnum, string principalId, string operatorId, DateTime createTime, string remark)
        {
            //出库记录
            //库存量改变
            //羊只进食量

            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            string id;
            for (int i = 0; i < listPrices.Count; i++)
            {
                id = Guid.NewGuid().ToString();

                FeedPrice fp = listPrices[i];

                sb.AppendFormat("update \"T_FeedInventory\" set \"Amount\"=\"Amount\"-:inventoryAmount{0} where \"KindId\"=:inventoryKindId{0};INSERT into \"T_InOutWarehouse\" (\"Id\",\"KindId\",\"Amount\",\"Direction\",\"Dispositon\",\"SurplusInventory\",\"PrincipalId\",\"OperatorId\",\"OperationDate\",\"CreateTime\",\"Remark\")VALUES(:id{0},:kindId{0},:amount{0},:direction{0},:dispositon{0},(select \"Amount\" from \"T_FeedInventory\" where \"KindId\"=:selectInventoryKindId{0}),:principalId{0},:operatorId{0},:operationDate{0},:createTime{0},:remark{0});INSERT into \"T_FeedInOut\" (\"InOutWarehouseId\")VALUES(:inOutWarehouseId{0});", i);
                pms.AddWithValue("inventoryAmount" + i, dictFeedAmount[fp.KindId]);
                pms.AddWithValue("inventoryKindId" + i, fp.KindId);

                pms.AddWithValue("id" + i, id);
                pms.AddWithValue("kindId" + i, fp.KindId);
                pms.AddWithValue("amount" + i, dictFeedAmount[fp.KindId]);
                pms.AddWithValue("direction" + i, (int)inOutWarehouseDirectionEnum);
                pms.AddWithValue("dispositon" + i, (int)outWarehouseDispositonEnum);

                pms.AddWithValue("selectInventoryKindId" + i, fp.KindId);

                pms.AddWithValue("principalId" + i, principalId);
                pms.AddWithValue("operatorId" + i, operatorId);
                pms.AddWithValue("operationDate" + i, operationDate);
                pms.AddWithValue("createTime" + i, createTime);
                pms.AddWithValue("remark" + i, remark);

                pms.AddWithValue("inOutWarehouseId" + i, id);

                for (int j = 0; j < listSheeps.Count; j++)
                {
                    Sheep4FeedOutWarehouse sp = listSheeps[j];

                    string m = string.Format("_{0}_{1}", i, j);
                    sb.AppendFormat("insert into \"T_FeedSheepOutWarehouse\" (\"Id\",\"ShepfoldId\",\"SheepId\",\"GrowthStage\",\"KindId\",\"Amount\",\"Price\",\"OutWarehouseId\")VALUES(:Id{0},:ShepfoldId{0},:SheepId{0},:GrowthStage{0},:KindId{0},:Amount{0},:Price{0},:OutWarehouseId{0});", m);

                    pms.AddWithValue("id" + m, Guid.NewGuid().ToString());
                    pms.AddWithValue("shepfoldId" + m, sp.SheepfoldId);
                    pms.AddWithValue("sheepId" + m, sp.Id);
                    pms.AddWithValue("growthStage" + m, (int)sp.GrowthStage);
                    pms.AddWithValue("kindId" + m, fp.KindId);
                    pms.AddWithValue("amount" + m, dictFeedAmount[fp.KindId] * 1.0 / listSheeps.Count);
                    pms.AddWithValue("price" + m, fp.Price);
                    pms.AddWithValue("outWarehouseId" + m, id);
                }
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }

        public void AddAreaName(string id, string name, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Area\" (\"Id\",\"Name\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:name,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public int GetAreaNameCount(string name)
        {
            string sql = "select count(\"Id\") from \"T_Area\" where trim(\"Name\")=:name";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);

            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms));
        }

        public void AddInputName(string id, string name, string category, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_InputName\" (\"Id\",\"Name\",\"Category\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:name,:category,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("category", category);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }
        public int GetInputNameCount(string name, string category)
        {
            string sql = "select count(\"Id\") from \"T_InputName\" where trim(\"Name\")=:name and \"Category\"=:category";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            pms.AddWithValue("category", category);

            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms));
        }

        [Transaction]
        public void AddFeed(string id, string feedNameId, string typeNameId, string areaId, string description, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? AllP, string operatorId, DateTime createTime)
        {
            string sql = "INSERT into \"T_Feed\" (\"Id\",\"NameId\",\"TypeId\",\"AreaId\")VALUES(:id,:nameId,:typeId,:areaId);INSERT into \"T_FeedDetail\" (\"KindId\",\"Description\",\"CP\",\"DMI\",\"EE\",\"CF\",\"NFE\",\"Ash\",\"NDF\",\"ADF\",\"Starch\",\"Ga\",\"Arg\",\"His\",\"Ile\",\"Leu\",\"Lys\",\"Met\",\"Cys\",\"Phe\",\"Tyr\",\"Thr\",\"Trp\",\"Val\",\"P\",\"Na\",\"Cl\",\"Mg\",\"K\",\"Fe\",\"Cu\",\"Mn\",\"Zn\",\"Se\",\"Carotene\",\"VE\",\"VB1\",\"VB2\",\"PantothenicAcid\",\"Niacin\",\"Biotin\",\"Folic\",\"Choline\",\"VB6\",\"VB12\",\"LinoleicAcid\",\"AllP\",\"IsEditable\",\"OperatorId\",\"CreateTime\")VALUES(:kindId,:description,:cP,:dMI,:eE,:cF,:nFE,:ash,:nDF,:aDF,:starch,:ga,:arg,:his,:ile,:leu,:lys,:met,:cys,:phe,:tyr,:thr,:trp,:val,:p,:na,:cl,:mg,:k,:fe,:cu,:mn,:zn,:se,:carotene,:vE,:vB1,:vB2,:pantothenicAcid,:niacin,:biotin,:folic,:choline,:vB6,:vB12,:linoleicAcid,:allP,:isEditable,:operatorId,:createTime)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("nameId", feedNameId);
            pms.AddWithValue("typeId", typeNameId);
            pms.AddWithValue("areaId", areaId);

            pms.AddWithValue("kindId", id);
            pms.AddWithValue("description", description);
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
            pms.AddWithValue("allP", AllP);
            pms.AddWithValue("isEditable", true);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public List<Feed> GetFeed(FeedFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER ( ORDER BY fd.\"CreateTime\" DESC, fd.\"IsEditable\" ) AS \"rownum\", f.\"Id\", n.\"Name\", d.\"Value\" AS \"Type\", A .\"Name\" AS \"Area\", fd.\"Description\", fd.\"IsEditable\" FROM \"T_Feed\" f JOIN \"T_FeedDetail\" fd ON fd.\"KindId\" = f.\"Id\" JOIN \"T_InputName\" n ON n.\"Id\" = f.\"NameId\" JOIN \"T_InventoryDict\" d ON d.\"Id\" = f.\"TypeId\" LEFT JOIN \"T_Area\" A ON A .\"Id\" = f.\"AreaId\"";

            string countSql = "select count(f.\"Id\") from \"T_Feed\" f join \"T_FeedDetail\" fd on fd.\"KindId\"=f.\"Id\" join \"T_InputName\" ine on ine.\"Id\"=f.\"NameId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\"";

            return GetPagedData<Feed, FeedFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<Feed> GetFeed(FeedFilter filter, int rowCount)
        {
            string querySql = "SELECT \"row_number\" () OVER ( ORDER BY fd.\"CreateTime\" DESC, fd.\"IsEditable\" ) AS \"rownum\", f.\"Id\", n.\"Name\", d.\"Value\" AS \"Type\", A .\"Name\" AS \"Area\", fd.\"Description\", fd.\"IsEditable\" FROM \"T_Feed\" f JOIN \"T_FeedDetail\" fd ON fd.\"KindId\" = f.\"Id\" JOIN \"T_InputName\" n ON n.\"Id\" = f.\"NameId\" JOIN \"T_InventoryDict\" d ON d.\"Id\" = f.\"TypeId\" LEFT JOIN \"T_Area\" A ON A .\"Id\" = f.\"AreaId\"";

            return GetRuledRowsData<Feed, FeedFilter>(rowCount, querySql, filter);
        }

        public Feed GetFeedByKindId(string kindId)
        {
            string sql = "select f.\"Id\",f.\"AreaId\",f.\"NameId\",f.\"TypeId\",fd.\"Description\",fd.\"CP\",fd.\"DMI\",fd.\"EE\",fd.\"CF\",fd.\"NFE\",fd.\"Ash\",fd.\"NDF\",fd.\"ADF\",fd.\"Starch\",fd.\"Ga\",fd.\"Arg\",fd.\"His\",fd.\"Ile\",fd.\"Leu\",fd.\"Lys\",fd.\"Met\",fd.\"Cys\",fd.\"Phe\",fd.\"Tyr\",fd.\"Thr\",fd.\"Trp\",fd.\"Val\",fd.\"P\",fd.\"Na\",fd.\"Cl\",fd.\"Mg\",fd.\"K\",fd.\"Fe\",fd.\"Cu\",fd.\"Mn\",fd.\"Zn\",fd.\"Se\",fd.\"Carotene\",fd.\"VE\",fd.\"VB1\",fd.\"VB2\",fd.\"PantothenicAcid\",fd.\"Niacin\",fd.\"Biotin\",fd.\"Folic\",fd.\"Choline\",fd.\"VB6\",fd.\"VB12\",fd.\"LinoleicAcid\",fd.\"AllP\",fd.\"IsEditable\" from \"T_Feed\" f join \"T_FeedDetail\" fd on fd.\"KindId\"=f.\"Id\" where f.\"Id\"=:kindId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("kindId", kindId);

            return GetData<Feed>(sql, pms).FirstOrDefault();
        }

        public List<FeedDetail> GetFeedDetail(string kindId)
        {
            string sql = "select fd.\"CP\",fd.\"DMI\",fd.\"EE\",fd.\"CF\",fd.\"NFE\",fd.\"Ash\",fd.\"NDF\",fd.\"ADF\",fd.\"Starch\",fd.\"Ga\",fd.\"Arg\",fd.\"His\",fd.\"Ile\",fd.\"Leu\",fd.\"Lys\",fd.\"Met\",fd.\"Cys\",fd.\"Phe\",fd.\"Tyr\",fd.\"Thr\",fd.\"Trp\",fd.\"Val\",fd.\"P\",fd.\"Na\",fd.\"Cl\",fd.\"Mg\",fd.\"K\",fd.\"Fe\",fd.\"Cu\",fd.\"Mn\",fd.\"Zn\",fd.\"Se\",fd.\"Carotene\",fd.\"VE\",fd.\"VB1\",fd.\"VB2\",fd.\"PantothenicAcid\",fd.\"Niacin\",fd.\"Biotin\",fd.\"Folic\",fd.\"Choline\",fd.\"VB6\",fd.\"VB12\",fd.\"LinoleicAcid\",fd.\"AllP\" from  \"T_FeedDetail\" fd where fd.\"KindId\"=:kindId";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("kindId", kindId);
            return GetData<FeedDetail>(sql, pms);
        }

        public List<FeedInOut> GetFeedInOut(FeedInOutFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY w.\"CreateTime\" DESC) \"rownum\",w.\"Id\",w.\"KindId\",w.\"Amount\",w.\"Direction\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",w.\"CreateTime\",w.\"Remark\",n.\"Name\",a.\"Name\" as \"Area\",d.\"Value\" as \"Type\",w.\"OperationDate\",w.\"Dispositon\" from \"T_FeedInOut\" io join \"T_InOutWarehouse\" w on w.\"Id\"=io.\"InOutWarehouseId\" join \"T_Feed\" f on f.\"Id\"=w.\"KindId\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\"";

            string countSql = "select count(w.\"Id\") from \"T_FeedInOut\" io join \"T_InOutWarehouse\" w on w.\"Id\"=io.\"InOutWarehouseId\" join \"T_Feed\" f on f.\"Id\"=w.\"KindId\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\"";

            return GetPagedData<FeedInOut, FeedInOutFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<FeedInOut> GetFeedInOut(FeedInOutFilter filter, int rowCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY w.\"CreateTime\" DESC) \"rownum\",w.\"Id\",w.\"KindId\",w.\"Amount\",w.\"Direction\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",w.\"CreateTime\",w.\"Remark\",n.\"Name\",a.\"Name\" as \"Area\",d.\"Value\" as \"Type\",w.\"OperationDate\",w.\"Dispositon\" from \"T_FeedInOut\" io join \"T_InOutWarehouse\" w on w.\"Id\"=io.\"InOutWarehouseId\" join \"T_Feed\" f on f.\"Id\"=w.\"KindId\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\"";
            return GetRuledRowsData<FeedInOut, FeedInOutFilter>(rowCount, querySql, filter);
        }
        public FeedInOut GetFeedInOutDetailById(string id)
        {
            string sql = "select w.\"Id\",w.\"KindId\",w.\"Amount\",w.\"Direction\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",w.\"CreateTime\",w.\"Remark\",n.\"Name\",a.\"Name\" as \"Area\",d.\"Value\" as \"Type\",w.\"OperationDate\" from \"T_FeedInOut\" io join \"T_InOutWarehouse\" w on w.\"Id\"=io.\"InOutWarehouseId\" join \"T_Feed\" f on f.\"Id\"=w.\"KindId\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\" where w.\"Id\"=:id and \"Direction\"=:direction and d.\"Category\"=:category";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("direction", (int)InOutWarehouseDirectionEnum.In);
            pms.AddWithValue("category", feedTypeCategory);

            return GetData<FeedInOut>(sql, pms).FirstOrDefault();
        }

        public List<FeedInventory> GetFeedInventory(FeedInventoryFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY inm.\"Name\") \"rownum\",fiy.\"Id\",fiy.\"KindId\",inm.\"Name\",a.\"Name\" as \"Area\",d.\"Value\" as \"Type\",fiy.\"Amount\" from \"T_FeedInventory\" fiy join \"T_Feed\" f on f.\"Id\"=fiy.\"KindId\" join \"T_InputName\" inm on inm.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\"";

            string countSql = "select count(fiy.\"Id\") from \"T_FeedInventory\" fiy join \"T_Feed\" f on f.\"Id\"=fiy.\"KindId\" join \"T_InputName\" inm on inm.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\"";

            return GetPagedData<FeedInventory, FeedInventoryFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<FeedInventory> GetFeedInventory(FeedInventoryFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY inm.\"Name\") \"rownum\",fiy.\"Id\",fiy.\"KindId\",inm.\"Name\",a.\"Name\" as \"Area\",d.\"Value\" as \"Type\",fiy.\"Amount\" from \"T_FeedInventory\" fiy join \"T_Feed\" f on f.\"Id\"=fiy.\"KindId\" join \"T_InputName\" inm on inm.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\"";
            return GetRuledRowsData<FeedInventory, FeedInventoryFilter>(rowsCount, querySql, filter);
        }

        public List<FeedInventory> GetFeedInventory()
        {
            string sql = "SELECT f.\"KindId\", f.\"Amount\", ip.\"Name\", i.\"Value\" AS \"Type\", aa.\"Name\" AS \"Area\" FROM \"T_FeedInventory\" AS f JOIN \"T_Feed\" AS fd ON f.\"KindId\" = fd.\"Id\" JOIN \"T_InputName\" AS ip ON ip.\"Id\" = fd.\"NameId\" JOIN \"T_InventoryDict\" AS i ON i.\"Id\" = fd.\"TypeId\" JOIN \"T_Area\" AS aa ON aa.\"Id\" = fd.\"AreaId\"";
            return this.GetData<FeedInventory>(sql, null);
        }

        public bool ValidateFeed(string id, string feedNameId, string typeNameId, string areaId)
        {
            string sql = "select count(\"Id\") from \"T_Feed\" where \"AreaId\"=:areaId and \"TypeId\"=:typeNameId and \"NameId\"=:feedNameId and \"Id\"<>:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("feedNameId", feedNameId);
            pms.AddWithValue("typeNameId", typeNameId);
            pms.AddWithValue("areaId", areaId);
            pms.AddWithValue("id", id);

            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms)) > 0;
        }

        [Transaction]
        public void UpdateFeed(string feedNameId, string typeNameId, string areaId, string description, float? CP, float? DMI, float? EE, float? CF, float? NFE, float? Ash, float? NDF, float? ADF, float? Starch, float? Ga, float? Arg, float? His, float? Ile, float? Leu, float? Lys, float? Met, float? Cys, float? Phe, float? Tyr, float? Thr, float? Trp, float? Val, float? P, float? Na, float? Cl, float? Mg, float? K, float? Fe, float? Cu, float? Mn, float? Zn, float? Se, float? Carotene, float? VE, float? VB1, float? VB2, float? PantothenicAcid, float? Niacin, float? Biotin, float? Folic, float? Choline, float? VB6, float? VB12, float? LinoleicAcid, float? AllP, string id)
        {
            string sql = "UPDATE \"T_Feed\" SET \"NameId\"=:nameId,\"TypeId\"=:typeId,\"AreaId\"=:areaId where \"Id\"=:id;UPDATE \"T_FeedDetail\" SET \"Description\"=:description,\"CP\"=:cP,\"DMI\"=:dMI,\"EE\"=:eE,\"CF\"=:cF,\"NFE\"=:nFE,\"Ash\"=:ash,\"NDF\"=:nDF,\"ADF\"=:aDF,\"Starch\"=:starch,\"Ga\"=:ga,\"Arg\"=:arg,\"His\"=:his,\"Ile\"=:ile,\"Leu\"=:leu,\"Lys\"=:lys,\"Met\"=:met,\"Cys\"=:cys,\"Phe\"=:phe,\"Tyr\"=:tyr,\"Thr\"=:thr,\"Trp\"=:trp,\"Val\"=:val,\"P\"=:p,\"Na\"=:na,\"Cl\"=:cl,\"Mg\"=:mg,\"K\"=:k,\"Fe\"=:fe,\"Cu\"=:cu,\"Mn\"=:mn,\"Zn\"=:zn,\"Se\"=:se,\"Carotene\"=:carotene,\"VE\"=:vE,\"VB1\"=:vB1,\"VB2\"=:vB2,\"PantothenicAcid\"=:pantothenicAcid,\"Niacin\"=:niacin,\"Biotin\"=:biotin,\"Folic\"=:folic,\"Choline\"=:choline,\"VB6\"=:vB6,\"VB12\"=:vB12,\"LinoleicAcid\"=:linoleicAcid,\"AllP\"=:allP where \"KindId\"=:kindId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("nameId", feedNameId);
            pms.AddWithValue("typeId", typeNameId);
            pms.AddWithValue("areaId", areaId);

            pms.AddWithValue("description", description);
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
            pms.AddWithValue("allP", AllP);

            pms.AddWithValue("kindId", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public bool IsFeedEditable(string id)
        {
            string sql = "select \"IsEditable\" from \"T_FeedDetail\" where \"KindId\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            object obj = AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
            return obj == null ? false : bool.Parse(obj.ToString());
        }

        public List<FeedWithAllFileds> GetFeedWithAllFileds()
        {
            string sql = "select f.\"Id\",n.\"Name\",f.\"NameId\",a.\"Name\" as \"Area\",f.\"AreaId\",d.\"Value\" as \"Type\",f.\"TypeId\",fd.\"Description\",fd.\"CP\",fd.\"DMI\",fd.\"EE\",fd.\"CF\",fd.\"NFE\",fd.\"Ash\",fd.\"NDF\",fd.\"ADF\",fd.\"Starch\",fd.\"Ga\",fd.\"Arg\",fd.\"His\",fd.\"Ile\",fd.\"Leu\",fd.\"Lys\",fd.\"Met\",fd.\"Cys\",fd.\"Phe\",fd.\"Tyr\",fd.\"Thr\",fd.\"Trp\",fd.\"Val\",fd.\"P\",fd.\"Na\",fd.\"Cl\",fd.\"Mg\",fd.\"K\",fd.\"Fe\",fd.\"Cu\",fd.\"Mn\",fd.\"Zn\",fd.\"Se\",fd.\"Carotene\",fd.\"VE\",fd.\"VB1\",fd.\"VB2\",fd.\"PantothenicAcid\",fd.\"Niacin\",fd.\"Biotin\",fd.\"Folic\",fd.\"Choline\",fd.\"VB6\",fd.\"VB12\",fd.\"LinoleicAcid\",fd.\"AllP\",fd.\"IsEditable\" from \"T_Feed\" f join \"T_FeedDetail\" fd on fd.\"KindId\"=f.\"Id\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\"  and d.\"Category\"=:category";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("category", feedTypeCategory);
            return GetData<FeedWithAllFileds>(sql, pms);
        }

        #endregion

        #region 药品
        public List<Medicine> GetMedicineId(string nameId, string manufacturerId, string typeId)
        {
            string sql = "select \"Id\" from \"T_Medicine\" where \"NameId\"=:nameId and \"ManufacturerId\"=:manufacturerId and \"TypeId\"=:typeId";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("nameId", nameId);
            pms.AddWithValue("manufacturerId", manufacturerId);
            pms.AddWithValue("typeId", typeId);

            return GetData<Medicine>(sql, pms);
        }

        public List<Medicine> GetMedicineKindId(string nameId, string manufacturerId, string typeId, DateTime expirationDate)
        {
            string sql = "select c.\"Id\" from \"T_Medicine\" m join \"T_MedicineCrucial\" c on m.\"Id\"=c.\"KindId\" where \"NameId\"=:nameId and \"ManufacturerId\"=:manufacturerId and \"TypeId\"=:typeId and c.\"ExpirationDate\"=:expirationDate";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("nameId", nameId);
            pms.AddWithValue("manufacturerId", manufacturerId);
            pms.AddWithValue("typeId", typeId);
            pms.AddWithValue("expirationDate", expirationDate);

            return GetData<Medicine>(sql, pms);
        }

        public object ValidateMedicineCount(string kindId)
        {
            string sql = "select \"Amount\" from \"T_MedicineInventory\" where \"KindId\"=:kindId";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("kindId", kindId);

            return AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
        }

        public List<InOutAmount> GetMedicineInOutAmount(string kindId, DateTime operationDate)
        {
            string sql = "select  \"sum\"(w.\"Amount\") as \"Amount\",\"Direction\" from \"T_MedicineInOut\" mio  join \"T_InOutWarehouse\" w on mio.\"InOutWarehouseId\"=w.\"Id\" where w.\"KindId\"=:kindId and \"OperationDate\">:operationDate GROUP BY w.\"Direction\"";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("kindId", kindId);
            pms.AddWithValue("operationDate", operationDate);

            return GetData<InOutAmount>(sql, pms);
        }

        [Transaction]
        public void AddManufacturer(string id, string name, string department, string contactInfo, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Manufacture\" (\"CooperaterId\")VALUES(:cooperaterId);INSERT into \"T_Cooperater\" (\"Id\",\"Name\",\"Department\",\"ContactInfo\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:name,:department,:contactInfo,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("cooperaterId", id);

            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);
            pms.AddWithValue("department", department);
            pms.AddWithValue("contactInfo", contactInfo);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        [Transaction]
        public void AddMedicine(string id, string nameId, string manufacturerId, string typeId, string medicineUnit, string operatorId, DateTime createTime, string remark)
        {
            //\"ExpirationDate\"
            //:expirationDate
            string sql = "INSERT into \"T_Medicine\" (\"Id\",\"NameId\",\"ManufacturerId\",\"TypeId\")VALUES(:id,:nameId,:manufacturerId,:typeId);INSERT into \"T_MedicineDetail\" (\"KindId\",\"Unit\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:kindId,:unit,:operatorId,:createTime,:remark)";

            //expirationDate与serialNumber暂时未使用
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("nameId", nameId);
            pms.AddWithValue("manufacturerId", manufacturerId);
            pms.AddWithValue("typeId", typeId);

            pms.AddWithValue("kindId", id);
            pms.AddWithValue("unit", medicineUnit);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            //pms.AddWithValue("expirationDate", expirationDate);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        [Transaction]
        public void AddMedicineInOutWarehouse(string id, string kindId, float amount, DateTime operationDate, InOutWarehouseDirectionEnum direction, OutWarehouseDispositonEnum dispositon, string principalId, string operatorId, DateTime createTime, string remark, bool hasInvertory)
        {
            //InOutWarehouse
            //MedicineInOUtWarehouse
            //MedicineInventory

            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            sb.Append("INSERT into \"T_InOutWarehouse\" (\"Id\",\"KindId\",\"Amount\",\"Direction\",\"Dispositon\",\"PrincipalId\",\"OperatorId\",\"OperationDate\",\"CreateTime\",\"Remark\")VALUES(:id,:kindId,:amount,:direction,:dispositon,:principalId,:operatorId,:operationDate,:createTime,:remark);INSERT into \"T_MedicineInOut\" (\"InOutWarehouseId\")VALUES(:inOutWarehouseId);");

            pms.AddWithValue("id", id);
            pms.AddWithValue("kindId", kindId);
            pms.AddWithValue("amount", amount);
            pms.AddWithValue("direction", (int)direction);
            pms.AddWithValue("dispositon", (int)dispositon);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("inOutWarehouseId", id);

            pms.AddWithValue("inventoryAmount", amount);
            pms.AddWithValue("inventoryKindId", kindId);
            if (hasInvertory)
                sb.Append("update \"T_MedicineInventory\" set \"Amount\"=\"Amount\"" + (direction == InOutWarehouseDirectionEnum.In ? "+" : "-") + ":inventoryAmount where \"KindId\"=:inventoryKindId");
            else
            {
                sb.Append("INSERT into \"T_MedicineInventory\" (\"Id\",\"KindId\",\"Amount\")VALUES(:inventoryId,:inventoryKindId,:inventoryAmount)");
                pms.AddWithValue("inventoryId", Guid.NewGuid());
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }

        [Transaction]
        public void AddMedicineInOutWarehouse(string id, string kindId, DateTime expirationDate, float amount, DateTime operationDate, InOutWarehouseDirectionEnum direction, OutWarehouseDispositonEnum dispositon, string principalId, string operatorId, DateTime createTime, string remark)
        {
            //T_MedicineCrucial(药品关键性表（Id为药品的KindId）)

            //InOutWarehouse
            //MedicineInOUtWarehouse
            //MedicineInventory

            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            sb.Append("INSERT into \"T_MedicineCrucial\" (\"Id\",\"KindId\",\"ExpirationDate\")VALUES(:crucialId,:kindId,:expirationDate);INSERT into \"T_InOutWarehouse\" (\"Id\",\"KindId\",\"Amount\",\"Direction\",\"Dispositon\",\"PrincipalId\",\"OperatorId\",\"OperationDate\",\"CreateTime\",\"Remark\")VALUES(:id,:crucialId2,:amount,:direction,:dispositon,:principalId,:operatorId,:operationDate,:createTime,:remark);INSERT into \"T_MedicineInOut\" (\"InOutWarehouseId\")VALUES(:inOutWarehouseId);INSERT into \"T_MedicineInventory\" (\"Id\",\"KindId\",\"Amount\")VALUES(:inventoryId,:inventoryKindId,:inventoryAmount)");
            string crucialId = Guid.NewGuid().ToString();

            pms.AddWithValue("crucialId", crucialId);
            pms.AddWithValue("kindId", kindId);//药品表的Id
            pms.AddWithValue("expirationDate", expirationDate);

            pms.AddWithValue("id", id);//出入库Id
            pms.AddWithValue("crucialId2", crucialId);//药品决定性表的Id（药品表Id+过期时间）
            pms.AddWithValue("amount", amount);
            pms.AddWithValue("direction", (int)direction);
            pms.AddWithValue("dispositon", (int)dispositon);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("inOutWarehouseId", id);//出入库Id

            pms.AddWithValue("inventoryAmount", amount);
            pms.AddWithValue("inventoryKindId", crucialId);//库存，药品决定性Id

            pms.AddWithValue("inventoryId", Guid.NewGuid());

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }

        public List<Medicine> GetMedicine(MedicineFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY n.\"Name\") as \"rownum\",m.\"Id\",md.\"CreateTime\",md.\"Remark\",d.\"Value\" as \"Type\",md.\"Unit\",n.\"Name\",c.\"Name\" as \"Manufacturer\",c.\"Department\",u.\"UserName\" as \"OperatorName\" from \"T_Medicine\" m join \"T_MedicineDetail\" md on md.\"KindId\"=m.\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Manufacture\" mtr on mtr.\"CooperaterId\"=m.\"ManufacturerId\" join \"T_Cooperater\" c on c.\"Id\"=mtr.\"CooperaterId\" join \"T_User\" u on u.\"Id\"=md.\"OperatorId\"";

            string countSql = "select count(m.\"Id\") from \"T_Medicine\" m join \"T_MedicineDetail\" md on md.\"KindId\"=m.\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Manufacture\" mtr on mtr.\"CooperaterId\"=m.\"ManufacturerId\" join \"T_Cooperater\" c on c.\"Id\"=mtr.\"CooperaterId\" join \"T_User\" u on u.\"Id\"=md.\"OperatorId\"";

            return GetPagedData<Medicine, MedicineFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<Medicine> GetMedicine(MedicineFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY n.\"Name\") as \"rownum\",m.\"Id\",md.\"CreateTime\",md.\"Remark\",d.\"Value\" as \"Type\",md.\"Unit\",n.\"Name\",c.\"Name\" as \"Manufacturer\",c.\"Department\",u.\"UserName\" as \"OperatorName\" from \"T_Medicine\" m join \"T_MedicineDetail\" md on md.\"KindId\"=m.\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Manufacture\" mtr on mtr.\"CooperaterId\"=m.\"ManufacturerId\" join \"T_Cooperater\" c on c.\"Id\"=mtr.\"CooperaterId\" join \"T_User\" u on u.\"Id\"=md.\"OperatorId\"";
            return GetRuledRowsData<Medicine, MedicineFilter>(rowsCount, querySql, filter);
        }

        public Medicine GetMedicineByKindId(string kindId)
        {
            string sql = "select m.\"Id\",m.\"NameId\",m.\"ManufacturerId\",md.\"Remark\" from \"T_Medicine\" m join \"T_MedicineDetail\" md on md.\"KindId\"=m.\"Id\" where m.\"Id\"=:kindId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("kindId", kindId);

            return GetData<Medicine>(sql, pms).FirstOrDefault();
        }

        public List<MedicineInventory> GetMedicineInventory(MedicineInventoryFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY n.\"Name\") as \"rownum\",mi.\"Id\",mi.\"KindId\",n.\"Name\",d.\"Value\" as \"Type\",md.\"Unit\",c.\"Name\" as \"Manufacturer\",c.\"Department\",mi.\"Amount\",mc.\"ExpirationDate\" FROM \"T_MedicineInventory\" mi join \"T_MedicineCrucial\"  mc on mi.\"KindId\"=mc.\"Id\" JOIN \"T_Medicine\" M on mc.\"KindId\"=m.\"Id\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Manufacture\" mtr on mtr.\"CooperaterId\"=m.\"ManufacturerId\" join \"T_Cooperater\" c on c.\"Id\"=mtr.\"CooperaterId\"";

            string countSql = "select count(mi.\"Id\") FROM \"T_MedicineInventory\" mi join \"T_MedicineCrucial\"  mc on mi.\"KindId\"=mc.\"Id\" JOIN \"T_Medicine\" M on mc.\"KindId\"=m.\"Id\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Manufacture\" mtr on mtr.\"CooperaterId\"=m.\"ManufacturerId\" join \"T_Cooperater\" c on c.\"Id\"=mtr.\"CooperaterId\"";

            return GetPagedData<MedicineInventory, MedicineInventoryFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<MedicineInventory> GetMedicineInventory(MedicineInventoryFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY n.\"Name\") as \"rownum\",mi.\"Id\",mi.\"KindId\",n.\"Name\",c.\"Name\" as \"Manufacturer\",c.\"Department\",d.\"Value\" as \"Type\",md.\"Unit\",mi.\"Amount\",mc.\"ExpirationDate\" FROM \"T_MedicineInventory\" mi join \"T_MedicineCrucial\"  mc on mi.\"KindId\"=mc.\"Id\" JOIN \"T_Medicine\" M on mc.\"KindId\"=m.\"Id\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Manufacture\" mtr on mtr.\"CooperaterId\"=m.\"ManufacturerId\" join \"T_Cooperater\" c on c.\"Id\"=mtr.\"CooperaterId\"";
            return GetRuledRowsData<MedicineInventory, MedicineInventoryFilter>(rowsCount, querySql, filter);
        }

        public List<MedicineInOut> GetMenicineInOut(MedicineInOutFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY w.\"CreateTime\" DESC) as \"rownum\",w.\"Id\",w.\"KindId\",w.\"Amount\",w.\"Direction\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",w.\"CreateTime\",w.\"OperationDate\",w.\"Remark\",n.\"Name\",c.\"Name\" as \"Manufacturer\",c.\"Department\",d.\"Value\" as \"Type\",md.\"Unit\",mc.\"ExpirationDate\" from \"T_MedicineInOut\" mio join \"T_InOutWarehouse\" w on w.\"Id\"=mio.\"InOutWarehouseId\" join \"T_MedicineCrucial\"  mc on w.\"KindId\"=mc.\"Id\" JOIN \"T_Medicine\" M on mc.\"KindId\"=m.\"Id\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Manufacture\" mt on mt.\"CooperaterId\"=m.\"ManufacturerId\" join \"T_Cooperater\" c on c.\"Id\"=mt.\"CooperaterId\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\"";

            string countSql = "select count(w.\"Id\") from \"T_MedicineInOut\" mio join \"T_InOutWarehouse\" w on w.\"Id\"=mio.\"InOutWarehouseId\" join \"T_MedicineCrucial\"  mc on w.\"KindId\"=mc.\"Id\" JOIN \"T_Medicine\" M on mc.\"KindId\"=m.\"Id\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Manufacture\" mt on mt.\"CooperaterId\"=m.\"ManufacturerId\" join \"T_Cooperater\" c on c.\"Id\"=mt.\"CooperaterId\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\"";

            return GetPagedData<MedicineInOut, MedicineInOutFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<MedicineInOut> GetMenicineInOut(MedicineInOutFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY w.\"CreateTime\" DESC) as \"rownum\",w.\"Id\",w.\"KindId\",w.\"Amount\",w.\"Direction\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",w.\"CreateTime\",w.\"OperationDate\",w.\"Remark\",n.\"Name\",c.\"Name\" as \"Manufacturer\",c.\"Department\",d.\"Value\" as \"Type\",md.\"Unit\",mc.\"ExpirationDate\" from \"T_MedicineInOut\" mio join \"T_InOutWarehouse\" w on w.\"Id\"=mio.\"InOutWarehouseId\" join \"T_MedicineCrucial\"  mc on w.\"KindId\"=mc.\"Id\" JOIN \"T_Medicine\" M on mc.\"KindId\"=m.\"Id\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Manufacture\" mt on mt.\"CooperaterId\"=m.\"ManufacturerId\" join \"T_Cooperater\" c on c.\"Id\"=mt.\"CooperaterId\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\"";

            return GetRuledRowsData<MedicineInOut, MedicineInOutFilter>(rowsCount, querySql, filter);
        }
        public MedicineInOut GetMenicineInOutDetailById(string id)
        {
            string sql = "select w.\"Id\",w.\"KindId\",w.\"Amount\",w.\"Direction\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",w.\"CreateTime\",w.\"OperationDate\",w.\"Remark\",n.\"Name\",c.\"Name\" as \"Manufacturer\",c.\"Department\",d.\"Value\" as \"Type\",mc.\"ExpirationDate\" from \"T_MedicineInOut\" mio join \"T_InOutWarehouse\" w on w.\"Id\"=mio.\"InOutWarehouseId\" join \"T_MedicineCrucial\"  mc on w.\"KindId\"=mc.\"Id\" JOIN \"T_Medicine\" M on mc.\"KindId\"=m.\"Id\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Manufacture\" mt on mt.\"CooperaterId\"=m.\"ManufacturerId\" join \"T_Cooperater\" c on c.\"Id\"=mt.\"CooperaterId\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\" where w.\"Id\"=:id and \"Direction\"=:direction and d.\"Category\"=:category";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("direction", (int)InOutWarehouseDirectionEnum.In);
            pms.AddWithValue("category", feedTypeCategory);

            return GetData<MedicineInOut>(sql, pms).FirstOrDefault();
        }

        public bool ValidateMedicine(string id, string nameId, string manufacturerId, string typeId)
        {
            string sql = "select count(\"Id\") from \"T_Medicine\"  where \"NameId\"=:nameId and \"ManufacturerId\"=:manufacturerId and \"TypeId\"=:typeId and \"Id\"<>:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("nameId", nameId);
            pms.AddWithValue("manufacturerId", manufacturerId);
            pms.AddWithValue("typeId", typeId);
            pms.AddWithValue("id", id);

            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms)) > 0;
        }

        [Transaction]
        public void UpdateMedicine(string nameId, string manufacturerId, string typeId, string remark, string id)
        {
            string sql = "UPDATE \"T_Medicine\" SET \"NameId\"=:nameId,\"ManufacturerId\"=:manufacturerId, and \"TypeId\"=:typeId where \"Id\"=:id;UPDATE \"T_MedicineDetail\" SET \"Remark\"=:remark where \"KindId\"=:kindId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("nameId", nameId);
            pms.AddWithValue("manufacturerId", manufacturerId);
            pms.AddWithValue("typeId", typeId);
            pms.AddWithValue("id", id);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("kindId", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }
        #endregion

        #region 其他

        public List<Other> GetOtherKindId(string nameId)
        {
            string sql = "select \"Id\" from \"T_Other\"  where \"Id\"=:id";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", nameId);

            return GetData<Other>(sql, pms);
        }

        [Transaction]
        public void AddOther(string id, string name, string unit, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Other\" (\"Id\",\"Name\")VALUES(:id,:name);INSERT into \"T_OtherDetail\" (\"KindId\",\"Unit\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:kindId,:unit,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);

            pms.AddWithValue("kindId", id);
            pms.AddWithValue("unit", unit);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public object ValidateOtherCount(string kindId)
        {
            string sql = "select \"Amount\" from \"T_OtherInventory\" where \"KindId\"=:kindId";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("kindId", kindId);

            return AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
        }

        public List<InOutAmount> GetOtherInOutAmount(string kindId, DateTime operationDate)
        {
            string sql = "select  \"sum\"(w.\"Amount\") as \"Amount\",\"Direction\" from \"T_OtherInOut\" o  join \"T_InOutWarehouse\" w on o.\"InOutWarehouseId\"=w.\"Id\" where w.\"KindId\"=:kindId and \"OperationDate\">:operationDate GROUP BY w.\"Direction\"";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("kindId", kindId);
            pms.AddWithValue("operationDate", operationDate);

            return GetData<InOutAmount>(sql, pms);
        }

        [Transaction]
        public void AddOtherInOutWarehouse(string id, string kindId, float amount, DateTime operationDate, InOutWarehouseDirectionEnum direction, OutWarehouseDispositonEnum dispositon, string principalId, string operatorId, DateTime createTime, string remark, bool hasInvertory)
        {
            //InOutWarehouse
            //OtherInOUtWarehouse
            //OtherInventory

            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            sb.Append("INSERT into \"T_InOutWarehouse\" (\"Id\",\"KindId\",\"Amount\",\"Direction\",\"Dispositon\",\"PrincipalId\",\"OperatorId\",\"OperationDate\",\"CreateTime\",\"Remark\")VALUES(:id,:kindId,:amount,:direction,:dispositon,:principalId,:operatorId,:operationDate,:createTime,:remark);INSERT into \"T_OtherInOut\" (\"InOutWarehouseId\")VALUES(:inOutWarehouseId);");

            pms.AddWithValue("id", id);
            pms.AddWithValue("kindId", kindId);
            pms.AddWithValue("amount", amount);
            pms.AddWithValue("direction", (int)direction);
            pms.AddWithValue("dispositon", (int)dispositon);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            pms.AddWithValue("inOutWarehouseId", id);

            pms.AddWithValue("inventoryAmount", amount);
            pms.AddWithValue("inventoryKindId", kindId);

            if (hasInvertory)
                sb.Append("update \"T_OtherInventory\" set \"Amount\"=\"Amount\"" + (direction == InOutWarehouseDirectionEnum.In ? "+" : "-") + ":inventoryAmount where \"KindId\"=:inventoryKindId");
            else
            {
                sb.Append("INSERT into \"T_OtherInventory\" (\"Id\",\"KindId\",\"Amount\")VALUES(:inventoryId,:inventoryKindId,:inventoryAmount)");
                pms.AddWithValue("inventoryId", Guid.NewGuid());
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }

        public bool ValidateOther(string id, string name)
        {
            string sql = "select count(\"Id\") from \"T_Other\"  where \"Name\"=:name and \"Id\"<>:id";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("name", name);
            pms.AddWithValue("id", id);

            return Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms)) > 0;
        }

        public List<Other> GetOther(OtherFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY od.\"CreateTime\") as \"rownum\",o.\"Id\",o.\"Name\",od.\"Unit\",od.\"CreateTime\",od.\"Remark\",u.\"UserName\" as \"OperatorName\" from \"T_Other\" o join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\" join \"T_User\" u on u.\"Id\"=od.\"OperatorId\"";

            string countSql = "select count(o.\"Id\") from \"T_Other\" o join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\" join \"T_User\" u on u.\"Id\"=od.\"OperatorId\"";

            return GetPagedData<Other, OtherFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<Other> GetOther(OtherFilter filter, int rowCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY od.\"CreateTime\") as \"rownum\",o.\"Id\",o.\"Name\",od.\"Unit\",od.\"CreateTime\",od.\"Remark\",u.\"UserName\" as \"OperatorName\" from \"T_Other\" o join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\" join \"T_User\" u on u.\"Id\"=od.\"OperatorId\"";
            return GetRuledRowsData<Other, OtherFilter>(rowCount, querySql, filter);
        }

        public Other GetOtherByKindId(string kindId)
        {
            string sql = "select o.\"Id\",o.\"Name\",od.\"Unit\",od.\"Remark\" from \"T_Other\" o join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\" where \"KindId\"=:kindId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("kindId", kindId);

            return GetData<Other>(sql, pms).FirstOrDefault();
        }

        public List<OtherInOut> GetOtherInOut(OtherInOutFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY w.\"CreateTime\") as \"rownum\",w.\"Id\",w.\"KindId\",w.\"Amount\",w.\"Direction\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",w.\"OperationDate\",w.\"CreateTime\",w.\"Remark\",o.\"Name\",od.\"Unit\",w.\"Dispositon\" from \"T_OtherInOut\" oio join \"T_InOutWarehouse\" w on w.\"Id\"=oio.\"InOutWarehouseId\" join \"T_Other\" o on o.\"Id\"=w.\"KindId\" join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\"";

            string countSql = "select count( w.\"Id\")  from \"T_OtherInOut\" oio join \"T_InOutWarehouse\" w on w.\"Id\"=oio.\"InOutWarehouseId\" join \"T_Other\" o on o.\"Id\"=w.\"KindId\" join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\"";

            return GetPagedData<OtherInOut, OtherInOutFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<OtherInOut> GetOtherInOut(OtherInOutFilter filter, int rowCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY w.\"CreateTime\") as \"rownum\",w.\"Id\",w.\"KindId\",w.\"Amount\",w.\"Direction\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",w.\"OperationDate\",w.\"CreateTime\",w.\"Remark\",o.\"Name\",od.\"Unit\",w.\"Dispositon\" from \"T_OtherInOut\" oio join \"T_InOutWarehouse\" w on w.\"Id\"=oio.\"InOutWarehouseId\" join \"T_Other\" o on o.\"Id\"=w.\"KindId\" join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\"";
            return GetRuledRowsData<OtherInOut, OtherInOutFilter>(rowCount, querySql, filter);
        }
        public OtherInOut GetOtherInOutDetailById(string id)
        {
            string sql = "select \"row_number\"() over(ORDER BY w.\"CreateTime\") as \"rownum\",w.\"Id\",w.\"KindId\",w.\"Amount\",w.\"Direction\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",w.\"OperationDate\",w.\"CreateTime\",w.\"Remark\",o.\"Name\",od.\"Unit\" from \"T_OtherInOut\" oio join \"T_InOutWarehouse\" w on w.\"Id\"=oio.\"InOutWarehouseId\" join \"T_Other\" o on o.\"Id\"=w.\"KindId\" join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\" join \"T_Employee\" e on e.\"Id\"=w.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=w.\"OperatorId\" where w.\"Id\"=:id and \"Direction\"=:direction";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("direction", (int)InOutWarehouseDirectionEnum.In);

            return GetData<OtherInOut>(sql, pms).FirstOrDefault();
        }

        public List<OtherInventory> GetOtherInventory(OtherInventoryFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\"() over(ORDER BY o.\"Name\") as \"rownum\",oi.\"Id\",oi.\"Amount\",o.\"Name\",od.\"Unit\",od.\"Remark\" from \"T_OtherInventory\" oi join \"T_Other\" o on o.\"Id\"=oi.\"KindId\" join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\"";

            string countSql = "select count(o.\"Id\") from \"T_OtherInventory\" oi join \"T_Other\" o on o.\"Id\"=oi.\"KindId\" join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\"";

            return GetPagedData<OtherInventory, OtherInventoryFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<OtherInventory> GetOtherInventory(OtherInventoryFilter filter, int rowCount)
        {
            string querySql = "SELECT \"row_number\"() over(ORDER BY o.\"Name\") as \"rownum\",oi.\"Id\",oi.\"Amount\",o.\"Name\",od.\"Unit\",od.\"Remark\" from \"T_OtherInventory\" oi join \"T_Other\" o on o.\"Id\"=oi.\"KindId\" join \"T_OtherDetail\" od on od.\"KindId\"=o.\"Id\"";
            return GetRuledRowsData<OtherInventory, OtherInventoryFilter>(rowCount, querySql, filter);
        }

        [Transaction]
        public void UpdateOther(string name, string unit, string remark, string id)
        {
            string sql = "UPDATE \"T_Other\" SET \"Name\"=:name where \"Id\"=:id;UPDATE \"T_OtherDetail\" SET \"Unit\"=:unit,\"Remark\"=:remark where \"KindId\"=:kindId";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("id", id);
            pms.AddWithValue("name", name);

            pms.AddWithValue("unit", unit);
            pms.AddWithValue("remark", remark);
            pms.AddWithValue("kindId", id);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        #endregion

        #endregion
    }
}
