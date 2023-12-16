using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnnouncementServer
{
    class AnnouncementSystem
    {
        public ClientConfig config;
        public AnnouncementQueue Queue;

        public AnnouncementSystem()
        {
            LoadConfig();
            Queue = new AnnouncementQueue();
        }
        public void LoadConfig()
        {
            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                ClientConfig items = JsonConvert.DeserializeObject<ClientConfig>(json);

                config = items;
            }

            foreach(Announcement a in config.Announcements)
            {
                a.FillSounds(config.Sounds);
            }

            foreach(IntercomGroup ig in config.IntercomGroups)
            {
                ig.FillIntercoms(config.Intercoms);
            }
        }

        public void Announce(string intercomGroup, string announcement)
        {
            Announcement a = GetAnnouncementFromName(announcement);

            foreach(Intercom i in config.Intercoms)
            {
                if(i.Name == intercomGroup)
                {
                    Queue.AddAnnouncementToQueue(a, i);
                }
            }

            foreach(IntercomGroup ig in config.IntercomGroups)
            {
                if(ig.GroupName == intercomGroup)
                {
                    Queue.AddAnnouncementToQueue(a, ig);
                }
            }
        }

        public static bool IsInactive(AnnouncementCommand ac)
        {
            return ac.AnnouncementCommandActive;
        }

        public void PerformAnnouncements()
        {
            Queue.PlayQueue();
        }

        public void StopAll()
        {
            foreach(Intercom i in config.Intercoms)
            {
                i.IssueCommand("stopall");
            }
        }

        public void Stop(string intercomGroupName)
        {
            Cmd(GetIntercomGroupFromName(intercomGroupName).Intercoms, "stopall");            
        }

        public void Stop(Announcement a)
        {
            //todo  
        }

        private void Cmd(List<Intercom> intercoms, string cmd)
        {
            List<string> r = new List<string>();
            foreach(Intercom i in intercoms)
            {
                r.Add(i.GetCommandUrl(cmd));
            }

            ExecuteWebRequests(r);
        }

        private void ExecuteWebRequests(List<string> urls)
        {
            using (var client = new WebClient())
            {
                foreach(string url in urls)
                {
                    try
                    {
                        client.DownloadString(url);
                    }
                    catch
                    {
                        Console.WriteLine("error");
                    }
                    Console.WriteLine(url);
                }
            }
        }

        private IntercomGroup GetIntercomGroupFromName(string name)
        {
            return config.IntercomGroups.Where(t => t.GroupName == name).FirstOrDefault();
        }

        private Announcement GetAnnouncementFromName(string name)
        {
            return config.Announcements.Where(t => t.AnnouncementName == name).FirstOrDefault();
        }

        public bool IntercomNameExist(string n)
        {
            foreach(Intercom i in config.Intercoms)
            {
                if(i.Name == n) return true;
            }

            return false;
        }

        public bool IntercomGroupExists(string n)
        {
            foreach(IntercomGroup ig in config.IntercomGroups)
            {
                if (ig.GroupName == n) return true;
            }

            return false;
        }

        public bool IntercomOrIntercomGroupExists(string n)
        {
            return (IntercomNameExist(n) || IntercomGroupExists(n));
        }
    }
}
