using Spring.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.BindingFilter
{
    public class SymptomBindFilter : SicknessBindFilter
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


                return list;
            };
        }
    }
}
