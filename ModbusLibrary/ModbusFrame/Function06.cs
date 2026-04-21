using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusLibrary.Crc;

namespace ModbusLibrary.ModbusFrame
{
    public class Function06
    {
        public static byte[] CreateWriteHoldingRegistersFrame(byte slavId,ushort startAddress,ushort values)
        {
            List <byte> frame = new List <byte> ();

            frame.Add((byte) (slavId));
            frame.Add((byte)(0X06));

            frame.Add((byte)(startAddress >> 8));
            frame.Add ((byte)(startAddress & 0XFF));

            frame.Add((byte)(values >> 8));
            frame.Add((byte)(values & 0xFF));

            byte[] crc=CRC_Calculator.CRC(frame.ToArray());
            frame.AddRange (crc);   
            return frame.ToArray ();
        }
    }
}
