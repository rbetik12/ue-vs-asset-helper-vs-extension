using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace UE_VSAssetHelper
{
    [Guid("fa170f54-c8e4-4c39-bca8-7d90f2e23d99")]
    public class UE_VSAssetHelperWindow : ToolWindowPane
    {
        public UE_VSAssetHelperWindow() : base(null)
        {
            Caption = "UE_VSAssetHelperWindow";
            Content = new UE_VSAssetHelperWindowControl();
        }
    }
}
