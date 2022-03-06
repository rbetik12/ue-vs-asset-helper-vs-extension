using System.Net;
using System.Threading.Tasks;

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
                ueClient.Connect(IPAddress.Parse("127.0.0.1"), 8080);
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
