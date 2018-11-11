using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.Formula
{
    /// <summary>
    /// 某种饲料与配方的相关度
    /// </summary>
    public class FeedFormulaRelevancy
    {
        public string Name { get; set; }

        public string Area { get; set; }

        public string Type { get; set; }

        public int Relevancy { get; set; }
    }
}
