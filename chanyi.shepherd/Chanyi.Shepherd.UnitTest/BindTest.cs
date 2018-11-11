using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chanyi.Shepherd.QueryModel.BindingModel;
using System.Collections.Generic;
using Chanyi.Shepherd.QueryModel.BindingFilter;
using Chanyi.Shepherd.QueryModel;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class BindTest
    {
        [TestMethod]
        public void InitializeTest()
        {
            Helper.GetService().Initialize();
            Assert.IsNotNull("1");
        }

        [TestMethod]
        public void SheepBindTest()
        {
            SheepBindFilter filter = new SheepBindFilter() { Gender=GenderEnum.Male, GrowthStage=GrowthStageEnum.Lamb };

            List<SheepBind> list= Helper.GetService().GetSheepBind(filter);
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void BreedBindTest()
        {
            List<BreedBind> list = Helper.GetService().GetBreedBind();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void SheepfoldTest()
        {
            List<SheepfoldBind> list = Helper.GetService().GetSheepfoldBind();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void EmployeeBindTest()
        {
            List<EmployeeBind> list = Helper.GetService().GetEmployeeBind();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void UserBindTest()
        {
            List<UserBind> list = Helper.GetService().GetUserBind();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void GetMoveSheepfoldBind()
        { 
            var dict=Helper.Service.GetMoveSheepfoldBind();
            Assert.IsNotNull(dict);
        }

        [TestMethod]
        public void GetDiseaseTypeBind()
        {
            //var dict = Helper.Service.GetDiseaseTypeBind();
            var dict = Helper.Service.GetSymptomTypeBind();
            Assert.IsNotNull(dict);
        }

        [TestMethod]
        public void GetInputsBind()
        {
            //var feedNameList = Helper.Service.GetFeedNameBind();
            //var feedTypeList = Helper.Service.GetFeedTypeBind();
            //var feedTypeList2 = Helper.Service.GetFeedTypeBind("C7EEE931-71AF-24B4-3B47-7883568A7945");
            //var areaTypeList = Helper.Service.GetAreaBind();
            //var areaTypeList2 = Helper.Service.GetAreaBind("C7EEE931-71AF-24B4-3B47-7883568A7945", "F28FF613-0D35-F8B1-9D20-AE7647E538CA");
            var medicineList = Helper.Service.GetMedicineNameBind();
            var ManufactureList = Helper.Service.GetManufactureBind();
            var ManufactureList2 = Helper.Service.GetManufactureBind("0775ea91-6511-4685-8f27-63c456f7b9d8");
            

            Assert.IsTrue(1==1);
        }
    }
}
