using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSBibMatStudent;
using CSBibMatStudent.Complex;

namespace CSBibMatStudent.AlgebraLiniowa
{
    public static class MetodaGaussa
    {
        public static string[] Komunikat = 
        { /*0*/ "Brak błędu",
          /*1 5*/ "Odwracana macierz we wzorcu OdwMacGaussa musi być kwadratowa ",
          /*2 6*/ "Warunek przerwania poszukiwania elementu głównego metody eliminacji Gaussa w wzorcu OdwMacGausa ",
          /*3 7*/ "Niezgodność wymiarów macierzy w metodzie SkalRowMacTyp ",
          /*4 9*/ "Niezgodność wymiarów macierzy w metodzie RozRowMacGaussa ",
          /*5 10*/ "Warunek przerwania poszukiwania elementu głównego metody eliminacji Gaussa w metodzie RozRowMacGaussa "
        };
        public static void PiszKomunikat(int k)
        {
            MessageBox.Show(Komunikat[k]);
        }

        #region Statyczne

        /// <summary> 
        /// Funkcja do skalowania rÓwnania macierzowego A*X=B
        /// </summary>
        /// <param name="A">A - macierz kwadratowa</param>
        /// <param name="B">B - wektor wyrazów wolnych</param>
        /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
        public static int SkalRowMacTyp(double[,] A, double[] B)
        {

            double ln2, S, a2 = 2.0;
            int i, j;
            int S1;
            if (A.GetLength(0) == B.Length)
            {
                ln2 = Math.Log(a2);
                for (i = 1; i <= A.GetLength(0) - 1; i++)
                {
                    S = 0;
                    for (j = 1; j <= A.GetLength(0) - 1; j++) S += Math.Abs(A[i, j]);
                    S1 = (int)Math.Round(Math.Log(S) / ln2 + 1);
                    S = Math.Pow(2, S1);
                    for (j = 1; j <= A.GetLength(0) - 1; j++) A[i, j] *= S;
                    B[i] *= S;
                }
                return 0;
            }
            else return 3;
        }
        //-------------------------------------------------------------------------

        /// <summary>
        /// Funkcja do skalowania rÓwnania macierzowego A*X=B
        /// </summary>
        /// <param name="A">A - macierz kwadratowa</param>
        /// <param name="B">B - macierz wyrazów wolnych</param>
        /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
        public static int SkalRowMacTyp(double[,] A, double[,] B)
        {
            double ln2, S, a2 = 2.0;
            int i, j, S1, N, U, R;
            N = A.GetLength(0) - 1;
            R = B.GetLength(0) - 1;
            U = B.GetLength(1) - 1;
            if (N == R)
            {
                ln2 = Math.Log(a2);
                for (i = 1; i <= N; i++)
                {
                    S = 0;
                    for (j = 1; j <= N; j++) S += Math.Abs(A[i, j]);
                    S1 = (int)Math.Round(Math.Log(S) / ln2 + 1);
                    S = Math.Pow(2, S1);
                    for (j = 1; j <= N; j++) A[i, j] *= S;
                    for (j = 1; j <= U; j++) B[i, j] *= S;
                }
                return 0;
            }
            else return 3;
        }

        public static int SkalRowMacTyp(Complex.Complex[,] A, Complex.Complex[,] B)
        {
            double ln2, S, a2 = 2.0;
            //Complex.Complex.Complex.Complex
            int i, j, S1, N, U, R;
            N = A.GetLength(0) - 1;
            R = B.GetLength(0) - 1;
            U = B.GetLength(1) - 1;
            if (N == R)
            {
                ln2 = Math.Log(a2);
                for (i = 1; i <= N; i++)
                {
                    S = 0;
                    for (j = 1; j <= N; j++) S += Complex.ComplexMath.Abs(A[i, j]);
                    S1 = (int)Math.Round(Math.Log(S) / ln2 + 1);
                    S = Math.Pow(2, S1);
                    for (j = 1; j <= N; j++) A[i, j] *= S;
                    for (j = 1; j <= U; j++) B[i, j] *= S;
                }
                return 0;
            }
            else return 3;
        }

        public static int SkalRowMacTyp(Complex.Complex[,] A, Complex.Complex[] B)
        {
            double ln2, S, a2 = 2.0;
            //Complex.Complex.Complex.Complex
            int i, j, S1, N, R;
            N = A.GetLength(0) - 1;
            R = B.GetLength(0) - 1;
            if (N == R)
            {
                ln2 = Math.Log(a2);
                for (i = 1; i <= N; i++)
                {
                    S = 0;
                    for (j = 1; j <= N; j++) S += Complex.ComplexMath.Abs(A[i, j]);
                    S1 = (int)Math.Round(Math.Log(S) / ln2 + 1);
                    S = Math.Pow(2, S1);
                    for (j = 1; j <= N; j++) A[i, j] *= S;
                    B[i] *= S;
                }
                return 0;
            }
            else return 3;
        }


        public static int RozRowMacGaussa(double[,] A, double[] B, double[] X, double eps)
        {
            int i, j, k, blad, N, M, R;
            double T, MA, a1 = 1.0;
            double ZT, ZS;
            N = A.GetLength(0) - 1; M = A.GetLength(1) - 1; R = B.Length - 1;
            if (N == M && N == R)
            {
                //blad=SkalRowMacTyp(A,B);
                //Konstrukcja ciagu macierzy A(i)  oraz ciagu macierzy B(i)
                for (i = 1; i <= N; i++)
                {
                    //Wybór elementu głównego
                    T = Math.Abs(A[i, i]); k = i;
                    for (j = i + 1; j <= N; j++)
                    {
                        MA = Math.Abs(A[j, i]);
                        if (MA > T) { T = MA; k = j; };
                    }
                    if (T < eps)
                    { //Warunek przerwania poszukiwania elementu głównego
                        // nie istnieje rozwiązanie równania macierzowego, detA=0
                        blad = 5;
                        return blad;
                    }
                    if (i == k) { ZT = A[i, i]; }
                    else
                    {
                        //Zamiana wiersza k-tego z i-tym macierzy MacB
                        ZS = B[k]; B[k] = B[i]; B[i] = ZS;
                        ZT = A[k, i];
                        for (j = N; j >= i; j--)
                        { //Zamiana wiersza k-tego z i-tym macierzy MacA
                            ZS = A[k, j]; A[k, j] = A[i, j]; A[i, j] = ZS;
                        }//j
                    }
                    ZT = a1 / ZT;
                    A[i, i] = ZT;
                    for (j = i + 1; j <= N; j++)
                    {
                        ZS = A[j, i] * ZT;
                        B[j] -= B[i] * ZS;
                        for (k = i + 1; k <= N; k++)
                            A[j, k] -= A[i, k] * ZS;
                    }//j
                }//i;
                // Rozwiązywanie układu trójkątnego metodą postępowania wstecz
                for (i = N; i >= 1; i--)
                {
                    ZT = B[i];
                    for (j = i + 1; j <= N; j++) ZT -= A[i, j] * X[j];
                    X[i] = ZT * A[i, i];
                }//i;
                blad = 0;
            }
            else blad = 4;
            return blad;
        }


        //----------------------------------------------------------------------
        public static int RozRowMacGaussa(double[,] A, double[,] B,
                        double[,] X, double eps)
        {
            int i, j, k, blad, N, R, M, U;
            double T, MA, a1 = 1;
            double ZT, ZS;
            N = A.GetLength(0) - 1;
            M = A.GetLength(1) - 1;
            R = B.GetLength(0) - 1;
            U = B.GetLength(1) - 1;
            if (R == N && M == N)
            {
                blad = SkalRowMacTyp(A, B);
                //Konstrukcja ciagu macierzy A(i)  oraz ciagu macierzy B(i)
                for (i = 1; i <= N; i++)
                {
                    //Wybór elementu głównego
                    T = Math.Abs(A[i, i]); k = i;
                    for (j = i + 1; j <= N; j++)
                    {
                        MA = Math.Abs(A[j, i]);
                        if (MA > T) { T = MA; k = j; };
                    }
                    if (T < eps)
                    { //Warunek przerwania poszukiwania elementu głównego
                        // nie istnieje rozwiązanie równania macierzowego, detA=0
                        blad = 5;
                        return blad;
                    }
                    if (i == k)
                    { ZT = A[i, i]; }
                    else
                    {
                        //Zamiana elementu k-tego z i-tym w macierzy B 
                        for (j = 1; j <= U; j++)
                        {
                            ZS = B[k, j]; B[k, j] = B[i, j]; B[i, j] = ZS;
                        }
                        ZT = A[k, i];
                        for (j = N; j >= i; j--)
                        { //Zamiana wiersza k-tego z i-tym macierzy MacA
                            ZS = A[k, j]; A[k, j] = A[i, j]; A[i, j] = ZS;
                        }//j
                    }
                    ZT = a1 / ZT;
                    A[i, i] = ZT;
                    for (j = i + 1; j <= N; j++)
                    {
                        ZS = A[j, i] * ZT;
                        for (k = 1; k <= U; k++) B[j, k] -= B[i, k] * ZS;
                        for (k = i + 1; k <= N; k++) A[j, k] -= A[i, k] * ZS;
                    }//j
                }//i;
                // Rozwiązywanie układu trójkątnego metodą postępowania wstecz
                for (k = 1; k <= U; k++)
                    for (i = N; i >= 1; i--)
                    {
                        ZT = B[i, k];
                        for (j = i + 1; j <= N; j++) ZT -= A[i, j] * X[j, k];
                        X[i, k] = ZT * A[i, i];
                    }//i;
                blad = 0;
            }
            else blad = 4;
            return blad;
        }
        //---------------------------------------------------------------------
        public static int RozRowMacGaussa(Complex.Complex[,] A, Complex.Complex[,] B,
                        Complex.Complex[,] X, double eps)
        {
            int i, j, k, blad, N, R, M, U;
            double T, MA, a1 = 1.0;
            Complex.Complex ZT, ZS;
            N = A.GetLength(0) - 1;
            M = A.GetLength(1) - 1;
            R = B.GetLength(0) - 1;
            U = B.GetLength(1) - 1;
            if (R == N && M == N)
            {
                blad = SkalRowMacTyp(A, B);
                //Konstrukcja ciagu macierzy A(i)  oraz ciagu macierzy B(i)
                for (i = 1; i <= N; i++)
                {
                    //Wybór elementu głównego
                    T = Complex.ComplexMath.Abs(A[i, i]); k = i;
                    for (j = i + 1; j <= N; j++)
                    {
                        MA = Complex.ComplexMath.Abs(A[j, i]);
                        if (MA > T) { T = MA; k = j; };
                    }
                    if (T < eps)
                    { //Warunek przerwania poszukiwania elementu głównego
                        // nie istnieje rozwiązanie równania macierzowego, detA=0
                        blad = 5;
                        return blad;
                    }
                    if (i == k)
                    { ZT = A[i, i]; }
                    else
                    {
                        //Zamiana elementu k-tego z i-tym w macierzy B 
                        for (j = 1; j <= U; j++)
                        {
                            ZS = B[k, j]; B[k, j] = B[i, j]; B[i, j] = ZS;
                        }
                        ZT = A[k, i];
                        for (j = N; j >= i; j--)
                        { //Zamiana wiersza k-tego z i-tym macierzy MacA
                            ZS = A[k, j]; A[k, j] = A[i, j]; A[i, j] = ZS;
                        }//j
                    }
                    ZT = a1 / ZT;
                    A[i, i] = ZT;
                    for (j = i + 1; j <= N; j++)
                    {
                        ZS = A[j, i] * ZT;
                        for (k = 1; k <= U; k++) B[j, k] -= B[i, k] * ZS;
                        for (k = i + 1; k <= N; k++) A[j, k] -= A[i, k] * ZS;
                    }//j
                }//i;
                // Rozwiązywanie układu trójkątnego metodą postępowania wstecz
                for (k = 1; k <= U; k++)
                    for (i = N; i >= 1; i--)
                    {
                        ZT = B[i, k];
                        for (j = i + 1; j <= N; j++) ZT -= A[i, j] * X[j, k];
                        X[i, k] = ZT * A[i, i];
                    }//i;
                blad = 0;
            }
            else blad = 4;
            return blad;
        }
        //------------------------------------------------------------------

        public static int RozRowMacGaussa(Complex.Complex[,] A, Complex.Complex[] B,
                                Complex.Complex[] X, double eps)
        {
            int i, j, k, blad, N, R, M;
            double T, MA, a1 = 1.0;
            Complex.Complex ZT, ZS;
            N = A.GetLength(0) - 1;
            M = A.GetLength(1) - 1;
            R = B.GetLength(0) - 1;
            if (R == N && M == N)
            {
                blad = SkalRowMacTyp(A, B);
                //Konstrukcja ciagu macierzy A(i)  oraz ciagu macierzy B(i)
                for (i = 1; i <= N; i++)
                {
                    //Wybór elementu głównego
                    T = Complex.ComplexMath.Abs(A[i, i]); k = i;
                    for (j = i + 1; j <= N; j++)
                    {
                        MA = Complex.ComplexMath.Abs(A[j, i]);
                        if (MA > T) { T = MA; k = j; };
                    }
                    if (T < eps)
                    { //Warunek przerwania poszukiwania elementu głównego
                        // nie istnieje rozwiązanie równania macierzowego, detA=0
                        blad = 5;
                        return blad;
                    }
                    if (i == k)
                    { ZT = A[i, i]; }
                    else
                    {
                        //Zamiana elementu k-tego z i-tym w macierzy B 
                        ZS = B[k]; B[k] = B[i]; B[i] = ZS;
                        ZT = A[k, i];
                        for (j = N; j >= i; j--)
                        { //Zamiana wiersza k-tego z i-tym macierzy MacA
                            ZS = A[k, j]; A[k, j] = A[i, j]; A[i, j] = ZS;
                        }//j
                    }
                    ZT = a1 / ZT;
                    A[i, i] = ZT;
                    for (j = i + 1; j <= N; j++)
                    {
                        ZS = A[j, i] * ZT;
                       
                        B[j] -= B[i] * ZS;
                        for (k = i + 1; k <= N; k++) A[j, k] -= A[i, k] * ZS;
                    }//j
                }//i;
                // Rozwiązywanie układu trójkątnego metodą postępowania wstecz
                for (i = N; i >= 1; i--)
                {
                    ZT = B[i];
                    for (j = i + 1; j <= N; j++) ZT -= A[i, j] * X[j];
                    X[i] = ZT * A[i, i];
                }//i;
                blad = 0;
            }
            else blad = 4;
            return blad;
        }
        //------------------------------------------------------------------

        /// <summary>
        /// Metoda odwracenie macierzy rzeczywistej i zapisanie jej 
        /// na miejscu oryginalnej wg algorytmu Gaussa 
        /// </summary>
        /// <param name="A">Macierz kwadratowa rzędu NW=A.GetLength(0)</param>
        /// <param name="eps">dokładność określenia zerowego elementu 
        /// przy poszukiwaniu elementu głównego</param>
        /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
        public static int OdwMacGaussa(double[,] A, double eps)
        {
            int NW, NK, blad, k;
            double MmaxA, d, ZS;
            double a1 = 1;
            double a0 = 0;
            double f, maxA = 0;
            NW = A.GetLength(0) - 1; NK = A.GetLength(1) - 1;
            //vector<int> M(NW+1);
            int[] M = new int[NW + 1];
            //InicjacjaWektora(M,NW);
            if (NK == NW)
            {
                for (int i = 1; i <= NW; i++)
                {
                    // Częściowy wybór elementu głównego
                    MmaxA = a0; k = 1;
                    for (int j = i; j <= NW; j++)
                    {
                        d = Math.Abs(A[j, i]);
                        if (MmaxA < d) { MmaxA = d; maxA = A[j, i]; k = j; }
                    }//j
                    if (MmaxA < eps)
                    {
                        blad = 2;
                        return blad;
                    }
                    // Zapisywanie wskaźników wierszy występowania elementu
                    // ekstremalnego w i-tej iteracji w postaci wektora M[i]
                    M[i] = k;
                    A[k, i] = a1;
                    // Przestawienie i-tego wiersza z k-tym
                    for (int j = 1; j <= NW; j++) A[k, j] /= maxA;
                    //swap(MacA[k],MacA[i]);
                    for (int j = 1; j >= NW; j++)
                    { //Zamiana wiersza k-tego z i-tym macierzy A
                        ZS = A[k, j]; A[k, j] = A[i, j]; A[i, j] = ZS;
                    }//j
                    // Generacja ciągu macierzy

                    for (int j = 1; j <= NW; j++)
                        if (j != i)
                        {
                            f = A[j, i]; A[j, i] = 0;
                            for (int l = 1; l <= NW; l++) A[j, l] -= f * A[i, l];
                        }//j
                }//i
                // Przestawianie kolumn macierzy zgodnie z wektorem wskaźników M[i] (1.55) }
                for (int i = NW; i >= 1; i--)
                {
                    k = M[i];
                    if (k != i)
                        for (int j = 1; j <= NW; j++)
                        {
                            //swap(MacA[j,i], MacA[j,k]);
                            ZS = A[j, i]; A[j, i] = A[j, k]; A[j, k] = ZS;
                        }
                }//i
                return 0;
            }
            else
            { return 1; }
        }
        /// <summary>
        /// Metoda odwracenie macierzy zespolonej i zapisanie jej 
        /// na miejscu oryginalnej wg algorytmu Gaussa 
        /// </summary>
        /// <param name="A">Macierz kwadratowa rzędu NW=A.GetLength(0)</param>
        /// <param name="eps">dokładność określenia zerowego elementu 
        /// przy poszukiwaniu elementu głównego</param>
        /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
        public static int OdwMacGaussa(Complex.Complex[,] A, double eps)
        {
            int NW, NK, blad, k;
            double MmaxA, d;
            double a1 = 1;
            double a0 = 0;
            Complex.Complex f, ZS, maxA = 0;
            NW = A.GetLength(0) - 1; NK = A.GetLength(1) - 1;
            //vector<int> M(NW+1);
            int[] M = new int[NW + 1];
            //InicjacjaWektora(M,NW);
            if (NK == NW)
            {
                for (int i = 1; i <= NW; i++)
                {
                    // Częściowy wybór elementu głównego
                    MmaxA = a0; k = 1;
                    for (int j = i; j <= NW; j++)
                    {
                        d = Complex.ComplexMath.Abs(A[j, i]);
                        if (MmaxA < d) { MmaxA = d; maxA = A[j, i]; k = j; }
                    }//j
                    if (MmaxA < eps)
                    {
                        blad = 2;
                        return blad;
                    }
                    // Zapisywanie wskaźników wierszy występowania elementu
                    // ekstremalnego w i-tej iteracji w postaci wektora M[i]
                    M[i] = k;
                    A[k, i] = a1;
                    // Przestawienie i-tego wiersza z k-tym
                    for (int j = 1; j <= NW; j++) A[k, j] /= maxA;
                    //swap(MacA[k],MacA[i]);
                    for (int j = 1; j >= NW; j++)
                    { //Zamiana wiersza k-tego z i-tym macierzy A
                        ZS = A[k, j]; A[k, j] = A[i, j]; A[i, j] = ZS;
                    }//j
                    // Generacja ciągu macierzy

                    for (int j = 1; j <= NW; j++)
                        if (j != i)
                        {
                            f = A[j, i]; A[j, i] = 0;
                            for (int l = 1; l <= NW; l++) A[j, l] -= f * A[i, l];
                        }//j
                }//i
                // Przestawianie kolumn macierzy zgodnie z wektorem wskaźników M[i] (1.55) }
                for (int i = NW; i >= 1; i--)
                {
                    k = M[i];
                    if (k != i)
                        for (int j = 1; j <= NW; j++)
                        {
                            //swap(MacA[j,i], MacA[j,k]);
                            ZS = A[j, i]; A[j, i] = A[j, k]; A[j, k] = ZS;
                        }
                }//i
                return 0;
            }
            else
            { return 1; }
        }

        #endregion Statyczne
    }
}
