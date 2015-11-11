using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;

using Fiddler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace FireFiddler
{
    /*
     * X-Wf-1-1-1-1: 260|[{"Type":"LOG","File":"\/data\/web\/svnfiles\/web\/iyouxi.vip.qq.com\/AMS3.0\/Controllers\/Oz.php","Line":200},{"cmd":"report","uin":2621207959,"actid":35071,"subactid":1,"from":"ui.ptlogin2.qq.com\/cgi-bin\/login","fromid":0,"op":1,"num":1,"domain_id":"201"}]|
     */

    /// <summary>
    /// 包信息
    /// </summary>
    public class Packet
    {
        private IEnumerable<HTTPHeaderItem> mHTTPHeaders;
        /// <summary>
        /// 存储的是所有数据
        /// </summary>
        private List<Tuple<int, string>> mData = new List<Tuple<int, string>>();
        private const string token = "X-Wf-1-1-1-";
        private string mIdentity = "";

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Identity
        {
            get
            {
                if (!string.IsNullOrEmpty(mIdentity))
                {
                    //使用 sha1 算法 散列 数据
                    var sha1 = new SHA1CryptoServiceProvider();
                    mIdentity = Encoding.UTF8.GetString(sha1.ComputeHash(Encoding.UTF8.GetBytes(mUrl)));
                }
                return mIdentity;
            }
        }

        /// <summary>
        /// 包的url
        /// </summary>
        public string Url { get { return mUrl; } }
        private string mUrl;

        public Packet(string url, IEnumerable<HTTPHeaderItem> headeritems)
        {
            mHTTPHeaders = headeritems;
            mUrl = url;
            UnPacket();
        }

        private Packet(string url)
        {
            mUrl = url;
        }

        private void UnPacket()
        {
            string msg = "";
            foreach (var item in mHTTPHeaders.Where(p => p.Name.IndexOf(token) > -1))
            {
                msg += item.Value;
                var end = !msg.Trim().EndsWith("\\");

                if (end)
                {
                    int headerIndex;
                    int.TryParse(item.Name.Substring(token.Length), out headerIndex);

                    mData.Add(Tuple.Create<int, string>(headerIndex, msg));
                    msg = "";
                }
            }
            mData = mData.OrderBy(p => p.Item1).ToList();
        }

        /// <summary>
        /// 呈现treeview
        /// </summary>
        public void Render(TreeNodeCollection nodes)
        {
            foreach (var item in mData)
            {
                nodes.Add(ConvertToTN(item.Item2));
            }
        }


        private TreeNode ConvertToTN(string text)
        {
            var data = JsonConvert.DeserializeObject(text) as JArray;
            DebugInfo debug = new DebugInfo(data.First as JObject);
            TreeNode node = new TreeNode();
            node.Name = debug.Label;
            node.Tag = debug;

            node.Nodes.Add(ConvertToTN(data.Last));

            return node;
        }

        private TreeNode ConvertToTN(JToken token)
        {
            TreeNode node = new TreeNode();
            switch (token.Type)
            {
                case JTokenType.Array: //不支持
                    break;
                case JTokenType.String:
                    node.Name = token.ToObject<string>();
                    break;
                case JTokenType.Object:
                    var body = token as JObject;
                    foreach (var item in body)
                    {
                        TreeNode child = new TreeNode(item.Key);
                        node.Nodes.Add(child);
                        child.Nodes.Add(ConvertToTN(item.Value));
                    }
                    break;
                default: //不存在
                    break;
            }
            return node;
        }

        /// <summary>
        /// 保存包
        /// </summary>
        public void Save()
        {
            StringBuilder content = new StringBuilder(100);
            foreach (var item in mData)
            {
                content.AppendLine(item.Item2);
            }

            File.WriteAllText(GetFilePath(), content.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// 从文件中加载 包
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Packet LoadFile(string url)
        {
            Packet pk = new Packet(url);

            string[] content = File.ReadAllLines(pk.GetFilePath(), Encoding.UTF8);
            int index = 0;

            foreach (var item in content)
            {
                pk.mData.Add(Tuple.Create(index, item));
            }

            return null;
        }

        private string GetFilePath()
        {
            return Path.Combine(Util.LogPath, Identity + ".txt");
        }

        /// <summary>
        /// 获取过滤到的 HTTPHeader
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HTTPHeaderItem> GetFilterHeader()
        {
            return mHTTPHeaders.Where(p => p.Name.IndexOf(token) > -1);
        }
    }
}
