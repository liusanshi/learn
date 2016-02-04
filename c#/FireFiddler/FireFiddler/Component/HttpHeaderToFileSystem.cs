using System;
using System.IO;
using System.Text;

namespace FireFiddler.Component
{
    /// <summary>
    /// http头保存到文件系统
    /// </summary>
    public class HttpHeaderToFileSystem : IHttpHeaderPersistence
    {
        public void Save(Packet packet)
        {
            StringBuilder content = new StringBuilder(100);
            foreach (var item in packet.Data)
            {
                content.AppendLine(item.Item2);
            }

            File.WriteAllText(GetFilePath(packet.Identity), content.ToString(), Encoding.UTF8);
        }

        public Packet Load(string url)
        {
            Packet pk = new Packet(url);

            string[] content = File.ReadAllLines(this.GetFilePath(pk.Identity), Encoding.UTF8);
            int index = 0;

            foreach (var item in content)
            {
                pk.Data.Add(Tuple.Create(index, item));
            }

            return pk;
        }

        public void Clear()
        {
            try
            {
                foreach (var item in Directory.GetFiles(Util.LogPath, "*.txt"))
                {
                    File.Delete(item);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获取文件存放的地址
        /// </summary>
        /// <returns></returns>
        private string GetFilePath(string identity)
        {
            return Path.Combine(Util.LogPath, identity + ".txt");
        }
    }
}
