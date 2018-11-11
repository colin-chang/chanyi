using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Input
{
    public class OtherFilter : BaseModelWithPrincipalFilter
    {
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("od.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("o.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }

                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("o.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                return list;
            };
        }
    }
}
