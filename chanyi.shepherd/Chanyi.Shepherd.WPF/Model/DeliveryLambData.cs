using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chanyi.Shepherd.QueryModel.AddModel.BaseInfo;

namespace Chanyi.Shepherd.WPF.Model
{
    /// <summary>
    /// 分娩时的羔羊信息
    /// </summary>
    public class DeliveryLambData : Sheep
    {
        public bool IsChecked { get; set; }

        public new string BirthWeight { get; set; }

        public new string Gender { get; set; }

        public string Sheepfold { get; set; }
    }
}
