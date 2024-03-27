using System.Net.Sockets;
using System.Net;

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
            //Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpListener listener = new TcpListener(ipPoint); // bind

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                //listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                //listenSocket.Listen(10);
                listener.Start();
                Console.WriteLine("Server started! Waiting for connection...");
                //Console.ReadLine();
                //Socket handler = listenSocket.Accept();
                TcpClient client = listener.AcceptTcpClient();

                while (client.Connected)
                {
                    // handler.Receive(); - get data from client
                    // handler.Send();    - sent data to client

                    NetworkStream ns = client.GetStream();

                    // ns.Write() - send data to client
                    // ns.Read()  - receive data from client

                    // получаем сообщение
                    //StringBuilder builder = new StringBuilder();
                    //int bytes = 0; // количество полученных байтов
                    //byte[] data = new byte[256]; // буфер для получаемых данных

                    //do
                    //{
                    //    bytes = handler.Receive(data);
                    //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    //}
                    //while (handler.Available > 0);

                    StreamReader sr = new StreamReader(ns);
                    string response = sr.ReadLine();

                    //Console.WriteLine($"{handler.RemoteEndPoint} - {builder.ToString()} at {DateTime.Now.ToShortTimeString()}");
                    Console.WriteLine($"{client.Client.RemoteEndPoint} - {response} at {DateTime.Now.ToShortTimeString()}");

                    // отправляем ответ
                    string message = "Message was send!";
                    //data = Encoding.Unicode.GetBytes(message);
                    //handler.Send(data);

                    StreamWriter sw = new StreamWriter(ns);
                    sw.WriteLine(message);

                    sw.Flush();

                    // закриваємо потокі
                    //sr.Close();
                    //sw.Close();
                    //ns.Close();
                }
                // закрываем сокет
                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
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
