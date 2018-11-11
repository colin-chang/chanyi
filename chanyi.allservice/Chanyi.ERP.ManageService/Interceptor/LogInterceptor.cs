using AopAlliance.Intercept;
using Common.Logging;
using System;
using System.Diagnostics;

namespace Chanyi.ERP.ManageService.Interceptor
{
    public class LogInterceptor : IMethodInterceptor
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IMethodInterceptor Members

        public object Invoke(IMethodInvocation invocation)
        {
            Stopwatch sw = new Stopwatch();
            try
            {
                sw.Start();
                return invocation.Proceed();
            }
            catch (Exception ex)
            {
                sw.Stop();
                Logger.Error(String.Format("服务（{0}）处理时，发生错误。", invocation.Method.Name), ex);
                throw;
            }
            finally
            {
                sw.Stop();
                if (invocation.Method.Name.PadRight(7,' ').Substring(0, 7) == "publish")
                    Logger.Debug(String.Format("服务（{0}）处理时间:{1}毫秒", invocation.Method.Name, sw.ElapsedMilliseconds));
            }
        }

        #endregion
    }
}
