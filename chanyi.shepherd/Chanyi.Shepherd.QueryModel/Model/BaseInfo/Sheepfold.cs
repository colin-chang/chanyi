using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.QueryModel.Model.BaseInfo
{
    /// <summary>
    /// 圈舍
    /// </summary>
    public class Sheepfold : BaseModel
    {
        public string Name { get; set; }

        /// <summary>
        /// 羊圈管理员
        /// </summary>
        public string Administrator { get; set; }

        public string AdministratorName { get; set; }

        //羊只数量、圈舍名、管理员、创建者、创建时间

        public int SheepCount { get; set; }

        /// <summary>
        /// 是否是系统保留的，表示表示虚拟的死亡或者出售后的圈舍
        /// </summary>
        public bool SysFlag { get; set; }
    }
}
