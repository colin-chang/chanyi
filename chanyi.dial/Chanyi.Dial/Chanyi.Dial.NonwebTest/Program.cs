using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chanyi.Dial.NonwebTest
{
    class Program
    {
        static void Main(string[] args)
        {
            StartSession();
            Console.ReadKey();
        }

        static async void StartSession()
        {
            string url = "http://192.168.31.238:20003";
            var hubConn = new HubConnection(url);
            hubConn.TraceLevel = TraceLevels.All;
            hubConn.TraceWriter = Console.Out;
            var cookies = new CookieContainer();
            cookies.Add(new CookieCollection { 
                new Cookie("UserAgent", "app","/","192.168.31.238"), 
                new Cookie("UserId", "17090099602","/","192.168.31.238") 
            });
            hubConn.CookieContainer = cookies;

            var hubProxy = hubConn.CreateHubProxy("Transmitter");
            hubProxy["UserAgent"] = "app";

            await hubConn.Start().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Console.WriteLine("Error connecting to " + url + " Are you using the right URL?");
                    Console.ReadKey();
                    hubConn.Stop();
                }
            });

            //hubProxy.On<IEnumerable<string>>("ListWebUsers", users =>
            //{
            //    foreach (string u in users)
            //        Console.WriteLine(u);
            //});

            hubProxy.On<IEnumerable<string>>("ListWebUser",ListWebUser);

            
            //await hubProxy.Invoke("BindWebUser","xxx");
            //hubConn.Stop();
        }

        private async static void ListWebUser(IEnumerable<string> users)
        { 
        
        }
    }
}
