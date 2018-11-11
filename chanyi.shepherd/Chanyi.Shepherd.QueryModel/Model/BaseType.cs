using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model
{

    public abstract class BaseType:BaseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
