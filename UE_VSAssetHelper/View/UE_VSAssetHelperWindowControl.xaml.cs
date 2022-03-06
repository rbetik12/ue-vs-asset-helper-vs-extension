using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using UE_VSAssetHelper.UE;

namespace UE_VSAssetHelper
{
    public partial class UE_VSAssetHelperWindowControl : UserControl
    {
        private UEController ueController;
        private UE_VSAssetHelperWindow window;

        public UE_VSAssetHelperWindowControl()
        {
            InitializeComponent();
            ueController = UEController.Get();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (window == null) 
            {
                window = UE_VSAssetHelperWindow.instance;
            }
            ueController.SendIDERequestAsync(RequestType.OPEN, window.selectedClassName);
            ueController.ReceiveIDEResponse();
        }

        private void button2_Click(object sender, RoutedEventArgs e) 
        {
            if (window == null)
            {
                window = UE_VSAssetHelperWindow.instance;
            }
            ueController.SendIDERequestAsync(RequestType.GET_INFO, window.selectedClassName);
            var resp = ueController.ReceiveIDEResponse();
            var data = JsonConvert.DeserializeObject<BlueprintClassObject>(resp.answerString);
            window.SetAssetInfoText($"Object name: {data.ObjectName}\nClass: {data.ClassName}\nSuper class: {data.SuperClassName}\nUE package: {data.PackageName}");
        }
    }
}