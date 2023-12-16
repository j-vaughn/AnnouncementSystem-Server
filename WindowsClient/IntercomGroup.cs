using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnouncementServer
{
    class IntercomGroup
    {
        public List<string> IntercomNames { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public List<Intercom> Intercoms { get; set; }

        public void FillIntercoms(List<Intercom> intercoms)
        {
            Intercoms = new List<Intercom>();

            foreach (string s in IntercomNames)
            {
                Intercoms.Add(intercoms.Where(t => t.Name == s).FirstOrDefault());
            }
        }
    }
}
