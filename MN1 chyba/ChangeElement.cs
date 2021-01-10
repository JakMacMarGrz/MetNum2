using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MN1_chyba
{
    public partial class ChangeElement : Form
    {
        public ChangeElement(int _index, int _value)
        {
            InitializeComponent();

            //przekazanie parametrów do nowego formularza
            index = _index;
            value = _value;
        }

        public int value;
        private int index;
        public bool isUpdated = false;

        private void ChangeElement_Load(object sender, EventArgs e)
        {
            label_RN.Text = "RN" + (index + 1).ToString();
            comboBox_elementType.SelectedIndex = value - 1;
        }

        //przyciski funkcyjne
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            value = comboBox_elementType.SelectedIndex;
            isUpdated = true;
            this.Close();
        }
    }
}
