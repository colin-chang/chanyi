using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model
{
    public abstract class BaseModel
    {
        public string Id { get; set; }
        
        public string OperatorId { get; set; }

        public string OperatorName { get; set; }

        public DateTime? CreateTime { get; set; }

        public string Remark { get; set; }
    }
}
