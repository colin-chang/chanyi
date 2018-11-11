using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chanyi.Shepherd.Services;
using Chanyi.Shepherd.IServices;
using System.Text.RegularExpressions;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class System
    {
        [TestMethod]
        public void TestLoginMethod()
        {
            ServiceResult<string> sr = Helper.GetService().Login("111", "111");
            //Assert.AreEqual("04", sr.Code);
            Assert.AreEqual(ResultStatus.OK, sr.Status);
        }

        [TestMethod]
        public void UpdatePassword()
        {
            //ServiceResult<bool> sr = Helper.GetService().UpdatePassword("111", "FB47506E-41BE-B16B-F8DE-56F572993A99");
            // Assert.IsTrue(sr.Result);
        }



        [TestMethod]
        public void Test()
        {
            //var r = Helper.Service.Update();
            //Assert.IsTrue(r.Result);
        }

    }
}
