using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model
{
    public class BaseModelWithPrincipal :BaseModel
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public string PrincipalId { get; set; }

        public string PrincipalName { get; set; }
    }
}
