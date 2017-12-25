using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Attendance.Classes;
using MQTTnet;
using MQTTnet.Client;


namespace Attendance.Forms
{
    public partial class frmServerStatus : Form
    {

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);       
        private static string pcname = Utils.Helper.GetLocalPCName();        
        private static IMqttClient mqtc;


        public frmServerStatus()
        {
            InitializeComponent();            
            
        }


        public  void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.rtxtLoginMessage.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] {  text });
            }
            else
            {
                if (this.rtxtLoginMessage.Lines.Count() > 300)
                    this.rtxtLoginMessage.Text = "";

                rtxtLoginMessage.Select(0, 0);
                rtxtLoginMessage.SelectedText = text + Environment.NewLine;

            }
        }

        private void frmServerStatus_Load(object sender, EventArgs evt)
        {
            Connect();
            lblServer.Text = Globals.G_ServerWorkerIP;
        }


        private void frmServerStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
             mqtc.DisconnectAsync();
            
        }


        private void Connect()
        {
            
            var clientoptions = new MqttClientOptionsBuilder()
            .WithTcpServer(Globals.G_ServerWorkerIP, 1884) // Port is optional
            .Build();

            mqtc = new MqttFactory().CreateMqttClient();
            mqtc.ConnectAsync(clientoptions);

            mqtc.Connected += async (s, evt) =>
            {
                SetText("### CONNECTED WITH SERVER ###" + Environment.NewLine);
                await mqtc.SubscribeAsync(new TopicFilterBuilder().WithTopic("Server/Status").Build());
                SetText("### SUBSCRIBED ###" + Environment.NewLine);
            };

            mqtc.Disconnected += async (s, evtdisconnected) =>
            {
                SetText("### DISCONNECTED FROM SERVER ###" + Environment.NewLine);
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await mqtc.ConnectAsync(clientoptions);
                }
                catch
                {
                    SetText("### RECONNECTING FAILED ### PLEASE CONTACT SERVER ADMINISTRATOR" + Environment.NewLine);

                }
            };

            mqtc.ApplicationMessageReceived += (s, msgreceived) =>
            {
                //SetText("### RECEIVED APPLICATION MESSAGE ###" + Environment.NewLine);
                SetText(string.Format("{0} Received : {1}", pcname, Encoding.UTF8.GetString(msgreceived.ApplicationMessage.Payload)));
                   
            };

        }


    }
}
