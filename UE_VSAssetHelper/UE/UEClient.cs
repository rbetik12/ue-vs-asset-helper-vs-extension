using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace UE_VSAssetHelper.UE
{
    class UEClient
    {
        private readonly TcpClient client;

        public UEClient()
        {
            client = new TcpClient();
        }

        public void Connect(IPAddress host, int port)
        {
            client.Connect(host, port);
        }

        public void SendIDERequest(IDERequest request)
        {
            var jsonStr = JsonConvert.SerializeObject(request);
            var bytes = Encoding.ASCII.GetBytes(jsonStr);

            client.GetStream().Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            client.GetStream().Write(bytes, 0, bytes.Length);
        }

        public IDEResponse ReceiveIDEResponse()
        {
            byte[] rawSize = new byte[4];
            client.GetStream().Read(rawSize, 0, rawSize.Length);
            int size = BitConverter.ToInt32(rawSize, 0);

            byte[] ansBytes = new byte[size];
            client.GetStream().Read(ansBytes, 0, ansBytes.Length);
            var str = Encoding.UTF8.GetString(ansBytes);

            var obj = JsonConvert.DeserializeObject<IDEResponse>(str);
            return obj;
        }
    }
}
