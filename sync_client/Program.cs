using System.Net.Sockets;
using System.Net;

namespace sync_client
{
    class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 4041; // порт сервера
        static string address = "127.0.0.10"; // адрес сервера
        //static string address = "10.10.36.100"; // адрес сервера
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            //Socket socket = null;
            //socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpClient client = new TcpClient();

            // подключаемся к удаленному хосту
            //socket.Connect(ipPoint);
            client.Connect(ipPoint);

            string message = "";
            try
            {
                while (message != "end")
                {
                    Console.Write("Enter a message:");
                    message = Console.ReadLine();

                    NetworkStream ns = client.GetStream();


                    // ns.Write() - send data
                    // ns.Read()  - receive data

                    //byte[] data = Encoding.Unicode.GetBytes(message);
                    //socket.Send(data);

                    StreamWriter sw = new StreamWriter(ns);
                    sw.WriteLine(message);//Hello

                    sw.Flush(); // send all buffered data and clear the buffer // 1Kb

                    // получаем ответ
                    //data = new byte[256]; // буфер для ответа
                    //StringBuilder builder = new StringBuilder();
                    //int bytes = 0; // количество полученных байт

                    //do
                    //{
                    //    bytes = socket.Receive(data, data.Length, 0);
                    //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    //}
                    //while (socket.Available > 0);
                    //string response = builder.ToString();

                    StreamReader sr = new StreamReader(ns);
                    string response = sr.ReadLine();

                    Console.WriteLine("server response: " + response);

                    // закриваємо потокі
                    //sw.Close();
                    //sr.Close();
                    //ns.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // закрываем сокет
                //socket.Shutdown(SocketShutdown.Both);
                //socket.Disconnect(true);
                //socket.Close();
                client.Close();
            }
        }
    }
}
