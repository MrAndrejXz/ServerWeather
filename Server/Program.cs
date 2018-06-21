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
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;

namespace Server
{
    public class Program
    {
        static Thread x, y;
        public static int count = 0;
        public static Queue<Packet> Queue = new Queue<Packet>();
        static Server server = new Server();

        static void Main(string[] args)
        {
            Start();
        }

        static void Start()
        {
            Console.WriteLine("Сервер запущен");
            Console.WriteLine("IP сервера: " + System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString());
            Console.WriteLine("Порт: 15228\n********************\n\n");
            // Отдельный поток для сервера
            x = new Thread(ServerStart);
            // Поток которыей следит за очередью пакетов
            y = new Thread(QueueThread);
            y.Start();
            x.Start();
        }


        static void QueueThread()
        {
            while (true)
            {
                Thread.Sleep(10);
                {
                    if (Queue.Count != 0)
                    {
                        var x = Queue.Dequeue();
                        if (x == null) continue;
                        if(x.Sensor.Time==null)
                        x.Sensor.Time = "Null";
                        #region Здесь вствляем объект X в БД и всё
                        Console.WriteLine("[" + SqlDB.QueryDB("SELECT count(*) FROM new_schema.new_table;").Rows[0][0].ToString() + "]\nПакет от: " + x.IP.IP.ToString()+":"+x.IP.Port.ToString());
                        Console.WriteLine("Температура: " + x.Sensor.Temp.ToString());
                        Console.WriteLine("Давление: " + x.Sensor.Pressure.ToString());
                        Console.WriteLine("Влажность: " + x.Sensor.AirHumidity.ToString());
                        Console.WriteLine("Время сбора: " + x.Sensor.Time.ToString());
                        Console.WriteLine("\n********************\n");
                        SqlDB.QueryDB("INSERT INTO new_schema.new_table (IP, Temp, Pressure, AirHumidity, Time) "
                        + "VALUES ('" + x.IP.IP.ToString()+":"+x.IP.Port.ToString() + "', '" + x.Sensor.Temp.ToString() + "', '" + x.Sensor.Pressure.ToString() + "', '" + x.Sensor.AirHumidity.ToString() + "', '" + x.Sensor.Time.ToString() + "');");
                        //dataGridView1.DataSource = null;
                        Thread.Sleep(10);
                        #endregion
                    }
                }
            }
        }

        static void ServerStart()
        {
            server.ServerStart(15228);
        }
    }
}
