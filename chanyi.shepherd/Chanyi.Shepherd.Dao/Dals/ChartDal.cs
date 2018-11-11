using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.Model.Chart;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region Chart

        public List<PeriodsSheepGrowthStageCount> GetPeriodsSheepGrowthStageCount(DateTime date, string breedId, GenderEnum? gender = null)
        {
            StringBuilder sbSql = new StringBuilder();
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            sbSql.Append("SELECT t.\"GrowthStage\",COALESCE(sp.count,0) as \"Count\" FROM(	select s.\"GrowthStage\",\"count\"(s.\"Id\")	from \"T_Sheep\" s ");

            //取日期到次日零点
            date = date.AddDays(1);

            sbSql.Append("where s.\"Status\"!="+(int)SheepStatusEnum.Outer+ " and s.\"Id\" not IN ( SELECT d.\"SheepId\" FROM \"T_DeathManage\" d WHERE d.\"DeathDate\" <:deathDate UNION SELECT sellSheep.\"SheepId\" FROM \"T_SellSheep\" sellSheep JOIN \"T_SellSheepBatch\" ssb ON sellSheep.\"SellSheepBatchId\" = ssb.\"SellId\" JOIN \"T_Sell\" sell ON sell.\"Id\" = ssb.\"SellId\" WHERE sell.\"OperationDate\" <:sellDate UNION  SELECT \"Id\" AS \"SheepId\" FROM \"T_Sheep\" WHERE \"Status\"!=" + (int)SheepStatusEnum.Outer + " and \"CreateTime\" >:createDate )");
            pms.AddWithValue("deathDate", date);
            pms.AddWithValue("sellDate", date);
            pms.AddWithValue("createDate", date);

            if (gender != null)
            {
                sbSql.Append("AND s.\"Gender\"=:gender ");
                pms.AddWithValue("gender", (int)gender);
            }

            if (!string.IsNullOrWhiteSpace(breedId))
            {
                sbSql.Append("and \"BreedId\"=@breedId ");
                pms.AddWithValue("breedId", breedId);
            }


            sbSql.Append("GROUP BY s.\"GrowthStage\" )sp RIGHT join (select 0 as \"GrowthStage\" UNION	select 1 UNION 	select 2 UNION	select 3 union select 4)t on t.\"GrowthStage\"=sp.\"GrowthStage\" ORDER BY t.\"GrowthStage\"");

            return GetData<PeriodsSheepGrowthStageCount>(sbSql.ToString(), pms);
        }

        public List<PeriodsSellSheepCount> GetPeriodsSellSheepCount(DateTime dtStart, DateTime dtEnd)
        {
            StringBuilder sbSql = new StringBuilder();

            sbSql.Append("SELECT tm.\"Month\", COALESCE (ts.\"Count\", 0) AS \"Count\" FROM ( SELECT to_char( s.\"OperationDate\", 'YYYY-MM' ) AS \"Month\", SUM (ssb.\"SellCount\") AS \"Count\" FROM \"T_SellSheepBatch\" ssb JOIN \"T_Sell\" s ON ssb.\"SellId\" = s.\"Id\" WHERE s.\"OperationDate\" < :sellEndDate AND s.\"OperationDate\" > :sellStartDate GROUP BY \"Month\" ) ts RIGHT JOIN (");

            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sellStartDate", dtStart);
            pms.AddWithValue("sellEndDate", dtEnd);

            //拼接SELECT '2015-9' AS "Month" UNION， 保证没有出售羊只的月份数量为0
            DateTime dtTmp = dtStart;
            List<string> list = new List<string>();
            do
            {
                list.Add(dtTmp.ToString("yyyy-MM"));
                dtTmp = dtTmp.AddMonths(1);

            } while (!dtTmp.ToString("yyyy-MM").Equals(dtEnd.ToString("yyyy-MM")) || dtTmp.Month < dtEnd.Month);

            list.Add(dtEnd.ToString("yyyy-MM"));

            sbSql.Append(string.Join(" union ", list.Select(t => string.Format("select '{0}' as \"Month\"", t))));


            sbSql.Append(") tm ON tm.\"Month\" = ts.\"Month\" ORDER BY to_date(tm.\"Month\",'yyyy-MM')");

            return GetData<PeriodsSellSheepCount>(sbSql.ToString(), pms);
        }

        #endregion
    }
}
