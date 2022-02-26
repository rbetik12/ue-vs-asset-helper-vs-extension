using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

using Task = System.Threading.Tasks.Task;

namespace BlueprintParser
{
    struct IDEResponseHeader
    {
        public String status;
    }

    struct IDEResponse
    {
        public IDEResponseHeader header;
        public String answerString;
    }

    [Guid("4829405a-3a62-4c88-8ba1-8e134e49801f")]
    public class BlueprintParserWindow : ToolWindowPane
    {
        private DTE dte = null;

        public static String className = null;

        public BlueprintParserWindow() : base(null)
        {
            this.Caption = "BlueprintParserWindow";

            this.Content = new BlueprintParserWindowControl();

            ThreadHelper.ThrowIfNotOnUIThread();
            dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as EnvDTE.DTE;
            dte.Events.WindowEvents.WindowActivated += OnWindowActivated;
        }

        private void OnWindowActivated(Window gotFocus, Window lostFocus)
        {
            var content = this.Content as BlueprintParserWindowControl;
            var windowGrid = content.Content as System.Windows.Controls.Grid;
            var stackPanel = windowGrid.Children[0] as System.Windows.Controls.StackPanel;
            var textBlock = stackPanel.Children[0] as System.Windows.Controls.TextBlock;

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
                //Task.Run(() => SendIDERequest(className));
            }
        }
    }
}
