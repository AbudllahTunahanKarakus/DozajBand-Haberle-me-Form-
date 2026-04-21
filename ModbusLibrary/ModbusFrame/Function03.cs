using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusLibrary.Crc;

namespace ModbusLibrary.ModbusFrame
{
    public class Function03
    {
        public static byte[] CreateReadHoldingRegistersFrame(byte slaveId,ushort startAddress,ushort quantity)
        {   //Dinamiik bir Liste oluşturuluyo eklmesıi ve çıkarması için sınır olmadığı için.
            List<byte> frame = new List<byte>();
            //paketin ilk byte her zaman slaveId gösterır
            //slaveıd haberleşceğimiz alet ıle bağın sağlanması ve habgı slaveden okunması ya ad yazılması gerektıgı anlaşılır
            frame.Add(slaveId);
            frame.Add(0X03);//Function Code

            //Paketin ilk 8 bit sağa kaydırılr MSB alınır
            frame.Add((byte)(startAddress >> 8));
            frame.Add((byte)(startAddress & 0xFF));//start adersidnde ise en dusuk bytle LSB ile 

            //Okuncak maddde bılgıısı 
            frame.Add((byte)(quantity >> 8));
            frame.Add((byte)(quantity & 0xFF));

            byte[] crc = CRC_Calculator.CRC(frame.ToArray()); //CRC Hesaplama
            frame.AddRange(crc);//crc ıle olustruualan 2 bytlek sınır
            return frame.ToArray();//byte dizisine dondurulcek
        }
    }
}
