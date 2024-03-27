using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using SharedData;
using System.Buffers;
using System.Runtime.Serialization.Formatters.Binary;

namespace sync_server
{
    class Program
    {
        static int port = 4041; // порт для приема входящих запросов
        static void Main(string[] args)
        {
            // получаем адреса для запуска сокета
            IPAddress iPAddress = IPAddress.Parse("127.0.0.10");//Dns.GetHostEntry("localhost").AddressList[1]; //localhost
            //IPAddress iPAddress = IPAddress.Parse("10.10.36.100");//Dns.GetHostEntry("localhost").AddressList[1]; //localhost
            IPEndPoint ipPoint = new IPEndPoint(iPAddress, port);

            // создаем сокет
            TcpListener listener = new TcpListener(ipPoint); // bind

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listener.Start();
                Console.WriteLine("Server started! Waiting for connection...");
                TcpClient client = listener.AcceptTcpClient();

                while (client.Connected)
                {

                    NetworkStream ns = client.GetStream();
                    BinaryFormatter bf = new BinaryFormatter();
                    Request request = (Request)bf.Deserialize(ns);


                    Console.WriteLine($"{client.Client.RemoteEndPoint} \n " +
                        $"A = {request.A} . B = {request.B} at {DateTime.Now.ToShortTimeString()}");

                    // отправляем ответ
                    double res = 0;
                    switch (request.Operation)
                    {
                        case OperationType.Add:res = request.A + request.B;break;
                        case OperationType.Sub:res = request.A - request.B;break;
                        case OperationType.Mult:res = request.A * request.B;break;
                        case OperationType.Div:res = request.A / request.B;break;
                    }
                    string message = $"Result {request.A}  {request.Operation}  {request.B} = {res}";
                    StreamWriter sw = new StreamWriter(ns);
                    sw.WriteLine(message);
                    sw.Flush();

                }
                // закрываем сокет
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            listener.Stop();
        }
    }
}
