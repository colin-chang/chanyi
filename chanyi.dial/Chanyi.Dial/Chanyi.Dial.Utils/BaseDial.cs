using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace Chanyi.Dial.Utils
{
    public abstract class BaseDial : IDial
    {
        #region 基本参量
        protected const string USER_AGENT = "app";
        protected const string _58REGEX = "<ulclass=\"contact\"><liclass=\"black\">.*?</li><liclass=\"yellow\">([\\d,-]{7,13})</li></ul>";
        protected const string _GJREGEX = "<ahref=\"tel:([\\d,-]{7,13})\".+";
        public abstract string UserId { get; set; }
        public abstract string Host { get; set; }
        public abstract string Port { get; set; }
        public string Url {
            get {
                if (string.IsNullOrWhiteSpace (Host) || string.IsNullOrWhiteSpace (Port))
                    return null;
                return string.Format ("http://{0}:{1}", Host, Port);
            }
        }

        protected HubConnection hubConn;
        protected IHubProxy hubProxy;
        #endregion

        public async Task StartSession (Action<bool> completeHandler)
        {
            if (string.IsNullOrWhiteSpace (Url)) {
                ShowDialog ("请正确填写服务地址和端口！");
                return;
            }

            hubConn = new HubConnection (Url);
            var cookies = new CookieContainer ();
            cookies.Add (new CookieCollection {
                new Cookie("UserAgent", USER_AGENT,"/",Host),
                new Cookie("UserId", UserId,"/",Host)
            });
            hubConn.CookieContainer = cookies;
            hubProxy = hubConn.CreateHubProxy ("Transmitter");
            RegisterClientMethod ();

            bool success = false;
            await hubConn.Start ().ContinueWith (t => {
                if (t.IsFaulted)
                    hubConn.Stop ();
                success = !t.IsFaulted;
            });

            if (completeHandler == null)
                return;
            completeHandler (success);
        }

        private void RegisterClientMethod ()
        {
            hubProxy.On<IEnumerable<string>> ("ListWebUsers", ListWebUsers);
            hubProxy.On<string> ("Dial", url => Dial (url));
        }

        public void ListWebUsers (IEnumerable<string> webUsers)
        {
            Storage.OnlineWebUsers = webUsers;
        }

        public async Task BindWebUser (IEnumerable<string> userIds, Action exceptionHandler)
        {
            string json = JsonConvert.SerializeObject (userIds);
            try {
                await hubProxy.Invoke ("BindWebUser", json);
            } catch {
                ShowDialog ("绑定失败，与服务器通讯错误，请确保通信服务开启后重启监听!");
                if (exceptionHandler == null)
                    return;
                exceptionHandler ();
            }
        }

        public async Task Dial (string url)
        {
            string phoneNumber = await FetchPhoneNumber (url);
            if (string.IsNullOrWhiteSpace (phoneNumber)) {
                ShowDialog ("该帖子没有联系方式！");
                return;
            }
            CallPhoneNumber (phoneNumber);
        }

        public abstract void CallPhoneNumber (string phoneNumber);

        public void Destroy ()
        {
            Storage.IsAssigned = false;
            Storage.OnlineWebUsers = null;
            Storage.BindUsers = null;
            if (hubConn == null)
                return;

            hubConn.Stop ();
        }

        public async Task ReBindWebUser (Action showOnlineUserDialog, Action exceptionHandler)
        {
            try {
                await hubProxy.Invoke ("ListWebUsers");
                if (showOnlineUserDialog == null)
                    return;
                showOnlineUserDialog ();
            } catch {
                ShowDialog ("更新用户列表失败,与服务器通讯错误，请确保通信服务开启后重启监听!");
                if (exceptionHandler == null)
                    return;
                exceptionHandler ();
            }
        }

        public abstract void ShowDialog (string message);

        /// <summary>
        /// 抓取给定URL中手机号
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<string> FetchPhoneNumber (string url)
        {
            string phoneNumber = string.Empty;

            HttpWebRequest request = HttpWebRequest.CreateHttp (url);
            //request.Headers ["User-Agent"] = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1";
            request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1";
            var response = await request.GetResponseAsync () as HttpWebResponse;
            if (response == null)
                return phoneNumber;

            using (var stream = response.GetResponseStream ()) {
                using (var reader = new StreamReader (stream)) {
                    string html = await reader.ReadToEndAsync ();
                    html = html.Replace ("\r", "").Replace ("\n", "").Replace ("\t", "").Replace (" ", "");
                    var match = Regex.Match (html, MapRegex (url));
                    if (match.Success)
                        phoneNumber = match.Groups [1].Value;

                    return phoneNumber;
                }
            }
        }

        /// <summary>
        /// 根据域名匹配正则
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string MapRegex (string url)
        {
            string domain = GetDomain (url);
            if (string.IsNullOrWhiteSpace (domain))
                return null;

            if (domain.EndsWith ("58.com"))
                return _58REGEX;
            if (domain.EndsWith ("ganji.com"))
                return _GJREGEX;
            return null;
        }

        /// <summary>
        /// 获取指定URL的域名部门
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetDomain (string url)
        {
            if (string.IsNullOrWhiteSpace (url))
                return null;

            var match = Regex.Match (url, "http://(.*?)/.+");
            if (!match.Success)
                return null;

            return match.Groups [1].Value.ToLower ();
        }
    }
}

