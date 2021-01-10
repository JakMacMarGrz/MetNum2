using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSBibMatStudent.Complex;

namespace CSBibMatStudent
{
    public delegate double FunkcjaRealeReale(double x);

    public delegate double[] FunWektorWektorDelegate(double[] X);
    public delegate double[] FunNielinRowRozniczkoweDelegate(double[] X, double t);

    public delegate Complex.Complex FunkcjaComplexReale(double x);
    public delegate double FunRealDelegate(double[] X);
    public delegate void FunNieLinDelegate(double[] F, double[] X);
    public delegate void FunNieLinListDelegate(List<double> F, List<double> X);
    public delegate void FunNonLinearDiffEquationDelegate(List<double> F, List<double> X, double t);
    public delegate void FunWymLinDiffEqutionDelegate(List<double> F, double t);
    
    public class Global
    {
        public delegate double FunctionDelegate(double[] X);
        public delegate double MethodDelegate(Global.FunctionDelegate f, ref double[] X, object[] par);

        public static void WaitForKey()
        {
            Console.WriteLine();
            Console.WriteLine("Nacisnij dowolny klawisz...");
            Console.ReadKey(true);
        }

        public static void WarnUser(string message)
        {
            WarnUser(message, MessageBoxIcon.Information);
        }

        public static void WarnUser(string message, MessageBoxIcon icon)
        {
            MessageBox.Show(message, "CSBibMat", MessageBoxButtons.OK,
                icon, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult AskUser(string msg)
        {
            return MessageBox.Show(msg, "CSBibMat", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }
    }

    public static class MyExtensions
    {

        public static List<double> Clone(this List<double> source)
        {
            List<double> dest = new List<double>(source.Count);
            for (int i = 0; i < source.Count; i++)            
                dest.Add(source[i]);
            return dest;
        }

    }

    
}
