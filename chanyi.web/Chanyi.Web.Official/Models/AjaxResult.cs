using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chanyi.Web.Official.Models
{
    public class AjaxResult<T>
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsOk { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        public AjaxResult(T result) : this(result, false, -1, null) { }

        public AjaxResult(T result, bool isOK) : this(result, isOK, -1, null) { }

        public AjaxResult(T result, bool isOk, int code, string message)
        {
            this.Result = result;
            this.IsOk = isOk;
            this.Code = code;
            this.Message = message;
        }
    }
}