using System;
using System.IO;
using System.Text;
using System.Web.Security;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 防伪数据的序列化
    /// </summary>
    internal class AntiForgeryDataSerializer
    {
        /// <summary>
        /// 序列化二进制读取
        /// </summary>
        private sealed class SerializingBinaryReader : BinaryReader
        {
            public SerializingBinaryReader(Stream input)
                : base(input)
            {
            }
            public string ReadBinaryString()
            {
                int num = base.Read7BitEncodedInt();
                byte[] array = this.ReadBytes(num * 2);
                char[] array2 = new char[num];
                for (int i = 0; i < array2.Length; i++)
                {
                    array2[i] = (char)((int)array[2 * i] | (int)array[2 * i + 1] << 8);
                }
                return new string(array2);
            }
            public override string ReadString()
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// 序列化二进制写入
        /// </summary>
        private sealed class SerializingBinaryWriter : BinaryWriter
        {
            public SerializingBinaryWriter(Stream output)
                : base(output)
            {
            }
            public override void Write(string value)
            {
                throw new NotImplementedException();
            }
            public void WriteBinaryString(string value)
            {
                byte[] array = new byte[value.Length * 2];
                for (int i = 0; i < value.Length; i++)
                {
                    char c = value[i];
                    array[2 * i] = (byte)c;
                    array[2 * i + 1] = (byte)(c >> 8);
                }
                base.Write7BitEncodedInt(value.Length);
                this.Write(array);
            }
        }
        /// <summary>
        /// 反序列化 防伪数据
        /// </summary>
        /// <param name="serializedTicket"></param>
        /// <returns></returns>
        internal static AntiForgeryData Deserializer(byte[] serializedTicket)
        {
            AntiForgeryData result;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(serializedTicket))
                {
                    using (SerializingBinaryReader serializingBinaryReader = new SerializingBinaryReader(memoryStream))
                    {
                        byte b = serializingBinaryReader.ReadByte();
                        if (b != 1)
                        {
                            result = null;
                        }
                        else
                        {
                            result = new AntiForgeryData
                            {
                                Salt = serializingBinaryReader.ReadBinaryString(),
                                Value = serializingBinaryReader.ReadBinaryString(),
                                CreationDate = new DateTime(serializingBinaryReader.ReadInt64()),
                                Username = serializingBinaryReader.ReadBinaryString()
                            };
                        }
                    }
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 序列化防伪数据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        internal static byte[] Serializer(AntiForgeryData token)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (SerializingBinaryWriter serializingBinaryWriter = new SerializingBinaryWriter(memoryStream))
                {
                    serializingBinaryWriter.WriteBinaryString(token.Salt);
                    serializingBinaryWriter.WriteBinaryString(token.Value);
                    serializingBinaryWriter.Write(token.CreationDate.Ticks);
                    serializingBinaryWriter.WriteBinaryString(token.Username);
                    result = memoryStream.ToArray();
                }
            }
            return result;
        }

        /// <summary>
        /// 反序列化 防伪数据
        /// </summary>
        /// <param name="serializedToken"></param>
        /// <returns></returns>
        public virtual AntiForgeryData Deserialize(string serializedToken)
        {
            if (string.IsNullOrEmpty(serializedToken))
            {
                throw new ArgumentException("参数不能为空或者null", "serializedToken");
            }
            return new AntiForgeryData(FormsAuthentication.Decrypt(serializedToken));
        }
        /// <summary>
        /// 序列化防伪数据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual string Serialize(AntiForgeryData token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            return FormsAuthentication.Encrypt(token.ConvertToFormsTicket());
        }
    }
}
