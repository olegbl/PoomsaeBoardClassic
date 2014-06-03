using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PoomsaeBoard
{
    public class StringServer
    {
        public enum States {
            OFFLINE,
            ONLINE
        }

        private States state;
        public States State {
            get {
                return this.state;
            }
        }

        private List<TcpClient> clients;
        public List<TcpClient> Clients
        {
            get
            {
                return this.clients;
            }
        }

        private TcpListener tcpListener;
        private Thread tcpThread;

        public StringServer()
        {
            this.state = StringServer.States.OFFLINE;
        }

        public void Start(IPAddress ip = null, int port = 3000)
        {
            if (ip == null) ip = IPAddress.Any;

            this.clients = new List<TcpClient>();

            this.tcpListener = new TcpListener(IPAddress.Any, port);

            this.tcpThread = new Thread(new ThreadStart(listen));
            this.tcpThread.Start();

            this.state = StringServer.States.ONLINE;
        }

        public void Stop()
        {
            if (this.tcpThread != null)
                this.tcpThread.Abort();
            this.tcpThread = null;

            if (this.tcpListener != null)
                this.tcpListener.Stop();
            this.tcpListener = null;

            if (this.clients != null)
                foreach (TcpClient client in clients)
                    client.Close();
            this.clients = null;

            this.state = StringServer.States.OFFLINE;
        }

        private void listen()
        {
            this.tcpListener.Start();

            while (true)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient();
                this.clients.Add(client);
                this.OnClientConnected(new ClientConnectedEventArgs(client));
            }
        }

        // Events
        public delegate void ClientConnectedEventHandler(object sender, ClientConnectedEventArgs e);
        public event ClientConnectedEventHandler ClientConnected;
        public class ClientConnectedEventArgs : EventArgs
        {
            private TcpClient client;
            public TcpClient Client
            {
                get
                {
                    return this.client;
                }
            }

            public ClientConnectedEventArgs()
            {
                this.client = null;
            }

            public ClientConnectedEventArgs(TcpClient client)
            {
                this.client = client;
            }
        }
        protected virtual void OnClientConnected(ClientConnectedEventArgs e)
        {
            if (ClientConnected != null)
                ClientConnected(this, e);
        }
    }
}
