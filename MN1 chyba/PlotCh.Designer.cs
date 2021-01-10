namespace MN1_chyba
{
    partial class PlotCh
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.comboBox_element = new System.Windows.Forms.ComboBox();
            this.trackBar_Imin = new System.Windows.Forms.TrackBar();
            this.trackBar_Imax = new System.Windows.Forms.TrackBar();
            this.label_Imin_value = new System.Windows.Forms.Label();
            this.label_Imax_value = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_plot = new System.Windows.Forms.Button();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Imin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Imax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox_element
            // 
            this.comboBox_element.BackColor = System.Drawing.Color.White;
            this.comboBox_element.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_element.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBox_element.FormattingEnabled = true;
            this.comboBox_element.Items.AddRange(new object[] {
            "RN1",
            "RN2",
            "RN3"});
            this.comboBox_element.Location = new System.Drawing.Point(12, 12);
            this.comboBox_element.Name = "comboBox_element";
            this.comboBox_element.Size = new System.Drawing.Size(100, 28);
            this.comboBox_element.TabIndex = 49;
            // 
            // trackBar_Imin
            // 
            this.trackBar_Imin.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.trackBar_Imin.Location = new System.Drawing.Point(166, 12);
            this.trackBar_Imin.Maximum = 100;
            this.trackBar_Imin.Minimum = 1;
            this.trackBar_Imin.Name = "trackBar_Imin";
            this.trackBar_Imin.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.trackBar_Imin.Size = new System.Drawing.Size(128, 45);
            this.trackBar_Imin.TabIndex = 50;
            this.trackBar_Imin.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_Imin.Value = 5;
            this.trackBar_Imin.Scroll += new System.EventHandler(this.trackBar_Imin_Scroll);
            // 
            // trackBar_Imax
            // 
            this.trackBar_Imax.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.trackBar_Imax.Location = new System.Drawing.Point(428, 12);
            this.trackBar_Imax.Maximum = 100;
            this.trackBar_Imax.Minimum = 1;
            this.trackBar_Imax.Name = "trackBar_Imax";
            this.trackBar_Imax.Size = new System.Drawing.Size(128, 45);
            this.trackBar_Imax.TabIndex = 51;
            this.trackBar_Imax.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_Imax.Value = 5;
            this.trackBar_Imax.Scroll += new System.EventHandler(this.trackBar_Imax_Scroll);
            // 
            // label_Imin_value
            // 
            this.label_Imin_value.AutoSize = true;
            this.label_Imin_value.Font = new System.Drawing.Font("Arial", 15F);
            this.label_Imin_value.Location = new System.Drawing.Point(294, 9);
            this.label_Imin_value.Name = "label_Imin_value";
            this.label_Imin_value.Size = new System.Drawing.Size(17, 23);
            this.label_Imin_value.TabIndex = 52;
            this.label_Imin_value.Text = "-";
            this.label_Imin_value.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Imax_value
            // 
            this.label_Imax_value.AutoSize = true;
            this.label_Imax_value.Font = new System.Drawing.Font("Arial", 15F);
            this.label_Imax_value.Location = new System.Drawing.Point(379, 9);
            this.label_Imax_value.Name = "label_Imax_value";
            this.label_Imax_value.Size = new System.Drawing.Size(17, 23);
            this.label_Imax_value.TabIndex = 53;
            this.label_Imax_value.Text = "-";
            this.label_Imax_value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 15F);
            this.label2.Location = new System.Drawing.Point(343, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 46);
            this.label2.TabIndex = 54;
            this.label2.Text = "-\r\n[A]";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_plot
            // 
            this.button_plot.BackColor = System.Drawing.Color.Cornsilk;
            this.button_plot.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_plot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_plot.Location = new System.Drawing.Point(607, 12);
            this.button_plot.Name = "button_plot";
            this.button_plot.Size = new System.Drawing.Size(144, 28);
            this.button_plot.TabIndex = 55;
            this.button_plot.Text = "rysuj";
            this.button_plot.UseVisualStyleBackColor = false;
            this.button_plot.Click += new System.EventHandler(this.button_plot_Click);
            // 
            // chart
            // 
            chartArea2.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea2);
            this.chart.Location = new System.Drawing.Point(16, 53);
            this.chart.Name = "chart";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Name = "Series1";
            this.chart.Series.Add(series2);
            this.chart.Size = new System.Drawing.Size(767, 385);
            this.chart.TabIndex = 56;
            this.chart.Text = "chart1";
            // 
            // PlotCh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.button_plot);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_Imax_value);
            this.Controls.Add(this.label_Imin_value);
            this.Controls.Add(this.trackBar_Imax);
            this.Controls.Add(this.trackBar_Imin);
            this.Controls.Add(this.comboBox_element);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlotCh";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Charakterystyka prądowo - napięciowa";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Imin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Imax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_element;
        private System.Windows.Forms.TrackBar trackBar_Imin;
        private System.Windows.Forms.TrackBar trackBar_Imax;
        private System.Windows.Forms.Label label_Imin_value;
        private System.Windows.Forms.Label label_Imax_value;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_plot;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
    }
}