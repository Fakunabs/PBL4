using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shared;
namespace SimpleServer
{
    public class Client
    {
        public Socket Socket { get; private set; }

        public NetworkStream Stream { get; private set; }
        public BinaryReader Reader { get; private set; }
        public BinaryWriter Writer { get; private set; }
        public string NickName { get; private set; }

        private Thread thread;

        public Client(Socket socket)
        {
            Socket = socket;

            Stream = new NetworkStream(Socket, true);
            Writer = new BinaryWriter(Stream, Encoding.UTF8);
            Reader = new BinaryReader(Stream, Encoding.UTF8);
        }

        public void Start()
        {
            thread = new Thread(new ThreadStart(SocketMethod));
            thread.Start();
        }

        public void Stop(bool abortThread = false)
        {
            Socket.Close();

            if (thread.IsAlive)
                thread.Abort();
        }
        private void SocketMethod()
        {
            SimpleServerC.SocketMethod(this);
        }

        public void SetNickName(string nickName)
        {
            this.NickName = nickName;
        }

        public void SendText(Client fromClient, string text)
        {
            if (!Socket.Connected)
                return;

            string message = "*" + text + "*";
            if (fromClient.NickName != null)
                message = fromClient.NickName + ": " + text;

            ChatMessagePacket chatMessagePacket = new ChatMessagePacket(message);
            Send(chatMessagePacket);
        }

        public void Send(Packet data)
        {
            MemoryStream mem = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(mem, data);
            byte[] buffer = mem.GetBuffer();

            Writer.Write(buffer.Length);
            Writer.Write(buffer);
            Writer.Flush();
        }
    }
}
