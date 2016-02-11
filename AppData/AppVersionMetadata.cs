using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateApp.AppData
{
    public sealed class AppVersionMetadata
    {
        public int Id { get; set; }

        public string PublishedTime { get; set; }
        
        public string Version { get; set; }

        public string Name { get; set; }
    }
}
