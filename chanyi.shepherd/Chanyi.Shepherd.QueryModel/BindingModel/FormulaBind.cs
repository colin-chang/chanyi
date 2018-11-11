using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.BindingModel
{
    public class FormulaBind
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ApplyTo { get; set; }
        /// <summary>
        /// 不良症状
        /// </summary>
        public string SideEffect { get; set; }
    }
}
