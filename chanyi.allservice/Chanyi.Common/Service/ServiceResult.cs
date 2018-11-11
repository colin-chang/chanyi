using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.Common.Service
{
    public interface IServiceResult
    {
        ResultStatus GetStatus();
        string GetMessage();
    }

    [DataContract]
    public class ServiceResult<T> : IServiceResult
    {
        private T result;
        private ResultStatus status;
        private int code;
        private string message;


        public ServiceResult(T result)
        {
            this.result = result;
            this.status = ResultStatus.OK;
        }

        public ServiceResult(ResultStatus status, string message)
        {
            this.status = status;
            this.message = message;
        }

        public ServiceResult(T result, ResultStatus status)
        {
            this.result = result;
            this.status = status;
        }

        public ServiceResult(T result, ResultStatus status, string message)
        {
            this.result = result;
            this.status = status;
            this.message = message;
        }

        public ServiceResult(T result, ResultStatus status, int code, string message)
        {
            this.result = result;
            this.status = status;
            this.code = code;
            this.message = message;
        }


        [DataMember]
        public T Result
        {
            get { return this.result; }
            set { this.result = value; }
        }

        [DataMember]
        public ResultStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        [DataMember]
        public int Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        [DataMember]
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }


        public ResultStatus GetStatus()
        {
            return this.Status;
        }
        public string GetMessage()
        {
            return this.Message;
        }
    }

}
