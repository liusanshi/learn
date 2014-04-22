using System;
using System.IO;
using System.Text;
using System.Threading;
using System.IO.Pipes;

namespace NamePipeServer
{
    class Program
    {
        static int NumThread = 4;

        static void Main(string[] args)
        {
            int i = 0;
            Thread[] server = new Thread[NumThread];

            Console.WriteLine("\n*** 命名管道简单例子 ***\n");
            Console.WriteLine("等待客户端连接...\n");

            for (i = 0; i < NumThread; i++)
            {
                server[i] = new Thread(serverThread);
                server[i].Start();
            }
            
            Thread.Sleep(250);

            while (i > 0)
            {
                for (int j = 0; j < NumThread; j++)
                {
                    if (server[j] != null)
                    {
                        if (server[j].Join(250))
                        {
                            Console.WriteLine("服务线程[{0}] 完成.", server[j].ManagedThreadId);
                            server[j] = null;
                            i--;
                        }
                    }
                }
            }
            Console.WriteLine("\n服务端执行完毕.");
            Console.Read();
        }

        static void serverThread()
        {
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut, NumThread);

            int threadId = Thread.CurrentThread.ManagedThreadId;
            pipeServer.WaitForConnection();//等待连接

            Console.WriteLine("客户端线程id:[{0}].", threadId);
            try
            {
                StreamString s = new StreamString(pipeServer);

                s.WriteString("我是服务");
                string file = s.ReadString();

                ReadFileToStream filereader = new ReadFileToStream(file, s);
                
                Console.WriteLine("Reading file: {0} on thread[{1}] as user: {2}.",
                file, threadId, pipeServer.GetImpersonationUserName());

                pipeServer.RunAsClient(filereader.Start);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }

    public class StreamString
    {
        private Stream iostream;
        private UnicodeEncoding encoding;

        public StreamString(Stream s)
        {
            iostream = s;
            encoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            int len = 0;

            len = iostream.ReadByte() * 256; Console.WriteLine("第一个*256：" + len);
            len += iostream.ReadByte();      Console.WriteLine("第二个：" + len);
            byte[] data = new byte[len];
            iostream.Read(data, 0, len);

            return encoding.GetString(data);
        }

        public int WriteString(string outString)
        {
            byte[] data = encoding.GetBytes(outString);
            int len = data.Length;
            if (len > UInt16.MaxValue) 
            {
                len = (int)UInt16.MaxValue;
            }

            iostream.WriteByte((byte)(len / 256)); Console.WriteLine("总数量：" + len);
            iostream.WriteByte((byte)(len & 255));
            iostream.Write(data, 0, len);

            return data.Length + 2;
        }
    }

    public class ReadFileToStream
    {
        private string file;
        private StreamString ss;

        public ReadFileToStream(string f, StreamString sstring)
        {
            file = f;
            ss = sstring;
        }

        public void Start()
        {
            ss.WriteString(File.ReadAllText(file) + "=======================服务端调用========================");
        }
    }

}
