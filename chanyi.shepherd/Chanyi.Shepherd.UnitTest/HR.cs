using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chanyi.Shepherd.QueryModel.Filter.BaseInfo;
using Chanyi.Shepherd.QueryModel.Model.BaseInfo;
using System.Collections.Generic;
using Chanyi.Shepherd.QueryModel.Filter.HR;
using Chanyi.Shepherd.QueryModel.Model.System;
using Chanyi.Shepherd.QueryModel.Model.HR;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class HR
    {
        //Employee:   A45598D2-59DD-4663-95BA-C9E955677204
        //User:       3013EAF8-18C0-404C-A847-7844E9A5D2DF
        //Duty:       6d4d0782-dc61-49b6-ae79-24d0cfb8b46f

        [TestMethod]
        public void AddEmployeeTest()
        {
            var result = Helper.Service.AddEmployee("y", QueryModel.GenderEnum.Female, "417757199010028518", DateTime.Now.AddYears(-3), 2000M, "1110", "6d4d0782-dc61-49b6-ae79-24d0cfb8b46f", QueryModel.EmployeeStatusEnum.OnJob, null, "3013EAF8-18C0-404C-A847-7844E9A5D2DF", "巴扎黑");
            Assert.IsNotNull(result.Result);
        }
    }
}
