using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Spring.Data.Common;
using Spring.Transaction.Interceptor;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.AddModel.Finance;
using Chanyi.Shepherd.QueryModel.Filter.Finance;
using Chanyi.Shepherd.QueryModel.Model.Finance;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region Finance

        #region 卖
        [Transaction]
        public void AddSellSheep(List<AddSellSheep> list, decimal totalPrice, float totalWeight, string purchaserId, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark, string sysSheepfoldId)
        {
            //sheep
            //sell
            //sellSheepBatch
            //sellSheep

            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();

            //sell
            sb.Append("INSERT into \"T_Sell\" (\"Id\",\"Price\",\"PurchaserId\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:price,:purchaserId,:operationDate,:principalId,:operatorId,:createTime,:remark);");
            string sellId = Guid.NewGuid().ToString();
            pms.AddWithValue("id", sellId);
            pms.AddWithValue("price", totalPrice);
            pms.AddWithValue("purchaserId", purchaserId);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            sb.Append("INSERT into \"T_SellSheepBatch\" (\"SellId\",\"SerialNumber\",\"SellCount\",\"TotalWeight\")VALUES(:sellId,(select current_date||' 第'||(count(\"SellId\")+1)||'批' from \"T_SellSheepBatch\"  where \"SerialNumber\" like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%' ),:sellCount,:totalWeight);");
            pms.AddWithValue("sellId", sellId);
            pms.AddWithValue("sellCount", list.Count);
            pms.AddWithValue("totalWeight", totalWeight);

            sb.Append("UPDATE \"T_Sheep\" SET \"Status\"=:status,\"SheepfoldId\"=:sheepfoldId where \"Id\" in ('" + string.Join("','", list.Select(t => t.SheepId)) + "');");

            pms.AddWithValue("status", (int)SheepStatusEnum.Selled);
            pms.AddWithValue("sheepfoldId", sysSheepfoldId);

            for (int i = 0; i < list.Count; i++)
            {
                sb.AppendFormat("INSERT into \"T_SellSheep\" (\"Id\",\"SellSheepBatchId\",\"SheepId\",\"Weight\",\"Price\")VALUES(:id{0},:sellSheepBatchId{0},:sheepId{0},:weight{0},:price{0});", i);

                AddSellSheep model = list[i];

                pms.AddWithValue("id" + i, Guid.NewGuid().ToString());
                pms.AddWithValue("sellSheepBatchId" + i, sellId);
                pms.AddWithValue("sheepId" + i, model.SheepId);
                pms.AddWithValue("weight" + i, model.Weight);
                pms.AddWithValue("price" + i, model.Price);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }
        public List<SellSheep> GetSellSheepIds(List<string> sheepIds)
        {
            string sql = "select \"SheepId\" from \"T_SellSheep\" where \"SheepId\" in ('" + string.Join("','", sheepIds) + "')";
            return GetData<SellSheep>(sql, null);
        }

        public List<SellSheepBatch> GetSellSheepBath(SellSheepBatchFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER ( ORDER BY ssb.\"SerialNumber\" DESC ) as  \"rownum\", s.\"Id\", ssb.\"SerialNumber\", ssb.\"SellCount\", ssb.\"TotalWeight\", s.\"Price\", s.\"OperationDate\", C .\"Name\" AS \"Purchaser\", C .\"Department\" AS \"Department\", u.\"UserName\" AS \"OperatorName\", e.\"Name\" AS \"PrincipalName\", s.\"CreateTime\", s.\"Remark\" FROM \"T_SellSheepBatch\" ssb JOIN \"T_Sell\" s ON ssb.\"SellId\" = s.\"Id\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_Purchaser\" P ON P .\"CooperaterId\" = s.\"PurchaserId\" JOIN \"T_Cooperater\" C ON C .\"Id\" = P .\"CooperaterId\"";

            string countSql = "SELECT count(s.\"Id\") FROM \"T_SellSheepBatch\" ssb JOIN \"T_Sell\" s ON ssb.\"SellId\" = s.\"Id\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_Purchaser\" P ON P .\"CooperaterId\" = s.\"PurchaserId\" JOIN \"T_Cooperater\" C ON C .\"Id\" = P .\"CooperaterId\"";

            return GetPagedData<SellSheepBatch, SellSheepBatchFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<SellSheepBatch> GetSellSheepBath(SellSheepBatchFilter filter, int rowsCount)
        {
            string querySql = "SELECT \"row_number\" () OVER ( ORDER BY ssb.\"SerialNumber\" DESC ) as  \"rownum\", s.\"Id\", ssb.\"SerialNumber\", ssb.\"SellCount\", ssb.\"TotalWeight\", s.\"Price\", s.\"OperationDate\", C .\"Name\" AS \"Purchaser\", C .\"Department\" AS \"Department\", u.\"UserName\" AS \"OperatorName\", e.\"Name\" AS \"PrincipalName\", s.\"CreateTime\", s.\"Remark\" FROM \"T_SellSheepBatch\" ssb JOIN \"T_Sell\" s ON ssb.\"SellId\" = s.\"Id\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_Purchaser\" P ON P .\"CooperaterId\" = s.\"PurchaserId\" JOIN \"T_Cooperater\" C ON C .\"Id\" = P .\"CooperaterId\"";
            return GetRuledRowsData<SellSheepBatch, SellSheepBatchFilter>(rowsCount, querySql, filter);
        }

        public List<SellSheep> GetSellSheep(SellSheepFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY s.\"CreateTime\" DESC) AS \"rownum\", s.\"Id\", sp.\"SerialNumber\", s.\"Price\", C .\"Name\" AS \"Purchaser\", C .\"Department\", s.\"OperationDate\", sp.\"Gender\", sp.\"GrowthStage\", b.\"Name\" AS \"Breed\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", s.\"CreateTime\", s.\"Remark\", ss.\"Weight\" FROM \"T_SellSheep\" ss JOIN \"T_SellSheepBatch\" ssb ON ssb.\"SellId\" = ss.\"SellSheepBatchId\" JOIN \"T_Sell\" s ON s.\"Id\" = ssb.\"SellId\" JOIN \"T_Sheep\" sp ON sp.\"Id\" = ss.\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = sp.\"BreedId\" JOIN \"T_Purchaser\" P ON P .\"CooperaterId\" = s.\"PurchaserId\" JOIN \"T_Cooperater\" C ON C .\"Id\" = P .\"CooperaterId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\"";

            string countSql = "select count(s.\"Id\") FROM \"T_SellSheep\" ss JOIN \"T_SellSheepBatch\" ssb ON ssb.\"SellId\" = ss.\"SellSheepBatchId\" JOIN \"T_Sell\" s ON s.\"Id\" = ssb.\"SellId\" JOIN \"T_Sheep\" sp ON sp.\"Id\" = ss.\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = sp.\"BreedId\" JOIN \"T_Purchaser\" P ON P .\"CooperaterId\" = s.\"PurchaserId\" JOIN \"T_Cooperater\" C ON C .\"Id\" = P .\"CooperaterId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\"";

            return GetPagedData<SellSheep, SellSheepFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<SellSheep> GetSellSheep(SellSheepFilter filter, int rowsCount)
        {
            string querySql = "SELECT \"row_number\" () OVER (ORDER BY s.\"CreateTime\" DESC) AS \"rownum\", s.\"Id\", sp.\"SerialNumber\", s.\"Price\", C .\"Name\" AS \"Purchaser\", C .\"Department\", s.\"OperationDate\", sp.\"Gender\", sp.\"GrowthStage\", b.\"Name\" AS \"Breed\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", s.\"CreateTime\", s.\"Remark\", ss.\"Weight\" FROM \"T_SellSheep\" ss JOIN \"T_SellSheepBatch\" ssb ON ssb.\"SellId\" = ss.\"SellSheepBatchId\" JOIN \"T_Sell\" s ON s.\"Id\" = ssb.\"SellId\" JOIN \"T_Sheep\" sp ON sp.\"Id\" = ss.\"SheepId\" JOIN \"T_Breed\" b ON b.\"Id\" = sp.\"BreedId\" JOIN \"T_Purchaser\" P ON P .\"CooperaterId\" = s.\"PurchaserId\" JOIN \"T_Cooperater\" C ON C .\"Id\" = P .\"CooperaterId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\"";
            return GetRuledRowsData<SellSheep, SellSheepFilter>(rowsCount, querySql, filter);
        }

        public List<SellSheep> GetSellSheep(string batchId, int pageIndex, int pageSize, out int totalCount)
        {
            return GetSellSheep(new SellSheepFilter() { BatchId = batchId }, pageIndex, pageSize, out totalCount);
        }

        [Transaction]
        public void AddSellManure(string id, decimal price, string purchaserId, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_SellManure\" (\"SellId\")VALUES(:sellId);INSERT into \"T_Sell\" (\"Id\",\"Price\",\"PurchaserId\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:price,:purchaserId,:operationDate,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sellId", id);

            pms.AddWithValue("id", id);
            pms.AddWithValue("price", price);
            pms.AddWithValue("purchaserId", purchaserId);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public List<SellManure> GetSellManure(SellManureFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY s.\"CreateTime\" DESC) as \"rownum\",s.\"Id\",s.\"Price\",c.\"Name\" as \"Purchaser\",s.\"OperationDate\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",s.\"CreateTime\",s.\"Remark\" from \"T_SellManure\" sm join \"T_Sell\" s on s.\"Id\"=sm.\"SellId\" join \"T_Purchaser\" p on p.\"CooperaterId\"=s.\"PurchaserId\" join \"T_Cooperater\" c on c.\"Id\"=p.\"CooperaterId\" join \"T_Employee\" e on e.\"Id\"=s.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=s.\"OperatorId\"";

            string countSql = "select count(s.\"Id\") from \"T_SellManure\" sm join \"T_Sell\" s on s.\"Id\"=sm.\"SellId\" join \"T_Purchaser\" p on p.\"CooperaterId\"=s.\"PurchaserId\" join \"T_Cooperater\" c on c.\"Id\"=p.\"CooperaterId\" join \"T_Employee\" e on e.\"Id\"=s.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=s.\"OperatorId\"";

            return GetPagedData<SellManure, SellManureFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<SellManure> GetSellManure(SellManureFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY s.\"CreateTime\" DESC) as \"rownum\",s.\"Id\",s.\"Price\",c.\"Name\" as \"Purchaser\",s.\"OperationDate\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",s.\"CreateTime\",s.\"Remark\" from \"T_SellManure\" sm join \"T_Sell\" s on s.\"Id\"=sm.\"SellId\" join \"T_Purchaser\" p on p.\"CooperaterId\"=s.\"PurchaserId\" join \"T_Cooperater\" c on c.\"Id\"=p.\"CooperaterId\" join \"T_Employee\" e on e.\"Id\"=s.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=s.\"OperatorId\"";
            return GetRuledRowsData<SellManure, SellManureFilter>(rowsCount, querySql, filter);
        }

        [Transaction]
        public void AddSellWool(string id, float amount, decimal price, string purchaserId, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_SellWool\" (\"SellId\",\"Amount\")VALUES(:sellId,:amount);INSERT into \"T_Sell\" (\"Id\",\"Price\",\"PurchaserId\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:price,:purchaserId,:operationDate,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sellId", id);
            pms.AddWithValue("amount", amount);

            pms.AddWithValue("id", id);
            pms.AddWithValue("price", price);
            pms.AddWithValue("purchaserId", purchaserId);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public List<SellWool> GetSellWool(SellWoolFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY s.\"CreateTime\" DESC) as \"rownum\",s.\"Id\",s.\"Price\",w.\"Amount\",c.\"Name\" as \"Purchaser\",s.\"OperationDate\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",s.\"CreateTime\",s.\"Remark\" from \"T_SellWool\" w join \"T_Sell\" s on s.\"Id\"=w.\"SellId\" join \"T_Purchaser\" p on p.\"CooperaterId\"=s.\"PurchaserId\" join \"T_Cooperater\" c on c.\"Id\"=p.\"CooperaterId\" join \"T_Employee\" e on e.\"Id\"=s.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=s.\"OperatorId\"";

            string countSql = "select count(s.\"Id\") from \"T_SellWool\" w join \"T_Sell\" s on s.\"Id\"=w.\"SellId\" join \"T_Purchaser\" p on p.\"CooperaterId\"=s.\"PurchaserId\" join \"T_Cooperater\" c on c.\"Id\"=p.\"CooperaterId\" join \"T_Employee\" e on e.\"Id\"=s.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=s.\"OperatorId\"";

            return GetPagedData<SellWool, SellWoolFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<SellWool> GetSellWool(SellWoolFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY s.\"CreateTime\" DESC) as \"rownum\",s.\"Id\",s.\"Price\",w.\"Amount\",c.\"Name\" as \"Purchaser\",s.\"OperationDate\",e.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",s.\"CreateTime\",s.\"Remark\" from \"T_SellWool\" w join \"T_Sell\" s on s.\"Id\"=w.\"SellId\" join \"T_Purchaser\" p on p.\"CooperaterId\"=s.\"PurchaserId\" join \"T_Cooperater\" c on c.\"Id\"=p.\"CooperaterId\" join \"T_Employee\" e on e.\"Id\"=s.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=s.\"OperatorId\"";
            return GetRuledRowsData<SellWool, SellWoolFilter>(rowsCount, querySql, filter);
        }

        public List<SellFeed> GetSellFeedIds(List<string> ids)
        {
            string sql = "select \"LinkId\" from \"T_SellFeed\" sf where sf.\"LinkId\" in ('" + string.Join("','", ids) + "')";

            return GetData<SellFeed>(sql, null);
        }
        [Transaction]
        public void AddSellFeed(List<AddSellInput> list, string operatorId, DateTime createTime, string remark)
        {
            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            for (int i = 0; i < list.Count; i++)
            {
                sb.AppendFormat("INSERT into \"T_Sell\" (\"Id\",\"Price\",\"PurchaserId\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id{0},:price{0},:purchaserId{0},:operationDate{0},:principalId{0},:operatorId{0},:createTime{0},:remark{0}); INSERT into \"T_SellFeed\" (\"SellId\",\"LinkId\")VALUES(:sellId{0},:linkId{0});", i);
                string id = Guid.NewGuid().ToString();
                AddSellInput model = list[i];

                pms.AddWithValue("id" + i, id);
                pms.AddWithValue("price" + i, model.Price);
                pms.AddWithValue("purchaserId" + i, model.PurchaserId);
                pms.AddWithValue("operationDate" + i, model.operationDate);
                pms.AddWithValue("principalId" + i, model.PrincipalId);
                pms.AddWithValue("operatorId" + i, operatorId);
                pms.AddWithValue("createTime" + i, createTime);
                pms.AddWithValue("remark" + i, remark);

                pms.AddWithValue("sellId" + i, id);
                pms.AddWithValue("linkId" + i, model.LinkId);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }
        public List<SellFeed> GetSellFeed(SellFeedFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = "SELECT \"row_number\" () OVER (ORDER BY s.\"CreateTime\" DESC) AS \"rownum\", s.\"Id\", n.\"Name\", A .\"Name\" AS \"Area\", d.\"Value\" AS \"Type\",w.\"Amount\", s.\"Price\", C .\"Name\" AS \"Purchaser\", s.\"OperationDate\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", s.\"CreateTime\", s.\"Remark\" FROM \"T_SellFeed\" sf JOIN \"T_Sell\" s ON sf.\"SellId\" = s.\"Id\" JOIN \"T_InOutWarehouse\" w ON w.\"Id\" = sf.\"LinkId\" JOIN \"T_Feed\" f ON f.\"Id\" = w.\"KindId\" JOIN \"T_InputName\" n ON n.\"Id\" = f.\"NameId\" JOIN \"T_Area\" A ON A .\"Id\" = f.\"AreaId\" JOIN \"T_InventoryDict\" d ON d.\"Id\" = f.\"TypeId\"  JOIN \"T_Cooperater\" C ON C .\"Id\" = s.\"PurchaserId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\"";

            string countSql = "select count(s.\"Id\") FROM \"T_SellFeed\" sf JOIN \"T_Sell\" s ON sf.\"SellId\" = s.\"Id\" JOIN \"T_InOutWarehouse\" w ON w.\"Id\" = sf.\"LinkId\" JOIN \"T_Feed\" f ON f.\"Id\" = w.\"KindId\" JOIN \"T_InputName\" n ON n.\"Id\" = f.\"NameId\" JOIN \"T_Area\" A ON A .\"Id\" = f.\"AreaId\" JOIN \"T_InventoryDict\" d ON d.\"Id\" = f.\"TypeId\" JOIN \"T_Cooperater\" C ON C .\"Id\" = s.\"PurchaserId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\"";
            return GetPagedData<SellFeed, SellFeedFilter>(pageIndex, pageSize, out totalCount, countSql, sql, filter);
        }
        public List<SellFeed> GetSellFeed(SellFeedFilter filter, int rowsCount)
        {
            string sql = "SELECT \"row_number\" () OVER (ORDER BY s.\"CreateTime\" DESC) AS \"rownum\", s.\"Id\", n.\"Name\", A .\"Name\" AS \"Area\", d.\"Value\" AS \"Type\",w.\"Amount\" ,s.\"Price\", C .\"Name\" AS \"Purchaser\", s.\"OperationDate\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", s.\"CreateTime\", s.\"Remark\" FROM \"T_SellFeed\" sf JOIN \"T_Sell\" s ON sf.\"SellId\" = s.\"Id\" JOIN \"T_InOutWarehouse\" w ON w.\"Id\" = sf.\"LinkId\" JOIN \"T_Feed\" f ON f.\"Id\" = w.\"KindId\" JOIN \"T_InputName\" n ON n.\"Id\" = f.\"NameId\" JOIN \"T_Area\" A ON A .\"Id\" = f.\"AreaId\" JOIN \"T_InventoryDict\" d ON d.\"Id\" = f.\"TypeId\"  JOIN \"T_Cooperater\" C ON C .\"Id\" = s.\"PurchaserId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\"";

            return GetRuledRowsData<SellFeed, SellFeedFilter>(rowsCount, sql, filter);
        }

        public List<SellOther> GetSellOtherIds(List<string> ids)
        {
            string sql = "select \"LinkId\" from \"T_SellOther\" sf where sf.\"LinkId\" in ('" + string.Join("','", ids) + "')";

            return GetData<SellOther>(sql, null);
        }

        [Transaction]
        public void AddSellOther(List<AddSellInput> list, string operatorId, DateTime createTime, string remark)
        {
            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            for (int i = 0; i < list.Count; i++)
            {
                sb.AppendFormat("INSERT into \"T_Sell\" (\"Id\",\"Price\",\"PurchaserId\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id{0},:price{0},:purchaserId{0},:operationDate{0},:principalId{0},:operatorId{0},:createTime{0},:remark{0}); INSERT into \"T_SellOther\" (\"SellId\",\"LinkId\")VALUES(:sellId{0},:linkId{0});", i);
                string id = Guid.NewGuid().ToString();
                AddSellInput model = list[i];

                pms.AddWithValue("id" + i, id);
                pms.AddWithValue("price" + i, model.Price);
                pms.AddWithValue("purchaserId" + i, model.PurchaserId);
                pms.AddWithValue("operationDate" + i, model.operationDate);
                pms.AddWithValue("principalId" + i, model.PrincipalId);
                pms.AddWithValue("operatorId" + i, operatorId);
                pms.AddWithValue("createTime" + i, createTime);
                pms.AddWithValue("remark" + i, remark);

                pms.AddWithValue("sellId" + i, id);
                pms.AddWithValue("linkId" + i, model.LinkId);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }
        public List<SellOther> GetSellOther(SellOtherFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = "SELECT \"row_number\" () OVER (ORDER BY s.\"CreateTime\" DESC) AS \"rownum\", s.\"Id\", o.\"Name\", w.\"Amount\", s.\"Price\", C .\"Name\" AS \"Purchaser\", s.\"OperationDate\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", s.\"CreateTime\", s.\"Remark\" FROM \"T_SellOther\" so JOIN \"T_Sell\" s ON s.\"Id\" = so.\"SellId\" JOIN \"T_InOutWarehouse\" w ON w.\"Id\" = so.\"LinkId\" JOIN \"T_Other\" o ON o.\"Id\" = w.\"KindId\" JOIN \"T_Cooperater\" C ON C .\"Id\" = s.\"PurchaserId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\"";

            string countSql = "select count(s.\"Id\") FROM \"T_SellOther\" so JOIN \"T_Sell\" s ON s.\"Id\" = so.\"SellId\" JOIN \"T_InOutWarehouse\" w ON w.\"Id\" = so.\"LinkId\" JOIN \"T_Other\" o ON o.\"Id\" = w.\"KindId\" JOIN \"T_Cooperater\" C ON C .\"Id\" = s.\"PurchaserId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\"";
            return GetPagedData<SellOther, SellOtherFilter>(pageIndex, pageSize, out totalCount, countSql, sql, filter);
        }
        public List<SellOther> GetSellOther(SellOtherFilter filter, int rowsCount)
        {
            string sql = "SELECT \"row_number\" () OVER (ORDER BY s.\"CreateTime\" DESC) AS \"rownum\", s.\"Id\", o.\"Name\", w.\"Amount\", s.\"Price\", C .\"Name\" AS \"Purchaser\", s.\"OperationDate\", e.\"Name\" AS \"PrincipalName\", u.\"UserName\" AS \"OperatorName\", s.\"CreateTime\", s.\"Remark\" FROM \"T_SellOther\" so JOIN \"T_Sell\" s ON s.\"Id\" = so.\"SellId\" JOIN \"T_InOutWarehouse\" w ON w.\"Id\" = so.\"LinkId\" JOIN \"T_Other\" o ON o.\"Id\" = w.\"KindId\" JOIN \"T_Cooperater\" C ON C .\"Id\" = s.\"PurchaserId\" JOIN \"T_Employee\" e ON e.\"Id\" = s.\"PrincipalId\" JOIN \"T_User\" u ON u.\"Id\" = s.\"OperatorId\"";

            return GetRuledRowsData<SellOther, SellOtherFilter>(rowsCount, sql, filter);
        }

        #endregion

        #region 买

        [Transaction]
        public void AddElectricCharge(string id, float amount, decimal money, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_ElectricCharge\" (\"ExpenditureId\",\"Amount\")VALUES(:expenditureId,:amount);INSERT into \"T_Expenditure\" (\"Id\",\"Money\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:money,:operationDate,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("expenditureId", id);
            pms.AddWithValue("amount", amount);

            pms.AddWithValue("id", id);
            pms.AddWithValue("money", money);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public List<ElectricCharge> GetElectricCharge(ElectricChargeFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",ec.\"Amount\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_ElectricCharge\" ec join \"T_Expenditure\" e on e.\"Id\"=ec.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            string countSql = "select count(e.\"Id\") from \"T_ElectricCharge\" ec join \"T_Expenditure\" e on e.\"Id\"=ec.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            return GetPagedData<ElectricCharge, ElectricChargeFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<ElectricCharge> GetElectricCharge(ElectricChargeFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",ec.\"Amount\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_ElectricCharge\" ec join \"T_Expenditure\" e on e.\"Id\"=ec.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";
            return GetRuledRowsData<ElectricCharge, ElectricChargeFilter>(rowsCount, querySql, filter);
        }

        [Transaction]
        public void AddWaterRate(string id, float amount, decimal money, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_WaterRate\" (\"ExpenditureId\",\"Amount\")VALUES(:expenditureId,:amount);INSERT into \"T_Expenditure\" (\"Id\",\"Money\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:money,:operationDate,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("expenditureId", id);
            pms.AddWithValue("amount", amount);

            pms.AddWithValue("id", id);
            pms.AddWithValue("money", money);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public List<WaterRate> GetWaterRate(WaterRateFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",w.\"Amount\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_WaterRate\" w join \"T_Expenditure\" e on e.\"Id\"=w.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            string countSql = "select count(e.\"Id\") from \"T_WaterRate\" w join \"T_Expenditure\" e on e.\"Id\"=w.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            return GetPagedData<WaterRate, WaterRateFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<WaterRate> GetWaterRate(WaterRateFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",w.\"Amount\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_WaterRate\" w join \"T_Expenditure\" e on e.\"Id\"=w.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";
            return GetRuledRowsData<WaterRate, WaterRateFilter>(rowsCount, querySql, filter);
        }

        [Transaction]
        public void AddPayoff(string id, string employeeId, decimal money, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Payoff\" (\"ExpenditureId\",\"EmployeeId\")VALUES(:expenditureId,:employeeId);INSERT into \"T_Expenditure\" (\"Id\",\"Money\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:money,:operationDate,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("expenditureId", id);
            pms.AddWithValue("employeeId", employeeId);

            pms.AddWithValue("id", id);
            pms.AddWithValue("money", money);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public List<Payoff> GetPayoff(PayoffFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",p.\"EmployeeId\",emp.\"Name\" as \"EmployeeName\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_Payoff\" p join \"T_Employee\" emp on emp.\"Id\"=p.\"EmployeeId\" join \"T_Expenditure\" e on e.\"Id\"=p.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            string countSql = "select count(e.\"Id\") from \"T_Payoff\" p join \"T_Employee\" emp on emp.\"Id\"=p.\"EmployeeId\" join \"T_Expenditure\" e on e.\"Id\"=p.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            return GetPagedData<Payoff, PayoffFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<Payoff> GetPayoff(PayoffFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",p.\"EmployeeId\",emp.\"Name\" as \"EmployeeName\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_Payoff\" p join \"T_Employee\" emp on emp.\"Id\"=p.\"EmployeeId\" join \"T_Expenditure\" e on e.\"Id\"=p.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";
            return GetRuledRowsData<Payoff, PayoffFilter>(rowsCount, querySql, filter);
        }

        [Transaction]
        public void AddIncidentals(string id, decimal money, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            string sql = "INSERT into \"T_Incidentals\" (\"ExpenditureId\")VALUES(:expenditureId);INSERT into \"T_Expenditure\" (\"Id\",\"Money\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:money,:operationDate,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("expenditureId", id);

            pms.AddWithValue("id", id);
            pms.AddWithValue("money", money);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);

        }

        public List<Incidentals> GetIncidentals(IncidentalsFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_Incidentals\" i join \"T_Expenditure\" e on e.\"Id\"=i.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            string countSql = "select count(e.\"Id\") from \"T_Incidentals\" i join \"T_Expenditure\" e on e.\"Id\"=i.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            return GetPagedData<Incidentals, IncidentalsFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<Incidentals> GetIncidentals(IncidentalsFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_Incidentals\" i join \"T_Expenditure\" e on e.\"Id\"=i.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";
            return GetRuledRowsData<Incidentals, IncidentalsFilter>(rowsCount, querySql, filter);
        }

        [Transaction]
        public void AddBuySheep(string id, string sheepId, string source, decimal money, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark, float? weight)
        {
            string sql = "INSERT into \"T_BuySheep\" (\"ExpenditureId\",\"SheepId\",\"Source\",\"Weight\")VALUES(:expenditureId,:sheepId,:source,:weight);INSERT into \"T_Expenditure\" (\"Id\",\"Money\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id,:money,:operationDate,:principalId,:operatorId,:createTime,:remark)";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("expenditureId", id);
            pms.AddWithValue("sheepId", sheepId);
            pms.AddWithValue("source", source);
            pms.AddWithValue("weight", weight);

            pms.AddWithValue("id", id);
            pms.AddWithValue("money", money);
            pms.AddWithValue("operationDate", operationDate);
            pms.AddWithValue("principalId", principalId);
            pms.AddWithValue("operatorId", operatorId);
            pms.AddWithValue("createTime", createTime);
            pms.AddWithValue("remark", remark);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, pms);
        }

        public List<BuySheep> GetBuySheep(BuySheepFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",s.\"SerialNumber\",b.\"Source\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\",b.\"Weight\" from \"T_BuySheep\" b join \"T_Sheep\" s on s.\"Id\"=b.\"SheepId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            string countSql = "select count(e.\"Id\") from \"T_BuySheep\" b join \"T_Sheep\" s on s.\"Id\"=b.\"SheepId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            return GetPagedData<BuySheep, BuySheepFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<BuySheep> GetBuySheep(BuySheepFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",s.\"SerialNumber\",b.\"Source\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\",b.\"Weight\" from \"T_BuySheep\" b join \"T_Sheep\" s on s.\"Id\"=b.\"SheepId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";
            return GetRuledRowsData<BuySheep, BuySheepFilter>(rowsCount, querySql, filter);
        }

        [Transaction]
        public void AddBuyFeed(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            StringBuilder sb = new StringBuilder();
            List<string> keys = inputExpenditure.Keys.ToList();
            IDbParameters pms = AdoTemplate.CreateDbParameters();

            for (int i = 0; i < keys.Count; i++)
            {
                sb.AppendFormat("INSERT into \"T_BuyFeed\" (\"ExpenditureId\",\"LinkId\")VALUES(:expenditureId{0},:linkId{0});INSERT into \"T_Expenditure\" (\"Id\",\"Money\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id{0},:money{0},:operationDate{0},:principalId{0},:operatorId{0},:createTime{0},:remark{0});", i);

                string id = Guid.NewGuid().ToString();

                pms.AddWithValue("expenditureId" + i, id);
                pms.AddWithValue("linkId" + i, keys[i]);

                pms.AddWithValue("id" + i, id);
                pms.AddWithValue("money" + i, inputExpenditure[keys[i]]);
                pms.AddWithValue("operationDate" + i, operationDate);
                pms.AddWithValue("principalId" + i, principalId);
                pms.AddWithValue("operatorId" + i, operatorId);
                pms.AddWithValue("createTime" + i, createTime);
                pms.AddWithValue("remark" + i, remark);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);

        }
        public List<BuyFeed> GetBuyFeedIds(List<string> linkIds)
        {
            string sql = "select \"LinkId\" from \"T_BuyFeed\" where \"LinkId\" in ('" + string.Join("','", linkIds) + "')";
            return GetData<BuyFeed>(sql, null);
        }

        public List<BuyFeed> GetBuyFeed(BuyFeedFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",w.\"Id\" as \"LinkId\",n.\"Name\",d.\"Value\" as \"Type\",a.\"Name\" as \"Area\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_BuyFeed\" b join \"T_InOutWarehouse\" w on w.\"Id\"=b.\"LinkId\" join \"T_Feed\" f on f.\"Id\"=w.\"KindId\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            string countSql = "select count(e.\"Id\")  from \"T_BuyFeed\" b join \"T_InOutWarehouse\" w on w.\"Id\"=b.\"LinkId\" join \"T_Feed\" f on f.\"Id\"=w.\"KindId\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            return GetPagedData<BuyFeed, BuyFeedFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }
        public List<BuyFeed> GetBuyFeed(BuyFeedFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",w.\"Id\" as \"LinkId\",n.\"Name\",d.\"Value\" as \"Type\",a.\"Name\" as \"Area\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_BuyFeed\" b join \"T_InOutWarehouse\" w on w.\"Id\"=b.\"LinkId\" join \"T_Feed\" f on f.\"Id\"=w.\"KindId\" join \"T_InputName\" n on n.\"Id\"=f.\"NameId\" join \"T_InventoryDict\" d on d.\"Id\"=f.\"TypeId\" left join \"T_Area\" a on a.\"Id\"=f.\"AreaId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";
            return GetRuledRowsData<BuyFeed, BuyFeedFilter>(rowsCount, querySql, filter);
        }

        [Transaction]
        public void AddBuyMedicine(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            List<string> keys = inputExpenditure.Keys.ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                sb.AppendFormat("INSERT into \"T_BuyMedicine\" (\"ExpenditureId\",\"LinkId\")VALUES(:expenditureId{0},:linkId{0});INSERT into \"T_Expenditure\" (\"Id\",\"Money\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id{0},:money{0},:operationDate{0},:principalId{0},:operatorId{0},:createTime{0},:remark{0});", i);

                string id = Guid.NewGuid().ToString();

                pms.AddWithValue("expenditureId" + i, id);
                pms.AddWithValue("linkId" + i, keys[i]);

                pms.AddWithValue("id" + i, id);
                pms.AddWithValue("money" + i, inputExpenditure[keys[i]]);
                pms.AddWithValue("operationDate" + i, operationDate);
                pms.AddWithValue("principalId" + i, principalId);
                pms.AddWithValue("operatorId" + i, operatorId);
                pms.AddWithValue("createTime" + i, createTime);
                pms.AddWithValue("remark" + i, remark);
            }

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);
        }
        public List<BuyMedicine> GetBuyMedicineIds(List<string> linkIds)
        {
            string sql = "select \"LinkId\" from \"T_BuyMedicine\" where \"LinkId\" in ('" + string.Join("','", linkIds) + "')";
            return GetData<BuyMedicine>(sql, null);
        }

        public List<BuyMedicine> GetBuyMedicine(BuyMedicineFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",w.\"Id\" as \"LinkId\",n.\"Name\",c.\"Name\" as \"Manufacturer\",c.\"Department\",d.\"Value\" as \"Type\",md.\"Unit\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_BuyMedicine\" b join \"T_InOutWarehouse\" w on w.\"Id\"=b.\"LinkId\" join \"T_MedicineCrucial\" mc on mc.\"Id\"=w.\"KindId\" join \"T_Medicine\" m on m.\"Id\"=mc.\"KindId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Cooperater\" c on c.\"Id\"=m.\"ManufacturerId\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            string countSql = "select count(e.\"Id\") from \"T_BuyMedicine\" b join \"T_InOutWarehouse\" w on w.\"Id\"=b.\"LinkId\" join \"T_Medicine\" m on m.\"Id\"=w.\"KindId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Cooperater\" c on c.\"Id\"=m.\"ManufacturerId\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            return GetPagedData<BuyMedicine, BuyMedicineFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<BuyMedicine> GetBuyMedicine(BuyMedicineFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",w.\"Id\" as \"LinkId\",n.\"Name\",c.\"Name\" as \"Manufacturer\",c.\"Department\",d.\"Value\" as \"Type\",md.\"Unit\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_BuyMedicine\" b join \"T_InOutWarehouse\" w on w.\"Id\"=b.\"LinkId\" join \"T_MedicineCrucial\" mc on mc.\"Id\"=w.\"KindId\" join \"T_Medicine\" m on m.\"Id\"=mc.\"KindId\" join \"T_InputName\" n on n.\"Id\"=m.\"NameId\" join \"T_Cooperater\" c on c.\"Id\"=m.\"ManufacturerId\" JOIN \"T_MedicineDetail\" md ON md.\"KindId\" = M .\"Id\" join \"T_InventoryDict\" d on d.\"Id\"=m.\"TypeId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";
            return GetRuledRowsData<BuyMedicine, BuyMedicineFilter>(rowsCount, querySql, filter);
        }

        [Transaction]
        public void AddBuyOther(Dictionary<string, decimal> inputExpenditure, DateTime operationDate, string principalId, string operatorId, DateTime createTime, string remark)
        {
            StringBuilder sb = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            List<string> keys = inputExpenditure.Keys.ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                sb.AppendFormat("INSERT into \"T_BuyOther\" (\"ExpenditureId\",\"LinkId\")VALUES(:expenditureId{0},:linkId{0});INSERT into \"T_Expenditure\" (\"Id\",\"Money\",\"OperationDate\",\"PrincipalId\",\"OperatorId\",\"CreateTime\",\"Remark\")VALUES(:id{0},:money{0},:operationDate{0},:principalId{0},:operatorId{0},:createTime{0},:remark{0});", i);

                string id = Guid.NewGuid().ToString();

                pms.AddWithValue("expenditureId" + i, id);
                pms.AddWithValue("linkId" + i, keys[i]);

                pms.AddWithValue("id" + i, id);
                pms.AddWithValue("money" + i, inputExpenditure[keys[i]]);
                pms.AddWithValue("operationDate" + i, operationDate);
                pms.AddWithValue("principalId" + i, principalId);
                pms.AddWithValue("operatorId" + i, operatorId);
                pms.AddWithValue("createTime" + i, createTime);
                pms.AddWithValue("remark" + i, remark);
            }
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sb.ToString(), pms);

        }
        public List<BuyOther> GetBuyOtherIds(List<string> linkIds)
        {
            string sql = "select \"LinkId\" from \"T_BuyOther\" where \"LinkId\" in ('" + string.Join("','", linkIds) + "')";
            return GetData<BuyOther>(sql, null);
        }

        public List<BuyOther> GetBuyOther(BuyOtherFilter filter, int pageIndex, int pageSize, out int totalCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",w.\"Id\" as \"LinkId\",o.\"Name\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_BuyOther\" b join \"T_InOutWarehouse\" w on w.\"Id\"=b.\"LinkId\" join \"T_Other\" o on o.\"Id\"=w.\"KindId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            string countSql = "select count(e.\"Id\") from \"T_BuyOther\" b join \"T_InOutWarehouse\" w on w.\"Id\"=b.\"LinkId\" join \"T_Other\" o on o.\"Id\"=w.\"KindId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            return GetPagedData<BuyOther, BuyOtherFilter>(pageIndex, pageSize, out totalCount, countSql, querySql, filter);
        }

        public List<BuyOther> GetBuyOther(BuyOtherFilter filter, int rowsCount)
        {
            string querySql = "select \"row_number\"() over(ORDER BY e.\"CreateTime\" desc) as \"rownum\",e.\"Id\",w.\"Id\" as \"LinkId\",o.\"Name\",e.\"Money\",e.\"OperationDate\",ee.\"Name\" as \"PrincipalName\",u.\"UserName\" as \"OperatorName\",e.\"CreateTime\",e.\"Remark\" from \"T_BuyOther\" b join \"T_InOutWarehouse\" w on w.\"Id\"=b.\"LinkId\" join \"T_Other\" o on o.\"Id\"=w.\"KindId\" join \"T_Expenditure\" e on e.\"Id\"=b.\"ExpenditureId\" join \"T_Employee\" ee on ee.\"Id\"=e.\"PrincipalId\" join \"T_User\" u on u.\"Id\"=e.\"OperatorId\"";

            return GetRuledRowsData<BuyOther, BuyOtherFilter>(rowsCount, querySql, filter);
        }

        public List<FeedPrice> GetCurrentFeedPrices(IEnumerable<string> listFeedIds)
        {
            string sql = string.Format("select tk.\"KindId\",COALESCE (\"Price\",'0') as \"Price\" from (select t.\"KindId\",\"Price\" from(select \"row_number\"() over(PARTITION by w.\"KindId\" ORDER BY w.\"OperationDate\" DESC,w.\"CreateTime\" desc) rownum,w.\"KindId\",e.\"Money\"/w.\"Amount\" as \"Price\" from \"T_BuyFeed\" bf join \"T_Expenditure\" e on bf.\"ExpenditureId\"=e.\"Id\" join \"T_FeedInOut\" fio on fio.\"InOutWarehouseId\"=bf.\"LinkId\" JOIN \"T_InOutWarehouse\" w on w.\"Id\"=fio.\"InOutWarehouseId\" where w.\"KindId\" in ('{0}'))t where t.rownum=1)tp right join ({1})tk on tk.\"KindId\"=tp.\"KindId\"",
                string.Join("','", listFeedIds),
                string.Join(" union ", listFeedIds.Select(t => string.Format("select '{0}'||'' as \"KindId\" ", t))));//'{0}'||'',最后的||''拼接上一空字符串，强制进行类型转化，明确类型为字符串类型

            return GetData<FeedPrice>(sql, null);
        }

        #endregion

        #endregion
    }
}
