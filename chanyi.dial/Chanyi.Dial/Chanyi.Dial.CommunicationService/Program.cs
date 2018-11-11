using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Chanyi.Dial.CommunicationService
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string url = GetListenUrl();
            if (string.IsNullOrWhiteSpace(url))
            {
                Console.WriteLine("未获取正确的IP地址…");
                return;
            }

            Console.WriteLine("Starting Service…");
            try
            {
                using (WebApp.Start(url))
                {
                    Console.WriteLine("Server running at " + url);
                    Console.WriteLine("---------------------------------------------------------------");
                    while (true)
                        Console.ReadKey(true);
                }
            }
            catch (TargetInvocationException ex)
            {
                string msg = ex.InnerException.Message;
                if (msg.Contains("拒绝访问"))
                    Console.WriteLine("服务启动失败,请确保以管理员身份运行！");
                if (msg.Contains("注册冲突"))
                    Console.WriteLine("服务监听地址或端口已占用！");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }


        /// <summary>
        /// 获取服务监听地址（首选配置文件，没有则采用本机IP）
        /// </summary>
        /// <returns></returns>
        static string GetListenUrl()
        {
            string ip = ConfigurationManager.AppSettings["IP"];
            ip = string.IsNullOrWhiteSpace(ip) ? GetIP() : ip;
            string port = ConfigurationManager.AppSettings["Port"];

            return string.IsNullOrWhiteSpace(ip) ? null : string.Format("http://{0}:{1}", ip, port);
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        static string GetIP()
        {
            string hostName = Dns.GetHostName();
            IPAddress[] addressList = Dns.GetHostAddresses(hostName);
            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            Console.WriteLine("IP地址获取失败，请手动输入本机IP地址…");
            string input = Console.ReadLine();
            IPAddress ipAddr;
            if (!IPAddress.TryParse(input, out ipAddr))
                return null;
            return ipAddr.ToString();
        }
    }
}
