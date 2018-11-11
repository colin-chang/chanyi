using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chanyi.Shepherd.QueryModel.Filter.Multiplying;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class Multiplying
    {
        [TestMethod]
        public void MatingTest()
        {
            //var result= Helper.GetService().AddMating("1617140b-c257-4fff-b05f-654fc6f96761", "cbca2fa4-cc98-4d1d-9564-ab8369b68997", DateTime.Now.AddMonths(-1), "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "9bed1d4d-e0c0-427f-afdc-abe26a095719", null);

            int pageIndex = 0;
            int pageSize = 5;
            int totalCount = 0;
            MatingFilter filter = new MatingFilter() { IsRemindful = true };

            var list = Helper.GetService().GetMating(filter, pageIndex, pageSize, out totalCount);

            Assert.AreEqual(3, totalCount);

            //MatingFilter filter = new MatingFilter() { IsRemindful = true };

            //var list = Helper.GetService().GetMating(filter);

            //Assert.AreEqual(3,list.Count);
        }

        [TestMethod]
        public void AbortionTest()
        {
            //var result= Helper.GetService().AddAbortion("1617140b-c257-4fff-b05f-654fc6f96761", "cbca2fa4-cc98-4d1d-9564-ab8369b68997", DateTime.Now.AddMonths(-1), "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "9bed1d4d-e0c0-427f-afdc-abe26a095719", null);
            //var result = Helper.GetService().AddAbortion("1617140b-c257-4fff-b05f-654fc6f96761", "奔跑过度", "羊肉串", DateTime.Now.AddDays(20), "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "9bed1d4d-e0c0-427f-afdc-abe26a095719", "公羊着急的追着母羊跑");

            //Assert.IsNotNull(result.Result);
            
            var result1 = Helper.Service.DeleteDeathManage("c6a15a71-fff7-4ebd-8d15-3f735e06c168");
            var result = Helper.Service.DeleteAbortion("98b0d0b5-3c5d-4a01-b260-acb13811e1f2");
            var result2 = Helper.Service.DeleteDelivery("5589fff4-538e-48a3-8a3b-06c7aef355e4");
            var result3 = Helper.Service.DeleteMating("3ffefdef-2e2b-498a-8ea9-4688bd6f13ae");
            Assert.IsTrue(1==1);
        }

        [TestMethod]
        public void AddDelivery()
        {
            //var result = Helper.GetService().AddDelivery("张成", QueryModel.DeliveryWayEnum.Deliver, 3, 3, DateTime.Now.AddDays(-2), "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "9bed1d4d-e0c0-427f-afdc-abe26a095719", "这只羊有问题");
            //Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void AddAblactation()
        {
            var result = Helper.GetService().AddAblactation("2", float.Parse("2.2"), DateTime.Now.AddDays(-2), "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "9bed1d4d-e0c0-427f-afdc-abe26a095719", "妈蛋的更新不了");
            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void AddAssessStudsheep()
        {
            var result = Helper.GetService().AddAssessStudsheep("2", float.Parse("3.5"), float.Parse("150"), float.Parse("3.6"), DateTime.Now.AddDays(-1), "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "9bed1d4d-e0c0-427f-afdc-abe26a095719", "妈蛋的更新不了");
            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void AddFirstAssess()
        {
            var result = Helper.GetService().AddFirstAssess("2", float.Parse("150"), float.Parse("3.6"), DateTime.Now.AddDays(-1), "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "9bed1d4d-e0c0-427f-afdc-abe26a095719", "妈蛋的更新不了");
            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void AddSecondAssess()
        {
            var result = Helper.GetService().AddSecondAssess("2", float.Parse("3.3"), float.Parse("4.4"), float.Parse("150"), float.Parse("3.6"), DateTime.Now.AddDays(-1), "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "9bed1d4d-e0c0-427f-afdc-abe26a095719", "妈蛋的更新不了");
            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void AddThirdAssess()
        {
            var result = Helper.GetService().AddThirdAssess("2", float.Parse("4.4"), float.Parse("150"), float.Parse("3.6"), DateTime.Now.AddDays(-1), "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "9bed1d4d-e0c0-427f-afdc-abe26a095719", "妈蛋的更新不了");
            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void GetAblactation()
        {
            int totalcount;
            var result = Helper.Service.GetAblactation(new AblactationFilter(),1,5,out totalcount);
            Assert.IsNotNull(result);
        }

    }
}
