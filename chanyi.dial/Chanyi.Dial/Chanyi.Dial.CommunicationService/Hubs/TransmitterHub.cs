using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Configuration;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace Chanyi.Dial.CommunicationService.Hubs
{
    [HubName("Transmitter")]
    public class TransmitterHub : Hub
    {
        private readonly string USEID = ConfigurationManager.AppSettings["UserId"];
        private readonly string USERAGENT = ConfigurationManager.AppSettings["UserAgent"];
        private const string USERAGENT_APP = "app";
        private const string USERAGENT_WEB = "web";

        /// <summary>
        /// Web端与APP端绑定列表
        /// </summary>
        private static ConcurrentDictionary<string, ConcurrentBag<string>> wMa = new ConcurrentDictionary<string, ConcurrentBag<string>>();

        /// <summary>
        /// App端列表
        /// </summary>
        private static ConcurrentBag<string> apps = new ConcurrentBag<string>();

        /// <summary>
        /// 客户端标识(web or app)
        /// </summary>
        private string UserAgent
        {
            get
            {
                var cookie = this.Context.RequestCookies[USERAGENT];
                if (cookie == null)
                    return null;
                return cookie.Value;
            }
        }

        /// <summary>
        /// 用户标识(Web端使用自定义用户名，app端使用手机号)
        /// </summary>
        private string UserId
        {
            get
            {
                if (!this.Context.Request.Cookies.ContainsKey(USEID))
                    return null;
                var cookie = this.Context.RequestCookies[USEID];
                if (cookie == null)
                    return null;
                return cookie.Value;
            }
            set
            {
                var cookie = new Microsoft.AspNet.SignalR.Cookie(USEID, value);
                if (this.Context.RequestCookies.ContainsKey(USEID))
                    this.Context.RequestCookies[USEID] = cookie;
                else
                    this.Context.RequestCookies.Add(USEID, cookie);
            }
        }

        public override Task OnConnected()
        {
            if (string.Equals(this.UserAgent, USERAGENT_WEB, StringComparison.CurrentCultureIgnoreCase))
            {
                //web
                if (string.IsNullOrWhiteSpace(this.UserId))
                    return null;
                else if (!wMa.ContainsKey(this.UserId))
                {
                    wMa[this.UserId] = new ConcurrentBag<string>();
                    this.BroadcastWebUsers2Apps();
                }
            }
            else if (string.Equals(this.UserAgent, USERAGENT_APP, StringComparison.CurrentCultureIgnoreCase))
            {
                //app
                if (apps.Contains(this.UserId))
                    return null;

                apps.Add(this.UserId);
                this.ListWebUsers();
            }
            Log("New Connection", "User Agent|" + this.UserAgent);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (string.Equals(this.UserAgent, USERAGENT_WEB, StringComparison.CurrentCultureIgnoreCase))
            {
                //web
                if (!wMa.ContainsKey(this.UserId))
                    return null;

                ConcurrentBag<string> item;
                if (!wMa.TryRemove(this.UserId, out item))
                {
                    wMa[this.UserId] = null;
                    wMa.Keys.Remove(this.UserId);
                }
                this.BroadcastWebUsers2Apps();
            }
            else if (string.Equals(this.UserAgent, USERAGENT_APP, StringComparison.CurrentCultureIgnoreCase))
            {
                //app
                if (!apps.Contains(this.UserId))
                    return null;

                //移除app
                var aps = apps.ToList();
                aps.Remove(this.UserId);
                apps = new ConcurrentBag<string>(aps);

                foreach (string web in wMa.Keys)
                {
                    var ps = wMa[web];
                    if (!ps.Contains(this.UserId))
                        continue;

                    var taps = ps.ToList();
                    taps.Remove(this.UserId);
                    ps = new ConcurrentBag<string>(taps);
                }
            }
            this.Log("User Leave", "User Agent|" + this.UserAgent);
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// 判断web端用户名是否可用
        /// </summary>
        /// <param name="webUserName"></param>
        /// <returns></returns>
        public async Task<bool> IsUserNameEnabled(string webUserId)
        {
            bool result = !wMa.ContainsKey(webUserId);
            this.Log("Validate UserName", "User Name|" + webUserId, "Result|" + result);
            return result;
        }

        /// <summary>
        /// 新增web端用户
        /// </summary>
        /// <param name="webUserId"></param>
        /// <returns></returns>
        public async Task AddWebUser(string webUserId)
        {
            bool isEnabled = await this.IsUserNameEnabled(webUserId);
            if (!isEnabled)
                return;

            this.UserId = webUserId;
            if (wMa.ContainsKey(webUserId))
                return;

            wMa[this.UserId] = new ConcurrentBag<string>();
            this.Log("Add WebUser");
            this.BroadcastWebUsers2Apps();
        }

        /// <summary>
        /// app端绑定web用户
        /// </summary>
        /// <param name="webUserIds"></param>
        /// <returns></returns>
        public async Task BindWebUser(string userIds)
        {
            string[] webUserIds = JsonConvert.DeserializeObject<string[]>(userIds);
            if (webUserIds == null)
                return;

            foreach (string w in wMa.Keys)
            {
                var aps = wMa[w];
                if (webUserIds.Contains(w))
                {
                    if (aps.Contains(this.UserId))
                        continue;
                    aps.Add(this.UserId);
                }
                else
                {
                    if (!aps.Contains(this.UserId))
                        continue;
                    var taps = apps.ToList();
                    taps.Remove(this.UserId);
                    wMa[w] = new ConcurrentBag<string>(taps);
                }
            }

            this.Log("Bind WebUser", "APP|" + this.UserId, "Web|" + userIds);
        }

        /// <summary>
        /// 拨打给定URL对应的电话
        /// </summary>
        /// <param name="url"></param>
        public void Dial(string url)
        {
            Log("Receive Message", "From|" + this.UserId, "Content|" + url);
            this.PushUrl2App(url);
        }

        /// <summary>
        /// 向当前连接app推送在线Web用户列表
        /// </summary>
        public void ListWebUsers()
        {
            if (this.UserAgent == USERAGENT_WEB)
                return;

            this.Clients.Caller.ListWebUsers(wMa.Keys);
            this.Log("List Web Users", "Receiver|" + this.UserId, "Content|" + string.Join(",", wMa.Keys));
        }

        ///// <summary>
        ///// 抓取给定URL中手机号
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public async Task<string> FetchPhoneNumber(string url)
        //{
        //    string phoneNumber = string.Empty;

        //    HttpWebRequest request = HttpWebRequest.CreateHttp(url);
        //    request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1";
        //    var response = await request.GetResponseAsync() as HttpWebResponse;
        //    if (response == null)
        //        return phoneNumber;

        //    using (var stream = response.GetResponseStream())
        //    {
        //        using (var reader = new StreamReader(stream))
        //        {
        //            string html = await reader.ReadToEndAsync();
        //            html = html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
        //            var match = Regex.Match(html, MapRegex(url));
        //            if (match.Success)
        //            {
        //                phoneNumber = match.Groups[1].Value;
        //                this.PushPhoneNumber2App(phoneNumber);
        //            }

        //            return phoneNumber;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 根据域名匹配正则
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //string MapRegex(string url)
        //{
        //    string domain = this.GetDomain(url);
        //    if (string.IsNullOrWhiteSpace(domain))
        //        return null;

        //    if (domain.EndsWith("58.com"))
        //        return ConfigurationManager.AppSettings["58regex"];
        //    if (domain.EndsWith("ganji.com"))
        //        return ConfigurationManager.AppSettings["gjregex"];
        //    return null;
        //}

        ///// <summary>
        ///// 获取指定URL的域名部门
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //private string GetDomain(string url)
        //{
        //    if (string.IsNullOrWhiteSpace(url))
        //        return null;

        //    var match = Regex.Match(url, "http://(.*?)/.+");
        //    if (!match.Success)
        //        return null;

        //    return match.Groups[1].Value.ToLower();
        //}

        /// <summary>
        /// 向所有app端广播web用户列表
        /// </summary>
        private void BroadcastWebUsers2Apps()
        {
            this.Clients.Users(apps.ToList()).ListWebUsers(wMa.Keys);
            this.Log("Broadcast Message", "Receiver|" + string.Join(", ", apps), "Content|" + string.Join(",", wMa.Keys));
        }

        /// <summary>
        /// 向当前web用户绑定app推送当前拦截URL
        /// </summary>
        /// <param name="url"></param>
        private void PushUrl2App(string url)
        {
            var aps = wMa[this.UserId].ToList();
            this.Clients.Users(aps).Dial(url);
            this.Log("Push Message", "Receiver|" + string.Join(", ", aps), "Content|" + url);
        }

        private void Log(string title, params string[] items)
        {
            Console.WriteLine(string.Format("{0}\t{1}", title, DateTime.Now));
            if (items != null && items.Length > 0)
            {
                foreach (string item in items)
                    Console.WriteLine(item.Replace('|', ':'));
            }
            Console.WriteLine("User Id:" + this.UserId);
            Console.WriteLine("Connection Id:" + Context.ConnectionId);
            Console.WriteLine("---------------------------------------------------------------");
        }
    }
}