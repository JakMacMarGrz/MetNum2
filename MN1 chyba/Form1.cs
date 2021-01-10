using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSBibMatStudent.AlgebraLiniowa;
using CSBibMatStudent.Complex;
using CSBibMatStudent.RownaniaNieliniowe;

namespace MN1_chyba
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //przypisanie wartości domyślnych dla przedrostków, 
            //pozostałe wartości zostały zmienione w edytorze formularza
            comboBox_E1_multi.SelectedIndex = 2;
            comboBox_E2_multi.SelectedIndex = 2;
            comboBox_R1_multi.SelectedIndex = 2;
            comboBox_R2_multi.SelectedIndex = 2;
            comboBox_R3_multi.SelectedIndex = 2;
            comboBox_a_multi.SelectedIndex = 2;
            comboBox_b_multi.SelectedIndex = 2;
        }

        Parameters par;
        int[] RN_type = { 1, 2, 3 };

        private void button_calculate_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        //odczyt oraz konwesja danych z formularza
        private double ReadData(string key)
        {
            //utworzenie odwołań do kontrolek
            Label labelName = (Label)Controls.Find("label_" + key + "_val", true).FirstOrDefault();
            TextBox textBoxValue = (TextBox)Controls.Find("textBox_" + key + "_Value", true).FirstOrDefault();
            ComboBox comboBoxMulti = (ComboBox)Controls.Find("comboBox_" + key + "_multi", true).FirstOrDefault();

            double value;

            //odczyt wartości oraz sprawdzenie czy jest to liczba
            try
            {
                value = double.Parse(textBoxValue.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Wartość jednego z elementów nie jest poprawna (nie jest liczbą)",
                "Błędna wartość parametru - " + key, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return double.NaN;
            }

            //sprawdzenie czy wartość nie jest ujemna (poza wartościami źródeł)
            if (key == "R1" || key == "R2" || key == "R3")
            {
                if (value < 0)
                {
                    MessageBox.Show("Wartość jednego z elementów nie jest poprawna (jest ujemna)",
                    "Błędna wartość parametru - " + key, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return double.NaN;
                }
            }

            //uwzględnienie przedrostka w wyniki
            value = Multi(comboBoxMulti.SelectedIndex, value);

            return value;
        }

        //pomnożenie wyniku o przedrostek
        private double Multi(int index, double value)
        {
            switch (index)
            {
                case 0: value *= 1000000; break;
                case 1: value *= 1000; break;
                case 2: break;
                case 3: value /= 1000; break;
                case 4: value /= 1000000; break;
                case 5: value /= 1000000000; break;
                case 6: value /= 1000000000000; break;
            }
            return value;
        }

        //odczyt oraz zapis wartości do obiektu klasy Parameters
        private void LoadData()
        {
            par = new Parameters();

            //odczyt źródeł
            double foo = ReadData("E1");
            if (double.IsNaN(foo)) return;
            par.E1 = foo;

            foo = ReadData("E2");
            if (double.IsNaN(foo)) return;
            par.E2 = foo;

            //odczyt rezystancji
            foo = ReadData("R1");
            if (double.IsNaN(foo)) return;
            par.R1 = foo;

            foo = ReadData("R2");
            if (double.IsNaN(foo)) return;
            par.R2 = foo;

            foo = ReadData("R3");
            if (double.IsNaN(foo)) return;
            par.R3 = foo;


            //odczyt wartości parametrów
            foo = ReadData("a");
            if (double.IsNaN(foo)) return;
            par.a = foo;

            foo = ReadData("b");
            if (double.IsNaN(foo)) return;
            par.b = foo;

            CalculateResults();
        }

        //utworzenie macierzy i obliczenie rozpływu prądów
        private void CalculateResults()
        {
            par.RN_type = RN_type;

            par.RNL = new RowNieLinMetodaNewtona(par.CreateMatrix, 3, par.X, 1E-8, 1E-8, 1E-20, 1000);

            int status = par.RNL.Rozwiaz();
            if (status != 0)
            {
                MessageBox.Show("Wystąpił błąd podczas wykonywania obliczeń\nkod błędu: " + status.ToString(),
                "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //zapis rozpływu pradów w formularzu
            for (int i = 1; i <= 3; i++)
            {
                par.I[i] = par.RNL.X[i];
                Label l = (Label)Controls.Find("label_I" + i.ToString() + "_Value", true).FirstOrDefault();

                if (Math.Abs(par.I[i]) < 0.00001 || par.I[i] > 100000) l.Text = par.I[i].ToString("E5");
                else l.Text = par.I[i].ToString("N5");

                l.Text += " [A]";
            }

            //zapis błędu względnego
            double error = ((-par.I[1] + par.I[2] + par.I[3]) / par.I[1]) * 100;
            label_errorValue.Text = error.ToString("N3") + " [%]";

            ShowPower();
        }

        //obliczenie mocy czynnej oraz biernej dla źródeł oraz poszczególnych gałęzi/elementów
        private void ShowPower()
        {
            //źródła
            double powerIn = par.E1 * par.I[1] + par.E2 * par.I[3];
            label_E_power.Text = powerIn.ToString("N3") + " [W]";

            //gałąź I1
            double powerOut11 = par.R1 * par.I[1] * par.I[1];
            label_R1_power.Text = powerOut11.ToString("N3") + " [W]";

            double powerOut12 = par.UN(par.I[1], 1) * par.I[1];
            label_RN1_power.Text = powerOut12.ToString("N3") + " [W]";

            //gałąź I2
            double powerOut21 = par.R2 * par.I[2] * par.I[2];
            label_R2_power.Text = powerOut21.ToString("N3") + " [W]";

            double powerOut22 = par.UN(par.I[2], 2) * par.I[2];
            label_RN2_power.Text = powerOut22.ToString("N3") + " [W]";

            //gałąź I3
            double powerOut31 = par.R3 * par.I[3] * par.I[3];
            label_R3_power.Text = powerOut31.ToString("N3") + " [W]";

            double powerOut32 = par.UN(par.I[3], 3) * par.I[3];
            label_RN3_power.Text = powerOut32.ToString("N3") + " [W]";

            double powerOut = powerOut11 + powerOut12 + powerOut21 + powerOut22 + powerOut31 + powerOut32;
            label_R_power.Text = powerOut.ToString("N3") + " [W]";

            //błąd względny
            double error = ((powerOut - powerIn) / powerIn) * 100;

            label_errorPower.Text = error.ToString("N3") + " [%]";
        }

        //zmiana rodzaju elementu
        private void pictureBox_RN_DoubleClick(object sender, EventArgs e)
        {
            int index = int.Parse(((PictureBox)(sender)).Name[13].ToString()) - 1;

            ChangeElement change = new ChangeElement(index, RN_type[index]);
            change.ShowDialog();

            if (change.isUpdated == false) return;

            RN_type[index] = change.value + 1;
            Label label = (Label)Controls.Find("label_RN" + (index + 1) + "_type", true).FirstOrDefault();
            switch (change.value)
            {
                case 0:
                    label.Text = "I^2 * a"; break;
                case 1:
                    label.Text = "b * pierw(I)"; break;
                case 2:
                    label.Text = "sin(I*a) - cos(I/b)"; break;
            }
            
            ClearResults();
        }

        //wyczyszczenie dotychczasowych wyników
        private void textBox_E1_Value_TextChanged(object sender, EventArgs e)
        {
            ClearResults();
        }

        private void comboBox_E1_multi_SelectedValueChanged(object sender, EventArgs e)
        {
            ClearResults();
        }

        private void ClearResults()
        {
            label_R1_power.Text = "-"; label_R2_power.Text = "-"; label_R3_power.Text = "-"; label_R_power.Text = "-";
            label_I1_Value.Text = "-"; label_I2_Value.Text = "-"; label_I3_Value.Text = "-"; label_errorValue.Text = "-";

            label_RN1_power.Text = "-"; label_RN2_power.Text = "-"; label_RN3_power.Text = "-";
            label_E_power.Text = "-"; label_errorPower.Text = "-";
        }

        //wykreślanie charakterystyki
        private void button_plot_Click(object sender, EventArgs e)
        {
            if (label_I1_Value.Text == "-") return;

            PlotCh plot = new PlotCh(par);
            plot.ShowDialog();
        }
    }
}
