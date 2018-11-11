using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Breeding
{
    public class FirstAssessFilter : AssessFilter
    {
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                pms = Template.CreateDbParameters();
                return base.GetSqlWhereAndPms(out pms);
            };
        }
    }
}
