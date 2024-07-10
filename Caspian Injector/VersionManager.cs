using Caspian;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Caspian_Injector
{
    internal class VersionManager
    {


        static WebClient client = new WebClient();

        public static Dictionary<string, string> versions = new Dictionary<string, string>();

        public static void VersionInit() {

            string json = client.DownloadString(Config.versionURL);

            versions = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
    }
}
