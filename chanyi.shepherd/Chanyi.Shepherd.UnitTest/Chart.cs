using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chanyi.Shepherd.QueryModel;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class Chart
    {
        [TestMethod]
        public void GetPeriodsSheepGrowthStageCount()
        {
            //var pie = Helper.Service.GetPeriodsSheepGrowthStageCount(DateTime.Parse("2015-4-1"),GenderEnum.Female);
            //Assert.IsNotNull(pie);
        }

        [TestMethod]
        public void GetPeriodsSellSheepCount()
        {
            var pie = Helper.Service.GetPeriodsSellSheepCount();
            Assert.IsNotNull(pie);
        }
    }
}
