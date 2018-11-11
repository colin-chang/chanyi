
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using Chanyi.Web.Official.Models;
using Chanyi.Web.Official.ProductService;

namespace Chanyi.Web.Official.Controllers
{
    public class PriceController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tryout()
        {
            return View();
        }
    }
}