
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace ModbusLibrary.SerialPortManager
{
    public class SerialPortManager
    {
        public SerialPort _serialPort;
        public SerialPortManager(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);

            _serialPort.ReadTimeout = 200;
            _serialPort.WriteTimeout = 200;
        }
        //portun suan acık olup olmadıgını kontrol eden yapı
        public bool IsOpen()
        {
            return _serialPort.IsOpen;
        }
        //Bğlantıyı Fiziksel olarak acan 
        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }
        //Bağlantıyı guvenlı sekılde kapatır
        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }
        //byte[] dizisi halindeki Modbus paketını port uzerınden dısarıya (cihaza) gönderir.
        public void Send(byte[] data)
        {
            //data: gonderilcek dizi,0:başlangıc indexi,data.length kaç byte gonderılcek
            _serialPort.Write(data, 0, data.Length);
        }
        //cıhazdan gelen cevapı okuyan kısım
        public byte[] Read()
        {
            //Modbus RTU yavas haberleşen bı yapıdır bekleme zzazmanı koyulur
            Thread.Sleep(200);//RTU Frame bekleme
            //bufferın bellegınde suan kac byte beklıyor
            int bytes = _serialPort.BytesToRead;
            //Eger hıc verı gelmedıyse hata almamak ıcın bos bır dızı dondurur
            if (bytes == 0)
                return new byte[0];
            //Gelen veri miktarı kadar bır buffer oluştur
            byte[] buffer = new byte[bytes];
            //Bellekte bekleyen tüm verileri oluşturduğumuz buffer dizisinin içine aktar
            _serialPort.Read(buffer, 0, bytes);

            return buffer;//Okunan ham Modbus paketını dondurur.
        }
        // Burası 'void' değil, mutlaka 'byte[]' olmalı!
        public byte[] SendRecive(byte[] frame)
        {
            try
            {
                if (_serialPort != null && _serialPort.IsOpen)
                {
                    _serialPort.Write(frame, 0, frame.Length);
                    System.Threading.Thread.Sleep(100);

                    byte[] buffer = new byte[_serialPort.BytesToRead];
                    _serialPort.Read(buffer, 0, buffer.Length);

                    return buffer; // Mutlaka bir byte dizisi döndürmeli
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

    }
}
