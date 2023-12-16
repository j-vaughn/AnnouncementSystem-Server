using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnnouncementServer
{
    class Intercom
    {
        public int IntercomId { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public enum _LocationType { Indoor, Outdoor, Nursery, Other }
        public _LocationType LocationType { get; set; }
        public enum _UseType { Normal, EmergencyOnly }
        public _UseType UseType { get; set; }
        public int DefaultVolume { get; set; }
        public int EmergencyVolume { get; set; }
        public string AccessToken { get; set; }
        public bool Enabled { get; set; }

        public string CmdPath(Sound sound)
        {
            return "http://" + IpAddress + ":" + Port + "/intercom" + sound.FileName;
        }

        public List<string> CommandUrls(Announcement announcement)
        {
            return new List<string>();
        }

        public string TargetUrl(string filename, int volume, int priority, int id)
        {
            UriBuilder a = new UriBuilder();
            a.Scheme = "http";
            a.Host = IpAddress;
            a.Port = Port;
            a.Query = "type=sound&message=" + filename + "&volume=" + volume;

            return a.ToString();
        }

        public string GetCommandUrl(string cmd)
        {
            UriBuilder a = new UriBuilder();
            a.Scheme = "http";
            a.Host = IpAddress;
            a.Port = Port;
            a.Query = "type=cmd&cmd=" + cmd;

            return a.ToString();
        }

        public void IssueCommand(string cmd)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadDataAsync(new System.Uri(this.GetCommandUrl(cmd)));
                }
                catch
                {
                    Console.WriteLine("error");
                }

                Console.WriteLine(this.GetCommandUrl(cmd));
            }
        }
    }
}
