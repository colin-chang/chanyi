using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.HR
{
    public  class CooperaterFilter:BaseFilter
    {
        public string Name { get; set; }

        public string Id { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();


                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("c.\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    list.Add("c.\"Name\" like @Name");
                    pms.AddWithValue("Name", Name.Wrap("%"));
                }

                return list;
            };
        }

    }
}
