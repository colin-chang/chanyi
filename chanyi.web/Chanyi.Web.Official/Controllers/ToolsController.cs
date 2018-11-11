using System;
using System.Configuration;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using Chanyi.Web.Official.Models;

namespace Chanyi.Web.Official.Controllers
{
    public class ToolsController : Controller
    {
        public ActionResult VPN()
        {
            return View();
        }

        public ActionResult GetVpn()
        {
            string defaultMsg = "暂不可用";
            dynamic vpn = new ExpandoObject();
            AjaxResult<object> result;

            WebClient wc = new WebClient();
            try
            {
                string html = System.Text.Encoding.UTF8.GetString(wc.DownloadData(ConfigurationManager.AppSettings["FreeVPN"]));
                Match matchIP = Regex.Match(html, @"<p.*>IP地址：((\d{1,3}\.){3}\d{1,3}.*?)</p>");
                Match matchUid = Regex.Match(html, @"<p.*>用户名：(.+)</p>");
                if (!matchIP.Success || !matchUid.Success)
                {
                    vpn.IP = matchIP.Success ? matchIP.Groups[1].Value : defaultMsg;
                    vpn.Uid = matchUid.Success ? matchUid.Groups[1].Value : defaultMsg;
                    vpn.Pwd = defaultMsg;
                }
                else
                {
                    vpn.IP = matchIP.Groups[1].Value;
                    vpn.Uid = matchUid.Groups[1].Value;
                    Match pwdMatch = Regex.Match(html, "<iframe .*?src=\"(http://.*?)\".*?/?>");
                    vpn.Pwd = !pwdMatch.Success ? defaultMsg : wc.DownloadString(pwdMatch.Groups[1].Value);
                }
                result = new AjaxResult<object>(vpn, true);
            }
            catch
            {
                vpn.IP = defaultMsg;
                vpn.Uid = defaultMsg;
                vpn.Pwd = defaultMsg;
                result = new AjaxResult<object>(vpn, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetShadowSocks()
        {
            AjaxResult<object> result;
            WebClient wc = new WebClient();
            try
            {
                string html = System.Text.Encoding.UTF8.GetString(wc.DownloadData(ConfigurationManager.AppSettings["FreeShadowSocks"])).Replace("\n", "").Replace("\t", "").Replace("\r", "");
                html = Regex.Replace(html, @">\s+<", "><");

                string divs = Regex.Match(html, "(<section\\s+id=\"free\">(.+?)</section>)").Groups[1].Value;
                var matches = Regex.Matches(divs, "<h4.*?>(.+?)</h4>");
                List<string> list = new List<string>();
                foreach (Match match in matches)
                {
                    string mat = match.Groups[1].Value;
                    if (!mat.Contains('<'))
                        list.Add(mat.Substring(mat.IndexOf(':') + 1));
                }
                List<object> sdws = new List<object>();
                dynamic sdw_us = new ExpandoObject();
                sdw_us.Addr = list[0];
                sdw_us.Port = list[1];
                sdw_us.Pwd = list[2];
                sdw_us.Encry = list[3];
                sdws.Add(sdw_us);

                dynamic sdw_hk = new ExpandoObject();
                sdw_hk.Addr = list[4];
                sdw_hk.Port = list[5];
                sdw_hk.Pwd = list[6];
                sdw_hk.Encry = list[7];
                sdws.Add(sdw_hk);

                dynamic sdw_jp = new ExpandoObject();
                sdw_jp.Addr = list[8];
                sdw_jp.Port = list[9];
                sdw_jp.Pwd = list[10];
                sdw_jp.Encry = list[11];
                sdws.Add(sdw_jp);

                result = new AjaxResult<object>(sdws, true);
            }
            catch
            {
                result = new AjaxResult<object>(null, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadTryout()
        {
            return View();
        }
    }
}