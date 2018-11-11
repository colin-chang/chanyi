using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Assist
{
    public class SymptomFilter : SicknessFilter
    {

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("s.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (!string.IsNullOrWhiteSpace(OperatorId))
                {
                    list.Add("s.\"OperatorId\"=@OperatorId");
                    pms.AddWithValue("OperatorId", OperatorId);
                }
                if (StartCreateTime != null)
                {
                    list.Add("s.\"CreateTime\">=@StartCreateTime");
                    pms.AddWithValue("StartCreateTime", StartCreateTime);
                }
                if (EndCreateTime != null)
                {
                    list.Add("s.\"CreateTime\"<=@EndCreateTime");
                    pms.AddWithValue("EndCreateTime", EndCreateTime);
                }
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list.Add("s.\"Remark\" like @Remark");
                    pms.AddWithValue("Remark", Remark.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(PrincipalId))
                {
                    list.Add("s.\"PrincipalId\" = @PrincipalId");
                    pms.AddWithValue("PrincipalId", PrincipalId);
                }

                if (!string.IsNullOrWhiteSpace(TypeId))
                {
                    list.Add("s.\"TypeId\"=@TypeId");
                    pms.AddWithValue("TypeId", TypeId);
                }

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    list.Add("s.\"Name\" like @Name");
                    pms.AddWithValue("Name", Name.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(Description))
                {
                    list.Add("s.\"Description\" like @Description");
                    pms.AddWithValue("Description", Description.Wrap("%"));
                }


                return list;
            };
        }
    }
}
