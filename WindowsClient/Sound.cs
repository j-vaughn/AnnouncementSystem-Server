using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnnouncementServer
{
    class Sound
    {
        public int SoundId { get; set; }
        public string FileName { get; set; }
        public string SoundName { get; set; }
        public int SoundVolumeModifier { get; set; }
        public int PlayTime { get; set; }
    }
}
