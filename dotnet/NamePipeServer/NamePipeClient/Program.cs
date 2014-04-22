using System;
using System.Text;
using System.IO;
using System.Threading;
using System.IO.Pipes;
using System.Diagnostics;

namespace NamePipeClient
{
    class Program
    {
        static int numClient = 4;

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "spawnclient")
                {
                    NamedPipeClientStream pipeClient = new NamedPipeClientStream("127.0.0.1", "testpipe", PipeDirection.InOut,
                        PipeOptions.None, System.Security.Principal.TokenImpersonationLevel.Impersonation);

                    Console.WriteLine("连接服务端...");
                    pipeClient.Connect();

                    StreamString ss = new StreamString(pipeClient);

                    if (ss.ReadString() == "我是服务")
                    {
                        ss.WriteString(@"c:\textfile.txt");

                        Console.WriteLine(ss.ReadString());
                    }
                    else
                    {
                        Console.WriteLine("服务没有通过验证");
                    }

                    pipeClient.Close();
                    Thread.Sleep(4000);
                }
            }
            else
            {
                Console.WriteLine("\n*** Named pipe client stream with impersonation example ***\n");
                StartClients();
            }
        }

        static void StartClients()
        {
            int i;
            string currentProcessName = Environment.CommandLine;

            Process[] plist = new Process[numClient];

            Console.WriteLine("Spawning client processes...");

            if (currentProcessName.Contains(Environment.CurrentDirectory))
            {
                currentProcessName = currentProcessName.Replace(Environment.CurrentDirectory, string.Empty);
            }

            currentProcessName = currentProcessName.Replace("\\", "").Replace("\"", "");

            for (i = 0; i < numClient; i++)
            {
                plist[i] = Process.Start(currentProcessName, "spawnclient");
            }
            while (i > 0)
            {
                for (int j = 0; j < numClient; j++)
                {
                    if (plist[j] != null)
                    {
                        if (plist[j].HasExited)
                        {
                            Console.WriteLine("子进程：{0}已经退出", plist[j].Id);
                            plist[j] = null;
                            i--;
                        }
                        else
                        {
                            Thread.Sleep(250);
                        }
                    }
                }
            }
            Console.WriteLine("\nClient processes finished, exiting.");
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

            len = iostream.ReadByte() * 256;
            len += iostream.ReadByte();
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

            iostream.WriteByte((byte)(len / 256));
            iostream.WriteByte((byte)(len & 255));
            iostream.Write(data, 0, len);

            return data.Length + 2;
        }
    }
}
