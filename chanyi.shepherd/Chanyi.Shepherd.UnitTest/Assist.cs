using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chanyi.Shepherd.QueryModel.Filter.Assist;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class Assist
    {
        [TestMethod]
        public void GetDiseaseType()
        {
            //var list = Helper.Service.GetDiseaseType("0");
            //Assert.IsNull(list);
        }

        [TestMethod]
        public void GetDisease()
        {
            //var list = Helper.Service.GetDiseaseByType("8AE276F9-9173-22BF-4BEC-897CD887E9DC");
            //Assert.IsNotNull(list);
        }
        [TestMethod]
        public void GetDiseaseBySymptomName()
        {
            //var list = Helper.Service.GetDisease("吸");
            //var list = Helper.Service.GetDiseaseBySymptomIds("50e6dee4-3f6d-460f-b558-55c04c3188c1", "ac6b8355-6cff-4de5-8d3a-263f6e2ca602");
            //Assert.AreEqual(12, list.Count);
        }

        [TestMethod]
        public void GetSymptomType()
        {
            //var list = Helper.Service.GetSymptomType();
            //var list = Helper.Service.GetSymptom(new SymptomFilter());

            //Assert.AreEqual(12, list.Count);
        }

        

    }
}
