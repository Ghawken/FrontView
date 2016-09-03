using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.SystemProperties
{

    public class Rootobject
    {
        public string LocalAddress { get; set; }
        public string WanAddress { get; set; }
        public string ServerName { get; set; }
        public string Version { get; set; }
        public string OperatingSystem { get; set; }
        public string Id { get; set; }
    }


}
