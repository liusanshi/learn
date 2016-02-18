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
                if (string.IsNullOrEmpty(mIdentity))
                {
                    mIdentity = CreateIdentity(mUrl);
                }
                return mIdentity;
            }
        }

        /// <summary>
        /// 获取包的所有数据
        /// </summary>
        public List<Tuple<int, string>> Data
        {
            get { return mData; }
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

        internal Packet(string url)
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

                    int Start = msg.IndexOf('|') + 1;
                    int End = msg.LastIndexOf('|');

                    mData.Add(Tuple.Create<int, string>(headerIndex, msg.Substring(Start, End - Start)));
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
                var node = ConvertToTN(item.Item2);
                if (node != null)
                {
                    nodes.Add(node);
                }
            }
        }

        /// <summary>
        /// 是否有数据
        /// </summary>
        /// <returns></returns>
        public bool HasData()
        {
            return mData.Any();
        }

        private TreeNode ConvertToTN(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text)) return null;
                var data = JsonConvert.DeserializeObject(text) as JArray;
                if (data == null) return null;
                DebugInfo debug = new DebugInfo(data.First as JObject);
                TreeNode node = new TreeNode();
                node.Name = debug.Label;
                node.Tag = debug;
                node.Text = debug.Label;

                ConvertToTN(node, data.Last);

                return node;
            }
            catch (Exception ex)
            {
                Util.Log(ex);
                return null;
            }
        }

        private void ConvertToTN(TreeNode node, JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.String:
                    node.Name = token.ToObject<string>();
                    break;
                case JTokenType.Object:
                    var body = token as JObject;
                    foreach (var item in body)
                    {
                        if (item.Value.Type == JTokenType.String || item.Value.Type == JTokenType.Integer)
                        {
                            TreeNode child = new TreeNode(item.Key + " : " + item.Value.ToString());
                            node.Nodes.Add(child);
                        }
                        else
                        {
                            TreeNode child = new TreeNode(item.Key);
                            node.Nodes.Add(child);
                            ConvertToTN(child, item.Value);
                        }
                    }
                    break;
                case JTokenType.Array:
                    JArray jarr = token as JArray;
                    TreeNode array = new TreeNode("array-items");
                    node.Nodes.Add(array);
                    foreach (var item in jarr.AsJEnumerable())
                    {
                        ConvertToTN(array, item);
                    }
                    break;
                default: //不存在
                    break;
            }
        }

        /// <summary>
        /// 获取过滤到的 HTTPHeader
        /// </summary>
        /// <param name="mHTTPHeaders"></param>
        /// <returns></returns>
        public static IEnumerable<HTTPHeaderItem> GetFirePhpHeader(IEnumerable<HTTPHeaderItem> mHTTPHeaders)
        {
            return mHTTPHeaders.Where(p => p.Name.IndexOf(token) > -1);
        }

        /// <summary>
        /// 创建标识
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CreateIdentity(string url)
        {
            //使用 sha1 算法 散列 数据
            var sha1 = new SHA1CryptoServiceProvider();
            return Encoding.UTF8.GetString(sha1.ComputeHash(Encoding.UTF8.GetBytes(url)));
        }

        /// <summary>
        /// 创建标识
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string CreateIdentity(Session session)
        {
            return CreateIdentity(session.url);
        }
    }
}
