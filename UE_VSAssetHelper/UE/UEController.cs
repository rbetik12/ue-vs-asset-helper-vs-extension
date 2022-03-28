using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UE_VSAssetHelper.UE
{
    class UEController
    {
        private UEClient ueClient;
        private bool isConnected;
        private static UEController instance;

        public static UEController Get() 
        {
            if (instance == null) 
            { 
                instance = new UEController();
            }
            return instance;
        }

        public Task SendIDERequestAsync(RequestType type, string data)
        { 
            if (!isConnected)
            {
                try
                {
                    ueClient.Connect(IPAddress.Parse("127.0.0.1"), 8080);
                }
                catch (SocketException exception)
                {
                    var result = MessageBox.Show("Can't connect to Unreal Engine plugin. Check if editor with enabled plugin is running.",
                                    "Connection error",
                                     MessageBoxButtons.RetryCancel,
                                     MessageBoxIcon.Error);

                    if (result == DialogResult.Retry)
                    {
                        throw new UEPluginNotAvailableException();
                    }
                    else
                    {
                        throw new UEPluginConnectionClosedException();
                    }
                }
                isConnected = true;
            }

            IDERequest request = new IDERequest();
            request.type = type;
            request.data = data;

            return Task.Run(() => 
            {
                ueClient.SendIDERequest(request); 
            });
        }

        public IDEResponse ReceiveIDEResponse() 
        {
            return ueClient.ReceiveIDEResponse();
        }

        private UEController() 
        {
            isConnected = false;
            ueClient = new UEClient();
        }
    }
}
