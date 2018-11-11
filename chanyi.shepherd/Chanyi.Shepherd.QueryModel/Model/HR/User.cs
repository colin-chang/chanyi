using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.HR
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class User : BaseModelWithPrincipal
    {
        public string UserName { get; set; }

        public bool IsEnabled { get; set; }
    }
}
