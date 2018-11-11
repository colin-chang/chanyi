using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class ReportForms
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = Helper.Service.GetMultiplyReport(null, null);

            var list1 = Helper.Service.GetSellReport(null, null);
            var list2 = Helper.Service.GetBuyReport(null, null);
            var list3 = Helper.Service.GetFinanceReport(null, null);

            Assert.IsTrue(list.Count > 0);
        }
    }
}
