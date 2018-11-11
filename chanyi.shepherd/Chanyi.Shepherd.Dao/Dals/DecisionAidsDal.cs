using System.Collections.Generic;
using System.Data;
using System.Linq;

using Spring.Data.Common;
using Spring.Data.Core;

using Chanyi.Shepherd.Dao.Mappers;
using Chanyi.Shepherd.QueryModel.Model.Assist;

namespace Chanyi.Shepherd.Dao
{
    public partial class Dal
    {
        #region DecisionAids

        public List<FamilyTree> GetFamilyTree(string sheepId, int depth)
        {
            //string sql = "select * FROM getfamilytree(:sheepId,:depth);";
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sheepId", sheepId);

            //pms.AddWithValue("depth", depth);
            // return GetData<FamilyTree>(sql, pms);
            //存储过程调用方式
            pms.AddWithValue("len", depth);
            return AdoTemplate.QueryWithRowMapper<FamilyTree>(CommandType.StoredProcedure, "getfamilytree", new BaseRowMapper<FamilyTree>(), pms).ToList();
        }

        public List<AssistMating> GetAssistMating(string sheepId, int depth)
        {
            IDbParameters pms = AdoTemplate.CreateDbParameters();
            pms.AddWithValue("sheepId", sheepId);
            pms.AddWithValue("generation", depth);
            return AdoTemplate.QueryWithRowMapper<AssistMating>(CommandType.StoredProcedure, "AssistMating", new BaseRowMapper<AssistMating>(), pms).ToList();
        }

        //public int VarifyTwoSheepsCanMating(string maleId, string femaleId)
        //{
        //    string sql = "select count(1) FROM getfamilytree(:maleId,5) a where a.id in( select b.id from getfamilytree(:femaleId, 5)b)";
        //    IDbParameters pms = AdoTemplate.CreateDbParameters();
        //    pms.AddWithValue("maleId", maleId);
        //    pms.AddWithValue("femaleId", femaleId);

        //    object obj = AdoTemplate.ExecuteScalar(CommandType.Text, sql, pms);
        //    return obj == null ? 0 : Convert.ToInt32(obj);
        //}

        #endregion
    }
}