using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class HTTPServer
    {
        private bool isRunning;
        private int numberOfConnections;
        private Socket listener;

        public HTTPServer(int connections, int port)
        {
            this.numberOfConnections = connections;
            try
            {
                if (connections < 1)
                    throw new InvalidOperationException("only positive number of connections is allowed");

                isRunning = false;
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(new IPEndPoint(IPAddress.Any, port));
                listener.Listen(numberOfConnections);
            } catch (Exception e)
            {
                throw e;
            }
        }

        public void Start()
        {
            Thread serverThred = new Thread(new ThreadStart(Run));
            serverThred.Start();
        }

        private void Run()
        {
            isRunning = true;
            while(isRunning)
            {
                //Console.WriteLine("Waiting for connection...");
                Socket client = listener.Accept();
                //Console.WriteLine("Connected");
                HandleClient(client);
                client.Close();
            }
            isRunning = false;
        }

        private void HandleClient(Socket client)
        {
            NetworkStream netStream = new NetworkStream(client);
            StreamReader reader = new StreamReader(netStream);

            String message = "";
            while (reader.Peek() != -1)
            {
                message += reader.ReadLine() + "\n";
            }
            Request req = Request.GetRequest(message);
            Response resp = Response.From(req);
            resp.Post(netStream);
        }
    }
}
