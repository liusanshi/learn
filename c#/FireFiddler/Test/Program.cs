using FireFiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            RuleList rl = new RuleList();
            rl.Add(new RegExpRule { Disabled = true, Pattern = "/asdasdadad/" });
            rl.Add(new HostRule { Disabled = false, Host = "youxi.vip.qq.com" });
            rl.Add(new PathRule { Disabled = true, Path = "http://youxi.vip.qq.com/" });

            string filepath = @"d:\123.xml";

            rl.Save(filepath);

            var ret = RuleList.LoadFile(filepath);

            foreach (var item in ret)
            {
                Console.WriteLine("{{Disabled : {0}, value:}}", item.Disabled);
            }

            var data = JsonConvert.DeserializeObject("[{\"Type\":\"LOG\",\"Label\":\"$uin0\",\"File\":\"/data/home/payneliu/svn/isd_qqvipserver_rep/ClubAct_proj/trunk/iyouxi.vip.qq.com/AMS3.0/Modules/Util.php\",\"Line\":\"329\"},\"2621207959\"]") as JArray;


            var info = data.First as JObject;
            DebugInfo debug = new DebugInfo(info);

            var info2 = data.Last;

            Console.WriteLine(data);


            Console.Read();
        }
    }
}
