using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;
using Chanyi.Shepherd.QueryModel.Filter;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Multiplying
{
    /// <summary>
    /// 配种
    /// </summary>
    public class MatingFilter : BaseModelWithPrincipalFilter
    {
        /// <summary>
        /// 母羊Id
        /// </summary>
        public string FemaleId { get; set; }

        /// <summary>
        /// 母羊编号
        /// </summary>
        //public string FemaleSerialNumber { get; set; }

        /// <summary>
        /// 公羊Id
        /// </summary>
        public string MaleId { get; set; }

        /// <summary>
        /// 公羊编号
        /// </summary>
        //public string MaleSerialNumber { get; set; }

        /// <summary>
        /// 配种日期
        /// </summary>
        public DateTime? StartMatingDate { get; set; }
        public DateTime? EndMatingDate { get; set; }

        /// <summary>
        /// 是否进行预产期提醒
        /// </summary>
        public bool? IsRemindful { get; set; }

        public bool IsDel { get { return false; } }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                #region 基本信息
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("m.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("m.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }
                //if (!string.IsNullOrWhiteSpace(OperatorName))
                //{
                //    list.Add("u.\"UserName\" like @OperatorName");
                //    pms.AddWithValue("OperatorName", OperatorName.Wrap("%"));
                //}
                if (StartCreateTime != null)
                {
                    list.Add("m.\"CreateTime\">=@StartCreateTime");
                    pms.AddWithValue("StartCreateTime", StartCreateTime);
                }
                if (EndCreateTime != null)
                {
                    list.Add("m.\"CreateTime\"<=@EndCreateTime");
                    pms.AddWithValue("EndCreateTime", EndCreateTime);
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("m.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(PrincipalId))
                {
                    list.Add("m.\"PrincipalId\" = @PrincipalId");
                    pms.AddWithValue("PrincipalId", PrincipalId);
                }

                //if (!string.IsNullOrWhiteSpace(PrincipalName))
                //{
                //    list.Add("e.\"Name\" like @PrincipalName");
                //    pms.AddWithValue("PrincipalName", PrincipalName.Wrap("%"));
                //}
                #endregion

                if (!string.IsNullOrWhiteSpace(FemaleId))
                {
                    list.Add("m.\"FemaleId\" = @FemaleId");
                    pms.AddWithValue("FemaleId", FemaleId);
                }

                //if (!string.IsNullOrWhiteSpace(FemaleSerialNumber))
                //{
                //    list.Add("sf.\"SerialNumber\" like @FemaleSerialNumber");
                //    pms.AddWithValue("FemaleSerialNumber", FemaleSerialNumber.Wrap("%"));
                //}
                if (!string.IsNullOrWhiteSpace(MaleId))
                {
                    list.Add("m.\"MaleId\" = @MaleId");
                    pms.AddWithValue("MaleId", MaleId);
                }

                //if (!string.IsNullOrWhiteSpace(MaleSerialNumber))
                //{
                //    list.Add("sm.\"SerialNumber\" like @MaleSerialNumber");
                //    pms.AddWithValue("MaleSerialNumber", MaleSerialNumber.Wrap("%"));
                //}

                if (StartMatingDate != null)
                {
                    list.Add("m.\"MatingDate\">=@StartMatingDate");
                    pms.AddWithValue("StartMatingDate", StartMatingDate);
                }
                if (EndMatingDate != null)
                {
                    list.Add("m.\"MatingDate\"<=@EndMatingDate");
                    pms.AddWithValue("EndMatingDate", EndMatingDate);
                }
                if (IsRemindful != null)
                {
                    list.Add("m.\"IsRemindful\" = @IsRemindful");
                    pms.AddWithValue("IsRemindful", IsRemindful);
                }

                list.Add("\"IsDel\"=@IsDel");
                pms.AddWithValue("IsDel", IsDel);

                return list;
            };
        }
    }
}
