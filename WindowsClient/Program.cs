using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnnouncementServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Announcement Server";

            AnnouncementSystem a = new AnnouncementSystem();

            Task.Run(() => {
                Console.WriteLine("[ INFO  " + DateTime.Now.ToString("hh:mm:ss.fff") + " ] Queue processor started");
                while (true)
                {

                    a.PerformAnnouncements();
                    Thread.Sleep(1000);
                }
            });

            string ServerUrl = "http://localhost:" + a.config.WebPort + "/";
            var prefixes = new List<string>() { ServerUrl };
            using (HttpListener listener = new HttpListener())
            {

                foreach (string s in prefixes)
                {
                    listener.Prefixes.Add(s);
                }

                listener.Start();
                Console.WriteLine("[ INFO  " + DateTime.Now.ToString("hh:mm:ss.fff") + " ] Server Listening on " + ServerUrl);
                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;

                    string r = "Error - Invalid Command";

                    if(request.QueryString["k"] == a.config.WebKey)
                    {
                        // request received is to play something
                        if(request.QueryString["c"] == "play")
                        {
                            // play a sound: check for required query string values
                            if(!String.IsNullOrWhiteSpace(request.QueryString["t"]) && !String.IsNullOrWhiteSpace(request.QueryString["s"]))
                            {
                                // make sure provided intercom/intercomgroup name is valid and sound is valid
                                if(a.IntercomOrIntercomGroupExists(request.QueryString["t"]))
                                {
                                    // check sound exists?
                                    a.Announce(request.QueryString["t"], request.QueryString["s"]);
                                    r = "Success:  Announcement submitted to queue";
                                }
                            }
                        }

                        // request received to issue a command
                        if(request.QueryString["c"] == "cmd")
                        {
                            // request to stop all
                            if(request.QueryString["a"] == "stopall")
                            {
                                a.Queue.InactivateQueue();
                                a.StopAll();
                                r = "Success:  Announcements stopped and queue cleared";
                            }
                        }
                    }

                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;
                    // Construct a response.
                    string responseString = r;
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            }
        }
    }
}
