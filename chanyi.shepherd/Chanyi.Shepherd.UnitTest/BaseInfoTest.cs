using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chanyi.Shepherd.QueryModel.Filter.BaseInfo;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;
using System.Collections.Generic;
using Chanyi.Shepherd.QueryModel;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class BaseInfoTest
    {

        [TestMethod]
        public void TestBreedMethod()
        {
            BreedFilter filter = new BreedFilter() { };
            List<Breed> list = Helper.GetService().GetBreed(filter);
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void SheepMethod()
        {
            SheepFilter filter = new SheepFilter() { EndAblactationDate = DateTime.Now };
            int totalCount;
            int pageIndex = 2;
            int pageSize = 5;
            List<Sheep> list = Helper.GetService().GetSheep(filter, pageIndex, pageSize, out totalCount);

            var list2 = Helper.Service.GetSheep(filter, 30);

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void SheepfoldMethod()
        {
            //SheepfoldFilter filter = new SheepfoldFilter() { AdministratorName = "宗" };
            //List<Sheepfold> list = Helper.GetService().GetSheepfold(filter);
            //Assert.IsNotNull(list);
        }

        [TestMethod]
        public void AddSheepMethod()
        {
            for (int i = 0; i < 500; i++)
            {
                Helper.GetService().AddSheep("b0" + i, "0239cf2c-2979-4239-adba-acc10ae5789c", GenderEnum.Female, GrowthStageEnum.LambHog, OriginEnum.HomeBred, 10, 2, DateTime.Parse("2015-3-3"), "cbca2fa4-cc98-4d1d-9564-ab8369b68997", "2bfd0e5c-8bf5-461c-9090-cb0407c7b1e6", "527F614F-1B0D-43D3-B349-C8EBCA4CB196", "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "9bed1d4d-e0c0-427f-afdc-abe26a095719", "妈蛋" + i);
            }
            var result = Helper.GetService().AddSheep("b2", "0239cf2c-2979-4239-adba-acc10ae5789c", GenderEnum.Female, GrowthStageEnum.LambHog, OriginEnum.HomeBred, 10, 2, DateTime.Parse("2015-3-3"), "cbca2fa4-cc98-4d1d-9564-ab8369b68997", "2bfd0e5c-8bf5-461c-9090-cb0407c7b1e6", "527F614F-1B0D-43D3-B349-C8EBCA4CB196", "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "9bed1d4d-e0c0-427f-afdc-abe26a095719", "妈蛋");

            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void AddBreed()
        {
            var result = Helper.GetService().AddBreed("藏羚羊", "很能干", "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "巴扎黑");

            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void AddSheepFold()
        {
            var result = Helper.GetService().AddSheepFold("6号圈" , "9bed1d4d-e0c0-427f-afdc-abe26a095719", "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "巴扎黑");

            Assert.IsNotNull(result.Result);
        }
    }
}
