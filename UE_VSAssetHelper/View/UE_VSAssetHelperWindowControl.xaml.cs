using System.Windows;
using System.Windows.Controls;
using UE_VSAssetHelper.UE;

namespace UE_VSAssetHelper
{
    public partial class UE_VSAssetHelperWindowControl : UserControl
    {
        private UEController ueController;
        public UE_VSAssetHelperWindowControl()
        {
            this.InitializeComponent();
            ueController = UEController.Get();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var window = UE_VSAssetHelperWindow.instance;
             ueController.SendIDERequestAsync(RequestType.OPEN, window.selectedClassName);
            var resp = ueController.ReceiveIDEResponse();
        }
    }
}