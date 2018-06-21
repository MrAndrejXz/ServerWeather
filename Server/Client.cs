using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Drawing;
using System.Web.Script.Serialization;
using Server;
using MySql.Data.MySqlClient;
using System.IO;

namespace Server
{
    public class Client
    {
        // Сериализатор
        JavaScriptSerializer sr = new JavaScriptSerializer();
        void SendToClients(TcpClient Client, string text)
        {
            byte[] myReadBuffer = Encoding.Default.GetBytes(text.ToString());
            Client.GetStream().Write(myReadBuffer, 0, myReadBuffer.Length);
        }

        // Конструктор класса. Ему нужно передавать принятого клиента от TcpListener
        public Client(TcpClient Client)
        {
            int countSec = 0;
            // Объявим строку, в которой будет хранится запрос клиента
            string Request = string.Empty;
            // Буфер для хранения принятых от клиента данных
            byte[] Buffer = new byte[1024];
            while (true)
            {
                Thread.Sleep(1);
                countSec++;
                if (countSec == 3000)
                {
                    break;
                }
                Request = string.Empty;
                // Читаем из потока клиента до тех пор, пока от него поступают данные
                while (Client.GetStream().DataAvailable == true)
                {
                    byte[] buffer = new byte[Client.Available];
                    Client.GetStream().Read(buffer, 0, buffer.Length);
                    Request += Encoding.Default.GetString(buffer);
                }
                if (Request == string.Empty)
                    continue;
                // Парсим строку запроса
                if (Request == "<The End>")
                {
                    //MessageBox.Show(Client.Client.RemoteEndPoint.ToString());
                    break;
                }
                try
                {
                    var y = sr.Deserialize<Packet>(Request);
                    //lock (TCP_Server.TcpServerForm.mutex)
                    {
                        Program.Queue.Enqueue(y);
                        Program.count++;
                    }
                    Thread.Sleep(1);
                    SendToClients(Client, "<Accept>");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
            Client.Close();
        }
    }
}