using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnouncementServer
{
    class ClientConfig
    {
        public List<Intercom> Intercoms { get; set; }
        public List<Sound> Sounds { get; set; }
        public List<Announcement> Announcements { get; set; }
        public List<IntercomGroup> IntercomGroups { get; set; }
        public string WebKey { get; set; }
        public int WebPort { get; set; }

        

        public List<Intercom> GetIntercomsFromNames(List<String> intercomNames)
        {
            List<Intercom> r = new List<Intercom>();

            foreach (string s in intercomNames)
            {
                r.Add(Intercoms.Where(t => t.Name == s).FirstOrDefault());
            }

            return r;
        }
    }
}
