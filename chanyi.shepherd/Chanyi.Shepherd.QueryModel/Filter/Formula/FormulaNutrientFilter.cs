using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;
using Chanyi.Utility.Common;

namespace Chanyi.Shepherd.QueryModel.Filter.Formula
{
    public class FormulaNutrientFilter : BaseModelFilter
    {
        public string Name { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                pms = Template.CreateDbParameters();


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
