using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnouncementServer
{
    class Announcement
    {
        public int AnnouncementId { get; set; }
        public string AnnouncementName { get; set; }
        public int LoopTimes { get; set; }
        public int AnnouncementVolumeModifier { get; set; }
        public int Priority { get; set; }
        public List<string> SoundNames { get; set; }
        public List<Sound> Sounds { get; set; }



        public void FillSounds(List<Sound> sounds)
        {
            Sounds = new List<Sound>();

            foreach (string s in SoundNames)
            {
                Sounds.Add(sounds.Where(t => t.SoundName == s).FirstOrDefault());
            }
        }

    }
}
