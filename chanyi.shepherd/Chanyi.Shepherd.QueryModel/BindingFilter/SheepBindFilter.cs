using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.BindingFilter
{
    public class SheepBindFilter : BaseBindFilter
    {
        public GenderEnum? Gender { get; set; }

        public GrowthStageEnum? GrowthStage { get; set; }

        public SheepStatusEnum? Status { get; set; }

        public string SheepfoldId { get; set; }

        public string SerialNumber { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                //默认状态是正常
                list.Add("s.\"Status\"=@Status");
                pms.AddWithValue("Status", Status == null ? (int)SheepStatusEnum.Nomal : (int)Status);

                if (Gender != null)
                {
                    list.Add("s.\"Gender\"=@Gender");
                    pms.AddWithValue("Gender", (int)Gender);
                }

                if (GrowthStage != null)
                {
                    list.Add("s.\"GrowthStage\"=@GrowthStage");
                    pms.AddWithValue("GrowthStage", (int)GrowthStage);
                }

                if (Status != null)
                {
                    list.Add("s.\"Status\"=@Status");
                    pms.AddWithValue("Status", (int)Status);
                }
                else
                    list.Add("s.\"Status\"!=" + (int)SheepStatusEnum.Outer);


                if (!string.IsNullOrWhiteSpace(SheepfoldId))
                {
                    list.Add("s.\"SheepfoldId\"=@SheepfoldId");
                    pms.AddWithValue("SheepfoldId", SheepfoldId);
                }


                if (!string.IsNullOrWhiteSpace(SerialNumber))
                {
                    list.Add("\"SerialNumber\" like @SerialNumber");
                    //list.Add("\"lower\"(s.\"SerialNumber\")=\"lower\"(@SerialNumber)");
                    pms.AddWithValue("SerialNumber", SerialNumber.Wrap("%"));
                }

                list.Add("1=1 order by s.\"SerialNumber\" asc");

                return list;
            };
        }

        public override string ToSqlWhere(out IDbParameters pms)
        {
            List<string> listWhere = new List<string>();
            listWhere.AddRange(CreateGenerator()(out pms));

            return " where " + string.Join(" and ", listWhere);
        }
    }
}
