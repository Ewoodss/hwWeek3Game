using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            
            
            TcpListener listener = new TcpListener(IPAddress.Any, 42069);
            listener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for connection");

                //AcceptTcpClient waits for a connection from the client:
                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine("I am connected to " + IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()) + "on port number " + ((IPEndPoint)client.Client.RemoteEndPoint).Port.ToString());

                // handle this client in a new worker-thread 
                // and continue accepting new clients:
                Thread thread = new Thread(HandleClientThread);
                thread.Start(client);
            }

        }

        static void HandleClientThread(object obj)
        {
            TcpClient client = obj as TcpClient;
            WriteTextMessage(client,"hello");

            bool done = false;
            while (!done)
            {
                string received = ReadTextMessage(client);
                Console.WriteLine("Received: {0}", received);

                done = received.Equals("bye");
                if (done) WriteTextMessage(client, "BYE");
                else WriteTextMessage(client, "OK");
            }
            client.Close();
            Console.WriteLine("Connection closed");
        }





        public static void WriteTextMessage(TcpClient client, string message)
        {
            var stream = new StreamWriter(client.GetStream(), Encoding.ASCII, -1, true);
            {
                stream.WriteLine(message);
                stream.Flush();
            }
        }

        public static string ReadTextMessage(TcpClient client)
        {
            var stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            {
                return stream.ReadLine();
            }
        }

    }
}
