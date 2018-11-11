using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chanyi.Shepherd.QueryModel;
using Chanyi.Shepherd.QueryModel.Filter.Input;
using System.Collections.Generic;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class InOut
    {
        [TestMethod]
        public void AddFeedInOutWarehouse()
        {
            var result = Helper.Service.AddFeedInOutWarehouse("76973433-54C4-D032-1FF5-48E2548BA9FC", "F28FF613-0D35-F8B1-9D20-AE7647E538CA", "E0BB849F-544D-45CC-AC42-0456678AF749", 5, DateTime.Now.AddDays(-2), InOutWarehouseDirectionEnum.Out, "d69d46d1-6d78-4495-9528-3618a91d563b", "d69d46d1-6d78-4495-9528-3618a91d563b", "这批货很好");

            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void GetFeed()
        {
            int i = 0;
            var result = Helper.Service.GetFeed(new FeedFilter(),1,5,out i);

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void AddFeedBatchOutWarehouse()
        { 
            //(Dictionary<string, float> dictFeedAmount, List<string> shepfolds, DateTime operationDate, string principalId, string operatorId, string remark);

            Dictionary<string, float> dict = new Dictionary<string, float>();
            dict["5f62793c-025e-4f0c-bac2-8f6c3f035111"] = 21;
            dict["caa4a500-f0e2-4132-8591-3c2326f7c967"] = 22;
            dict["137d5596-13bd-4406-97a4-fe17f9414877"] = 23;

            List<string> list = new List<string>() { "809c0a2a-4a97-4fa8-bfc5-43c6c7b853ed"  };
            var result = Helper.Service.AddFeedBatchOutWarehouse(dict, list, DateTime.Today.AddDays(-1), "75091efd-4c77-4d95-89b3-1cfd08cea4b3", "9bed1d4d-e0c0-427f-afdc-abe26a095719", "UnitTest");

            Assert.IsTrue(result.Result==dict.Count);

        }

    }
}
