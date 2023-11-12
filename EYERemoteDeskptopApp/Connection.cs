using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EYERemoteDeskptopApp
{
    public class Connection
    {
        private ChatRoomForm _form;
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private BinaryWriter _writer;
        private BinaryReader _reader;
        private Thread _thread;

        public bool Connect(ChatRoomForm form, string hostname, int port, string nickname)
        {
            try
            {
                if (nickname.Length < 3)
                {
                    OutputText("Longer Nickname required");
                    return false;
                }

                _form = form;
                _tcpClient = new TcpClient();
                _tcpClient.Connect(hostname, port);
                _stream = _tcpClient.GetStream();
                _writer = new BinaryWriter(_stream, Encoding.UTF8);
                _reader = new BinaryReader(_stream, Encoding.UTF8);

                SetNickName(nickname);

                _thread = new Thread(new ThreadStart(ProcessServerResponse));
                _thread.Start();
            }
            catch (Exception e)
            {
                OutputText("Exception: " + e.Message);
                return false;
            }

            return true;
        }

        private void SetNickName(string nickname)
        {
            NickNamePacket chatMessagePacket = new NickNamePacket(nickname);
            Send(chatMessagePacket);
        }

        public void Disconnect()
        {
            try
            {
                _reader.Close();
                _writer.Close();
                _tcpClient.Close();
                _thread.Abort();
            }
            catch { }

            OutputText("Disconnected");
        }

        public void SendText(string text)
        {
            if (!_tcpClient.Connected)
                return;

            ChatMessagePacket chatMessagePacket = new ChatMessagePacket(text);
            Send(chatMessagePacket);
        }

        public void Send(Packet data)
        {
            MemoryStream mem = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(mem, data);
            byte[] buffer = mem.GetBuffer();

            _writer.Write(buffer.Length);
            _writer.Write(buffer);
            _writer.Flush();
        }
        private void ProcessServerResponse()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Read the number of incoming bytes
                int noOfIncomingBytes;
                while ((noOfIncomingBytes = _reader.ReadInt32()) != 0)
                {
                    // Read the bytes for noOfIncomingBytes amount
                    byte[] bytes = _reader.ReadBytes(noOfIncomingBytes);

                    //Store the bytes in a MemoryStream
                    MemoryStream memoryStream = new MemoryStream(bytes);

                    Packet packet = formatter.Deserialize(memoryStream) as Packet;

                    switch (packet.type)
                    {
                        case PacketType.CHATMESSAGE:
                            string message = ((ChatMessagePacket)packet).message;
                            OutputText(message);
                            break;
                    }
                }
            }
            catch { }
        }

        private delegate void AppendTextDelegate(string str);
        private void OutputText(string text)
        {
            _form.Invoke(new AppendTextDelegate(_form.AppendText), new object[] { text });
        }
    }
}

