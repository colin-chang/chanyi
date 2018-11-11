using System;
using System.Diagnostics;
using AopAlliance.Intercept;
using Common.Logging;

namespace Chanyi.ERP.ManageService.Interceptor
{
    public class PerformanceInterceptor : IMethodInterceptor
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string prefix = "Invocation took";


          
        #region IMethodInterceptor Members

        public object Invoke(IMethodInvocation invocation)
        {
            Stopwatch sw = new Stopwatch();
            try
            {
                sw.Start();
                return invocation.Proceed();
            }
            finally
            {
                sw.Stop();
                Logger.Debug(String.Format("服务（{0}）处理时间:{1}毫秒", invocation.Method.Name, sw.ElapsedMilliseconds));
            }
        }

        #endregion


        public string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }
    }
}
