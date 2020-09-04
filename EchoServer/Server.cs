using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace EchoServer
{
    public class Server
    {
        
        public  void Start()
        {
            TcpListener server = null;

            try
            {
                // Set the TCPlistener to port 7777
                Int32 port = 7777;
                IPAddress localIpAddress = IPAddress.Parse("192.168.24.122");

                // opretter en TCPlistener object med LocalIpAdress og Port som input
                server = new TcpListener(localIpAddress, port);

                // Start listening for client request
                server.Start();

                //buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop
                while (true)
                {
                    Console.WriteLine("Waiting for Connection...");

                    //Perform a block call to accept requests.
                    TcpClient socket = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream ns = socket.GetStream();


                    int i;

                    //Loop to receive all the data sent by the client.
                    while ((i = ns.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        //translate data bytes to ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}",data);
                        
                        //process the data sent by client
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response
                        ns.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);


                    }

                    //shutdown and end connection
                    socket.Close();
                }





            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            finally
            {
                // Stop listening for new clients
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();

        }
    }
}
