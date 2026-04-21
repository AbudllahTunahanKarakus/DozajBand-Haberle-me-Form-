using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ModbusLibrary;

namespace ModbusLibrary
{
  
    public partial class DozajDashboard : Form
    {
        SerialPortManager.SerialPortManager serial;
        ModbusMaster.ModbusMaster master;
        bool[] oncekiSinayller = new bool[16];
        bool[] lambaHafizasi = new bool[16];
        ushort komutVerisi31 = 0; //KOMUT1
        ushort gonderilecekKomut32 = 0; //KOMUT2
        //Textboxlar ıcın
        bool is30Changed = false; // Hedef Debi için
        bool is33Changed = false; // Ağırlık Seti için
        bool is34Changed = false; // Bunker Seti için

        public DozajDashboard()
        {
            InitializeComponent();
        }

        private void KomutBas32(ushort deger)
        {
            try
            {
                byte slaveID = (byte)numSlaveId.Value;
                master.WriteHoldingRegister(slaveID, 32, deger);
                lstLogs.Items.Add($"[{DateTime.Now:HH:mm:ss}] Komut 32 (Sistem/Kalib.) -> {deger}");
                lstLogs.TopIndex = lstLogs.Items.Count - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Komut 32 Gönderilemedi: " + ex.Message);
            }
        }

        // --- TOPLAYICI SIFIRLA (Bit 0 -> Değer: 1) ---
        private void btnTotalSifirla_Click(object sender, EventArgs e)
        {
            gonderilecekKomut32 ^= 1;
            btnTotalSifirla.BackColor = (gonderilecekKomut32 & 1) != 0 ? Color.Orange : Color.Gray;
            KomutBas32(gonderilecekKomut32);
        }

        // --- ARIZA SİL (Bit 1 -> Değer: 2) ---
        private void btnArizaSil_Click(object sender, EventArgs e)
        {
            gonderilecekKomut32 ^= 2;
            btnArizaSil.BackColor = (gonderilecekKomut32 & 2) != 0 ? Color.Gray : Color.Gray;
            KomutBas32(gonderilecekKomut32);
        }
        // --- ARIZA SİL (Bit 2 -> Değer: 4) ---
        private void btnKullanılmıyor1_Click(object sender, EventArgs e)
        {
            gonderilecekKomut32 ^= 4;
            btnKullanılmıyor1.BackColor = (gonderilecekKomut32 & 4) != 0 ? Color.Gray : Color.Gray;
            KomutBas32(gonderilecekKomut32);
        }
        // --- ARIZA SİL (Bit 3 -> Değer: 8) ---
        private void btnKullanılmıyor2_Click(object sender, EventArgs e)
        {
            gonderilecekKomut32 ^= 8;
            btnKullanılmıyor2.BackColor = (gonderilecekKomut32 & 8) != 0 ? Color.Green : Color.Gray;
            KomutBas32(gonderilecekKomut32);
        }

        // --- KALİBRASYON BAŞLA (Bit 4 -> Değer: 16) ---
        private void btnKalibrasyonBasla_Click(object sender, EventArgs e)
        {
            gonderilecekKomut32 ^= 16;
            btnKalibrasyonBasla.BackColor = (gonderilecekKomut32 & 16) != 0 ? Color.Yellow : Color.Gray;
            KomutBas32(gonderilecekKomut32);
        }

        // --- KALİBRASYON KABUL (Bit 5 -> Değer: 32) ---
        private void btnKalibrasyonKabul_Click(object sender, EventArgs e)
        {
            gonderilecekKomut32 ^= 32;
            btnKalibrasyonKabul.BackColor = (gonderilecekKomut32 & 32) != 0 ? Color.Green : Color.Gray;
            KomutBas32(gonderilecekKomut32);
        }

        // --- KALİBRASYON RED (Bit 6 -> Değer: 64) ---
        private void btnKalibrasyonRed_Click(object sender, EventArgs e)
        {
            gonderilecekKomut32 ^= 64;
            btnKalibrasyonRed.BackColor = (gonderilecekKomut32 & 64) != 0 ? Color.Red : Color.Gray;
            KomutBas32(gonderilecekKomut32);
        }

        // --- DOLUM DURDURULDU (Bit 7 -> Değer: 128) ---
        private void btnDolumDurdur_Click(object sender, EventArgs e)
        {
            gonderilecekKomut32 ^= 128;
            btnDolumDurdur.BackColor = (gonderilecekKomut32 & 128) != 0 ? Color.Red : Color.Gray;
            KomutBas32(gonderilecekKomut32);
        }
        private void Adres31Guncelle(ushort yeniDeger)
        {
            try
            {
                if (master != null)
                {
                    byte slaveID = (byte)numSlaveId.Value;
                    master.WriteHoldingRegister(slaveID, 31, yeniDeger);

                    // Loglama yapalım ki ne gittiğini görelim
                    lstLogs.Items.Add($"[{DateTime.Now:HH:mm:ss}] KOMUT 1 -> {yeniDeger}");
                    lstLogs.TopIndex = lstLogs.Items.Count - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Komut Gönderilemedi: " + ex.Message);
            }
        }

        // 1. ÇALIŞ (Bit 0 -> Değer: 1)
        private void btnCalis_Click(object sender, EventArgs e)
        {
            komutVerisi31 ^= 1;
            btnCalis.BackColor = (komutVerisi31 & 1) != 0 ? Color.Green : Color.Gray;
            Adres31Guncelle(komutVerisi31);
        }

        // 2. ÖN SİSTEM ÇALIŞIYOR (Bit 1 -> Değer: 2)
        private void btnOnSistemCalis_Click(object sender, EventArgs e)
        {
            komutVerisi31 ^= 2;
            btnOnSistemCalis.BackColor = (komutVerisi31 & 2) != 0 ? Color.Green : Color.Gray;
            Adres31Guncelle(komutVerisi31);
        }

        // 3. MOTOR ÇALIŞIYOR (Bit 2 -> Değer: 4)
        private void btnMotorCalis_Click(object sender, EventArgs e)
        {
            komutVerisi31 ^= 4;
            btnMotorCalis.BackColor = (komutVerisi31 & 4) != 0 ? Color.Green : Color.Gray;
            Adres31Guncelle(komutVerisi31);
        }

        // 4. MOTOR ARIZA (Bit 3 -> Değer: 8)
        private void btnMotorAriza_Click(object sender, EventArgs e)
        {
            komutVerisi31 ^= 8;
            btnMotorAriza.BackColor = (komutVerisi31 & 8) != 0 ? Color.Red : Color.Gray;
            Adres31Guncelle(komutVerisi31);
        }

        // 5. ÖN BESLEYİCİ ÇALIŞIYOR (Bit 4 -> Değer: 16)
        private void btnOnBesleyiciCalis_Click(object sender, EventArgs e)
        {
            komutVerisi31 ^= 16;
            btnOnBesleyiciCalis.BackColor = (komutVerisi31 & 16) != 0 ? Color.Green : Color.Gray;
            Adres31Guncelle(komutVerisi31);
        }

        // 6. ÖN BESLEYİCİ ARIZALI (Bit 5 -> Değer: 32)
        private void btnOnBesleyiciAriza_Click(object sender, EventArgs e)
        {
            komutVerisi31 ^= 32;
            btnOnBesleyiciAriza.BackColor = (komutVerisi31 & 32) != 0 ? Color.Red : Color.Gray;
            Adres31Guncelle(komutVerisi31);
        }

        // 7. UZAK ÇALIŞMA (Bit 6 -> Değer: 64)
        private void btnUzakCalisma_Click(object sender, EventArgs e)
        {
            komutVerisi31 ^= 64;
            btnUzakCalisma.BackColor = (komutVerisi31 & 64) != 0 ? Color.Green : Color.Gray;
            Adres31Guncelle(komutVerisi31);
        }

        // 8. YAKIN ÇALIŞMA (Bit 7 -> Değer: 128)
        private void btnYakinCalisma_Click(object sender, EventArgs e)
        {
            komutVerisi31 ^= 128;
            btnYakinCalisma.BackColor = (komutVerisi31 & 128) != 0 ? Color.Green : Color.Gray;
            Adres31Guncelle(komutVerisi31);
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string portName = cmbPort.SelectedItem.ToString();
                int baudRate = int.Parse(cmbBaudRate.SelectedItem.ToString());
                byte slaveID = (byte)numSlaveId.Value;

                serial = new SerialPortManager.SerialPortManager(portName, baudRate, System.IO.Ports.Parity.Even, 8, System.IO.Ports.StopBits.One);
                
                master = new ModbusMaster.ModbusMaster(serial);

                serial.Open();

                if (serial.IsOpen())
                {
                    lstLogs.Items.Add($"[{DateTime.Now:HH:mm}] Cihaza Bağlanıldı. Slave ID: {slaveID}");
                    timer1.Start(); // Okumayı başlat
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı Hatası: " + ex.Message);
            }
        }
        private void btnAyarlar_Click(object sender, EventArgs e)
        {
            try
            {
                byte slaveID = (byte)numSlaveId.Value;

                // --- ADRES 30: HEDEF DEBİ ---
                if (ushort.TryParse(txtTargetDose.Text, out ushort hedefDebi))
                {
                    master.WriteHoldingRegister(slaveID, 30, hedefDebi);
                    lstLogs.Items.Add($"[{DateTime.Now:HH:mm}] 30 -> Hedef: {hedefDebi}");
                    System.Threading.Thread.Sleep(50); // Çakışma riskine karşı minik bir es
                }

                // --- ADRES 33: AĞIRLIK SET DEĞERİ ---
                if (ushort.TryParse(txtAgirlikSet.Text, out ushort agirlikSet))
                {
                    master.WriteHoldingRegister(slaveID, 33, agirlikSet);
                    lstLogs.Items.Add($"[{DateTime.Now:HH:mm}] 33 -> Ağırlık Set: {agirlikSet}");
                    System.Threading.Thread.Sleep(50);
                }

                // --- ADRES 34: BUNKER AĞIRLIK SET DEĞERİ ---
                if (ushort.TryParse(txtBnkrAgirlikSetDeger.Text, out ushort bunkerSet))
                {
                    master.WriteHoldingRegister(slaveID, 34, bunkerSet);
                    lstLogs.Items.Add($"[{DateTime.Now:HH:mm}] 34 -> Bunker Set: {bunkerSet}");
                }

                lstLogs.TopIndex = lstLogs.Items.Count - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ayarlar Yazma Hatası: " + ex.Message);
            }
        }
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                byte slaveID = (byte)numSlaveId.Value;
                
                byte[] resSet = master.ReadHoldingRegisters(slaveID, 1, 1);
                // --- 1. Set Degeri (Adres 1 - Sadece Okunabilir) ---
                if (resSet != null && resSet.Length >= 5)
                {
                    ushort setDegeri = (ushort)((resSet[3] << 8) | resSet[4]);

                    lblSetDegeri.Text = setDegeri.ToString();
                }
                else
                {
                    // Hoca: "0 olduğunu anladığında eski rengine/haline dönsün" dediği için
                    // Veri gelmiyorsa veya bağlantı koptuysa "0" yapıyoruz
                    lblSetDegeri.Text = "0";
                }
                // --- 1. NOKTANIN YERİNİ OKU (Adres 17) ---
                byte[] resDP = master.ReadHoldingRegisters(slaveID, 17, 1);
                int dpValue = 0;
                if (resDP != null && resDP.Length >= 5)
                {
                    dpValue = (ushort)((resDP[3] << 8) | resDP[4]);
                }

                // --- 2. ANLIK DEBİ (Adres 3) ---
                byte[] resFlow = master.ReadHoldingRegisters(slaveID, 3, 1);
                if (resFlow != null && resFlow.Length >= 5)
                {
                    ushort rawFlow = (ushort)((resFlow[3] << 8) | resFlow[4]);
                    double correctedFlow = rawFlow;

                    // Hassasiyet Ayarı (if-else mantığınla)
                    if (dpValue == 1) correctedFlow = rawFlow / 10.0;
                    else if (dpValue == 2) correctedFlow = rawFlow / 100.0;
                    else if (dpValue == 3) correctedFlow = rawFlow / 1000.0;

                    lblFlowRate.Text = correctedFlow.ToString("F" + dpValue);
                }

                // --- 3. ORTALAMA DEBİ (Adres 2) ---
                byte[] resAvg = master.ReadHoldingRegisters(slaveID, 2, 1);
                if (resAvg != null && resAvg.Length >= 5)
                {
                    ushort rawAvg = (ushort)((resAvg[3] << 8) | resAvg[4]);
                    double correctedAvg = rawAvg;

                    // Hassasiyet Ayarı (Burası da aynı kurala tabi)
                    if (dpValue == 1) correctedAvg = rawAvg / 10.0;
                    else if (dpValue == 2) correctedAvg = rawAvg / 100.0;
                    else if (dpValue == 3) correctedAvg = rawAvg / 1000.0;

                    lblOrtalamaDebi.Text = correctedAvg.ToString("F" + dpValue);
                }

                // --- 4. AĞIRLIK OKU (Adres 4 - Sadece Okunabilir) ---
                byte[] resWeight = master.ReadHoldingRegisters(slaveID, 4, 1);

                if (resWeight != null && resWeight.Length >= 5)
                {
                    // Register birleştirme (High Byte << 8 | Low Byte)
                    ushort anlikAgirlik = (ushort)((resWeight[3] << 8) | resWeight[4]);

                    // Ekranda gösteriyoruz
                    lblAnlikAgirlik.Text = anlikAgirlik.ToString() + " kg";
                }
                else
                {
                    // Hoca: "0 olduğunu anladığında eski haline dönsün" dediği için
                    // Bağlantı koparsa veya veri sıfırsa "0 kg" yazdırıyoruz
                    lblAnlikAgirlik.Text = "0 kg";
                }
                //5.BANT HIZINI OKU (Adres 5)
                byte[] resSpeed = master.ReadHoldingRegisters(slaveID, 5, 1);
                if (resSpeed != null && resSpeed.Length >= 5)
                {
                    ushort speedValue = (ushort)((resSpeed[3] << 8) | resSpeed[4]);
                    lblBeltSpeed.Text = "%" + speedValue.ToString();
                }
                // --- 6. KONTROL ÇIKIŞI OKU (Adres 6 - Sadece Okunabilir) ---
                byte[] resControl = master.ReadHoldingRegisters(slaveID, 6, 1);

                if (resControl != null && resControl.Length >= 5)
                {
                    // Register birleştirme
                    ushort kontrolCikisi = (ushort)((resControl[3] << 8) | resControl[4]);
                    if(kontrolCikisi<=1000 && kontrolCikisi>=0) lblControlOut.Text = kontrolCikisi.ToString();
                    else lblControlOut.Text = "0";
                }
                else
                {
                    // Hoca: "0 olduğunu anladığında eski haline dönsün" dediği için
                    // Veri kesilirse veya cihaz kapalıysa "0" yazdırıyoruz
                    lblControlOut.Text = "0";
                }


                // --- 7. TOPLAYICI-1 OKU (Adres 7-8) [Not 1: 32-Bit] ---
                byte[] resTotal1 = master.ReadHoldingRegisters(slaveID, 7, 2);
                if (resTotal1 != null && resTotal1.Length >= 7)
                {
                    int low1 = (resTotal1[3] << 8) | resTotal1[4];
                    int high1 = (resTotal1[5] << 8) | resTotal1[6];
                    Int32 toplamMal1 =  low1 | (high1 << 16);

                    lblTotalWeight1.Text = toplamMal1.ToString() + " kg";
                }
                else
                {
                    lblTotalWeight1.Text = "0 kg"; // Hoca şartı: Veri yoksa 0
                }

                // --- 8. TOPLAYICI-2 OKU (Adres 9-10) [Not 1: 32-Bit] ---
                // Bu sende yoktu, tabloya göre EKLENDİ.
                byte[] resTotal2 = master.ReadHoldingRegisters(slaveID, 9, 2);
                if (resTotal2 != null && resTotal2.Length >= 7)
                {
                    int low2 = (resTotal2[3] << 8) | resTotal2[4];
                    int high2 = (resTotal2[5] << 8) | resTotal2[6];
                    Int32 toplamMal2 = low2 | (high2 << 16);

                    lblTotalWeight2.Text = toplamMal2.ToString() + " kg";
                }
                else
                {
                    lblTotalWeight2.Text = "0 kg"; // Hoca şartı: Veri yoksa 0
                }
                // --- 9. AĞIRLIK SET DEĞERİ OKU (Adres 11) ---
                byte[] resSetWeight = master.ReadHoldingRegisters(slaveID, 11, 1);
                if (resSetWeight != null && resSetWeight.Length >= 5)
                {
                    ushort agirlikSet = (ushort)((resSetWeight[3] << 8) | resSetWeight[4]);
                    lblAgirlikSet.Text = agirlikSet.ToString() + " kg";
                }
                else
                {
                    lblAgirlikSet.Text = "0 kg"; // Veri yoksa sıfıra çek
                }

                // --- 10. GEÇEN MAL TOPLAMI OKU (Adres 12) ---
                byte[] resPassedTotal = master.ReadHoldingRegisters(slaveID, 12, 1);
                if (resPassedTotal != null && resPassedTotal.Length >= 5)
                {
                    ushort gecenMal = (ushort)((resPassedTotal[3] << 8) | resPassedTotal[4]);
                    lblGecenToplam.Text = gecenMal.ToString() + " kg";
                }
                else
                {
                    lblGecenToplam.Text = "0 kg";
                }
                // --- 9. ÖN BESLEYİCİ ÇIKIŞI OKU (Adres 13) ---
                byte[] resPreFeeder = master.ReadHoldingRegisters(slaveID, 13, 1);

                if (resPreFeeder != null && resPreFeeder.Length >= 5)
                {
                    ushort onBesleyiciCikis = (ushort)((resPreFeeder[3] << 8) | resPreFeeder[4]);

                    // Tablo kısıtı: 0-1000 arası olmalı
                    if (onBesleyiciCikis <= 1000 && onBesleyiciCikis >= 0)
                    {
                        lblOnBesleyiciCikis.Text = onBesleyiciCikis.ToString();
                    }
                    else
                    {
                        lblOnBesleyiciCikis.Text = "0"; // Hatalı veri gelirse 0 yap
                    }
                }
                else
                {
                    // Hoca şartı: Veri kesilirse eski haline (0) dönsün
                    lblOnBesleyiciCikis.Text = "0";
                }

                //10. GİRİŞLERİ OKU (Adres 14) --
                byte[] resInput = master.ReadHoldingRegisters(slaveID, 14, 1);

                if (resInput != null && resInput.Length >= 5)
                {
                    ushort ınputValue = (ushort)((resInput[3] << 8) | resInput[4]);

                    // Bit kontrolü yaparak durumu 
                    bool calis = (ınputValue & 0x0001) != 0; // Bit 0
                    bool onSistemCalis = (ınputValue & 0x0002) != 0; // Bit 1
                    bool motorCalis = (ınputValue & 0x0004) != 0; // Bit 2
                    bool motorAriza = (ınputValue & 0x0008) != 0; // Bit 3
                    bool onBesleyiciCalis = (ınputValue & 0x0010) != 0;      // Bit 4
                    bool onBesleyiciAriza = (ınputValue & 0x0020) != 0;       // Bit 5
                    bool uzakCalisma = (ınputValue & 0x0040) != 0; // Bit 6
                    bool yakınCalisma = (ınputValue & 0x0080) != 0; // Bit 7
                    bool toplayiciSifirla = (ınputValue & 0x0100) != 0; // Bit 8
                    bool arizaSil = (ınputValue & 0x0200) != 0; // Bit 9
                    bool kullanılmıyor1 = (ınputValue & 0x0400) != 0; // Bit 10
                    bool kullanılmıyor2 = (ınputValue & 0x0800) != 0; // Bit 11
                    bool kalibrasyonBasla = (ınputValue & 0x1000) != 0; // Bit 12
                    bool kalibrasyonKabul = (ınputValue & 0x2000) != 0; // Bit 13
                    bool kalibrasyonRed = (ınputValue & 0x4000) != 0; // Bit 14
                    bool dolumDurduruldu = (ınputValue & 0x8000) != 0; // Bit 15

                    if (calis)
                    {

                        lblGiris0.BackColor = Color.Green;
                    }
                    else
                    {
                        lblGiris0.BackColor = Color.Gray;
                    }
                    if (onSistemCalis)
                    {

                        lblGiris1.BackColor = Color.Green;

                    }
                    else
                    {
                        lblGiris1.BackColor = Color.Gray;
                    }
                    if (motorCalis)
                    {

                        lblGiris2.BackColor = Color.Green;

                    }
                    else
                    {
                        lblGiris2.BackColor = Color.Gray;
                    }
                    if (motorAriza)
                    {

                        lblGiris3.BackColor = Color.Red;

                    }
                    else
                    {
                        lblGiris3.BackColor = Color.Gray;
                    }
                    if (onBesleyiciCalis)
                    {

                        lblGiris4.BackColor = Color.Green;

                    }
                    else
                    {
                        lblGiris4.BackColor = Color.Gray;
                    }

                    if (onBesleyiciAriza)
                    {
                        lblGiris5.BackColor = Color.Red;
                    }
                    else
                    {
                        lblGiris5.BackColor = Color.Gray;
                    }

                    if (uzakCalisma)
                    {

                        lblGiris6.BackColor = Color.Green;

                    }
                    else
                    {
                        lblGiris6.BackColor = Color.Gray;
                    }
                    if (yakınCalisma)
                    {

                        lblGiris7.BackColor = Color.Green;

                    }
                    else
                    {
                        lblGiris7.BackColor = Color.Gray;
                    }
                    if (toplayiciSifirla)
                    {

                        lblGiris8.BackColor = Color.Green;

                    }
                    else
                    {
                        lblGiris8.BackColor = Color.Gray;
                    }
                    if (arizaSil)
                    {

                        lblGiris9.BackColor = Color.Green;

                    }
                    else
                    {
                        lblGiris9.BackColor = Color.Gray;
                    }
                    if (kalibrasyonBasla)
                    {

                        lblGiris12.BackColor = Color.Green;

                    }
                    else
                    {
                        lblGiris12.BackColor = Color.Gray;
                    }
                    if (kalibrasyonKabul)
                    {

                        lblGiris13.BackColor = Color.Green;

                    }
                    else
                    {
                        lblGiris13.BackColor = Color.Gray;
                    }
                    if (kalibrasyonRed)
                    {

                        lblGiris14.BackColor = Color.Red;

                    }
                    else
                    {
                        lblGiris14.BackColor = Color.Gray;
                    }
                    if (dolumDurduruldu)
                    {

                        lblGiris15.BackColor = Color.Red;

                    }
                    else
                    {
                        lblGiris15.BackColor = Color.Gray;
                    }
                    if (kullanılmıyor1 && kullanılmıyor2)
                    {

                        lblGiris10.BackColor = Color.Gray;
                        lblGiris11.BackColor = Color.Gray;
                    }


                }

                //11.ÇIKISLARI OKU (Adres 15)--
                byte[] resOut = master.ReadHoldingRegisters(slaveID, 15, 1);

                if (resOut != null && resOut.Length >= 5)
                {
                    ushort outValue = (ushort)((resOut[3] << 8) | resOut[4]);

                    // Bit kontrolü yaparak durumu 
                    bool toleransAriza = (outValue & 0x0001) != 0; // Bit 0
                    bool agirlikAriza = (outValue & 0x0002) != 0; // Bit 1
                    bool acilDur = (outValue & 0x0004) != 0; // Bit 2
                    bool bandKaydiAriza = (outValue & 0x0008) != 0; // Bit 3
                    bool hizAriza = (outValue & 0x0010) != 0;      // Bit 4
                    bool debiAriza = (outValue & 0x0020) != 0;       // Bit 5
                    bool onBesleyiciCalis = (outValue & 0x0040) != 0; // Bit 6
                    bool calis = (outValue & 0x0080) != 0; // Bit 7
                    bool kullanılmıyor = (outValue & 0x0100) != 0; // Bit 8
                    bool patinajAriza = (outValue & 0x0200) != 0; // Bit 9
                    bool kalibrasyonRedKabulBekleme = (outValue & 0x0400) != 0; // Bit 10
                    bool kalibrasyonTolerans = (outValue & 0x0800) != 0; // Bit 11
                    bool kalibrasyonBasladi = (outValue & 0x1000) != 0; // Bit 12
                    bool motorAriza = (outValue & 0x2000) != 0; // Bit 13
                    bool sifirAriza = (outValue & 0x4000) != 0; // Bit 14
                    bool remote = (outValue & 0x8000) != 0; // Bit 15

                    if (toleransAriza)
                    {

                        lblCikis0.BackColor = Color.Red;
                    }
                    else
                    {
                        lblCikis0.BackColor = Color.Gray;
                    }
                    if (agirlikAriza)
                    {

                        lblCikis1.BackColor = Color.Red;

                    }
                    else
                    {
                        lblCikis1.BackColor = Color.Gray;
                    }
                    if (acilDur)
                    {

                        lblCikis2.BackColor = Color.Red;

                    }
                    else
                    {
                        lblCikis2.BackColor = Color.Gray;
                    }
                    if (bandKaydiAriza)
                    {

                        lblCikis3.BackColor = Color.Red;

                    }
                    else
                    {
                        lblCikis3.BackColor = Color.Gray;
                    }
                    if (hizAriza)
                    {

                        lblCikis4.BackColor = Color.Red;

                    }
                    else
                    {
                        lblCikis4.BackColor = Color.Gray;
                    }

                    if (debiAriza)
                    {
                        lblCikis5.BackColor = Color.Red;
                    }
                    else
                    {
                        lblCikis5.BackColor = Color.Gray;
                    }

                    if (onBesleyiciCalis)
                    {

                        lblCikis6.BackColor = Color.Red;

                    }
                    else
                    {
                        lblCikis6.BackColor = Color.Gray;
                    }
                    if (calis)
                    {

                        lblCikis7.BackColor = Color.Red;

                    }
                    else
                    {
                        lblCikis7.BackColor = Color.Gray;
                    }
                    if (patinajAriza)
                    {

                        lblCikis9.BackColor = Color.Red;

                    }
                    else
                    {
                        lblCikis9.BackColor = Color.Gray;
                    }
                    if (kalibrasyonRedKabulBekleme)
                    {

                        lblCikis10.BackColor = Color.Red;

                    }
                    else
                    {
                        lblCikis10.BackColor = Color.Gray;
                    }
                    if (kalibrasyonTolerans)
                    {

                        lblCikis11.BackColor = Color.Red;

                    }
                    else
                    {
                        lblCikis11.BackColor = Color.Gray;
                    }
                    if (kalibrasyonBasladi)
                    {

                        lblCikis12.BackColor = Color.Red;

                    }
                    else
                    {
                        lblCikis12.BackColor = Color.Gray;
                    }
                    if (motorAriza)
                    {

                        lblCikis13.BackColor = Color.Red;

                    }
                    else
                    {
                        lblCikis13.BackColor = Color.Gray;
                    }
                    if (sifirAriza )
                    {

                        lblCikis14.BackColor = Color.Red;
                        
                    }
                    else
                    {
                        lblCikis14.BackColor = Color.Gray;
                    }
                    if (remote)
                    {
                        lblCikis15.BackColor = Color.Green;
                    }
                    else
                    {
                        lblCikis15.BackColor = Color.Gray;
                    }
                    if (kullanılmıyor)
                    {

                        lblCikis8.BackColor = Color.Gray;

                    }


                }


                //12. STATUS OKU (Adres 16)--
                byte[] resStatus = master.ReadHoldingRegisters(slaveID, 16, 1);

                if (resStatus != null && resStatus.Length >= 5)
                {
                    ushort statusValue = (ushort)((resStatus[3] << 8) | resStatus[4]);

                    // Bit kontrolü yaparak durumu 
                    bool sistemCalisiyor = (statusValue & 0x0001) != 0; // Bit 0
                    bool yakıncalısmadurumunda = (statusValue & 0x0002) != 0; // Bit 1
                    bool ManuelDurumda = (statusValue & 0x0004) != 0; // Bit 2
                    bool KalibrasyonDurumunda = (statusValue & 0x0008) != 0; // Bit 3
                    bool bantKaydiArızası = (statusValue & 0x0010) != 0;      // Bit 4
                    bool acilDurDurumu = (statusValue & 0x0020) != 0;       // Bit 5
                    bool AğırlıkArıza = (statusValue & 0x0040) != 0; // Bit 6
                    bool SıfırArıza = (statusValue & 0x0080) != 0; // Bit 7
                    bool HızArıza = (statusValue & 0x0100) != 0; // Bit 8
                    bool DebiArıza = (statusValue & 0x0200) != 0; // Bit 9
                    bool NormalCalısmaEngel = (statusValue & 0x0400) != 0; // Bit 10
                    bool ToleransArıza = (statusValue & 0x0800) != 0; // Bit 11
                    bool SayıcıDarbesi = (statusValue & 0x1000) != 0; // Bit 12
                    bool MotorArıza = (statusValue & 0x2000) != 0; // Bit 13
                    bool Kullanılmıyo1 = (statusValue & 0x4000) != 0; // Bit 14
                    bool Kullanılmıyor2 = (statusValue & 0x8000) != 0; // Bit 15

                    if (sistemCalisiyor)
                    {

                        lblStatus0.BackColor = Color.Green;
                    }
                    else
                    {
                        lblStatus0.BackColor = Color.Gray;
                    }
                    if (yakıncalısmadurumunda)
                    {

                        lblStatus1.BackColor = Color.Yellow;

                    }
                    else
                    {
                        lblStatus1.BackColor = Color.Gray;
                    }
                    if (ManuelDurumda)
                    {

                        lblStatus2.BackColor = Color.Yellow;

                    }
                    else
                    {
                        lblStatus2.BackColor = Color.Gray;
                    }
                    if (KalibrasyonDurumunda)
                    {

                        lblStatus3.BackColor = Color.Yellow;

                    }
                    else
                    {
                        lblStatus3.BackColor = Color.Gray;
                    }
                    if (bantKaydiArızası)
                    {

                        lblStatus4.BackColor = Color.Red;

                    }
                    else
                    {
                        lblStatus4.BackColor = Color.Gray;
                    }

                    if (acilDurDurumu)
                    {
                        lblStatus5.BackColor = Color.Red;
                    }
                    else
                    {
                        lblStatus5.BackColor = Color.Gray;
                    }

                    if (AğırlıkArıza)
                    {

                        lblStatus6.BackColor = Color.Red;

                    }
                    else
                    {
                        lblStatus6.BackColor = Color.Gray;
                    }
                    if (SıfırArıza)
                    {

                        lblStatus7.BackColor = Color.Red;

                    }
                    else
                    {
                        lblStatus7.BackColor = Color.Gray;
                    }
                    if (HızArıza)
                    {

                        lblStatus8.BackColor = Color.Red;

                    }
                    else
                    {
                        lblStatus8.BackColor = Color.Gray;
                    }
                    if (DebiArıza)
                    {

                        lblStatus9.BackColor = Color.Red;

                    }
                    else
                    {
                        lblStatus9.BackColor = Color.Gray;
                    }
                    if (NormalCalısmaEngel)
                    {

                        lblStatus10.BackColor = Color.Red;

                    }
                    else
                    {
                        lblStatus10.BackColor = Color.Gray;
                    }
                    if (ToleransArıza)
                    {

                        lblStatus11.BackColor = Color.Red;

                    }
                    else
                    {
                        lblStatus11.BackColor = Color.Gray;
                    }
                    if (SayıcıDarbesi)
                    {

                        lblStatus12.BackColor = Color.Yellow;

                    }
                    else
                    {
                        lblStatus12.BackColor = Color.Gray;
                    }
                    if (MotorArıza)
                    {

                        lblStatus13.BackColor = Color.Red;

                    }
                    else
                    {
                        lblStatus13.BackColor = Color.Gray;
                    }
                    if (Kullanılmıyo1 && Kullanılmıyor2)
                    {

                        lblStatus14.BackColor = Color.Gray;
                        lblStatus15.BackColor = Color.Gray;
                    }

                    
                }

                // --- 13. BUNKER KALİBRASYON VERİLERİ (Adres 18, 19, 20) ---

                // Verileri oku
                byte[] res18 = master.ReadHoldingRegisters(slaveID, 18, 1);
                byte[] res19 = master.ReadHoldingRegisters(slaveID, 19, 1);
                byte[] res20 = master.ReadHoldingRegisters(slaveID, 20, 1);

                if (res18 != null && res19 != null && res20 != null)
                {
                    ushort v18 = (ushort)((res18[3] << 8) | res18[4]);
                    ushort v19 = (ushort)((res19[3] << 8) | res19[4]);
                    ushort v20 = (ushort)((res20[3] << 8) | res20[4]);

                    // EĞER herhangi bir veri geliyorsa (0 değilse) AKTİF yap
                    if (v18 > 0 || v19 > 0 || v20 > 0)
                    {
                        // Renkleri canlandır
                        lblBunkerAgirlik.ForeColor = Color.Black;
                        lblBunkerAgirlikSet.ForeColor = Color.Black;
                        lblBunkerAgirlikKontrolCikis.ForeColor = Color.Black;

                        // Değerleri yazdır
                        lblBunkerAgirlik.Text =  v18.ToString();
                        lblBunkerAgirlikSet.Text = v19.ToString();
                        lblBunkerAgirlikKontrolCikis.Text = v20.ToString();
                        groupBunker.ForeColor = SystemColors.MenuText;
                        groupBunker.Text = "Bunker Sistemi (AKTİF)"; // Eğer bir GroupBox içindelerse
                    }
                    else
                    {
                        // PASİF Durumu: Veri 0 ise sönük göster
                        lblBunkerAgirlik.ForeColor = Color.Silver;
                        lblBunkerAgirlikSet.ForeColor = Color.Silver;
                        lblBunkerAgirlikKontrolCikis.ForeColor = Color.Silver;
                        groupBunker.ForeColor = Color.Silver;

                        lblBunkerAgirlik.Text = "-";
                        lblBunkerAgirlikSet.Text = "-";
                        lblBunkerAgirlikKontrolCikis.Text = "-";

                        groupBunker.Text = "Bunker Sistemi (Devre Dışı)";
                    }
                }

                //14.Bunker Otomati Kalibrasyon İşlemleri (21,22,23)
                //21.Adres Otomatık Kaalibrasyon  Statusu
                byte[] bunStatus = master.ReadHoldingRegisters(slaveID, 21, 1);
                if(bunStatus != null && bunStatus.Length>=5)
                {
                    ushort bunValue = (ushort)((bunStatus[3] << 8) | bunStatus[4]);

                    bool kalibrasyonIslemde = (bunValue & 0x0001) != 0; // Bit 0
                    bool kalibrasyonTolerans = (bunValue & 0x0002) != 0; // Bit 1
                    bool kalibrasyonToleransDisindaRetKabulBeklemede = (bunValue & 0x0004) != 0; // Bit 2
                    bool bunkerAgirlikAriza = (bunValue & 0x0008) != 0; // Bit 3
                    bool bunkerSeviyesiAltSeviyeninAltinda = (bunValue & 0x0010) != 0; // Bit 4
                    bool bunkerSeviyesiUstSeviyeninUzerinde = (bunValue & 0x0020) != 0;// Bit 5
                    bool kullanılmıyor1 = (bunValue & 0x0040) != 0;       // Bit 6
                    bool kullanılmıyor2 = (bunValue & 0x0080) != 0; // Bit 7

                    if(kalibrasyonIslemde)
                    {
                        lblKalibrasyonIsleminde.BackColor = Color.Green;
                    }
                    else
                    {
                        lblKalibrasyonIsleminde.BackColor = Color.Gray;
                    }
                    if(kalibrasyonTolerans)
                    {
                        lblKalibrasyonToleransIcinde.BackColor = Color.Green;
                    }
                    else
                    {
                        lblKalibrasyonToleransIcinde.BackColor= Color.Gray;
                    }
                    if(kalibrasyonToleransDisindaRetKabulBeklemede)
                    {
                        lblKalibrasyonToleransDisinda.BackColor = Color.Green;
                    }
                    else
                    {
                        lblKalibrasyonToleransDisinda.BackColor= Color.Gray;
                    }
                    if(bunkerAgirlikAriza)
                    {
                        lblBunkerAgırlikAriza.BackColor = Color.Green;
                    }
                    else
                    {
                        lblBunkerAgırlikAriza.BackColor= Color.Gray;
                    }
                    if(bunkerSeviyesiAltSeviyeninAltinda)
                    {
                        lblbunkerSeviyesiAltSeviyeAltinda.BackColor = Color.Green;
                    }
                    else
                    {
                        lblbunkerSeviyesiAltSeviyeAltinda.BackColor =Color.Gray;
                    }
                    if(bunkerSeviyesiUstSeviyeninUzerinde)
                    {
                        lblBunkerSeviyeUstSeviyeUstunde.BackColor = Color.Green;
                    }
                    else
                    {
                        lblBunkerSeviyeUstSeviyeUstunde.BackColor= Color.Gray;
                    }
                    if(kullanılmıyor1&&kullanılmıyor2)
                    {
                        lblKullanılmıyor1.ForeColor = Color.Silver;
                        lblKullanılmıyo2 .ForeColor = Color.Silver;
                    }
                    

                }
                //22.Adres Otomatık Kalıbrasyon bunkerden olusan mıktar kg
                byte[] bunWeight = master.ReadHoldingRegisters(slaveID, 22, 1);
                if(bunWeight != null && bunWeight.Length >=5)
                {
                    ushort bunkerAgirlik = (ushort)((bunWeight[3] << 8) | bunWeight[4]);
                    lblBunkerBosalanAgirlik.Text = bunkerAgirlik.ToString() + " kg";
                }
                else
                {
                    lblBunkerBosalanAgirlik.Text = "0 kg";
                }
                //23.Adres Otomatık Kalıbrasyon Bandın Tarttıgı mıktar kg
                byte[] bantWeight = master.ReadHoldingRegisters(slaveID, 23, 1);
                if (bantWeight != null && bantWeight.Length >= 5)
                {
                    ushort bantAgirlik = (ushort)((bantWeight[3] << 8) | bantWeight[4]);
                    lblBandTarttigiAgirlik.Text = bantAgirlik.ToString() + " kg";
                }
                else
                {
                    lblBandTarttigiAgirlik.Text = "0 kg";
                }

                //15 31.ADRES KOMUT 1 
                // Adres 31'i oku ve buton renklerini cihazdaki gerçek değere göre düzelt
                byte[] res31Read = master.ReadHoldingRegisters(slaveID, 31, 1);
                if (res31Read != null && res31Read.Length >= 5)
                {
                    ushort v31 = (ushort)((res31Read[3] << 8) | res31Read[4]);
                    komutVerisi31 = v31; // Hafızadaki değeri cihazla eşitle

                    // Renkleri dışarıdan gelen veriye göre zorla güncelle
                    btnCalis.BackColor = (v31 & 0x0001) != 0 ? Color.Green : Color.Gray;
                    btnOnSistemCalis.BackColor = (v31 & 0x0002) != 0 ? Color.Green : Color.Gray;
                    btnMotorCalis.BackColor = (v31 & 0x0004) != 0 ? Color.Green : Color.Gray;
                    btnMotorAriza.BackColor = (v31 & 0x0008) != 0 ? Color.Red : Color.Gray;
                    btnOnBesleyiciCalis.BackColor = (v31 & 0x0010) != 0 ? Color.Green : Color.Gray;
                    btnOnBesleyiciAriza.BackColor = (v31 & 0x0020) != 0 ? Color.Red : Color.Gray;
                    btnUzakCalisma.BackColor = (v31 & 0x0040) != 0 ? Color.Green : Color.Gray;
                    btnYakinCalisma.BackColor = (v31 & 0x0080) != 0 ? Color.Green : Color.Gray;
                }
                //16 32.ADRES KOMUT 2
                byte[] res32Read = master.ReadHoldingRegisters(slaveID, 32, 1);
                if (res32Read != null && res32Read.Length >= 5)
                {
                    ushort v32 = (ushort)((res32Read[3] << 8) | res32Read[4]);
                    gonderilecekKomut32 = v32;

                    btnTotalSifirla.BackColor = (v32 & 1) != 0 ? Color.Orange : Color.Gray;
                    btnArizaSil.BackColor = (v32 & 2) != 0 ? Color.Green : Color.Gray;
                    btnKullanılmıyor1.BackColor = (v32 & 4) != 0 ? Color.Gray : Color.Gray;
                    btnKullanılmıyor2.BackColor = (v32 & 8) != 0 ? Color.Gray : Color.Gray;
                    btnKalibrasyonBasla.BackColor = (v32 & 16) != 0 ? Color.Yellow : Color.Gray;
                    btnKalibrasyonKabul.BackColor = (v32 & 32) != 0 ? Color.Green : Color.Gray;
                    btnKalibrasyonRed.BackColor = (v32 & 64) != 0 ? Color.Red : Color.Gray;
                    btnDolumDurdur.BackColor = (v32 & 128) != 0 ? Color.Red : Color.Gray;
                }
                //17 33.Adres Ağırlık Set
                // --- ADRES 30: HEDEF DEBİ ---
                byte[] res30 = master.ReadHoldingRegisters(slaveID, 30, 1);
                if (res30 != null && res30.Length >= 5)
                {
                    ushort v30 = (ushort)((res30[3] << 8) | res30[4]);
                    // Eğer kullanıcı kutuya tıklamamışsa (yazı yazmıyorsa) değeri güncelle
                    if (!is30Changed) txtTargetDose.Text = v30.ToString();
                    //if (!txtTargetDose.Focused) txtTargetDose.Text = v30.ToString();
                }

                // --- ADRES 33: AĞIRLIK SET DEĞERİ ---
                byte[] res33 = master.ReadHoldingRegisters(slaveID, 33, 1);
                if (res33 != null && res33.Length >= 5)
                {
                    ushort v33 = (ushort)((res33[3] << 8) | res33[4]);
                    //if (!txtAgirlikSet.Focused) txtAgirlikSet.Text = v33.ToString();
                    if (!is33Changed) txtAgirlikSet.Text = v33.ToString();
                }

                // --- ADRES 34: BUNKER AĞIRLIK SET DEĞERİ ---
                byte[] res34 = master.ReadHoldingRegisters(slaveID, 34, 1);
                if (res34 != null && res34.Length >= 5)
                {
                    ushort v34 = (ushort)((res34[3] << 8) | res34[4]);
                    //if (!txtBnkrAgirlikSetDeger.Focused) txtBnkrAgirlikSetDeger.Text = v34.ToString();
                    if (!is34Changed) txtBnkrAgirlikSetDeger.Text = v34.ToString();
                }



            }
            catch (Exception ex)
            {
                lstLogs.Items.Add($"[{DateTime.Now:HH:mm:ss}] Okuma Hatası: {ex.Message}");
                lstLogs.TopIndex = lstLogs.Items.Count - 1; // Otomatik aşağı kaydır
            }
        }

        private void txtTargetDose_TextChanged(object sender, EventArgs e)
        {
            if (txtTargetDose.Focused) is30Changed = true;
        }

        private void txtAgirlikSet_TextChanged(object sender, EventArgs e)
        {
            if (txtBnkrAgirlikSetDeger.Focused) is30Changed = true;
        }

        private void txtBnkrAgirlikSetDeger_TextChanged(object sender, EventArgs e)
        {
            if (txtBnkrAgirlikSetDeger.Focused) is30Changed = true;
        }
    }
}
