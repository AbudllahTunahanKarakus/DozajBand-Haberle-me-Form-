using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusLibrary.Crc;
using ModbusLibrary.ModbusFrame;

namespace ModbusLibrary.ModbusFrame
{
    public class Function16
    {

            // 'ushort[] values' kullanarak birden fazla değeri dizi olarak alıyoruz
            public static byte[] CreateWriteMultipleRegistersFrame(byte slavId, ushort startAddress, ushort[] values)
            {
                List<byte> frame = new List<byte>();

                // 1. Cihaz ID (Slave ID)
                frame.Add(slavId);

                // 2. Fonksiyon Kodu (Onluk 16, Hex 0x10)
                frame.Add(0x10);

                // 3. Başlangıç Adresi (High Byte ve Low Byte)
                frame.Add((byte)(startAddress >> 8));
                frame.Add((byte)(startAddress & 0xFF));

                // 4. Kayıt Sayısı (Kaç tane kutucuğa yazılacak?)
                ushort quantity = (ushort)values.Length;
                frame.Add((byte)(quantity >> 8));
                frame.Add((byte)(quantity & 0xFF));

                // 5. Toplam Byte Sayısı (Her veri 2 byte olduğu için: miktar * 2)
                byte byteCount = (byte)(quantity * 2);
                frame.Add(byteCount);

                // 6. Verilerin Pakete Eklenmesi (Döngü ile tüm dizi elemanlarını ekliyoruz)
                foreach (var val in values)
                {
                    frame.Add((byte)(val >> 8));
                    frame.Add((byte)(val & 0xFF));
                }

                // 7. CRC Hesaplama (Senin CRC_Calculator sınıfını kullanıyoruz)
                byte[] crc = CRC_Calculator.CRC(frame.ToArray());
                frame.AddRange(crc);

                return frame.ToArray();
            }
        }
    }




