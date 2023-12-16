using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnnouncementServer
{
    class AnnouncementCommand
    {
        public Announcement Ann1 { get; set; }
        private List<Intercom> TargetIntercoms { get; set; }

        public bool AnnouncementCommandActive { get; set; }
        public int Id { get; set; }
        public int LoopTimes { get; set; }
        public string TargetIntercomFriendlyName { get; set; }
        private readonly System.Net.Http.HttpClient httpClient;
        
        public AnnouncementCommand(Announcement a)
        {
            Ann1 = a;
            LoopTimes = a.LoopTimes;
            Random rnd = new Random();
            Id = rnd.Next(10000,99999);
            AnnouncementCommandActive = true;
            httpClient = new System.Net.Http.HttpClient();
        }

        public AnnouncementCommand(Announcement a, Intercom i) : this(a)
        {
            TargetIntercoms = new List<Intercom>();
            TargetIntercoms.Add(i);
            TargetIntercomFriendlyName = "intercom" + i.Name;
            Console.WriteLine("[ " + Id + " " + DateTime.Now.ToString("hh:mm:ss.fff") + " ] Added new announcement to queue.  Announce " + Ann1.AnnouncementName + " to " + TargetIntercomFriendlyName);
        }

        public AnnouncementCommand(Announcement a, IntercomGroup ig) : this(a)
        {
            TargetIntercoms = new List<Intercom>();

            foreach (Intercom i in ig.Intercoms)
            {
                TargetIntercoms.Add(i);
            }

            TargetIntercomFriendlyName = "group " + ig.GroupName;
            Console.WriteLine("[ " + Id + " " + DateTime.Now.ToString("hh:mm:ss.fff") + " ] Added new announcement to queue.  Announce " + Ann1.AnnouncementName + " to " + TargetIntercomFriendlyName);
        }

        public void Play()
        {
            int t = 0;
            foreach (var s in Ann1.Sounds)
            {
                List<string> r = new List<string>();

                foreach (var i in TargetIntercoms)
                {
                    if (AnnouncementCommandActive)
                    {
                        int annVol = i.DefaultVolume + Ann1.AnnouncementVolumeModifier + s.SoundVolumeModifier;

                        // if the calculated value is less than 0, we probably don't want it to be
                        // totally silent when played, set to 5
                        if (annVol < 0) annVol = 5;

                        r.Add(i.TargetUrl(s.FileName, annVol, (Ann1.Priority + t), Id));
                        Console.WriteLine("[ " + Id + " " + DateTime.Now.ToString("hh:mm:ss.fff") + " ] Playing " + s.SoundName + " on " + i.Name);
                    }
                }

                if(AnnouncementCommandActive)
                {
                    List<System.Uri> r2 = new List<System.Uri>();

                    foreach(string u in r)
                    {
                        r2.Add(new System.Uri(u));
                    }

                    ExecuteWebRequests(r2);
                    Thread.Sleep(s.PlayTime * 1000);
                    Thread.Sleep(500);
                }
                
                t++;
            }

            if (LoopTimes > 1)
            {
                LoopTimes--;
            }
            else
            {
                AnnouncementCommandActive = false;
                Console.WriteLine("[ " + Id + " " + DateTime.Now.ToString("hh:mm:ss.fff") + " ] Finished announcing " + Ann1.AnnouncementName + " to " + TargetIntercomFriendlyName);
                httpClient.Dispose();
            }
        }

        private void ExecuteWebRequests(List<System.Uri> urls)
        {
            Parallel.ForEach(urls, new ParallelOptions { MaxDegreeOfParallelism = 20 }, url =>
            {
                try
                {
                    httpClient.GetAsync(url);
                }
                catch (Exception e)
                {

                }
            });
        }
    }
}
