using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PoomsaeBoard
{
    public class Judge
    {
        delegate void voidDelegate();
        delegate void StringDelegate(String s);
        delegate void intStringDelegate(int i, String s);

        private int id = 0;
        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
                this.setValue(0, this.id.ToString());
            }
        }

        public String Status
        {
            get
            {
                return this.getValue(1);
            }
            set
            {
                this.setValue(1, value);
            }
        }

        public String Technical1
        {
            get
            {
                double temp = 0.0;
                Double.TryParse(this.getValue(2), out temp);
                return String.Format("{0:0.0}", temp);
            }
            set
            {
                double temp = 0.0;
                Double.TryParse(value, out temp);
                this.setValue(2, String.Format("{0:0.0}", temp));

            }
        }

        public String Presentation1
        {
            get
            {
                double temp = 0.0;
                Double.TryParse(this.getValue(3), out temp);
                return String.Format("{0:0.0}", temp);
            }
            set
            {
                double temp = 0.0;
                Double.TryParse(value, out temp);
                this.setValue(3, String.Format("{0:0.0}", temp));

            }
        }

        public String Technical2
        {
            get
            {
                double temp = 0.0;
                Double.TryParse(this.getValue(4), out temp);
                return String.Format("{0:0.0}", temp);
            }
            set
            {
                double temp = 0.0;
                Double.TryParse(value, out temp);
                this.setValue(4, String.Format("{0:0.0}", temp));

            }
        }

        public String Presentation2
        {
            get
            {
                double temp = 0.0;
                Double.TryParse(this.getValue(5), out temp);
                return String.Format("{0:0.0}", temp);
            }
            set
            {
                double temp = 0.0;
                Double.TryParse(value, out temp);
                this.setValue(5, String.Format("{0:0.0}", temp));

            }
        }

        public String Passphrase
        {
            get
            {
                return this.getValue(6);
            }
            set
            {
                this.setValue(6, value);
            }
        }

        private bool connected = false;
        public bool Connected
        {
            get
            {
                return this.connected;
            }
        }

        private RingControllerForm ring = null;
        private DataGridViewRow row;

        private TcpClient client = null;
        public TcpClient Client
        {
            get
            {
                return this.client;
            }
            set
            {
                this.connected = true;
                this.client = value;
                this.register();
                MessageService.Start(this.client, this.messageHandler, this.timeoutHandler, this.disconnectHandler);
                MessageService.sendMessage(this.client, "registered", this.id.ToString());
                this.setRules(this.ring.getRuleSet());
                MessageService.sendMessage(this.client, "clear");
            }
        }

        public Judge(RingControllerForm ring, DataGridViewRow row, int id)
        {
            this.ring = ring;
            this.row = row;
            this.id = id;

            this.row.Cells[1].Style.ForeColor = Color.Red;
        }
        
        private void register()
        {
            if (this.row.DataGridView.InvokeRequired)
            {
                this.row.DataGridView.Invoke(new voidDelegate(register));
                return;
            }

            this.connected = true;
            this.row.Cells[1].Value = "Connected";
            this.row.Cells[1].Style.ForeColor = Color.Green;
            this.row.Cells[6].ReadOnly = true;
        }

        private void unregister()
        {
            if (this.row.DataGridView.InvokeRequired)
            {
                this.row.DataGridView.Invoke(new voidDelegate(unregister));
                return;
            }

            this.client = null;
            this.connected = false;

            this.row.Cells[1].Value = "Offline";
            this.row.Cells[1].Style.ForeColor = Color.Red;
            this.row.Cells[6].ReadOnly = false;
        }

        public String getValue(int index)
        {
            return (String)this.row.Cells[index].Value;
        }

        public void setValue(int cell, String value)
        {
            if (this.row.DataGridView.InvokeRequired)
            {
                this.row.DataGridView.Invoke(new intStringDelegate(setValue), new Object[] { cell, value });
                return;
            }

            this.row.Cells[cell].Value = value;
        }

        public void setRules(Ruleset rules)
        {
            int length = 5 + (rules.presentations.Length) * 4;
            String[] args = new String[length];

            args[0] = "rules";
            args[1] = rules.technicalMin;
            args[2] = rules.technicalMax;
            args[3] = rules.technicalMinor;
            args[4] = rules.technicalMajor;

            for (int i = 0; i < rules.presentations.Length; i++)
            {
                args[5 + i * 4 + 0] = rules.presentations[i].name;
                args[5 + i * 4 + 1] = rules.presentations[i].min;
                args[5 + i * 4 + 2] = rules.presentations[i].max;
                args[5 + i * 4 + 3] = rules.presentations[i].step;
            }

            MessageService.sendMessage(this.client, args);
        }

        private bool messageHandler(String[] message)
        {
            //MessageBox.Show(String.Join(", ", message));

            if (message.Length == 2 && message[0].Equals("technical"))
            {
                switch (this.ring.PoomsaeNumber)
                {
                    case 1:
                        this.Technical1 = message[1];
                        break;
                    case 2:
                        this.Technical2 = message[1];
                        break;
                }
            }
            else if (message.Length == 2 && message[0].Equals("presentation"))
            {
                switch (this.ring.PoomsaeNumber)
                {
                    case 1:
                        this.Presentation1 = message[1];
                        break;
                    case 2:
                        this.Presentation2 = message[1];
                        break;
                }
            }
            else if (message.Length == 2 && message[0].Equals("query") && message[1].Equals("ring"))
            {
                MessageService.sendMessage(this.client, "ring", this.ring.RingNumber);
            }
            else if (message.Length == 2 && message[0].StartsWith("query") && message[1].Equals("name"))
            {
                MessageService.sendMessage(this.client, "name", this.ring.AthleteName);
            }
            else if (message.Length == 2 && message[0].StartsWith("query") && message[1].Equals("poomsae"))
            {
                MessageService.sendMessage(this.client, "poomsae", this.ring.PoomsaeNumber);
            }
            else if (message.Length == 2 && message[0].StartsWith("query") && message[1].Equals("rules"))
            {
                this.setRules(this.ring.getRuleSet());
            }

            return true;
        }

        private bool timeoutHandler(Exception e)
        {
            unregister();
            return false;
        }

        private bool disconnectHandler()
        {
            unregister();
            return false;
        }
    }
}
