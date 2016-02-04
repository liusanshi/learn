using FireFiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //RuleList rl = new RuleList();

            //rl.Disabled = true;


            //rl.Add(new RegExpRule { Disabled = true, Pattern = "/asdasdadad/" });
            //rl.Add(new HostRule { Disabled = false, Host = "youxi.vip.qq.com" });
            //rl.Add(new PathRule { Disabled = true, Path = "http://youxi.vip.qq.com/" });

            //string filepath = @"d:\123.xml";

            //rl.Save(filepath);

            //var ret = RuleList.LoadFile(filepath);

            //foreach (var item in ret)
            //{
            //    Console.WriteLine("{{Disabled : {0}, value:}}", item.Disabled);
            //}

            //var data = JsonConvert.DeserializeObject("[{\"Type\":\"LOG\",\"Label\":\"$uin0\",\"File\":\"/data/home/payneliu/svn/isd_qqvipserver_rep/ClubAct_proj/trunk/iyouxi.vip.qq.com/AMS3.0/Modules/Util.php\",\"Line\":\"329\"},\"2621207959\"]") as JArray;


            //var info = data.First as JObject;
            //DebugInfo debug = new DebugInfo(info);

            //var info2 = data.Last;

            //Console.WriteLine(data);


            FixedLengthDictionary<string, string> fldic = new FixedLengthDictionary<string, string>(2);

            fldic.Add("1", "2");
            fldic.Add("2", "2");
            fldic.Add("3", "2");
            fldic.Add("4", "2");

            fldic["1"] = "2";

            Console.ReadLine();
            //Console.ReadKey();

            
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

        }
    }
}
