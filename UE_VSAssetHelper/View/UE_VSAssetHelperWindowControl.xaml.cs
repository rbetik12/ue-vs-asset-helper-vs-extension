using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json;
using UE_VSAssetHelper.UE;
using EnvDTE;
using Window = EnvDTE.Window;
using System.Net.Sockets;

namespace UE_VSAssetHelper
{
    public partial class UE_VSAssetHelperWindowControl : UserControl
    {
        private UEController ueController;
        private DTE dte;
        private TextBlock fileNameTextBlock;
        private TextBlock assetInfoTextBlock;
        private string selectedClassName;

        public UE_VSAssetHelperWindowControl()
        {
            InitializeComponent();
            ueController = UEController.Get();

            ThreadHelper.ThrowIfNotOnUIThread();
            dte = Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as DTE;
            dte.Events.WindowEvents.WindowActivated += OnWindowActivate;

            fileNameTextBlock = FindName("Filename") as TextBlock;
            assetInfoTextBlock = FindName("Props") as TextBlock;
        }

        private string GetActiveClassName(string documentName) 
        {
            fileNameTextBlock.Text = documentName;
            var splittedName = documentName.Split('\\');
            var className = splittedName[splittedName.Length - 1];
            className = className.Remove(className.IndexOf("."), className.Length - className.IndexOf("."));
            return className;
        }

        private void OnWindowActivate(Window gotFocus, Window lostFocus)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (dte.ActiveDocument != null)
            {
                var documentName = dte.ActiveDocument.FullName;
                selectedClassName = GetActiveClassName(documentName);

                // TODO: Use this code for retrieving fields in C++ classes
                var codeModel = dte.ActiveDocument.ProjectItem.FileCodeModel;
                FileCodeModel fcm =
                    dte.ActiveDocument.ProjectItem.FileCodeModel;

                string children = "";
                foreach (CodeElement elem in fcm.CodeElements)
                {
                    children += elem.Name + Environment.NewLine;
                    foreach (CodeElement child in elem.Children)
                    {
                        children += child.Name + Environment.NewLine;
                    }
                }
                Console.WriteLine(children);
            }
        }

        private string GetAssetInfoText(BlueprintClassObject data)
        {
            var assetInfoText = $"Object name: {data.ObjectName}\nClass: {data.ClassName}\nSuper class: {data.SuperClassName}\nUE package: {data.PackageName}\n";
            var sb = new StringBuilder();
            sb.Append(assetInfoText);
            sb.Append("Properties:\n");
            foreach (KeyValuePair<string, string> entry in data.Properties)
            {
                sb.Append($"{entry.Key}: {entry.Value}\n");
            }

            return sb.ToString();
        }

        private void OpenInEditor(object sender, RoutedEventArgs e)
        {
            try
            {
                ueController.SendIDERequestAsync(RequestType.OPEN, selectedClassName);
                ueController.ReceiveIDEResponse();
            }
            catch (UEPluginNotAvailableException ex)
            {
                OpenInEditor(sender, e);
                return;
            }
            catch (UEPluginConnectionClosedException ex) 
            {
                return;
            }
        }

        private void GetAssetProperties(object sender, RoutedEventArgs e) 
        {
            try
            {
                ueController.SendIDERequestAsync(RequestType.GET_INFO, selectedClassName);
                var resp = ueController.ReceiveIDEResponse();
                if (resp.status != ResponseStatus.OK)
                {
                    return;
                }
                var data = JsonConvert.DeserializeObject<BlueprintClassObject>(resp.answerString);
                assetInfoTextBlock.Text = GetAssetInfoText(data);
            }
            catch (UEPluginNotAvailableException ex)
            {
                GetAssetProperties(sender, e);
                return;
            }
            catch (UEPluginConnectionClosedException ex)
            {
                return;
            }
        }
    }
}
