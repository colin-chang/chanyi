using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;

using Chanyi.Web.Official.Models;
using Chanyi.Web.Official.ProductService;
using System.Configuration;

namespace Chanyi.Web.Official.Controllers
{
    public class ShepherdController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Download(Customer customer)
        {
            const string RESULT_KEY = "tryoutResult";
            //出错处理
            var error = new Func<AjaxResult<object>, ActionResult>(res =>
                {
                    Response.Cookies[RESULT_KEY].Value = JsonConvert.SerializeObject(res);
                    return Redirect("~/Tools/DownloadTryout");
                });

            //校验失败
            if (!ModelState.IsValid)
                return error(new AjaxResult<object>(null, false, 300, "填写信息不合法！"));

            using (var service = new ManageServiceClient())
            {
                var result = service.CreateCustomerTryout(customer.Department, customer.Scale, customer.ContactPerson, customer.PhoneNumber, customer.Address);

                //打包成功
                var suc = new Func<int, ActionResult>(code =>
                    {
                        Response.Cookies[RESULT_KEY].Value = JsonConvert.SerializeObject(new AjaxResult<object>(null, true, code, "试用成功"));
                        return File(ConfigurationManager.AppSettings["TryoutFilePath"] + result.Result, "text/html", "牧羊人羊场管理系统(试用版).zip");
                    });
                //TODO:下载完成后删除

                //程序异常
                var exc = new Func<int, ActionResult>(code => error(new AjaxResult<object>(null, false, code, result.Message)));

                //试用已过期
                var over = new Func<ActionResult>(() => error(new AjaxResult<object>(null, false, 120, "使用已结束")));

                switch (result.Code)
                {
                    case 100:
                    case 101:
                        return suc(result.Code);
                    case 110:
                        return exc(result.Code);
                    case 120:
                        return over();
                    default:
                        return error(new AjaxResult<object>(null, false, 110, "未知错误"));
                }
            }
        }

        public ActionResult DownloadDocument(string version)
        {
            return File(string.Format("~/Files/Shepherd/shepherd-doc-{0}.pdf", version), "text/html", string.Format("shepherd-doc-{0}.pdf", version));
        }

        public ActionResult DownloadPPT(string version)
        {
            return File(string.Format("~/Files/Shepherd/shepherd-ppt-{0}.ppt", version), "text/html", string.Format("shepherd-ppt-{0}.ppt", version));
        }

        public ActionResult DownloadPubLog(string version)
        {
            return File(string.Format("~/Files/Shepherd/shepherd-publog-{0}.docx", version), "text/html", string.Format("shepherd-publog-{0}.docx", version));
        }
    }
}