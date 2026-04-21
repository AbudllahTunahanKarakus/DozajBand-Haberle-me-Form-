using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModbusLibrary.SerialPortManager;
using ModbusLibrary.ModbusMaster;
using System.Runtime.Remoting.Messaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ModbusLibrary
{

    public partial class Deneme1 : Form
    {
        bool isAutoReadding = false;
        private SerialPortManager.SerialPortManager serial;
        private ModbusMaster.ModbusMaster master;

        public Deneme1()
        {
            // Grafiğin X ekseni (zaman ekseni) ayarları


        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (master == null)
            {
                serial = new SerialPortManager.SerialPortManager("COM4", 9600, System.IO.Ports.Parity.Even, 8, System.IO.Ports.StopBits.One);
                master = new ModbusMaster.ModbusMaster(serial);
            }
            if (!serial.IsOpen())
            {
                try
                {


                    serial.Open();

                    if (serial.IsOpen())
                    {
                        btnConnect.Text = "DISCONNECT";
                        btnConnect.BackColor = Color.Red;
                        btnConnect.ForeColor = Color.White;

                        lblStatus.Text = "Bağlandı";
                        lblStatus.BackColor = Color.Green;
                        listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] Bağlantı Açıldı.");

                    }
                }
                catch (Exception ex)
                {
                    listBox1.Items.Add("Bağlantı Hatası: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    if (timer1.Enabled)
                        timer1.Stop();

                    serial.Close();
                    btnConnect.Text = "CONNECT";
                    btnConnect.BackColor = SystemColors.Control;
                    btnConnect.ForeColor = Color.Black;

                    lblStatus.Text = "Bağlantı Kesıldi";
                    lblStatus.BackColor = Color.Red;
                    lblStatus.ForeColor = Color.White;
                    listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] Bağlantı Kapatıldı.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("kapatma Hatası: " + ex.Message);
                }
            }




        }

        private void btnRead_Click(object sender, EventArgs e)
        {

            try
            {
                // Önemli: Simülatördeki 40001'i okumak için 0 adresini deniyoruz
                byte[] response = master.ReadHoldingRegisters(1, 1, 1);

                if (response == null)
                {
                    listBox1.Items.Add("Yanıt yok (Timeout)");
                    return;
                }

                string raw = BitConverter.ToString(response);
                listBox1.Items.Add("Gelen Paket: " + raw);

                // Eğer paket 01-03-00... şeklinde geliyorsa:
                if (response.Length >= 3 && response[2] == 0x00)
                {
                    listBox1.Items.Add("Hata: Simülatör bu adresi/isteği tanımıyor.");
                    return;
                }

                // Başarılı Paket Analizi (En az 7 byte gelmeli)
                if (response.Length >= 7)
                {
                    // response[3] ve [4] veridir
                    ushort sonuc = (ushort)(response[3] << 8 | response[4]);
                    listBox1.Items.Add("Okunan Değer: " + sonuc);
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Yazılım Hatası: " + ex.Message);
            }


        }

        private void btnWrite_Click(object sender, EventArgs e)
        {

            try
            {
                ushort adres = ushort.Parse(txtAddress.Text);
                ushort deger = ushort.Parse(txtValue.Text);

                // 1. ADIM: Önce gönderilecek paketi oluşturup ekrana yazdıralım
                // Bu sayede porttan ne fırlattığımızı görürüz (Örn: 01-06-00-01...)
                byte[] gidenPaket = ModbusLibrary.ModbusFrame.Function06.CreateWriteHoldingRegistersFrame(1, adres, deger);
                listBox1.Items.Add("GİDEN PAKET: " + BitConverter.ToString(gidenPaket));

                // 2. ADIM: Paketi gönder ve yanıtı al
                byte[] response = master.WriteHoldingRegister(1, adres, deger);

                if (response == null || response.Length == 0)
                {
                    listBox1.Items.Add("Hata: Yanıt Yok!");
                    return;
                }

                // 3. ADIM: Gelen yanıtı da ham paket (Hex) olarak yazdıralım
                listBox1.Items.Add("GELEN PAKET: " + BitConverter.ToString(response));

                // 4. ADIM: Onay kontrolü
                if (response.Length >= 8 && (response[1] & 0x80) == 0)
                {
                    listBox1.Items.Add("İşlem Başarılı! ✅");
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Hata: " + ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 0. Bağlantı koptuysa timer'ı zorla durdur (Güvenlik Kalkanı)
            if (serial == null || !serial.IsOpen())
            {
                timer1.Stop();
                btnStartAutoRead.Text = "START AUTO";
                return;
            }

            try
            {
                // 1. Cihazdan 2 adet register oku
                byte[] response = master.ReadHoldingRegisters(1, 0, 2);

                // 2. Yanıt paketini kontrol et
                if (response != null && response.Length >= 5)
                {
                    // 3. Veriyi sayıya çevir (Modbus standardına göre 3. byte uzunluk, 4-5. byte veridir)
                    ushort reg1 = (ushort)((response[3] << 8) | response[4]);

                    // 4. Grafiğe yeni noktayı ekle
                    chart1.Series["Sicaklik"].Points.AddY(reg1);

                    // 5. KAYDIRMA VE TEMİZLEME MANTIĞI (Scrolling Chart)
                    // Ekranda aynı anda 20 veri göster, fazlası gelirse sağa kaydır
                    chart1.ChartAreas[0].AxisX.ScaleView.Size = 20;
                    if (chart1.Series["Sicaklik"].Points.Count > 20)
                    {
                        // Grafiğin penceresini son eklenen veriye odakla (Kayan efekt)
                        chart1.ChartAreas[0].AxisX.ScaleView.Scroll(System.Windows.Forms.DataVisualization.Charting.ScrollType.Last);
                    }

                    // Belleği korumak için toplamda 100 veriden fazlasını tutma (Opsiyonel)
                    if (chart1.Series["Sicaklik"].Points.Count > 100)
                    {
                        chart1.Series["Sicaklik"].Points.RemoveAt(0);
                    }

                    // 6. ListBox ve Otomatik Kaydırma
                    listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] Okunan Değer: {reg1}");
                    listBox1.TopIndex = listBox1.Items.Count - 1;

                    // 7. CSV Loglama (Excel dostu format)
                    string logSatiri = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss};{reg1}\n";
                    System.IO.File.AppendAllText("Modbus_Kayitlari.csv", logSatiri);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda timer'ı durdurup kullanıcıyı uyarmak en güvenlisi
                timer1.Stop();
                isAutoReadding = false;
                btnStartAutoRead.Text = "START AUTO";
                MessageBox.Show("Haberleşme Hatası: " + ex.Message);
            }

            /*
            try
            {
                // 1. ADIM: Modbus üzerinden oku (Slave: 1, Adres: 0, Miktar: 2)
                // Senin az önce gördüğün "01-03-04..." paketini bu komut getiriyor.
                byte[] response = master.ReadHoldingRegisters(1, 0, 2);

                // 2. ADIM: Yanıtın doğruluğunu kontrol et
                // Gelen paket en az 5 byte olmalı (ID + Func + ByteCount + DataHigh + DataLow)
                if (response != null && response.Length >= 5)
                {
                    // 3. ADIM: Byte birleştirme (Parsing)
                    // Senin paketindeki "00 0C" kısmını (yani 12 değerini) sayıya çeviriyoruz.
                    ushort reg1 = (ushort)((response[3] << 8) | response[4]);

                    // 4. ADIM: Grafiği Güncelle
                    // "Sicaklik" serisine yeni noktayı ekle
                    chart1.Series["Sicaklik"].Points.AddY(reg1);

                    // Grafik ekranı kaydıran özellik (Son 30 veriyi gösterir)
                    if (chart1.Series["Sicaklik"].Points.Count > 30)
                    {
                        chart1.Series["Sicaklik"].Points.RemoveAt(0);
                    }

                    // 5. ADIM: ListBox ve Arayüz Bilgilendirme
                    listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] Okunan: {reg1}");

                    // ListBox'ın en aşağıya otomatik kaymasını sağla
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                // Hata olursa timer'ı durdur ki ekran hata mesajıyla dolmasın
                timer1.Stop();
                MessageBox.Show("Okuma hatası oluştu, bağlantıyı kontrol edin: " + ex.Message);
            }
        
            */







            /* try
             {
                 byte[] response = master.ReadHoldingRegisters(1, 0, 2);
                 if(response != null && response.Length >=8)
                 {
                     ushort reg1 = (ushort)((response[3] << 8) | response[4]);
                     ushort reg2 = (ushort)((response[5] << 8) | response[6]);
                     lblValue1.Text = "Sıcalık: " + reg1.ToString() + "*C";
                     lblValue2.Text = "Basınç: " + reg2.ToString() + "Bar";

                     listBox1.Items.Add("Oto Okuma: " + BitConverter.ToString(response));

                     listBox1.SelectedIndex=listBox1.Items.Count - 1;

                     chart1.Series["Sicaklik"].Points.AddY(reg1);
                     if (chart1.Series["Sicaklik"].Points.Count > 20) ;
                     {
                         chart1.Series["Sicaklik"].Points.RemoveAt(0);
                     }
                 }
             }
             catch (Exception ex) 
             {
                 timer1.Stop();
                 MessageBox.Show("Otomatık okuma hatası: " + ex);
             }
            */

        }


        private void btnStartAutoRead_Click(object sender, EventArgs e)
        {

            if (serial == null || !serial.IsOpen())
            {
                MessageBox.Show("Önce Cihazla bağlantı kurmalısın !!!");
                return;
            }

            if (isAutoReadding == false)
            {
                timer1.Interval = 1000;
                timer1.Enabled = true;
                timer1.Start();

                isAutoReadding = true;

                btnStartAutoRead.Text = "Stop AUTO";
                btnStartAutoRead.BackColor = Color.Red;
                btnStartAutoRead.ForeColor = Color.White;
                listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] Otamatık Okuma Başlatıldı");
            }
            else
            {
                timer1.Stop();
                timer1.Enabled = false;
                isAutoReadding = false;
                btnStartAutoRead.Text = "Start AUTO";
                btnStartAutoRead.BackColor = SystemColors.Control;
                btnStartAutoRead.ForeColor = Color.Black;
                listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] Otamatık Okuma Durduruldu ....");
            }

            /*
            if (master != null) // Bağlantı varsa
            {
                timer1.Start();
                btnStartAutoRead.Text = "Sistem Çalışıyor...";
                btnStartAutoRead.Enabled = false; // Çift basılmasın diye pasif yapıyoruz
            }
            else
            {
                MessageBox.Show("Önce bağlantıyı kurmalısın!");
            }*/

            /*
            if (timer1.Enabled)
            {
                timer1.Stop();
                btnStartAutoRead.Text = "Canlı İzlemeyi Başlat";
                btnStartAutoRead.BackColor = Color.LightGray;
            }
            else
            {
                // Portun açık olduğundan emin olmalısın
                if (master != null)
                {
                    timer1.Start();
                    btnStartAutoRead.Text = "İzlemeyi Durdur";
                    btnStartAutoRead.BackColor = Color.LightGreen;
                }
            }*/
        }

        private void btnWriteMltp_Click(object sender, EventArgs e)
        {

            if (serial == null || !serial.IsOpen())
            {
                MessageBox.Show("Bağlantı yok!!!");
                return;
            }

            // 1. Yazılacak ham değerler (Sayı dizisi - ushort[])
            ushort[] degerler = new ushort[3];
            bool v0 = ushort.TryParse(txtValMltp1.Text, out degerler[0]);
            bool v1 = ushort.TryParse(txtValMltp2.Text, out degerler[1]);
            bool v2 = ushort.TryParse(txtValMltp3.Text, out degerler[2]);

            if (!v0 || !v1 || !v2)
            {
                MessageBox.Show("Lütfen Geçerlş Sayı Giriniz !!!");
                return;
            }
            // 2. Paketi oluştur (Bu byte[] döner)
            try
            {
                byte[] gidenPaket = ModbusLibrary.ModbusFrame.Function16.CreateWriteMultipleRegistersFrame(1, 0, degerler);
                listBox1.Items.Add("Değerler Doğru Gönderildi");
                // 3. DOĞRU GÖNDERME YÖNTEMİ:
                // master içindeki SendAndReceive metoduna "byte[]" paketini veriyoruz.
                byte[] yanit = serial.SendRecive(gidenPaket);
                if (yanit != null)
                {
                    // --- LOGLAMA KISMI BURADA BAŞLIYOR ---

                    // Zaman damgasını ve değerleri bir satır haline getiriyoruz
                    // Format: 2026-03-06 14:30:05 -> Değerler: 10, 20, 30
                    string zaman = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string veriler = string.Join(", ", degerler);
                    string logSatiri = $"{zaman} -> Yazılan Değerler: {veriler}\n";

                    // Projenin çalıştığı klasöre "Modbus_Loglari.txt" adıyla kaydeder. 
                    // Dosya yoksa oluşturur, varsa sonuna ekler.
                    System.IO.File.AppendAllText("Modbus_Loglari.txt", logSatiri);

                    // Kullanıcıya bilgi ver
                    listBox1.Items.Add("İşlem Başarılı ve Loglandı! ✅");
                    // ---------------------------------------
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Gonderım Hatası!!!" + ex.Message);
            }



        }

        private void txtValMltp3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }

}
    
    
    

