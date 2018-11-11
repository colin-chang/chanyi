using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chanyi.Shepherd.QueryModel;
using System.Collections.Generic;
using Chanyi.Shepherd.QueryModel.Filter.Group;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class GroupManage
    {
        [TestMethod]
        public void AddDeathManage()
        {
            var result = Helper.Service.AddDeathManage("9c116364-1d1f-4598-9a4e-9b4959e0e948", "a", DeathDisposeEnum.Destroy, DateTime.Now.AddDays(-2), "A45598D2-59DD-4663-95BA-C9E955677204", "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "a");

            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void AddMoveSheepfold()
        {
            //List<string> list = new List<string>() { "fb078079-936d-4b0f-82d5-d08e5fb90420", "24120845-780e-487b-bff8-438d093ef150", "30e3a340-c7a1-4b7b-8329-1fd9be9df4a9" };

            //var result = Helper.Service.AddMoveSheepfold(list, "e1d9b67c-7864-43f5-971f-33bcb33dab7f", "A45598D2-59DD-4663-95BA-C9E955677204", "3013EAF8-18C0-404C-A847-7844E9A5D2DF");

            //Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public void GetMoveSheepfold()
        {
            int total;
            var result = Helper.Service.GetMoveSheepfold(new MoveSheepfoldFilter(),1,5,out total);
            Assert.IsNotNull(result);
        }


    }
}
