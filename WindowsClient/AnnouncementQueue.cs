using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnnouncementServer
{
    class AnnouncementQueue
    {
        public ConcurrentBag<AnnouncementCommand> Queue;

        public AnnouncementQueue()
        {
            Queue = new ConcurrentBag<AnnouncementCommand>();
        }

        public void AddAnnouncementToQueue(Announcement a, IntercomGroup ig)
        {
            Queue.Add(new AnnouncementCommand(a, ig));
        }

        public void AddAnnouncementToQueue(Announcement a, Intercom i)
        {
            Queue.Add(new AnnouncementCommand(a, i));
        }

        public string GetActiveAnnouncementsJson()
        {
            List<AnnouncementCommand> t = Queue.ToList();//.Where(x => x.AnnouncementCommandActive == true).ToList();

            return Newtonsoft.Json.JsonConvert.SerializeObject(t);
        }

        public List<AnnouncementCommand> GetActiveAnnouncements()
        {
            return Queue.ToList().Where(x => x.AnnouncementCommandActive == true).ToList();
        }

        public void PlayQueue()
        {
            foreach (AnnouncementCommand ac in Queue)
            {
                if (ac.AnnouncementCommandActive)
                {
                    ac.Play();

                    Thread.Sleep(2000);
                }
            }
        }

        public int ActiveAnnouncementCount()
        {
            return Queue.ToList().Where(x => x.AnnouncementCommandActive == true).ToList().Count();
        }

        public void ClearQueue()
        {
            Queue = new ConcurrentBag<AnnouncementCommand>();
        }

        public void InactivateQueue()
        {
            foreach(AnnouncementCommand ac in Queue)
            {
                ac.AnnouncementCommandActive = false;
            }
        }
    }
}
