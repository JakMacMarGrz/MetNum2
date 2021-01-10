using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MN1_chyba
{
    public partial class PlotCh : Form
    {
        public PlotCh(Parameters _par)
        {
            InitializeComponent();
            par = _par;

            comboBox_element.SelectedIndex = 0;
            label_Imin_value.Text = "- 0.5";
            label_Imax_value.Text = "0.5";
        }

        Parameters par;

        //wykreślenie charakterystyki
        private void button_plot_Click(object sender, EventArgs e)
        {
            int index = comboBox_element.SelectedIndex;

            chart.Series.Clear();
            chart.Series.Add(comboBox_element.SelectedItem.ToString());
            chart.Series[0].ChartType = SeriesChartType.Spline;
            chart.ChartAreas[0].AxisX.Title = "I [A]";
            chart.ChartAreas[0].AxisY.Title = "A [V]";

            for (double i = -((double)trackBar_Imin.Value) / 10; i <= ((double)trackBar_Imax.Value) / 10; i = i + 0.01) 
            {
                chart.Series[0].Points.AddXY(i, par.UN(i, index + 1));
            }
        }

        //zmiana wyświetlanej wartości zakresu
        private void trackBar_Imin_Scroll(object sender, EventArgs e)
        {
            label_Imin_value.Text = "- " + ((double)trackBar_Imin.Value / 10).ToString("N1");
        }

        private void trackBar_Imax_Scroll(object sender, EventArgs e)
        {
            label_Imax_value.Text = "- " + ((double)trackBar_Imax.Value / 10).ToString("N1");
        }
    }
}
