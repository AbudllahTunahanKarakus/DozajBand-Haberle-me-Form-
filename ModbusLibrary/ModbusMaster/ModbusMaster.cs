
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusLibrary.SerialPortManager;

namespace ModbusLibrary.ModbusMaster
{
    public class ModbusMaster
    {

        private SerialPortManager.SerialPortManager _serial;
        public ModbusMaster(SerialPortManager.SerialPortManager serial)
        {
            _serial = serial;
        }
        public bool IsOpen()
        {
            return _serial.IsOpen();
        }
        //OKUMA
        public byte[] ReadHoldingRegisters(byte slaveId, ushort start, ushort quantity)
        {
            byte[] frame = ModbusFrame.Function03.CreateReadHoldingRegistersFrame(slaveId, start, quantity);

            _serial.Send(frame);
            return _serial.Read();
        }
        //YAZMA
        public byte[] WriteHoldingRegister(byte slaveId, ushort start, ushort values)
        {
            byte[] frame = ModbusFrame.Function06.CreateWriteHoldingRegistersFrame(slaveId, start, values);
            _serial.Send(frame);
            return _serial.Read();
        }
        public byte[] WriteMultipleRegisters(byte slaveId, ushort start, ushort[] values)
        {
            // 1. Kütüphanendeki metodu kullanarak byte[] paketini oluşturur
            byte[] frame = ModbusLibrary.ModbusFrame.Function16.CreateWriteMultipleRegistersFrame(slaveId, start, values);


            return _serial.SendRecive(frame);
        }
    }
}
