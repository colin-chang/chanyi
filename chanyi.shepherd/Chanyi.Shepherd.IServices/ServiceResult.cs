using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chanyi.Shepherd.IServices
{
    public class ServiceResult<T> : ServiceResult
    {
        public new T Result { get; set; }

        public ServiceResult(T result)
        {
            this.Result = result;
            this.Status = ResultStatus.OK;
        }

        public ServiceResult(T result, ResultStatus status, string code)
        {
            this.Result = result;
            this.Status = status;
            this.Code = code;
        }

        public ServiceResult(T result, ResultStatus status, string code, string message)
        {
            this.Result = result;
            this.Status = status;
            this.Code = code;
            this.Message = message;
        }

        //private T result;
        //private ResultStatus status;
        //private string code;
        //private string message;

        //public T Result
        //{
        //    get { return this.result; }
        //    set { this.result = value; }
        //}

        //public ResultStatus Status
        //{
        //    get { return this.status; }
        //    set { this.status = value; }
        //}

        //public string Code
        //{
        //    get { return this.code; }
        //    set { this.code = value; }
        //}

        //public string Message
        //{
        //    get { return this.message; }
        //    set { this.message = value; }
        //}


        public ResultStatus GetStatus()
        {
            return this.Status;
        }
        public string GetMessage()
        {
            return this.Message;
        }
    }

    public class ServiceResult
    {
        public object Result { get; set; }

        public ResultStatus Status { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }
    }

    public enum ResultStatus
    {
        Unknown = 0,
        OK = 1,
        WANING = 2,
        ERROR = 0x63,
    }
}
