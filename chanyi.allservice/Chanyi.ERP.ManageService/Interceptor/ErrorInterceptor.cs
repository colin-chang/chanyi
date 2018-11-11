using AopAlliance.Intercept;
using Chanyi.Common.Service;
using Common.Logging;
using System;

namespace Chanyi.ERP.ManageService.Interceptor
{
    public class ErrorInterceptor : IMethodInterceptor
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IMethodInterceptor Members

        public object Invoke(IMethodInvocation invocation)
        {
            try
            {
                var result = invocation.Proceed();

                var serviceResult = result as IServiceResult;
                if (serviceResult != null)
                {
                    Logger.Debug(String.Format("服务（{0}）处理结果: status={1}    message={2}", invocation.Method.Name, serviceResult.GetStatus(), serviceResult.GetMessage()));
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(String.Format("服务（{0}）处理时，发生错误 {1}。", invocation.Method.Name, ex.ToString()));
                throw;
            }
        }

        #endregion
    }
}
