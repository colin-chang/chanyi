using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chanyi.Shepherd.QueryModel.Filter.Formula;

namespace Chanyi.Shepherd.UnitTest
{
    [TestClass]
    public class Formula
    {
        [TestMethod]
        public void TestMethod1()
        {
            int t;
            //var list = Helper.Service.GetFormula(new QueryModel.Filter.Formula.FormulaFilter(), 1, 5, out t);
            //var list2 = Helper.Service.GetFormulaFeed("0780C85B-E6CE-4CC8-BB92-CB6485DBDBB0");

            var list = Helper.Service.GetFormulaNutrient(new FormulaNutrientFilter(), 0, 5, out t);

            Assert.IsNotNull(list);
        }
    }
}
