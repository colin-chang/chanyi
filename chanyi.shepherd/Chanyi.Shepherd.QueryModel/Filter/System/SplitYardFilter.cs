using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.Filter.System
{
    public class SplitYardFilter : BaseFilter
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }

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

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    list.Add("s.\"Name\" like @Name");
                    pms.AddWithValue("Name", Name.Wrap("%"));
                }

                if (!string.IsNullOrWhiteSpace(Manager))
                {
                    list.Add("s.\"Manager\" like @Manager");
                    pms.AddWithValue("Manager", Manager.Wrap("%"));
                }

                return list;
            };
        }
    }
}
