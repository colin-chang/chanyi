using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Group
{
    public class MoveSheepfoldFilter : BaseModelWithPrincipalFilter
    {
        public string SheepId { get; set; }

        public string SourceSheepfoldId { get; set; }

        public string DestinationSheepfoldId { get; set; }

        public DateTime? StartOperationDate { get; set; }
        public DateTime? EndOperationDate { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("m.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
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

                if (!string.IsNullOrWhiteSpace(SheepId))
                {
                    list.Add("m.\"SheepId\"=@SheepId");
                    pms.AddWithValue("SheepId", SheepId);
                }

                if (!string.IsNullOrWhiteSpace(SourceSheepfoldId))
                {
                    list.Add("m.\"SourceSheepfoldId\"=@SourceSheepfoldId");
                    pms.AddWithValue("SourceSheepfoldId", SourceSheepfoldId);
                }

                if (!string.IsNullOrWhiteSpace(DestinationSheepfoldId))
                {
                    list.Add("m.\"DestinationSheepfoldId\"=@DestinationSheepfoldId");
                    pms.AddWithValue("DestinationSheepfoldId", DestinationSheepfoldId);
                }

                if (StartOperationDate != null)
                {
                    list.Add("m.\"OperationDate\">=@StartOperationDate");
                    pms.AddWithValue("StartOperationDate", StartOperationDate);
                }
                if (EndOperationDate != null)
                {
                    list.Add("m.\"OperationDate\"<=@EndOperationDate");
                    pms.AddWithValue("EndOperationDate", EndOperationDate);
                }

                return list;
            };
        }
    }
}
