using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSBibMatStudent.MinFunBO
{
    public static class FunkcjeWieluZmiennych
    {
        public static double Rosenbrock(double[] X)
        {
            double suma = 0, a, b;
            int N = X.Length - 1;
            for (int i = 1; i <= N - 1; i++)
            {
                a = X[i + 1] - X[i] * X[i];
                b = 1.0 - X[i];
                suma += 100.0 * a * a + b * b;
            }
            return suma;
        }

        public static double Funkcja1(double[] X)
        {
            double x = X[1] - 2.0;
            double y = X[2] + 2.0;
            double z = X[3] - 5.0;
            return 10.0 * x * x + 20.0 * y * y + 30.0 * z * z + 1.0;
        }
    }
}
