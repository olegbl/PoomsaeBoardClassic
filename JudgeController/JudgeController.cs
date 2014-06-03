using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PoomsaeBoard
{
    public partial class JudgeControllerForm : Form
    {
        public static IPAddress GetIPAddress()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Environment.MachineName);
            foreach (IPAddress address in hostEntry.AddressList)
                if (address.AddressFamily == AddressFamily.InterNetwork)
                    return address;
            return null;
        }

        protected TcpClient client = null;
        protected int id = 0;

        public JudgeControllerForm()
        {
            InitializeComponent();

            this.poomsae.Title = "Poomsae Scoring";

            this.connectMode();
        }

        protected void clearScore()
        {
            this.poomsae.clearScore();
        }

        // Connection Initiation

        protected void connect()
        {
            Int32 port = 0;
            IPAddress ip = null;
            String host = host_textbox.Text;

            if (!Int32.TryParse(port_textbox.Text, out port))
            {
                MessageBox.Show("Invalid port specified!\nA port should be an integer between " + UInt16.MinValue + " and " + UInt16.MaxValue + "."); 
                return;
            }

            IPAddress.TryParse(host, out ip);

            foreach (IPAddress a in Dns.GetHostAddresses(host))
                if (a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    ip = a;

            client = new TcpClient();
            try
            {
                if (ip == null) client.Connect(host, port);
                else client.Connect(ip, port);
            }
            catch (Exception error)
            {
                MessageBox.Show("Failed to connect to the server!\n" + error.Message);
                return;
            }

            MessageService.Start(this.client, this.messageHandler, this.timeoutHandler, this.disconnectHandler);
            MessageService.sendMessage(this.client, "register", passphrase_textbox.Text);

            connect_button.Enabled = false;
        }

        protected delegate void connectionDelegate();

        protected void connectMode()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new connectionDelegate(connectMode));
                return;
            }

            connect_button.Enabled = true;

            connect_control.Enabled = true;
            info_control.Hide();
            poomsae.Hide();

            this.Width = 296;
            this.Height = 159;
        }
        
        protected void scoreMode()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new connectionDelegate(scoreMode));
                return;
            }

            this.connect_control.Enabled = false;
            this.info_control.Show();
            this.poomsae.Show();

            this.Width = 841;
            this.Height = 408;

            this.judge_textbox.Text = "Judge # " + this.id;

            this.clearScore();

            MessageService.sendMessage(this.client, "query", "ring");
            MessageService.sendMessage(this.client, "query", "name");
            MessageService.sendMessage(this.client, "query", "poomsae");
        }
        
        // Connection Event Handlers

        protected bool messageHandler(String[] message)
        {
            if (message.Length == 1 && message[0].Equals("clear"))
            {
                this.setTextBoxText(name_textbox, "");
                this.clearScore();
            }
            else if (message.Length == 2 && message[0].Equals("registered"))
            {
                Int32.TryParse(message[1], out this.id);
                this.scoreMode();
            }
            else if (message.Length == 2 && message[0].Equals("rejected"))
            {
                MessageBox.Show("Failed to register with server.\n" + message[1]);
                this.connect_button.Enabled = true;
                return false;
            }
            else if (message.Length == 2 && message[0].Equals("name"))
            {
                this.setTextBoxText(name_textbox, "Athlete: " + message[1]);
            }
            else if (message.Length == 2 && message[0].Equals("ring"))
            {
                this.setTextBoxText(ring_textbox, "Ring # " + message[1]);
            }
            else if (message.Length == 2 && message[0].Equals("poomsae"))
            {
                this.setTextBoxText(poomsae_textbox, "Poomsae # " + message[1]);
            }
            else if (message.Length >= 5 && message[0].Equals("rules"))
            {
                this.poomsae.updateScales(message);
            }

            return true;
        }

        protected bool timeoutHandler(Exception e)
        {
            return true;
        }

        protected bool disconnectHandler()
        {
            connectMode();
            return false;
        }

        // Thread-Safe UI Operations
        delegate void setTextBoxTextDelegate(TextBox box, String text);
        protected void setTextBoxText(TextBox box, String text)
        { if (box.InvokeRequired) box.Invoke(new setTextBoxTextDelegate(setTextBoxText), new Object[] { box, text }); else box.Text = text; }

        // UI Event Handlers
        protected void onFormLoad(object sender, EventArgs e)
        {
            host_textbox.Text = GetIPAddress().ToString();
            port_textbox.Text = "3016";
        }

        protected void onFormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Environment.Exit(0);
                Application.Exit();
            }
            catch { }
        }

        protected void onConnectClick(object sender, EventArgs e)
        {
            this.connect();
        }

        private void onPoomsaeScoreChanged(object sender, EventArgs e)
        {
            MessageService.sendMessage(this.client, "technical", poomsae.Technical.String);
            MessageService.sendMessage(this.client, "presentation", poomsae.Presentation.String);
        }
    }
}
