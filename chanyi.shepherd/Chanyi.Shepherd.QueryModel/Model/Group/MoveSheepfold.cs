using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Group
{
    public class MoveSheepfold:BaseModelWithPrincipal
    {
        public string SerialNumber { get; set; }

        public string SourceSheepfoldName { get; set; }

        public string DestinationSheepfoldName { get; set; }

        public DateTime OperationDate { get; set; }

    }
}
