using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace server_app
{
    class Server
    {
        //initialize variables 
        private TcpListener _server;
        private Boolean _running;
        private int counter;
        private List<TcpClient> clientList;

        //constructor creates a tcplistener obj on localhost and port 8888
        //puts obj in waiting mode
        public Server()
        {
            _server = new TcpListener(IPAddress.Loopback, 8888);
            _server.Start();
            Console.WriteLine("Pending requests..");

            //set bool to true so when function is called, while loop is runned
            _running = true;
            HandleStream();
        }

        //function is called when an instance of server is created in main class
        //keep clients in loop and create thread for handling networkstream
        public void HandleStream()
        {
            clientList = new List<TcpClient>();

            while (_running)
            {
                //accepts client request on tcplistener obj
                TcpClient client = _server.AcceptTcpClient();
                clientList.Add(client);
                counter++;
                Console.WriteLine(counter + " connected..");

                //create thread for networkstream - use parameterizedThreadStart method holding networkStream obj
                Thread clientTh = new Thread(new ParameterizedThreadStart(ServerStream));
                clientTh.Start(client); 
            }
        }
        public void ServerStream(object o)
        {
            //get client socket from parameter passed to thread in handlestream func
            TcpClient clSocket = (TcpClient)o;

            NetworkStream stream = clSocket.GetStream();
            //create streams for client socket to writing and reading
            StreamWriter clWriter = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };           
            StreamReader clReader = new StreamReader(stream, Encoding.ASCII);

            //create bool to run while loop and string to store data
            string data = null;

            while (true)
            {
                //reads client stream
                data = clReader.ReadLine();

                //writes data from client
                clWriter.Write(data);
                clWriter.Flush();

            }
        }

        static void Main(string[] args)
        {
                    //make server obj to begin connection
                    Server server = new Server();
        }
    }
}
