namespace ModbusLibrary
{
    partial class Deneme1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnConnect = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnStartAutoRead = new System.Windows.Forms.Button();
            this.lblValue1 = new System.Windows.Forms.Label();
            this.lblValue2 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnWriteMltp = new System.Windows.Forms.Button();
            this.txtValMltp2 = new System.Windows.Forms.TextBox();
            this.txtValMltp1 = new System.Windows.Forms.TextBox();
            this.txtValMltp3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(33, 219);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(399, 68);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(33, 27);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(399, 186);
            this.listBox1.TabIndex = 1;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(33, 308);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(147, 62);
            this.btnRead.TabIndex = 2;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(285, 308);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(147, 62);
            this.btnWrite.TabIndex = 3;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(302, 376);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(100, 20);
            this.txtValue.TabIndex = 4;
            this.txtValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValMltp3_KeyPress);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(302, 444);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(100, 20);
            this.txtAddress.TabIndex = 5;
            this.txtAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValMltp3_KeyPress);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnStartAutoRead
            // 
            this.btnStartAutoRead.Location = new System.Drawing.Point(550, 36);
            this.btnStartAutoRead.Name = "btnStartAutoRead";
            this.btnStartAutoRead.Size = new System.Drawing.Size(147, 62);
            this.btnStartAutoRead.TabIndex = 6;
            this.btnStartAutoRead.Text = "AutoRead";
            this.btnStartAutoRead.UseVisualStyleBackColor = true;
            this.btnStartAutoRead.Click += new System.EventHandler(this.btnStartAutoRead_Click);
            // 
            // lblValue1
            // 
            this.lblValue1.AutoSize = true;
            this.lblValue1.Location = new System.Drawing.Point(547, 153);
            this.lblValue1.Name = "lblValue1";
            this.lblValue1.Size = new System.Drawing.Size(35, 13);
            this.lblValue1.TabIndex = 7;
            this.lblValue1.Text = "label1";
            // 
            // lblValue2
            // 
            this.lblValue2.AutoSize = true;
            this.lblValue2.Location = new System.Drawing.Point(662, 153);
            this.lblValue2.Name = "lblValue2";
            this.lblValue2.Size = new System.Drawing.Size(35, 13);
            this.lblValue2.TabIndex = 8;
            this.lblValue2.Text = "label2";
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(438, 251);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.IsXValueIndexed = true;
            series2.Legend = "Legend1";
            series2.Name = "Sicaklik";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(439, 222);
            this.chart1.TabIndex = 9;
            this.chart1.Text = "chart1";
            // 
            // btnWriteMltp
            // 
            this.btnWriteMltp.Location = new System.Drawing.Point(786, 141);
            this.btnWriteMltp.Name = "btnWriteMltp";
            this.btnWriteMltp.Size = new System.Drawing.Size(190, 61);
            this.btnWriteMltp.TabIndex = 10;
            this.btnWriteMltp.Text = "WriteMultiple";
            this.btnWriteMltp.UseVisualStyleBackColor = true;
            this.btnWriteMltp.Click += new System.EventHandler(this.btnWriteMltp_Click);
            // 
            // txtValMltp2
            // 
            this.txtValMltp2.Location = new System.Drawing.Point(824, 74);
            this.txtValMltp2.Name = "txtValMltp2";
            this.txtValMltp2.Size = new System.Drawing.Size(100, 20);
            this.txtValMltp2.TabIndex = 11;
            this.txtValMltp2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValMltp3_KeyPress);
            // 
            // txtValMltp1
            // 
            this.txtValMltp1.Location = new System.Drawing.Point(824, 48);
            this.txtValMltp1.Name = "txtValMltp1";
            this.txtValMltp1.Size = new System.Drawing.Size(100, 20);
            this.txtValMltp1.TabIndex = 12;
            this.txtValMltp1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValMltp3_KeyPress);
            // 
            // txtValMltp3
            // 
            this.txtValMltp3.Location = new System.Drawing.Point(824, 100);
            this.txtValMltp3.Name = "txtValMltp3";
            this.txtValMltp3.Size = new System.Drawing.Size(100, 20);
            this.txtValMltp3.TabIndex = 13;
            this.txtValMltp3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValMltp3_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(745, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "1.Değer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(745, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "2.Değer";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(745, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "3.Değer";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(30, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(57, 13);
            this.lblStatus.TabIndex = 17;
            this.lblStatus.Text = "Bağlı Değil";
            // 
            // Deneme1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 572);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtValMltp3);
            this.Controls.Add(this.txtValMltp1);
            this.Controls.Add(this.txtValMltp2);
            this.Controls.Add(this.btnWriteMltp);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.lblValue2);
            this.Controls.Add(this.lblValue1);
            this.Controls.Add(this.btnStartAutoRead);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnConnect);
            this.Name = "Deneme1";
            this.Text = "Deneme1";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnStartAutoRead;
        private System.Windows.Forms.Label lblValue1;
        private System.Windows.Forms.Label lblValue2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button btnWriteMltp;
        private System.Windows.Forms.TextBox txtValMltp2;
        private System.Windows.Forms.TextBox txtValMltp1;
        private System.Windows.Forms.TextBox txtValMltp3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStatus;
    }
}