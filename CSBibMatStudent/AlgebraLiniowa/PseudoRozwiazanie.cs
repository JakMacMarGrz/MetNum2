using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSBibMatStudent.AlgebraLiniowa
{
    public static class PseudoRozwiazanie
    {
      #region Statyczne

        /// <summary>
        /// Metoda pseudorozwiązania równania macierzowego nadokreślonego A*X=B
        /// metodą najmniejszych kwadratów
        /// </summary>
        /// <param name="A">Macierz główna o M=A.GetLength(0)-1 wierszach</param>
        /// <param name="B">Macierz wyrazów wolnych o B.GetLength(0) - 1 wierszach
        /// i B.GetLength(1) - 1 kolumnach</param>
        /// <param name="X">Macierz pseudorozwiązania o X.GetLength(0)-1 wierszach
        /// i X.GetLength(1)-1 kolumnach</param>
        /// <param name="eps">dokładność rozróżnienia zerowej kolumny (np. eps=1E-20)</param>
        /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
        public static int PseRozNajmniejszeKwadraty(double[,] A,
                     double[,] B, double[,] X, double eps)
        {
          int Blad,i,j,k,M,N,R,U;
          double  S  ;
          M=A.GetLength(0)-1;  
          N=A.GetLength(1)-1;
          U = B.GetLength(0) - 1;
          R = B.GetLength(1) - 1;
          double[] Y=new double[M+1];
          double[,] AA = new double[N + 1, N + 1];
          double[,] BB = new double[N + 1, R + 1];
          if (M == U && B.GetLength(1) == X.GetLength(1) && X.GetLength(0) - 1 == N)
            { 
              //Obliczanie iloczynu macierzy transponowanej A tj.
              //At z macierzą  B i umieszczenie jej 
              //w tablicy BB:=At*B
              for (i=1; i<=R; i++)
                { // Zapamiętanie i-tej kolumny macierzy B jako wektora Y
                  for (k=1; k<=M; k++) Y[k]=B[k,i];
                  for (j=1; j<=N; j++)
                    {
                      S=0;
                      for (k=1; k<=M; k++) S+=A[k,j]*Y[k];
                      // Podstawienie iloczynu At*Y w i-tej
                      // kolumnie tablicy BB
                      BB[j,i]=S ;
                    }//j
                }//i
               //Obliczanie iloczynu macierzy transponowanej A
               //tj. At z macierzą A i umieszczenie jej 
               //w tablicy AA:=At*A
             
              for (i=1; i<=N; i++)
                { // Zapamiętanie i-tej kolumny macierzy A jako wektora Y
                  for (k=1; k<=M; k++) Y[k]=A[k,i];
                  for (j=i; j<=N; j++)
                    {
                      S=0;
                      for (k=1; k<=M; k++) S+=A[k,j]*Y[k];
                      // Podstawienie iloczynu podmacierzy At,wyciętej
                      //od i-tego wiersza z wektorem Y oraz umieszczanie
                      //wyniku tego iloczynu w i-tej kolumnie macierzy MacA
                      //od i-tego wiersza począwszy tj. dla j>=i
                      AA[j,i]=S ;
                    }//j
                }//i
              for (i=1; i<N; i++)
              for (j=i+1; j<=N; j++) AA[i,j]=AA[j,i];
              Blad = MetodaGaussa.RozRowMacGaussa(AA, BB, X, eps);
              if (Blad!=0) return 28;
                  else return 0;
            }
            else return 27;
        }//PseRozNajmniejszeKwadraty
        //----------------------------------------------------------------------------
        /// <summary>
        /// Metoda pseudorozwiązania równania macierzowego nadokreślonego A*X=B
        /// metodą najmniejszych kwadratów
        /// </summary>
        /// <param name="A">Macierz główna o M=A.GetLength(0)-1 wierszach</param>
        /// <param name="B">Macierz wyrazów wolnych o B.GetLength(0) - 1 wierszach
        /// i B.GetLength(1) - 1 kolumnach</param>
        /// <param name="X">Macierz pseudorozwiązania o X.GetLength(0)-1 wierszach
        /// i X.GetLength(1)-1 kolumnach</param>
        /// <param name="eps">dokładność rozróżnienia zerowej kolumny (np. eps=1E-20)</param>
        /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
        public static int PseRozNajmniejszeKwadraty(double[,] A,
                     double[] B, double[] X, double eps)
        {
            int Blad, i, j, k, M, N, U;
            double S;
            M = A.GetLength(0) - 1;
            N = A.GetLength(1) - 1;
            U = B.Length - 1;
            double[] Y = new double[M + 1];
            double[,] AA = new double[N + 1, N + 1];
            double[] BB = new double[N + 1];
            if (M == U &&  X.Length - 1 == N)
            {
                //Obliczanie iloczynu macierzy transponowanej A tj.
                //At z wektorem B i umieszczenie jej 
                //w wektorze BB:=At*B
              
                // Zapamiętanie wektora B jako wektora Y
                for (k = 1; k <= M; k++) Y[k] = B[k];
                for (j = 1; j <= N; j++)
                {
                    S = 0;
                    for (k = 1; k <= M; k++) S += A[k, j] * Y[k];
                    // Podstawienie iloczynu At*Y w wektorze BB
                    BB[j] = S;
                }//j
                //Obliczanie iloczynu macierzy transponowanej A
                //tj. At z macierzą A i umieszczenie jej 
                //w tablicy AA:=At*A

                for (i = 1; i <= N; i++)
                { // Zapamiętanie i-tej kolumny macierzy A jako wektora Y
                    for (k = 1; k <= M; k++) Y[k] = A[k, i];
                    for (j = i; j <= N; j++)
                    {
                        S = 0;
                        for (k = 1; k <= M; k++) S += A[k, j] * Y[k];
                        // Podstawienie iloczynu podmacierzy At,wyciętej
                        //od i-tego wiersza z wektorem Y oraz umieszczanie
                        //wyniku tego iloczynu w i-tej kolumnie macierzy MacA
                        //od i-tego wiersza począwszy tj. dla j>=i
                        AA[j, i] = S;
                    }//j
                }//i
                for (i = 1; i < N; i++)
                    for (j = i + 1; j <= N; j++) AA[i, j] = AA[j, i];
                Blad = MetodaGaussa.RozRowMacGaussa(AA, BB, X, eps);
                if (Blad != 0) return 28;
                else return 0;
            }
            else return 27;
        }//PseRozNajmniejszeKwadraty
        //----------------------------------------------------------------------------
         
        /// <summary>
        /// Metoda  pseudorozwiązania równania macierzowego nadokreślonego A*X=B
        /// </summary>
        /// <param name="A">macierz główna o A.GetLength(0) - 1 wierszach
        /// i A.GetLength(1) - 1 kolumnach</param>
        /// <param name="B">macierz wyrazów wolnych o  B.GetLength(0) - 1 wierszach
        /// i B.GetLength(1) - 1 kolumnach </param>
        /// <param name="X">macierz rozwiązania o B.GetLength(0) - 1 wierszach
        /// i B.GetLength(1) - 1 kolumnach</param>
        /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
        public static int PseRozQ(double[,] A,
                                     double[,] B, double[,] X)
        {
          double a1=1.0;
          int i,k,j,N1,M,N,R,U  ;
          int r,l;
          double Vk,Vp, W1,beta,R1,S  ;
          M = A.GetLength(0) - 1;      N = A.GetLength(1) - 1;
          U = B.GetLength(0) - 1;      R = B.GetLength(1) - 1;
          double[,] P = new double[M + 1, M + 1];
          double[,] Ar = new double[M + 1, N + 1];
          double[,] Br = new double[M + 1, R + 1];
          double[] V = new double[M + 1];
          //----------------------------------------------------------------------------
          if (M==U && M>=N )
            {
              Vk=0;
              for (i=1; i<=M; i++)
                Vk+=A[i,1]*A[i,1];
              Vp=Math.Sqrt(Vk);
              W1=Vp+A[1,1];   beta=(a1)/(Vk+Vp*A[1,1]);
              V[1]=W1;
              for (i=2; i<=M; i++)
                V[i]=A[i,1];
              //                            T
              //   Oblicza macierz beta*W*W  stopnia L*L
              for (i=1; i<=M; i++)
                for (j=i; j<=M; j++)
                  {
                    S=beta*V[i]*V[j];
                    P[i,j]=S;
                  }
              for (i=1; i<=M;i++)
                for (j=i; j<=M;j++)
                   P[j,i]=P[i,j];
              //                             T
              // Oblicza macierz Q=I-beta*W*W  stopnia L*L
              for (i=1; i<=M; i++)
                for (j=1; j<=M; j++)
                   P[i,j]=-P[i,j];
              for (i=1; i<=M; i++)
                   P[i,i]+=a1;
              // Oblicza iloczyn macierzy P(k)*A(k) w pierwszym kroku iteracyjnym
              // (wzór (1.103)) z uwzględnieniem oszczędności operacji
              // wynikających z rozrzedzenia macierzy P(k) oraz A(k)
              k=1;
              for (i=1; i<=M; i++)
                for (j=1; j<=N; j++) Ar[i,j]=A[i,j];
              for (i=k; i<=M; i++)
                for (j=k+1; j<=N; j++)
                 {
                   S=0;
                   for (r=k; r<=M; r++) S+=P[i,r]*Ar[r,j];
                   A[i,j]=S;
                 }
              i=k; j=k; S=0;
              for (r=k; r<=M; r++)
                  S+=P[i,r]*Ar[r,j];
              A[i,j]=S;
              for (i=k+1; i<=M; i++)
                  A[i,k]=0;
             // Oblicza iloczyn macierzy P(k)*A(k) w pierwszym kroku iteracyjnym
             // (wzór (2.128)) z uwzględnieniem oszczędności operacji
             // wynikających z rozrzedzenia macierzy P(k)
              k=1;
              for (i=1; i<=M; i++)
                for (j=1; j<=R; j++) Br[i,j]=B[i,j];
              for (i=k; i<=M; i++)
                for (j=1; j<=R; j++)
                  {
                     S=0;
                     for (l=k; l<=M; l++)
                          S+=P[i,l]*Br[l,j];
                     B[i,j]=S  ;
                  }
              // Pętla przemnożeń ortogonalnych wg (2.128), przy czym
              // N1=N-1 gdy A jest macierzą kwadratową oraz
              // N1=N gdy A jest macierzą prostokątną M*N
              if (M==N)  N1=N-1;  else N1=N;
              for (k=2; k<=N1; k++)
                {
                  Vk=0;
                  for (i=k; i<=M; i++)
                        Vk+=A[i,k]*A[i,k];
                  Vp=Math.Sqrt(Vk) ;
                  W1=Vp+A[k,k];   beta=a1/(Vk+Vp*A[k,k]);
                  V[1]=W1;
                  for (i=k+1; i<=M; i++)
                        V[i-k+1]=A[i,k];
                 //                          T
                 //  Oblicza macierz beta*W*W  stopnia L*L
                  for (i=1; i<=M-k+1; i++)
                   for (j=i; j<=M-k+1; j++)
                      P[i,j]=V[i]*V[j]*beta;
                  for (i=1; i<=M-k+1;i++)
                    for (j=i; j<=M-k+1;j++)
                      P[j,i]=P[i,j] ;
                  //                             T
                  // Oblicza macierz Q=I-beta*W*W  stopnia L*L
                  for (i=1; i<=M-k+1; i++)
                    for (j=1; j<=M-k+1; j++)
                      P[i,j]=-P[i,j];
                  for (i=1; i<=M-k+1; i++)
                      P[i,i]=a1+P[i,i] ;
                  // Umieszcza macierz Q(k) stopnia L*L w macierzy P zachowując
                  // strukturę (2.124)
                  for (i=0; i<=M-k; i++)
                    for (j=1; j<=M-k+1; j++) P[M-i,j]=P[M-k+1-i,j];
                  for (j=0; j<=M-k; j++)
                    for (i=k; i<=M; i++) P[i,M-j]=P[i,M-k+1-j];
                  for (i=1; i<=M; i++)
                    for (j=1; j<=k-1; j++) P[i,j]=0;
                  for (i=1; i<=k-1; i++)
                    for (j=k; j<=M; j++) P[i,j]=0;
                  for (i=1; i<=k-1; i++) P[i,i]=a1 ;
                  // Oblicza iloczyn macierzy P(k)*A(k) w k-tym kroku iteracyjnym
                  // (wzór (2.125)) z uwzględnieniem oszczędności operacji
                  // wynikajacych z rozrzedzenia macierzy P(k) oraz A(k)
                  for (i=1; i<=M; i++)
                    for (j=1; j<=N; j++) Ar[i,j]=A[i,j];
                  for (i=k; i<=M; i++)
                    for (j=k+1; j<=N; j++)
                     {
                       S=0;
                       for (r=k; r<=M; r++) S+=P[i,r]*Ar[r,j];
                       A[i,j]=S;
                     }
                  i=k; j=k; S=0;
                  for (r=k; r<=M; r++) S+=P[i,r]*Ar[r,j];
                  A[i,j]=S;
                  for (i=k+1; i<=M; i++) A[i,k]=0;
                  // Oblicza iloczyn macierzy P(k)*A(k) w k-tym kroku iteracyjnym
                  // (wzór (2.128)) z uwzględnieniem oszczędności operacji
                  // wynikających z rozrzedzenia macierzy P(k)
                  for (i=1; i<=M; i++)
                    for (j=1; j<=R; j++) Br[i,j]=B[i,j];
                  for (i=k; i<=M; i++)
                    for (j=1; j<=R; j++)
                      {
                        S=0;
                        for (l=k; l<=M; l++) S+=P[i,l]*Br[l,j];
                        B[i,j]=S  ;
                      }
                }//k
                //--------------------------------------------------------------
               // Rozwiązanie równania macierzowego o strukturze macierzy
               // trójkątnej (2.130) równoważnego z równaniem wejściowym (2.127)
               // metodą postępowania wstecz
              for (k=1; k<=R; k++)
               for (i=N; i>=1; i--)
                {
                  R1=B[i,k];
                  for (j=i+1; j<=N; j++) R1-=A[i,j]*X[j,k];
                  X[i,k]=R1/A[i,i] ;
                }//k,i
              return 0;
            } else return 26 ;
        }//PseRozQ
        //-------------------------------------------------------------------
        /// <summary>
        /// Metoda  pseudorozwiązania równania macierzowego nadokreślonego A*X=B
        /// </summary>
        /// <param name="A">macierz główna o A.GetLength(0) - 1 wierszach
        /// i A.GetLength(1) - 1 kolumnach</param>
        /// <param name="B">macierz wyrazów wolnych o  B.GetLength(0) - 1 wierszach
        /// i B.GetLength(1) - 1 kolumnach </param>
        /// <param name="X">macierz rozwiązania o B.GetLength(0) - 1 wierszach
        /// i B.GetLength(1) - 1 kolumnach</param>
        /// <returns>return 0 brak błędu, inna wartość = numer błędu</returns>
        public static int PseRozQ(double[,] A,
                                     double[] B, double[] X)
        {
            double a1 = 1.0;
            int i, k, j, N1, M, N, U;
            int r, l;
            double Vk, Vp, W1, beta, R1, S;
            M = A.GetLength(0) - 1; N = A.GetLength(1) - 1;
            U = B.Length - 1; 
            double[,] P = new double[M + 1, M + 1];
            double[,] Ar = new double[M + 1, N + 1];
            double[] Br = new double[M + 1];
            double[] V = new double[M + 1];
            //----------------------------------------------------------------------------
            if (M == U && M >= N)
            {
                Vk = 0;
                for (i = 1; i <= M; i++)
                    Vk += A[i, 1] * A[i, 1];
                Vp = Math.Sqrt(Vk);
                W1 = Vp + A[1, 1]; beta = (a1) / (Vk + Vp * A[1, 1]);
                V[1] = W1;
                for (i = 2; i <= M; i++)
                    V[i] = A[i, 1];
                //                            T
                //   Oblicza macierz beta*W*W  stopnia L*L
                for (i = 1; i <= M; i++)
                    for (j = i; j <= M; j++)
                    {
                        S = beta * V[i] * V[j];
                        P[i, j] = S;
                    }
                for (i = 1; i <= M; i++)
                    for (j = i; j <= M; j++)
                        P[j, i] = P[i, j];
                //                             T
                // Oblicza macierz Q=I-beta*W*W  stopnia L*L
                for (i = 1; i <= M; i++)
                    for (j = 1; j <= M; j++)
                        P[i, j] = -P[i, j];
                for (i = 1; i <= M; i++)
                    P[i, i] += a1;
                // Oblicza iloczyn macierzy P(k)*A(k) w pierwszym kroku iteracyjnym
                // (wzór (1.103)) z uwzględnieniem oszczędności operacji
                // wynikających z rozrzedzenia macierzy P(k) oraz A(k)
                k = 1;
                for (i = 1; i <= M; i++)
                    for (j = 1; j <= N; j++) Ar[i, j] = A[i, j];
                for (i = k; i <= M; i++)
                    for (j = k + 1; j <= N; j++)
                    {
                        S = 0;
                        for (r = k; r <= M; r++) S += P[i, r] * Ar[r, j];
                        A[i, j] = S;
                    }
                i = k; j = k; S = 0;
                for (r = k; r <= M; r++)
                    S += P[i, r] * Ar[r, j];
                A[i, j] = S;
                for (i = k + 1; i <= M; i++)
                    A[i, k] = 0;
                // Oblicza iloczyn macierzy P(k)*A(k) w pierwszym kroku iteracyjnym
                // (wzór (2.128)) z uwzględnieniem oszczędności operacji
                // wynikających z rozrzedzenia macierzy P(k)
                k = 1;
                for (i = 1; i <= M; i++) Br[i] = B[i];
                    //for (j = 1; j <= R; j++) Br[i, j] = B[i, j];
                for (i = k; i <= M; i++)
                    //for (j = 1; j <= R; j++)
                {
                    S = 0;
                    for (l = k; l <= M; l++)
                        S += P[i, l] * Br[l];
                    B[i] = S;
                }
                // Pętla przemnożeń ortogonalnych wg (2.128), przy czym
                // N1=N-1 gdy A jest macierzą kwadratową oraz
                // N1=N gdy A jest macierzą prostokątną M*N
                if (M == N) N1 = N - 1; else N1 = N;
                for (k = 2; k <= N1; k++)
                {
                    Vk = 0;
                    for (i = k; i <= M; i++)
                        Vk += A[i, k] * A[i, k];
                    Vp = Math.Sqrt(Vk);
                    W1 = Vp + A[k, k]; beta = a1 / (Vk + Vp * A[k, k]);
                    V[1] = W1;
                    for (i = k + 1; i <= M; i++)
                        V[i - k + 1] = A[i, k];
                    //                          T
                    //  Oblicza macierz beta*W*W  stopnia L*L
                    for (i = 1; i <= M - k + 1; i++)
                        for (j = i; j <= M - k + 1; j++)
                            P[i, j] = V[i] * V[j] * beta;
                    for (i = 1; i <= M - k + 1; i++)
                        for (j = i; j <= M - k + 1; j++)
                            P[j, i] = P[i, j];
                    //                             T
                    // Oblicza macierz Q=I-beta*W*W  stopnia L*L
                    for (i = 1; i <= M - k + 1; i++)
                        for (j = 1; j <= M - k + 1; j++)
                            P[i, j] = -P[i, j];
                    for (i = 1; i <= M - k + 1; i++)
                        P[i, i] = a1 + P[i, i];
                    // Umieszcza macierz Q(k) stopnia L*L w macierzy P zachowując
                    // strukturę (2.124)
                    for (i = 0; i <= M - k; i++)
                        for (j = 1; j <= M - k + 1; j++) P[M - i, j] = P[M - k + 1 - i, j];
                    for (j = 0; j <= M - k; j++)
                        for (i = k; i <= M; i++) P[i, M - j] = P[i, M - k + 1 - j];
                    for (i = 1; i <= M; i++)
                        for (j = 1; j <= k - 1; j++) P[i, j] = 0;
                    for (i = 1; i <= k - 1; i++)
                        for (j = k; j <= M; j++) P[i, j] = 0;
                    for (i = 1; i <= k - 1; i++) P[i, i] = a1;
                    // Oblicza iloczyn macierzy P(k)*A(k) w k-tym kroku iteracyjnym
                    // (wzór (2.125)) z uwzględnieniem oszczędności operacji
                    // wynikajacych z rozrzedzenia macierzy P(k) oraz A(k)
                    for (i = 1; i <= M; i++)
                        for (j = 1; j <= N; j++) Ar[i, j] = A[i, j];
                    for (i = k; i <= M; i++)
                        for (j = k + 1; j <= N; j++)
                        {
                            S = 0;
                            for (r = k; r <= M; r++) S += P[i, r] * Ar[r, j];
                            A[i, j] = S;
                        }
                    i = k; j = k; S = 0;
                    for (r = k; r <= M; r++) S += P[i, r] * Ar[r, j];
                    A[i, j] = S;
                    for (i = k + 1; i <= M; i++) A[i, k] = 0;
                    // Oblicza iloczyn macierzy P(k)*A(k) w k-tym kroku iteracyjnym
                    // (wzór (2.128)) z uwzględnieniem oszczędności operacji
                    // wynikających z rozrzedzenia macierzy P(k)
                    for (i = 1; i <= M; i++) Br[i] = B[i];
                        //for (j = 1; j <= R; j++) Br[i, j] = B[i, j];
                    for (i = k; i <= M; i++)
                        //for (j = 1; j <= R; j++)
                    {
                        S = 0;
                        for (l = k; l <= M; l++) S += P[i, l] * Br[l];
                        B[i] = S;
                    }
                }//k
                //--------------------------------------------------------------
                // Rozwiązanie równania macierzowego o strukturze macierzy
                // trójkątnej (2.130) równoważnego z równaniem wejściowym (2.127)
                // metodą postępowania wstecz
                //for (k = 1; k <= R; k++)
                for (i = N; i >= 1; i--)
                {
                    R1 = B[i];
                    for (j = i + 1; j <= N; j++) R1 -= A[i, j] * X[j];
                    X[i] = R1 / A[i, i];
                }//k,i
                return 0;
            }
            else return 26;
        }//PseRozQ

        #endregion Statyczne



    }
}
