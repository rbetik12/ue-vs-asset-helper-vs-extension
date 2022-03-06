using System;
using System.Windows;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

namespace UE_VSAssetHelper
{
    enum RequestType 
    {
        GET_INFO,
        OPEN
    }

    enum ResponseStatus 
    {
        OK,
        ERROR
    }

    struct IDEResponse
    {
        public ResponseStatus status;
        public String answerString;
    }

    struct IDERequest
    {
        public RequestType type;
        public String data;
    }
    struct BlueprintClassObject 
    {
        public int Index;
        public String ObjectName;
        public String ClassName;
        public String SuperClassName;
        public String PackageName;
    }

    public partial class UE_VSAssetHelperWindowControl : UserControl
    {
        private TcpClient ueClient = null;

        public UE_VSAssetHelperWindowControl()
        {
            this.InitializeComponent();

            ueClient = new TcpClient();
            ueClient.Connect(IPAddress.Parse("127.0.0.1"), 8080);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => 
            {
                var request = new IDERequest();
                request.type = RequestType.GET_INFO;
                request.data = UE_VSAssetHelperWindow.className;

                SendIDERequest(request);
                var response = ReceiveIDEResponse();
                switch (request.type)
                {
                    case RequestType.OPEN:
                        break;
                    case RequestType.GET_INFO:
                        var obj = JsonConvert.DeserializeObject<BlueprintClassObject>(response.answerString);
                        Dispatcher.Invoke(() => {
                            UE_VSAssetHelperWindow.infoBlock.Text = $"Object Name: {obj.ObjectName}\nClass: {obj.ClassName}\nSuper class:{obj.SuperClassName}\nPackage name:{obj.PackageName}";
                        });
                        break;
                    default:
                        break;
                }
            });
        }

        private IDEResponse ReceiveIDEResponse() 
        {
            byte[] rawSize = new byte[4];
            ueClient.GetStream().Read(rawSize, 0, rawSize.Length);
            int size = BitConverter.ToInt32(rawSize, 0);

            byte[] ansBytes = new byte[size];
            ueClient.GetStream().Read(ansBytes, 0, ansBytes.Length);
            var str = Encoding.UTF8.GetString(ansBytes);

            var obj = JsonConvert.DeserializeObject<IDEResponse>(str);
            return obj;
        }

        private void SendIDERequest(IDERequest request)
        {
            var jsonStr = JsonConvert.SerializeObject(request);
            var bytes = Encoding.ASCII.GetBytes(jsonStr);

            ueClient.GetStream().Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            ueClient.GetStream().Write(bytes, 0, bytes.Length);
        }
    }
}