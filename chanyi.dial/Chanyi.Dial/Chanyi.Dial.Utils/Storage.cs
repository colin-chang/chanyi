using System.Collections.Generic;
using System;

namespace Chanyi.Dial.Utils
{
    /// <summary>
    /// 数据存储
    /// </summary>
    public static class Storage
    {
        /// <summary>
        /// 当前手机绑定的PC端用户
        /// </summary>
        private static IList<string> _bindUsers;
        public static IList<string> BindUsers
        {
            get
            {
                if (_bindUsers == null)
                    _bindUsers = new List<string>();
                return _bindUsers;
            }
            set { _bindUsers = value; }
        }

        /// <summary>
        /// 当期在线PC端用户
        /// </summary>
        private static IEnumerable<string> _olnusrs;
        public static IEnumerable<string> OnlineWebUsers
        {
            get
            {
                if (_olnusrs == null)
                    _olnusrs = new List<string>();
                return _olnusrs;
            }
            set
            {
                _olnusrs = value;
            }
        }

        /// <summary>
        /// 是否已绑定PC端
        /// </summary>
        /// <value>The is assigned.</value>
        public static bool IsAssigned
        {
            get;
            set;
        }

        /// <summary>
        /// 主线程执行委托
        /// </summary>
        /// <value>The begin invoke on main thread.</value>
        public static Action<Action> MainThreadDispacher
        {
            get;
            set;
        }
    }
}

