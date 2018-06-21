using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Server
{
    public class Sensors_Item
    {
        public int Temp { get; set; }
        public int Pressure { get; set; }
        public int AirHumidity { get; set; }
        public string Time { get; set; }
    }

    public class IP_Item
    {
        // IP 
        public string IP;
        // Порт
        public int Port;
    }

    public class Packet
    {
        // IP датчика
        public IP_Item IP;
        // Значения сенсоров
        public Sensors_Item Sensor;
        // Пройденный путь
        public List<IP_Item> lIP;
    }


}