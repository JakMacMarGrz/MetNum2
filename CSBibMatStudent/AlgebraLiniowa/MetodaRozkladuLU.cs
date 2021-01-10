using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSBibMatStudent.Complex;

namespace CSBibMatStudent.AlgebraLiniowa
{
    public static class MetodaRozkladuLU
    {
        
        #region Statycznie
           /// <summary>
           ///Metoda do rozwiązywania równań macierzowych A*X=B
           ///o elementach typu double dla standardowej postaci tablic
           /// metodą rozkładu LU wg algorytmu Crouta - Doolitta'a
           /// </summary>
           /// <param name="A">Macierz kwadratowa rzędu N=A.GetLength(0)-1</param>
           /// <param name="B">Macierz wyrazów wolnych o N=X.A.GetLength(0)-1-wierszach i
           ///                 M=XA.GetLength(1)-1-kolumnach</param>
           /// <param name="X">Macierz rozwiązania o N=X.A.GetLength(0)-1  -wierszach i
           ///                 M=XA.GetLength(1)-1  -kolumnach</param>
           /// <param name="eps">dokładność rozróżnienia zerowej kolumny (np. eps=1E-25)</param>
           /// <returns> return 0 gdy brak błędu, inna wartość = numer błędu</returns>
           public static int RozRowMacCroutDoolitta(double [,] A, double [,] B,
                                        double[,] X, double eps)
           {
               int i, j, k, k1, N, M, R, U, blad;
               double S;
               double T, T1;
               double W;
               N = A.GetLength(0) - 1; M = A.GetLength(1) - 1;
               R = B.GetLength(0) - 1; U = B.GetLength(1) - 1;
               if (R == N && M == N)
               {
                   for (i = 1; i <= N; i++)
                   {
                       T = Math.Abs(A[i,i]); k = i;
                       if (T < eps)
                       {
                           for (j = i + 1; j <= N; j++)
                           {
                               T1 = Math.Abs(A[j,i]);
                               if (T1 > T) { T = T1; k = j; }
                           }
                           if (T < eps)
                           { blad = 13; return blad; }
                           // Zamiana wiersza k-tego z i-tym
                           if (i != k)
                           {
                               for (j = 1; j <= U; j++)
                               {
                                  W = B[k,j]; B[i,j] = B[k,j]; B[k,j] = W;
                               }
                               for (j = 1; j <= N; j++)
                               {
                                   W = A[k,j]; A[i,j] = A[k,j]; A[k,j] = W;
                               }
                           }
                       }
                   }//i
                   //Skalowanie macierzy A i wektora wyrazow wolnych B
                   //MetGaussa.SkalRowMacTyp(A,B);
                   //Rozkład LU macierzy A wg algorytmu Crouta-Doolittle'a  A=L*U }
                   for (k = 1; k <= N - 1; k++)
                   {
                       k1 = k + 1;
                       if (A[k,k] != 0)
                       {
                           for (i = k1; i <= N; i++)
                               if (A[k,i] != 0) A[k,i] /= A[k,k];
                           for (i = k1; i <= N; i++)
                               for (j = k1; j <= N; j++)
                                   if (A[i,k] != 0 && A[k,j] != 0)
                                       A[i,j] -= A[i,k] * A[k,j];
                       }
                       else
                       { blad = 11; return blad; }
                   };//k
                   //Rozwiązanie układu trójkątnego dolnego
                   //L*Y=B metodą postępowania w przód
                   for (k = 1; k <= U; k++)
                   {
                       B[1,k] /= A[1,1];
                       for (i = 2; i <= N; i++)
                       {
                           S = B[i,k];
                           for (j = 1; j <= i - 1; j++) S -= A[i,j] * B[j,k];
                           B[i,k] = S / A[i,i];
                       }//i
                   }//k
                   //Rozwiązanie układu trójkątnego górnego
                   //UX=Y metodą postępowania wstecz
                   for (k = 1; k <= U; k++)
                       for (i = N; i >= 1; i--)
                       {
                           for (j = i + 1; j <= N; j++) B[i,k] -= A[i,j] * X[j,k];
                           X[i,k] = B[i,k];
                       }//k,i
                   return 0;
               }
               else return 12;
           }
          //---------------------------------------------------------------------------
           /// <summary>
           ///Metoda do rozwiązywania równań macierzowych A*X=B
           ///o elementach typu double dla standardowej postaci tablic
           /// metodą rozkładu LU wg algorytmu Crouta - Doolitta'a
           /// </summary>
           /// <param name="A">Macierz kwadratowa rzędu N=A.GetLength(0)-1</param>
           /// <param name="B">Macierz wyrazów wolnych o N=X.A.GetLength(0)-1 elementach</param>
           /// <param name="X">Macierz rozwiązania o N=X.A.GetLength(0)-1  elementach</param>
           /// <param name="eps">dokładność rozróżnienia zerowej kolumny (np. eps=1E-25)</param>
           /// <returns> return 0 gdy brak błędu, inna wartość = numer błędu</returns>
           public static int RozRowMacCroutDoolitta(double[,] A, double[] B,
                                        double[] X, double eps)
           {
               int i, j, k, k1, N, M, R, blad;
               double S;
               double T, T1;
               double W;
               N = A.GetLength(0) - 1; M = A.GetLength(1) - 1;
               R = B.GetLength(0) - 1; 
               if (R == N && M == N)
               {
                   for (i = 1; i <= N; i++)
                   {
                       T = Math.Abs(A[i, i]); k = i;
                       if (T < eps)
                       {
                           for (j = i + 1; j <= N; j++)
                           {
                               T1 = Math.Abs(A[j, i]);
                               if (T1 > T) { T = T1; k = j; }
                           }
                           if (T < eps)
                           { blad = 13; return blad; }
                           // Zamiana wiersza k-tego z i-tym
                           if (i != k)
                           {
                              W = B[k]; B[i] = B[k]; B[k] = W;
                              for (j = 1; j <= N; j++)
                              {
                                  W = A[k, j]; A[i, j] = A[k, j]; A[k, j] = W;
                              }
                           }
                       }
                   }//i
                   //Skalowanie macierzy A i wektora wyrazow wolnych B
                   //MetGaussa.SkalRowMacTyp(A,B);
                   //Rozkład LU macierzy A wg algorytmu Crouta-Doolittle'a  A=L*U }
                   for (k = 1; k <= N - 1; k++)
                   {
                       k1 = k + 1;
                       if (A[k, k] != 0)
                       {
                           for (i = k1; i <= N; i++)
                               if (A[k, i] != 0) A[k, i] /= A[k, k];
                           for (i = k1; i <= N; i++)
                               for (j = k1; j <= N; j++)
                                   if (A[i, k] != 0 && A[k, j] != 0)
                                       A[i, j] -= A[i, k] * A[k, j];
                       }
                       else
                       { blad = 11; return blad; }
                   };//k
                   //Rozwiązanie układu trójkątnego dolnego
                   //L*Y=B metodą postępowania w przód
                   B[1] /= A[1, 1];
                   for (i = 2; i <= N; i++)
                   {
                      S = B[i];
                      for (j = 1; j <= i - 1; j++) S -= A[i, j] * B[j];
                      B[i] = S / A[i, i];
                   }//i
                   //Rozwiązanie układu trójkątnego górnego
                   //UX=Y metodą postępowania wstecz
                   for (i = N; i >= 1; i--)
                   {
                       for (j = i + 1; j <= N; j++) B[i] -= A[i, j] * X[j];
                       X[i] = B[i];
                   }//i
                   return 0;
               }
               else return 12;
           }
           //---------------------------------------------------------------------------
           /// <summary>
           ///Metoda do rozwiązywania równań macierzowych A*X=B
           ///o elementach typu complex dla standardowej postaci tablic
           /// metodą rozkładu LU wg algorytmu Crouta - Doolitta'a
           /// </summary>
           /// <param name="A">Macierz kwadratowa rzędu N=A.GetLength(0)-1</param>
           /// <param name="B">Macierz wyrazów wolnych o N=X.A.GetLength(0)-1-wierszach i
           ///                 M=XA.GetLength(1)-1-kolumnach</param>
           /// <param name="X">Macierz rozwiązania o N=X.A.GetLength(0)-1  -wierszach i
           ///                 M=XA.GetLength(1)-1  -kolumnach</param>
           /// <param name="eps">dokładność rozróżnienia zerowej kolumny (np. eps=1E-25)</param>
           /// <returns> return 0 gdy brak błędu, inna wartość = numer błędu</returns>
           public static int RozRowMacCroutDoolitta(Complex.Complex[,] A, Complex.Complex[,] B,
                                        Complex.Complex[,] X, double eps)
           {
               int i, j, k, k1, N, M, R, U, blad;
               Complex.Complex S,W;
               double T, T1;
               N = A.GetLength(0) - 1; M = A.GetLength(1) - 1;
               R = B.GetLength(0) - 1; U = B.GetLength(1) - 1;
               if (R == N && M == N)
               {
                   for (i = 1; i <= N; i++)
                   {
                       T = A[i, i].Abs; k = i;
                       if (T < eps)
                       {
                           for (j = i + 1; j <= N; j++)
                           {
                               T1 = A[j, i].Abs;
                               if (T1 > T) { T = T1; k = j; }
                           }
                           if (T < eps)
                           { blad = 13; return blad; }
                           // Zamiana wiersza k-tego z i-tym
                           if (i != k)
                           {
                               for (j = 1; j <= U; j++)
                               {
                                   W = B[k, j]; B[i, j] = B[k, j]; B[k, j] = W;
                               }
                               for (j = 1; j <= N; j++)
                               {
                                   W = A[k, j]; A[i, j] = A[k, j]; A[k, j] = W;
                               }
                           }
                       }
                   }//i
                   //Skalowanie macierzy A i wektora wyrazow wolnych B
                   //MetGaussa.SkalRowMacTyp(A,B);
                   //Rozkład LU macierzy A wg algorytmu Crouta-Doolittle'a  A=L*U }
                   for (k = 1; k <= N - 1; k++)
                   {
                       k1 = k + 1;
                       if (A[k, k] != 0)
                       {
                           for (i = k1; i <= N; i++)
                               if (A[k, i] != 0) A[k, i] /= A[k, k];
                           for (i = k1; i <= N; i++)
                               for (j = k1; j <= N; j++)
                                   if (A[i, k] != 0 && A[k, j] != 0)
                                       A[i, j] -= A[i, k] * A[k, j];
                       }
                       else
                       { blad = 11; return blad; }
                   };//k
                   //Rozwiązanie układu trójkątnego dolnego
                   //L*Y=B metodą postępowania w przód
                   for (k = 1; k <= U; k++)
                   {
                       B[1, k] /= A[1, 1];
                       for (i = 2; i <= N; i++)
                       {
                           S = B[i,k];
                           for (j = 1; j <= i - 1; j++) S -= A[i, j] * B[j, k];
                           B[i, k] = S / A[i, i];
                       }//i
                   }//k
                   //Rozwiązanie układu trójkątnego górnego
                   //UX=Y metodą postępowania wstecz
                   for (k = 1; k <= U; k++)
                       for (i = N; i >= 1; i--)
                       {
                           for (j = i + 1; j <= N; j++) B[i, k] -= A[i, j] * X[j, k];
                           X[i, k] = B[i, k];
                       }//k,i
                   return 0;
               }
               else return 12;
           }
           //---------------------------------------------------------------------------
           /// <summary>
           ///Metoda do rozwiązywania układu równań A*X=B
           ///o elementach typu complex dla standardowej postaci tablic
           /// metodą rozkładu LU wg algorytmu Crouta - Doolitta'a
           /// </summary>
           /// <param name="A">Macierz kwadratowa rzędu N=A.GetLength(0)-1</param>
           /// <param name="B">Macierz wyrazów wolnych o N=X.A.GetLength(0)-1 elementach </param>
           /// <param name="X">Macierz rozwiązania o N=X.A.GetLength(0)-1  elementach </param>
           /// <param name="eps">dokładność rozróżnienia zerowej kolumny (np. eps=1E-25)</param>
           /// <returns> return 0 gdy brak błędu, inna wartość = numer błędu</returns>
           public static int RozRowMacCroutDoolitta(Complex.Complex[,] A, Complex.Complex[] B,
                                        Complex.Complex[] X, double eps)
           {
               int i, j, k, k1, N, M, R, blad;
               Complex.Complex S,W;
               double T, T1;
               N = A.GetLength(0) - 1; M = A.GetLength(1) - 1;
               R = B.GetLength(0) - 1;
               if (R == N && M == N)
               {
                   for (i = 1; i <= N; i++)
                   {
                       T = A[i, i].Abs; k = i;
                       if (T < eps)
                       {
                           for (j = i + 1; j <= N; j++)
                           {
                               T1 =A[j, i].Abs;
                               if (T1 > T) { T = T1; k = j; }
                           }
                           if (T < eps)
                           { blad = 13; return blad; }
                           // Zamiana wiersza k-tego z i-tym
                           if (i != k)
                           {
                               W = B[k]; B[i] = B[k]; B[k] = W;
                               for (j = 1; j <= N; j++)
                               {
                                   W = A[k, j]; A[i, j] = A[k, j]; A[k, j] = W;
                               }
                           }
                       }
                   }//i
                   //Skalowanie macierzy A i wektora wyrazow wolnych B
                   //MetGaussa.SkalRowMacTyp(A,B);
                   //Rozkład LU macierzy A wg algorytmu Crouta-Doolittle'a  A=L*U }
                   for (k = 1; k <= N - 1; k++)
                   {
                       k1 = k + 1;
                       if (A[k, k] != 0)
                       {
                           for (i = k1; i <= N; i++)
                               if (A[k, i] != 0) A[k, i] /= A[k, k];
                           for (i = k1; i <= N; i++)
                               for (j = k1; j <= N; j++)
                                   if (A[i, k] != 0 && A[k, j] != 0)
                                       A[i, j] -= A[i, k] * A[k, j];
                       }
                       else
                       { blad = 11; return blad; }
                   };//k
                   //Rozwiązanie układu trójkątnego dolnego
                   //L*Y=B metodą postępowania w przód
                   B[1] /= A[1, 1];
                   for (i = 2; i <= N; i++)
                   {
                       S = B[i];
                       for (j = 1; j <= i - 1; j++) S -= A[i, j] * B[j];
                       B[i] = S / A[i, i];
                   }//i
                   //Rozwiązanie układu trójkątnego górnego
                   //UX=Y metodą postępowania wstecz
                   for (i = N; i >= 1; i--)
                   {
                       for (j = i + 1; j <= N; j++) B[i] -= A[i, j] * X[j];
                       X[i] = B[i];
                   }//i
                   return 0;
               }
               else return 12;
           }
           //---------------------------------------------------------------------------
           /// <summary>
           ///  Metoda rozwiązywania równania macierzowego  A*X=B o elementach rzeczywistych
           ///  metodą iteracji Gaussa-Seidela dla pełnych macierzy typu  double [,] 
           /// </summary>
           /// <param name="A">macierz kwadratowa rzędu N=A.GetLength(0)-1</param>
           /// <param name="B">macierz wyrazów wolnych o N=B.GetLength(0)-1  -wierszach i
           ///                  M=B.GetLength(1)t-1  -kolumnach</param>
           /// <param name="X">macierz rozwiązania o N=X.GetLength(0)-1  -wierszach i
           ///                  M=X.GetLength(1)-1  -kolumnach</param>
           /// <param name="eps">dokładność bezwzględna iteracji (np. eps=1E-8)</param>
           /// <param name="omega">parametr relaksacji omega =1 dla metody Gaussa-Seidela </param>
           /// <param name="maxit">zadana maksymalna liczba iteracji</param>
           /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
           public static int RozRowMacIGS(double[,] A, double[,] B, double[,] X,
                               double eps, double omega, int maxit)
           {
               int i, j, l, k, p, N, M, U, W;
               double R, R1, S, T, T1;
               double SZ, G;
               N = A.GetLength(0) - 1; M = A.GetLength(1) - 1;
               W = B.GetLength(0) - 1; U = B.GetLength(1) - 1;
               double[,] Y = new double[N + 1, U + 1];
               try
               {
                   if (W == N && M == N)
                   {  // Przekształcenie macierzy A do postaci z możliwie
                       //maksymalnymi co do modułu elementami na głównej
                       //przekątnej metodą przestawiania wierszy
                       for (i = 1; i <= N; i++)
                       {
                           T = Math.Abs(A[i,i]); k = i;
                           for (j = i + 1; j <= N; j++)
                           {
                               T1 = Math.Abs(A[j,i]);
                               if (T1 > T)
                               { T = T1; k = j; };
                           }
                           if (i != k)
                           {
                               //swap(B[k],B[i]); swap(A[k],A[i]);
                               for (l = 1; l <= U; l++)
                               {
                                   G = B[k,l]; B[k,l] = B[i,l]; B[i,l] = G;
                               }
                               for (l = 1; l <= N; l++)
                               {
                                   G = A[k,l]; A[k,l] = A[i,l]; A[i,l] = G;
                               }
                           }
                       };//i
                       //MetGaussa.SkalRowMacTyp(A,B);
                       p = 0;
                       for (i = 1; i <= N; i++)
                           for (j = 1; j <= U; j++) X[i,j] = B[i,j];
                       do
                       {
                           R1 = 0; p++;
                           for (i = 1; i <= N; i++)
                               for (j = 1; j <= U; j++) Y[i,j] = X[i,j];
                           for (l = 1; l <= U; l++)
                           {
                               R = 0.0;
                               for (i = 1; i <= N; i++)
                               {
                                   SZ = B[i,l];
                                   for (j = 1; j <= i - 1; j++) SZ -= A[i,j] * X[j,l];
                                   for (j = i; j <= N; j++) SZ -= A[i,j] * Y[j,l];
                                   X[i,l] = Y[i,l] + omega * SZ / A[i,i];
                                   S = Math.Abs(SZ);
                                   if (S > R) R = S;
                               }//i
                               if (R > R1) R1 = R;
                           }//l
                       }
                       while (!(R1 < eps || p > maxit));
                       if (p > maxit) throw new Exception("W metodzie iteracji Gaussa-Seidela przekroczono zadaną liczbę iteracji nie osiągają zadanej dokładności obliczeń ");//return 23 ;
                       else return 0;
                   }
                   else throw new Exception("W metodzie iteracji Gaussa-Seidela niewłaściwie utalone rozmiary macierzy");
               }
               catch
               {
                   throw new Exception("W metodzie iteracji Gaussa-Seidela pojawił się błąd");
                   //PiszKomBladAlgeLin(20);
                   //return 33;
               }
           }//RozRowMacIGS
           /// <summary>
           ///  Metoda rozwiązywania równania macierzowego  A*X=B o elementach zespolonej
           ///  metodą iteracji Gaussa-Seidela dla pełnych macierzy typu  complex [,] 
           /// </summary>
           /// <param name="A">macierz kwadratowa rzędu N=A.GetLength(0)-1</param>
           /// <param name="B">macierz wyrazów wolnych o N=B.GetLength(0)-1  -wierszach i
           ///                  M=B.GetLength(1)t-1  -kolumnach</param>
           /// <param name="X">macierz rozwiązania o N=X.GetLength(0)-1  -wierszach i
           ///                  M=X.GetLength(1)-1  -kolumnach</param>
           /// <param name="eps">dokładność bezwzględna iteracji (np. eps=1E-8)</param>
           /// <param name="omega">parametr relaksacji omega =1 dla metody Gaussa-Seidela </param>
           /// <param name="maxit">zadana maksymalna liczba iteracji</param>
           /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
           public static int RozRowMacIGS(Complex.Complex[,] A, Complex.Complex[,] B, Complex.Complex[,] X,
                               double eps, double omega, int maxit)
           {
               int i, j, l, k, p, N, M, U, W;
               double R, R1, S, T, T1;
               Complex.Complex SZ, G;
               N = A.GetLength(0) - 1; M = A.GetLength(1) - 1;
               W = B.GetLength(0) - 1; U = B.GetLength(1) - 1;
               Complex.Complex[,] Y = new Complex.Complex[N + 1, U + 1];
               try
               {
                   if (W == N && M == N)
                   {  // Przekształcenie macierzy A do postaci z możliwie
                       //maksymalnymi co do modułu elementami na głównej
                       //przekątnej metodą przestawiania wierszy
                       for (i = 1; i <= N; i++)
                       {
                           T = A[i, i].Abs; k = i;
                           for (j = i + 1; j <= N; j++)
                           {
                               T1 = A[j, i].Abs;
                               if (T1 > T)
                               { T = T1; k = j; };
                           }
                           if (i != k)
                           {
                               //swap(B[k],B[i]); swap(A[k],A[i]);
                               for (l = 1; l <= U; l++)
                               {
                                   G = B[k, l]; B[k, l] = B[i, l]; B[i, l] = G;
                               }
                               for (l = 1; l <= N; l++)
                               {
                                   G = A[k, l]; A[k, l] = A[i, l]; A[i, l] = G;
                               }
                           }
                       };//i
                       //MetGaussa.SkalRowMacTyp(A,B);
                       p = 0;
                       for (i = 1; i <= N; i++)
                           for (j = 1; j <= U; j++) X[i, j] = B[i, j];
                       do
                       {
                           R1 = 0; p++;
                           for (i = 1; i <= N; i++)
                               for (j = 1; j <= U; j++) Y[i, j] = X[i, j];
                           for (l = 1; l <= U; l++)
                           {
                               R = 0.0;
                               for (i = 1; i <= N; i++)
                               {
                                   SZ = B[i, l];
                                   for (j = 1; j <= i - 1; j++) SZ -= A[i, j] * X[j, l];
                                   for (j = i; j <= N; j++) SZ -= A[i, j] * Y[j, l];
                                   X[i, l] = Y[i, l] + omega * SZ / A[i, i];
                                   S = SZ.Abs;
                                   if (S > R) R = S;
                               }//i
                               if (R > R1) R1 = R;
                           }//l
                       }
                       while (!(R1 < eps || p > maxit));
                       if (p > maxit) throw new Exception("W metodzie iteracji Gaussa-Seidela przekroczono zadaną liczbę iteracji nie osiągają zadanej dokładności obliczeń ");//return 23 ;
                       else return 0;
                   }
                   else throw new Exception("W metodzie iteracji Gaussa-Seidela niewłaściwie utalone rozmiary macierzy");
               }
               catch
               {
                   throw new Exception("W metodzie iteracji Gaussa-Seidela pojawił się błąd");
                   //PiszKomBladAlgeLin(20);
                   //return 33;
               }
           }//RozRowMacIGS

           //---------------------------------------------------------------------------

            /// <summary>
            /// // Metoda odwracanie macierzy metodą rozkładu LU wg algorytmu
            /// Crouta-Doolittle'a i zapisanie jej na miejscu oryginalnej
            /// </summary>
            /// <param name="A">Macierz kwadratowa rzędu NW=A.GetLength(0)-1</param>
            /// <param name="eps">dokładność określenia zerowego elementu przy 
            ///     poszukiwaniu elementu głównego</param>
            /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
            public static int OdwMacCroutDoolitta(double[,] A , double eps)
            {
              int i,j,k,u,k1,NW,NK,Nu,Nj1,Nk1;
              double T,T1 ;
              double S,W;
              //NW=MacA.size()-1;  NK=MacA[NW].size()-1;
              NW = A.GetLength(0) - 1; NK = A.GetLength(1) - 1;
              //vector<Typ> X;  InicjacjaWektora(X,NW);
              //vector<int> M;  InicjacjaWektora(M,NW);
              int[] M = new int[NW + 1];
              double[] X = new double[NW + 1];
              if (NK==NW)
              { // Przekształcenie macierzy A do postaci  niezawierającej
                  // elementów na głównej przekątnej
                  for (i=1; i<=NW; i++)
                  {
                   T=Math.Abs(A[i,i]); k=i;
                   if (T<eps)
                     {
                       for (j=i+1; j<=NW; j++)
                          {
                              T1 = Math.Abs(A[j,i]);
                            if (T1>T)  { T=T1 ; k=j;}
                          }
                        if (T<eps) return 15;
                        // Zamiana wiersza k-tego z i-tym
                        if (i != k)
                        {
                          //swap(A[k], A[i]);
                          for (j = 1; j <= NW; j++)
                          {
                              W = A[k, j]; A[i, j] = A[k, j]; A[k, j] = W;
                          }
                        }
                      }
                    M[i]=k;
                  }//i
                  // Rozkład LU macierzy A wg algorytmu Doolittle'a  A=L*U
                  for (k=1; k<=NW-1; k++)
                    {
                       k1=k+1;
                       if (A[k,k]!=0)
                           {
                             for (i=k1; i<=NW; i++)
                             {
                              if (A[k,i]!=0) A[k,i]/=A[k,k];
                             }
                           } else return 15;
                       for (i=k1; i<=NW; i++)
                         for (j=k1; j<=NW; j++)
                           if (A[i,k]!=0 && A[k,j]!=0)
                                    A[i,j]-=A[i,k]*A[k,j];
                    }//k
                  // Rozwiązanie układu trójkątnego dolnego L tj. L*Y=I
                  // metodą postępowania w przód
                  double a1=1.0;
                  for (i=1; i<=NW; i++)
                    {
                       A[i,i]=a1/A[i,i];
                       for (j=1; j<=i-1; j++)
                         { S=0;
                           for (k=j; k<=i-1; k++)  S-=A[i,k]*A[k,j];
                           A[i,j]=S*A[i,i];
                         }//j
                    }//i;
                      // Rozwiązanie układu trójkątnego górnego tj. U*X=Y metodą
                      // postępowania wstecz
                  for (u=1; u<=NW-1; u++)
                    {
                      Nu=NW-u;
                      for (k=1; k<=u; k++)
                        {
                          S=0; Nk1=NW-k+1;
                          for (j=1; j<=u; j++)
                          {
                             Nj1=NW-j+1;
                             S-=A[Nu,Nj1]*A[Nj1,Nk1];
                          }
                          X[k]=S;
                        }//k
                      for (i=1; i<=Nu; i++)
                        {
                           S=A[Nu,i];
                           for (j=1; j<=u; j++)
                             { Nj1=NW-j+1;  S-=A[Nu,Nj1]*A[Nj1,i]; }
                           A[Nu,i]=S;
                         }//i
                      for (k=1; k<=u; k++) A[Nu,NW-k+1]=X[k];
                     }//u
                  for (i=NW; i>=1; i--)
                    {
                      k=M[i];
                      if (k!=i)
                          for (j = 1; j <= NW; j++)
                          {
                              //swap(A[j,i], A[j,k]);
                              W = A[j, i]; A[j, i] = A[j, k]; A[j, k] = W;
                          }
                     }//i
                   return 0;
                } else return 14 ;
            }//OdwMacC
            //--------------------------------------------------------------------
            /// <summary>
            /// // Metoda odwracanie macierzy metodą rozkładu LU wg algorytmu
            /// Crouta-Doolittle'a i zapisanie jej na miejscu oryginalnej
            /// </summary>
            /// <param name="A">Macierz kwadratowa rzędu NW=A.GetLength(0)-1</param>
            /// <param name="eps">dokładność określenia zerowego elementu przy 
            ///     poszukiwaniu elementu głównego</param>
            /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
            public static int OdwMacCroutDoolitta(Complex.Complex[,] A, double eps)
            {
                int i, j, k, u, k1, NW, NK, Nu, Nj1, Nk1;
                double T, T1;
                Complex.Complex S, W;
                //NW=MacA.size()-1;  NK=MacA[NW].size()-1;
                NW = A.GetLength(0) - 1; NK = A.GetLength(1) - 1;
                //vector<Typ> X;  InicjacjaWektora(X,NW);
                //vector<int> M;  InicjacjaWektora(M,NW);
                int[] M = new int[NW + 1];
                Complex.Complex[] X = new Complex.Complex[NW + 1];
                if (NK == NW)
                { // Przekształcenie macierzy A do postaci  niezawierającej
                    // elementów na głównej przekątnej
                    for (i = 1; i <= NW; i++)
                    {
                        T = Complex.ComplexMath.Abs(A[i, i]); k = i;
                        if (T < eps)
                        {
                            for (j = i + 1; j <= NW; j++)
                            {
                                T1 = Complex.ComplexMath.Abs(A[j, i]);
                                if (T1 > T) { T = T1; k = j; }
                            }
                            if (T < eps) return 15;
                            // Zamiana wiersza k-tego z i-tym
                            if (i != k)
                            {
                                //swap(A[k], A[i]);
                                for (j = 1; j <= NW; j++)
                                {
                                    W = A[k, j]; A[i, j] = A[k, j]; A[k, j] = W;
                                }
                            }
                        }
                        M[i] = k;
                    }//i
                    // Rozkład LU macierzy A wg algorytmu Doolittle'a  A=L*U
                    for (k = 1; k <= NW - 1; k++)
                    {
                        k1 = k + 1;
                        if (A[k, k] != 0)
                        {
                            for (i = k1; i <= NW; i++)
                            {
                                if (A[k, i] != 0) A[k, i] /= A[k, k];
                            }
                        }
                        else return 15;
                        for (i = k1; i <= NW; i++)
                            for (j = k1; j <= NW; j++)
                                if (A[i, k] != 0 && A[k, j] != 0)
                                    A[i, j] -= A[i, k] * A[k, j];
                    }//k
                    // Rozwiązanie układu trójkątnego dolnego L tj. L*Y=I
                    // metodą postępowania w przód
                    double a1 = 1.0;
                    for (i = 1; i <= NW; i++)
                    {
                        A[i, i] = a1 / A[i, i];
                        for (j = 1; j <= i - 1; j++)
                        {
                            S = 0;
                            for (k = j; k <= i - 1; k++) S -= A[i, k] * A[k, j];
                            A[i, j] = S * A[i, i];
                        }//j
                    }//i;
                    // Rozwiązanie układu trójkątnego górnego tj. U*X=Y metodą
                    // postępowania wstecz
                    for (u = 1; u <= NW - 1; u++)
                    {
                        Nu = NW - u;
                        for (k = 1; k <= u; k++)
                        {
                            S = 0; 
                            Nk1 = NW - k + 1;
                            for (j = 1; j <= u; j++)
                            {
                                Nj1 = NW - j + 1;
                                S -= A[Nu, Nj1] * A[Nj1, Nk1];
                            }
                            X[k] = S;
                        }//k
                        for (i = 1; i <= Nu; i++)
                        {
                            S = A[Nu, i];
                            for (j = 1; j <= u; j++)
                            { Nj1 = NW - j + 1; S -= A[Nu, Nj1] * A[Nj1, i]; }
                            A[Nu, i] = S;
                        }//i
                        for (k = 1; k <= u; k++) A[Nu, NW - k + 1] = X[k];
                    }//u
                    for (i = NW; i >= 1; i--)
                    {
                        k = M[i];
                        if (k != i)
                            for (j = 1; j <= NW; j++)
                            {
                                //swap(A[j,i], A[j,k]);
                                W = A[j, i]; A[j, i] = A[j, k]; A[j, k] = W;
                            }
                    }//i
                    return 0;
                }
                else return 14;
            }//OdwMacC



           #endregion Statycznie
    }
}
