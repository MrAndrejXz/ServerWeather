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
    public class Server
    {
        TcpListener Listener; // Объект, принимающий TCP-клиентов

        // Запуск сервера
        public void ServerStart(int Port)
        {
            Listener = new TcpListener(IPAddress.Any, Port);    // Создаем "слушателя" для указанного порта
            Listener.Start();                                   // Запускаем его
            while (true)
            {
                try
                {
                    // Принимаем нового клиента
                    TcpClient Client = Listener.AcceptTcpClient();
                    // Создаем поток
                    Thread Thread = new Thread(new ParameterizedThreadStart(ClientThread));
                    // И запускаем этот поток, передавая ему принятого клиента
                    Thread.Start(Client);
                }
                catch
                {

                }
            }
        }

        static void ClientThread(Object StateInfo)
        {
            // Просто создаем новый экземпляр класса Client и передаем ему приведенный к классу TcpClient объект StateInfo
            new Client((TcpClient)StateInfo);
        }

        // Остановка сервера
        public void ServerStop()
        {
            // Если "слушатель" был создан
            if (Listener != null)
            {
                // Остановим его
                Listener.Stop();
            }
        }

    }
}
