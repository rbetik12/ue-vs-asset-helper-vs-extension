using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

using Task = System.Threading.Tasks.Task;

namespace BlueprintParser
{
    /// <summary>
    /// Interaction logic for BlueprintParserWindowControl.
    /// </summary>
    public partial class BlueprintParserWindowControl : UserControl
    {
        private TcpClient ueClient = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="BlueprintParserWindowControl"/> class.
        /// </summary>
        public BlueprintParserWindowControl()
        {
            this.InitializeComponent();

            ueClient = new TcpClient();
            ueClient.Connect(IPAddress.Parse("127.0.0.1"), 8080);
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => SendIDERequest(BlueprintParserWindow.className));
        }

        private void SendIDERequest(string className)
        {
            var bytes = Encoding.ASCII.GetBytes(className);
            ueClient.GetStream().Write(bytes, 0, bytes.Length);

            byte[] rawSize = new byte[4];
            ueClient.GetStream().Read(rawSize, 0, rawSize.Length);
            int size = BitConverter.ToInt32(rawSize, 0);

            byte[] ansBytes = new byte[size];
            ueClient.GetStream().Read(ansBytes, 0, ansBytes.Length);
            var str = Encoding.UTF8.GetString(ansBytes);
            // str = FilterString(str);
            var json = JsonConvert.DeserializeObject<IDEResponse>(str);
            Console.WriteLine(json.answerString);
        }
    }
}