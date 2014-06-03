using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PoomsaeBoard
{
    public static class MessageService
    {
        public static Thread Start(
            TcpClient client = null,
            Func<String[], bool> messageHandler = null,
            Func<Exception, bool> timeoutHandler = null,
            Func<bool> disconnectHandler = null,
            int maxMessageSize = 4096
        ) {
            Thread thread = new Thread(new ParameterizedThreadStart(MessageService.receiveMessage));
            thread.Start(new MessageParameter(client, messageHandler, timeoutHandler, disconnectHandler, maxMessageSize));
            return thread;
        }

        private class MessageParameter {
            public TcpClient client;
            public Func<String[], bool> messageHandler;
            public Func<Exception, bool> timeoutHandler;
            public Func<bool> disconnectHandler;
            public int maxMessageSize;

            public MessageParameter(
                TcpClient client,
                Func<String[], bool> messageHandler,
                Func<Exception, bool> timeoutHandler,
                Func<bool> disconnectHandler,
                int maxMessageSize
            ) {
                this.client = client;
                this.messageHandler = messageHandler;
                this.timeoutHandler = timeoutHandler;
                this.disconnectHandler = disconnectHandler;
                this.maxMessageSize = maxMessageSize;
            }
        }

        private static void receiveMessage(Object parameters) {
            // Parse Parameters
            MessageParameter messageParameters = (MessageParameter)parameters;
            TcpClient client = messageParameters.client;
            Func<String[], bool> messageHandler = messageParameters.messageHandler;
            Func<Exception, bool> timeoutHandler = messageParameters.timeoutHandler;
            Func<bool> disconnectHandler = messageParameters.disconnectHandler;
            int maxMessageSize = messageParameters.maxMessageSize;

            // Check Parameters
            if (client == null) return;
            if (messageHandler == null) messageHandler = (String[] s) => { return true; };
            if (timeoutHandler == null) timeoutHandler = (Exception e) => { return true; };
            if (disconnectHandler == null) disconnectHandler = () => { return true; };

            // Initialize Variables
            ASCIIEncoding encoder = new ASCIIEncoding();
            String oldBuffer = "", buffer = "", message = "";
            byte[] packet = new byte[maxMessageSize];
            int packetSize, start, end;

            // Run Loop
            while (true)
            {
                // Check if Client is Connected
                if ((client == null || !client.Connected) && !disconnectHandler()) return;

                // Read Message
                packetSize = 0;
                try { packetSize = client.GetStream().Read(packet, 0, maxMessageSize); }

                // Check for Disconnect
                catch (Exception e) { if (!timeoutHandler(e)) return; }

                //messageHandler(new string[] {"Read " + packetSize + " bytes."});

                // Check for Empty Message
                if (packetSize <= 0)
                {
                    if (!timeoutHandler(null)) return;
                    else continue;
                }

                // Parse Message
                buffer = oldBuffer + encoder.GetString(packet, 0, packetSize);

                start = end = 0;
                while (true)
                {
                    start = buffer.IndexOf("[", end);
                    if (start == -1)
                    {
                        oldBuffer = "";
                        break;
                    }

                    end = buffer.IndexOf("]", start + 1);
                    if (end == -1)
                    {
                        oldBuffer = buffer.Substring(start);
                        break;
                    }

                    message = buffer.Substring(start + 1, end - start - 1);

                    if (!messageHandler(message.Split(','))) return;
                }
            }
        }

        public static bool sendMessage(TcpClient client, params String[] message)
        {
            if (client == null || !client.Connected || client.GetStream() == null || !client.GetStream().CanRead) return false;

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("[" + String.Join(",", message) + "]");

            try
            {
                client.GetStream().Write(buffer, 0, buffer.Length);
                client.GetStream().Flush();
                return true;
            } catch { return false; }
        }

        public static bool sendMessage(TcpClient client, params Object[] message)
        {
            String[] messages = new String[message.Length];
            for (int i = 0; i < message.Length; i++)
                messages[i] = message[i].ToString();
            return sendMessage(client, messages);
        }
    }
}