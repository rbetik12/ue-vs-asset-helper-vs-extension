using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace UE_VSAssetHelper
{
    [Guid("fa170f54-c8e4-4c39-bca8-7d90f2e23d99")]
    public class UE_VSAssetHelperWindow : ToolWindowPane
    {
        private DTE dte = null;

        public static String className = null;
        public static System.Windows.Controls.TextBlock infoBlock = null;
        public UE_VSAssetHelperWindow() : base(null)
        {
            this.Caption = "UE_VSAssetHelperWindow";
            this.Content = new UE_VSAssetHelperWindowControl();

            ThreadHelper.ThrowIfNotOnUIThread();
            dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as EnvDTE.DTE;
            dte.Events.WindowEvents.WindowActivated += OnWindowActivated;
        }

        private void OnWindowActivated(Window gotFocus, Window lostFocus)
        {
            var content = this.Content as UE_VSAssetHelperWindowControl;
            var windowGrid = content.Content as System.Windows.Controls.Grid;
            var stackPanel = windowGrid.Children[0] as System.Windows.Controls.StackPanel;
            var textBlock = stackPanel.Children[0] as System.Windows.Controls.TextBlock;
            var infoTextBlock = stackPanel.Children[2] as System.Windows.Controls.TextBlock;

            if (infoBlock == null)
            {
                infoBlock = infoTextBlock;
            }

            if (dte.ActiveDocument != null)
            {
                var documentName = dte.ActiveDocument.FullName;
                textBlock.Text = documentName;
                var splittedName = documentName.Split('\\');
                //var codeModel = dte.ActiveDocument.ProjectItem.FileCodeModel;
                //FileCodeModel fcm =
                //    dte.ActiveDocument.ProjectItem.FileCodeModel;

                //string children = "";
                //foreach (CodeElement elem in fcm.CodeElements)
                //{
                //    children += elem.Name + Environment.NewLine;
                //}           
                className = splittedName[splittedName.Length - 1];
                className = className.Remove(className.IndexOf("."), className.Length - className.IndexOf("."));
            }
        }
    }
}
