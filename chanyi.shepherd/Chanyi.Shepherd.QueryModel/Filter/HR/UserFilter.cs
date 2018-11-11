using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.HR
{
    public class UserFilter : BaseModelWithPrincipalFilter
    {
        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                pms = Template.CreateDbParameters();
                return new List<string>();
            };

        }
    }
}
