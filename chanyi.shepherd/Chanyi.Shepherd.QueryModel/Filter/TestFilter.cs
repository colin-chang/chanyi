using Spring.Data.Common;
using Spring.Data.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter
{
    public class TestFilter : BaseFilter
    {
        public string Id { get; set; }

        public string Name { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    list.Add("\"Id\"=@Id");
                    pms.AddWithValue("Id", Id);
                }
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    list.Add("\"Name\" like @Name");
                    pms.AddWithValue("Name", Name.Wrap("%"));
                }

                return list;
            };
        }
    }
}
