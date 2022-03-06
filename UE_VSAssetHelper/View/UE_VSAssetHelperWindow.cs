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
        public string selectedClassName { get; private set; }
        public static UE_VSAssetHelperWindow instance { get; private set; }
        private DTE dte;
        private TextBlock fileNameTextBlock;
        private TextBlock assetInfoTextBlock;

        public UE_VSAssetHelperWindow() : base(null)
        {
            Caption = "UE_VSAssetHelperWindow";
            Content = new UE_VSAssetHelperWindowControl();

            ThreadHelper.ThrowIfNotOnUIThread();
            dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as EnvDTE.DTE;
            dte.Events.WindowEvents.WindowActivated += OnWindowActivated;
        }

        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();
            instance = this;

            var content = Content as UE_VSAssetHelperWindowControl;
            var windowGrid = content.Content as Grid;
            var stackPanel = windowGrid.Children[0] as StackPanel;
            fileNameTextBlock = stackPanel.Children[0] as TextBlock;
            assetInfoTextBlock = stackPanel.Children[2] as TextBlock;
        }

        public void SetAssetInfoText(string info)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            assetInfoTextBlock.Text = info;
        }

        private void OnWindowActivated(Window gotFocus, Window lostFocus)
        {
            if (dte.ActiveDocument != null)
            {
                var documentName = dte.ActiveDocument.FullName;
                fileNameTextBlock.Text = documentName;
                var splittedName = documentName.Split('\\');
                var className = splittedName[splittedName.Length - 1];
                className = className.Remove(className.IndexOf("."), className.Length - className.IndexOf("."));
                selectedClassName = className;

                // TODO: Use this code for retrieving fields in C++ classes
                /*
                var codeModel = dte.ActiveDocument.ProjectItem.FileCodeModel;
                FileCodeModel fcm =
                    dte.ActiveDocument.ProjectItem.FileCodeModel;

                string children = "";
                foreach (CodeElement elem in fcm.CodeElements)
                {
                    children += elem.Name + Environment.NewLine;
                } 
                */
            }
        }
    }
}
