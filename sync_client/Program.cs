using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.NetworkInformation;
using System.Text.Unicode;
using System;
using System.Text;
using System.Text.Json;
using SharedData;

namespace sync_client
{
    class Program
    {
        static int port = 4041; // порт сервера
        static string address = "127.0.0.10"; // адрес сервера
        //static string address = "10.10.36.100"; // адрес сервера
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            TcpClient client = new TcpClient();

            // подключаемся к удаленному хосту
            client.Connect(ipPoint);

            try
            {
                while (true)
                {
                    Request request = new Request();
                   
                    Console.Write("Enter A:");
                    request.A = double.Parse( Console.ReadLine()!);
                    Console.Write("Enter B:");
                    request.B = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter Operation [1-4]:");
                    request.Operation = (OperationType)Enum.Parse( typeof(OperationType),
                        Console.ReadLine()!);


                    NetworkStream ns = client.GetStream();
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ns, request);

          
                    StreamReader sr = new StreamReader(ns);
                    string response = sr.ReadLine();

                    Console.WriteLine("server response: " + response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
