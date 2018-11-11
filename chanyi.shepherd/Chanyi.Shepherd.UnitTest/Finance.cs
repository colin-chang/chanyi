using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Chanyi.Shepherd.QueryModel.AddModel.Finance;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class Finance
    {
        [TestMethod]
        public void SellSheep()
        {
            List<AddSellSheep> list=new List<AddSellSheep>(){
                new AddSellSheep(){ SheepId="34CA279F-2AF7-D3A2-71BC-BE9F72F5B6FA", Price=1, Weight=2},
                new AddSellSheep(){ SheepId="8B4A9615-1F2C-2D85-2B15-7A0639A92170", Price=1, Weight=2}
            };

            var result = Helper.Service.AddSellSheep(list,5,10, "BDA0A8CA-76CA-3799-E878-B458458AD193", DateTime.Now.AddDays(-10), "A45598D2-59DD-4663-95BA-C9E955677204", "9bed1d4d-e0c0-427f-afdc-abe26a095719", "包含了");
            Assert.IsTrue(result.Result==5);
            
        }
    }
}
