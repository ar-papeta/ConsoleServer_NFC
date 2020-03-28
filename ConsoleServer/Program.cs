using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpListenerApp
{
    class Program
    {

        const int port = 8888; // порт для прослушивания подключений

        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                // запуск слушателя
                server.Start();

                while (true)
                {
                    // получаем входящее подключение
                    TcpClient client = server.AcceptTcpClient();
                    StringBuilder response = new StringBuilder();
                    byte[] data = new byte[256];
                    // получаем сетевой поток для чтения и записи
                    NetworkStream stream = client.GetStream();
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable); // пока данные есть в потоке  
                    //делим по точке полученное сообщение
                    String[] words = Convert.ToString(response).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    Console.WriteLine("SERIAL NUMBER: {0}",words[0]);
                    Console.WriteLine("UID: {0}", words[1]);
                    byte[] dataKey = new byte[256] { 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255, 25, 255, 255, 255, 255, 255, 255, 7, 128, 105, 255, 255, 255, 255, 255, 255 }; 
                    byte[] dataTransmit = new byte[1023];
                    for (int i = 0; i < 1023; i++){
                        if (i < 256)
                        {
                            dataTransmit[i] = dataKey[i];
                        }
                        else dataTransmit[i] = Convert.ToByte(0);
                    }
                    stream.Write(dataTransmit, 0, dataTransmit.Length);
                    // закрываем поток
                    stream.Close();
                    // закрываем подключение
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Main: " + e.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
        }
    }
}