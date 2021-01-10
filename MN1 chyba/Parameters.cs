using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSBibMatStudent.RownaniaNieliniowe;

namespace MN1_chyba
{
    public class Parameters
    {
        public double E1 { get; set; }
        public double E2 { get; set; }

        public double R1 { get; set; }
        public double R2 { get; set; }
        public double R3 { get; set; }

        public double a { get; set; }
        public double b { get; set; }

        public double[] X { get; set; } = new double[3 + 1] { 0, 1, -1, -1 };
        public double[] I { get; set; } = new double[3 + 1];
        
        public int[] RN_type { get; set; }
        public RowNieLinMetodaNewtona RNL { get; set; }

        //macierz F
        public double[] CreateMatrix(double[] I)
        {
            double[] F = new double[3 + 1];
            F[1] = E1 - R1 * I[1] - UN(I[1], 1) - R2 * I[2] - UN(I[2], 2);
            F[2] = E2 + UN(I[2], 2) + R2 * I[2] - R3 * I[3] - UN(I[3], 3);
            F[3] = I[1] - I[2] - I[3];

            return F;
        }

        //wyliczenie wartości napięcia dla elementu nieliniowego
        public double UN(double I, int index)
        {
            int type = RN_type[index - 1];

            switch (type)
            {
                case 1: return I * I * a;
                case 2: return b * Math.Sqrt(Math.Abs(I));
                case 3: return Math.Sin(I * a) - Math.Cos(I / b);
            }
            return 0;
        }
    }
}
