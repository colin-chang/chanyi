using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class DecisionAids
    {
        [TestMethod]
        public void GetFamilyTree()
        {
            var list = Helper.Service.GetFamilyTree("FCF3B0C4-9CCA-E733-326B-0802A4D06C6C",6);
            Assert.AreEqual(list.Count, 31);
        }
    }
}
