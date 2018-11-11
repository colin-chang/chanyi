using System;
using System.Collections.Generic;
using System.Text;

using Spring.Data.Common;

using Chanyi.Shepherd.QueryModel.Model.ReportForms;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region 报表

        public List<MultiplyReport> GetMultiplyReport(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT \"Month\", MAX ( CASE \"Type\" WHEN 'Abortion' THEN \"Total\" ELSE '0' END ) AS \"Abortion\", MAX ( CASE \"Type\" WHEN 'Delivery' THEN \"Total\" ELSE '0' END ) AS \"Delivery\", MAX ( CASE \"Type\" WHEN 'TotalCount' THEN \"Total\" ELSE '0' END ) AS \"TotalCount\", MAX ( CASE \"Type\" WHEN 'LiveMaleCount' THEN \"Total\" ELSE '0' END ) AS \"LiveMaleCount\", MAX ( CASE \"Type\" WHEN 'LiveFemaleCount' THEN \"Total\" ELSE '0' END ) AS \"LiveFemaleCount\", MAX ( CASE \"Type\" WHEN 'NormalWayCount' THEN \"Total\" ELSE '0' END ) AS \"NormalWayCount\" FROM ( SELECT to_char(\"AbortionDate\", 'YYYY-MM') AS \"Month\", COUNT (\"Id\") AS \"Total\", 'Abortion' AS \"Type\" FROM \"T_Abortion\" A WHERE \"AbortionDate\" >= @startDate AND \"AbortionDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"DeliveryDate\", 'YYYY-MM') AS \"Month\", COUNT (\"Id\") AS \"Total\", 'Delivery' AS \"Type\" FROM \"T_Delivery\" A WHERE \"DeliveryDate\" >= @startDate AND \"DeliveryDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"DeliveryDate\", 'YYYY-MM') AS \"Month\", SUM (\"TotalCount\") AS \"Total\", 'TotalCount' AS \"Type\" FROM \"T_Delivery\" A WHERE \"DeliveryDate\" >= @startDate AND \"DeliveryDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"DeliveryDate\", 'YYYY-MM') AS \"Month\", SUM (\"LiveFemaleCount\") AS \"Total\", 'LiveFemaleCount' AS \"Type\" FROM \"T_Delivery\" A WHERE \"DeliveryDate\" >= @startDate AND \"DeliveryDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"DeliveryDate\", 'YYYY-MM') AS \"Month\", SUM (\"LiveMaleCount\") AS \"Total\", 'LiveMaleCount' AS \"Type\" FROM \"T_Delivery\" A WHERE \"DeliveryDate\" >= @startDate AND \"DeliveryDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"DeliveryDate\", 'YYYY-MM') AS \"Month\", SUM ( CASE \"DeliveryWay\" WHEN 0 THEN 1 ELSE 0 END ) AS \"Total\", 'NormalWayCount' AS \"Type\" FROM \"T_Delivery\" A WHERE \"DeliveryDate\" >= @startDate AND \"DeliveryDate\" <= @endDate GROUP BY \"Month\" ) T GROUP BY \"Month\" ORDER BY \"Month\" DESC";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("startDate", startDate);
            pms.AddWithValue("endDate", endDate);

            return GetData<MultiplyReport>(sql, pms);
        }

        public List<SellReport> GetSellReport(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT \"Month\", MAX ( CASE \"Type\" WHEN 'SellFeed' THEN \"Total\" ELSE '0' END ) AS \"SellFeed\", MAX ( CASE \"Type\" WHEN 'SellManure' THEN \"Total\" ELSE '0' END ) AS \"SellManure\", MAX ( CASE \"Type\" WHEN 'SellOther' THEN \"Total\" ELSE '0' END ) AS \"SellOther\", MAX ( CASE \"Type\" WHEN 'SellSheep' THEN \"Total\" ELSE '0' END ) AS \"SellSheep\", MAX ( CASE \"Type\" WHEN 'SellWool' THEN \"Total\" ELSE '0' END ) AS \"SellWool\",max(case  \"Type\" when 'SellFeed' THEN \"Total\"else '0' end)+max(case  \"Type\" when 'SellManure' THEN \"Total\"else '0' end)+max(case  \"Type\" when 'SellOther' THEN \"Total\"else '0' end)+max(case  \"Type\" when 'SellSheep' THEN \"Total\"else '0' end)+max(case  \"Type\" when 'SellWool' THEN \"Total\"else '0' end) as \"Total\" FROM ( SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Price\") AS \"Total\", 'SellFeed' AS \"Type\" FROM \"T_Sell\" s  JOIN \"T_SellFeed\" sf ON sf.\"SellId\" = s.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Price\") AS \"Total\", 'SellManure' AS \"Type\" FROM \"T_Sell\" s  JOIN \"T_SellManure\" sm ON sm.\"SellId\" = s.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Price\") AS \"Total\", 'SellOther' AS \"Type\" FROM \"T_Sell\" s  JOIN \"T_SellOther\" so ON so.\"SellId\" = s.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (ss.\"Price\") AS \"Total\", 'SellSheep' AS \"Type\" FROM \"T_Sell\" s  JOIN \"T_SellSheep\" ss ON ss.\"SellSheepBatchId\" = s.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Price\") AS \"Total\", 'SellWool' AS \"Type\" FROM \"T_Sell\" s  JOIN \"T_SellWool\" sf ON sf.\"SellId\" = s.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" ) T GROUP BY \"Month\" ORDER BY \"Month\" DESC";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("startDate", startDate);
            pms.AddWithValue("endDate", endDate);

            return GetData<SellReport>(sql, pms);
        }

        public List<BuyReport> GetBuyReport(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT \"Month\", MAX ( CASE \"Type\" WHEN 'ElectricCharge' THEN \"Total\" ELSE '0' END ) AS \"ElectricCharge\", MAX ( CASE \"Type\" WHEN 'WaterRate' THEN \"Total\" ELSE '0' END ) AS \"WaterRate\", MAX ( CASE \"Type\" WHEN 'Payoff' THEN \"Total\" ELSE '0' END ) AS \"Payoff\", MAX ( CASE \"Type\" WHEN 'BuyFeed' THEN \"Total\" ELSE '0' END ) AS \"BuyFeed\", MAX ( CASE \"Type\" WHEN 'BuyMedicine' THEN \"Total\" ELSE '0' END ) AS \"BuyMedicine\", MAX ( CASE \"Type\" WHEN 'BuyOther' THEN \"Total\" ELSE '0' END ) AS \"BuyOther\", MAX ( CASE \"Type\" WHEN 'BuySheep' THEN \"Total\" ELSE '0' END ) AS \"BuySheep\", MAX ( CASE \"Type\" WHEN 'ElectricCharge' THEN \"Total\" ELSE '0' END ) + MAX ( CASE \"Type\" WHEN 'WaterRate' THEN \"Total\" ELSE '0' END ) + MAX ( CASE \"Type\" WHEN 'Payoff' THEN \"Total\" ELSE '0' END ) + MAX ( CASE \"Type\" WHEN 'BuyFeed' THEN \"Total\" ELSE '0' END ) + MAX ( CASE \"Type\" WHEN 'BuyMedicine' THEN \"Total\" ELSE '0' END ) + MAX ( CASE \"Type\" WHEN 'BuyOther' THEN \"Total\" ELSE '0' END ) + MAX ( CASE \"Type\" WHEN 'BuySheep' THEN \"Total\" ELSE '0' END ) AS \"Total\" FROM ( SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'ElectricCharge' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_ElectricCharge\" ec ON ec.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'WaterRate' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_WaterRate\" wr ON wr.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'Payoff' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_Payoff\" pf ON pf.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'BuyFeed' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_BuyFeed\" bf ON bf.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'BuyMedicine' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_BuyMedicine\" bm ON bm.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'BuyOther' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_BuyOther\" bo ON bo.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'BuySheep' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_BuySheep\" bs ON bs.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" ) T GROUP BY \"Month\" ORDER BY \"Month\" DESC";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("startDate", startDate);
            pms.AddWithValue("endDate", endDate);

            return GetData<BuyReport>(sql, pms);
        }

        public List<FinanceReport> GetFinanceReport(DateTime startDate, DateTime endDate)
        {
            string sql = "SELECT \"Month\", MAX ( CASE \"Type\" WHEN 'SellFeed' THEN \"Total\" ELSE '0' END ) AS \"SellFeed\", MAX ( CASE \"Type\" WHEN 'SellManure' THEN \"Total\" ELSE '0' END ) AS \"SellManure\", MAX ( CASE \"Type\" WHEN 'SellOther' THEN \"Total\" ELSE '0' END ) AS \"SellOther\", MAX ( CASE \"Type\" WHEN 'SellSheep' THEN \"Total\" ELSE '0' END ) AS \"SellSheep\", MAX ( CASE \"Type\" WHEN 'SellWool' THEN \"Total\" ELSE '0' END ) AS \"SellWool\", MAX ( CASE \"Type\" WHEN 'ElectricCharge' THEN \"Total\" ELSE '0' END ) AS \"ElectricCharge\", MAX ( CASE \"Type\" WHEN 'WaterRate' THEN \"Total\" ELSE '0' END ) AS \"WaterRate\", MAX ( CASE \"Type\" WHEN 'Payoff' THEN \"Total\" ELSE '0' END ) AS \"Payoff\", MAX ( CASE \"Type\" WHEN 'BuyFeed' THEN \"Total\" ELSE '0' END ) AS \"BuyFeed\", MAX ( CASE \"Type\" WHEN 'BuyMedicine' THEN \"Total\" ELSE '0' END ) AS \"BuyMedicine\", MAX ( CASE \"Type\" WHEN 'BuyOther' THEN \"Total\" ELSE '0' END ) AS \"BuyOther\", MAX ( CASE \"Type\" WHEN 'BuySheep' THEN \"Total\" ELSE '0' END ) AS \"BuySheep\", MAX ( CASE \"Type\" WHEN 'SellFeed' THEN \"Total\" ELSE '0' END ) + MAX ( CASE \"Type\" WHEN 'SellManure' THEN \"Total\" ELSE '0' END ) + MAX ( CASE \"Type\" WHEN 'SellOther' THEN \"Total\" ELSE '0' END ) + MAX ( CASE \"Type\" WHEN 'SellSheep' THEN \"Total\" ELSE '0' END ) + MAX ( CASE \"Type\" WHEN 'SellWool' THEN \"Total\" ELSE '0' END ) - MAX ( CASE \"Type\" WHEN 'ElectricCharge' THEN \"Total\" ELSE '0' END ) - MAX ( CASE \"Type\" WHEN 'WaterRate' THEN \"Total\" ELSE '0' END ) - MAX ( CASE \"Type\" WHEN 'Payoff' THEN \"Total\" ELSE '0' END ) - MAX ( CASE \"Type\" WHEN 'BuyFeed' THEN \"Total\" ELSE '0' END ) - MAX ( CASE \"Type\" WHEN 'BuyMedicine' THEN \"Total\" ELSE '0' END ) - MAX ( CASE \"Type\" WHEN 'BuyOther' THEN \"Total\" ELSE '0' END ) - MAX ( CASE \"Type\" WHEN 'BuySheep' THEN \"Total\" ELSE '0' END ) AS \"Total\" FROM ( SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Price\") AS \"Total\", 'SellFeed' AS \"Type\" FROM \"T_Sell\" s JOIN \"T_SellFeed\" sf ON sf.\"SellId\" = s.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Price\") AS \"Total\", 'SellManure' AS \"Type\" FROM \"T_Sell\" s JOIN \"T_SellManure\" sm ON sm.\"SellId\" = s.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Price\") AS \"Total\", 'SellOther' AS \"Type\" FROM \"T_Sell\" s JOIN \"T_SellOther\" so ON so.\"SellId\" = s.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (ss.\"Price\") AS \"Total\", 'SellSheep' AS \"Type\" FROM \"T_Sell\" s JOIN \"T_SellSheep\" ss ON ss.\"SellSheepBatchId\" = s.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Price\") AS \"Total\", 'SellWool' AS \"Type\" FROM \"T_Sell\" s JOIN \"T_SellWool\" sf ON sf.\"SellId\" = s.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'ElectricCharge' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_ElectricCharge\" ec ON ec.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'WaterRate' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_WaterRate\" wr ON wr.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'Payoff' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_Payoff\" pf ON pf.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'BuyFeed' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_BuyFeed\" bf ON bf.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'BuyMedicine' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_BuyMedicine\" bm ON bm.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'BuyOther' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_BuyOther\" bo ON bo.\"ExpenditureId\" = e.\"Id\" WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" UNION SELECT to_char(\"OperationDate\", 'YYYY-MM') AS \"Month\", \"sum\" (\"Money\") AS \"Total\", 'BuySheep' AS \"Type\" FROM \"T_Expenditure\" e JOIN \"T_BuySheep\" bs ON bs.\"ExpenditureId\" = e.\"Id\"  WHERE \"OperationDate\" >= @startDate AND \"OperationDate\" <= @endDate GROUP BY \"Month\" ) T GROUP BY \"Month\" ORDER BY \"Month\" DESC";

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("startDate", startDate);
            pms.AddWithValue("endDate", endDate);

            return GetData<FinanceReport>(sql, pms);
        }

        public List<FeedReport> GetFeedInventoryReport(DateTime startDate, DateTime endDate)
        {
            StringBuilder sbSql = new StringBuilder();

            sbSql.Append("SELECT T .\"Month\", n.\"Name\", d.\"Value\" AS \"Type\", A .\"Name\" AS \"Area\", T .\"Direction\", COALESCE (T .\"Amount\", 0) AS \"Amount\" FROM ( SELECT to_char( w.\"OperationDate\", 'YYYY-MM' ) AS \"Month\", \"KindId\", SUM (\"Amount\") AS \"Amount\", \"Direction\" FROM \"T_FeedInOut\" fio JOIN \"T_InOutWarehouse\" w ON fio.\"InOutWarehouseId\" = w.\"Id\" WHERE w.\"OperationDate\" >=:startDate AND w.\"OperationDate\" <=:endDate GROUP BY \"Month\", \"KindId\", \"Direction\" ) T JOIN \"T_Feed\" f ON f.\"Id\" = T .\"KindId\" JOIN \"T_InputName\" n ON n.\"Id\" = f.\"NameId\" JOIN \"T_InventoryDict\" d ON d.\"Id\" = f.\"TypeId\" JOIN \"T_Area\" A ON A .\"Id\" = f.\"AreaId\" ORDER BY to_date(T .\"Month\", 'YYYY-MM')");

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("startDate", startDate);
            pms.AddWithValue("endDate", endDate);

            return GetData<FeedReport>(sbSql.ToString(), pms);
        }

        public List<FeedSheepReport> GetFeedSheepReport(string sheepId)
        {
            string sql = "SELECT DISTINCT fsow.\"SheepId\", fsow.\"KindId\", n.\"Name\", A .\"Name\" AS \"Area\", T .\"Value\" AS \"Type\", SUM (fsow.\"Amount\") OVER ( PARTITION BY fsow.\"SheepId\", fsow.\"KindId\" ) AS \"TotalAmount\", SUM (fsow.\"Amount\" * fsow.\"Price\") OVER ( PARTITION BY fsow.\"SheepId\", fsow.\"KindId\" ) AS \"TotalPrice\" FROM \"T_FeedSheepOutWarehouse\" fsow JOIN \"T_Feed\" f ON f.\"Id\" = fsow.\"KindId\" JOIN \"T_InputName\" n ON n.\"Id\" = f.\"NameId\" LEFT JOIN \"T_Area\" A ON A .\"Id\" = f.\"AreaId\" JOIN \"T_InventoryDict\" T ON T .\"Id\" = f.\"TypeId\" WHERE fsow.\"SheepId\" = :sheepId AND fsow.\"Price\" <> '0'";


            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sheepId", sheepId);

            return GetData<FeedSheepReport>(sql, pms);
        }

        #endregion
    }
}
