using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chanyi.Web.Official.Controllers
{
    public class PartialsController : Controller
    {
        public ActionResult Header()
        {
            return PartialView("_HeaderPartial");
        }

        public ActionResult Footer()
        {
            return PartialView("_FooterPartial");
        }
    }
}