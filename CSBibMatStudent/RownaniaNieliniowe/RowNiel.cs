using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSBibMatStudent.AlgebraLiniowa;
using CSBibMatStudent;


namespace CSBibMatStudent.RownaniaNieliniowe
{
    
    public abstract class RowNieLinAbstr 
    {
        public double[] X;
        protected double[] F;
        protected int N, M, maxit;
        protected double eps, epsr, epsg;
        public RowNieLinAbstr(int N1, double[] X0, double eps, double epsr, double epsg, int maxit1)
        {
            N = N1;
            M = N1;
            //X = X0;
            this.eps = eps;
            this.epsr = epsr;
            this.epsg = epsg;
            maxit = maxit1;
            F = new double[N + 1];
            X = new double[N + 1];
            for (int i = 0; i <= N; i++)
            {
                F[i] = 0; X[i] = X0[i];
            }

        }
        /// <summary>
        /// Generacja macierzy Jacobiego
        /// Metoda jest wirtualna, można więc w klasach potomnych generować
        /// macierz Jacobiego analitycznie o ile jest to możliwe
        /// </summary>
        /// <param name="MacJac">Macierz Jacobiego</param>
        /// <param name="WekX">Punkt w którym oblicza się macierz Jacobiego</param>
        /// <param name="M">Liczba funkcji nieliniowych</param>
        /// <param name="eps2">Względny krok różniczkowania </param>
        /// <returns>return 0 brak błędu, różny od zera numer błędu</returns>
        public virtual int GeneracjaMacierzyJacobiego(double[,] MacJac,
                               double[] WekX, int M, double eps2)
        {
            int i, j;
            double h, r;
            double[] X1 = new double[N + 1];
            double[] Y1 = new double[M + 1];
            double[] WekY = new double[M + 1];
            WekY=FunNieLin(WekX);
            try
            {
                for (i = 1; i <= N; i++)
                {
                    r = Math.Abs(WekX[i]);
                    if (r > eps2) h = eps2 * r;
                    else h = eps2;
                    for (j = 1; j <= N; j++) X1[j] = WekX[j];
                    X1[i] = WekX[i] + h;
                    Y1=FunNieLin(X1);
                    for (j = 1; j <= M; j++) MacJac[j, i] = (Y1[j] - WekY[j]) / h;
                }//i
                return 0;
            }
            catch
            {
                return 23;
            }
        }//GeneracjaMacierzyJacobiego
       
        public abstract double[] FunNieLin(double[] X);
        public abstract int Rozwiaz();

    }
    //------------------------------------------------------
    /// <summary>
    /// Klas abstrakcyjna zawierająca metodę Rozwiaz() implementującą metodę Newtona
    /// </summary>
    public abstract class RowNieLinMetodaNewtonaAbstr : RowNieLinAbstr
    {
        /// <summary>
        /// Konstruktor metody Newtona
        /// </summary>
        /// <param name="N1">Liczba równań</param>
        /// <param name="X0">Wektor warunków początkowych iteracji</param>
        /// <param name="eps">Dokładność obliczeń</param>
        /// <param name="epsr">Względny krok różniczkowania przy obliczaniu numerycznym Jacobiego </param>
        /// <param name="epsg">Dokładność rozróżnienia maksymalnego elementu głównego w metodzie eliminacji Gauus 
        ///  w metodzie Newtona</param>
        /// <param name="maxit1">Maksymalna liczba iteracji</param>
        public RowNieLinMetodaNewtonaAbstr(int N1, double[] X0, double eps, double epsr, double epsg, int maxit1) :
            base(N1, X0, eps, epsr, epsg, maxit1)
        { }
        //----------------------------------
        public override int Rozwiaz()
        {
            int i, k, blad = 0;
            double[] dX = new double[N + 1];
            double[] XX = new double[N + 1];
            double[] Y = new double[N + 1];
            double[,] MacJac = new double[N + 1, N + 1];
            double ni,ni1;
            k = 0;
            do
            {//Konstrukcja procesu iteracyjnego 
                ni = 0;
                ni1 = 0;
                k++;
                //Konstrukcja macierzy Jacobiego MacJac 
                blad = GeneracjaMacierzyJacobiego(MacJac, X, N, epsr);
                Y=FunNieLin(X);
                for (i = 1; i <= N; i++) XX[i] = Y[i];
                //Rozwiązanie układu )
                blad = MetodaGaussa.RozRowMacGaussa(MacJac, XX, dX, epsg);
                if (blad == 0)
                {
                    for (i = 1; i <= N; i++)
                    {
                        ni += Math.Abs(dX[i]);
                        ni1 += Math.Abs(Y[i]);
                        X[i] -= dX[i];
                    }
                }
                else return 2;
            }
            while (!(ni < eps || k > maxit || blad != 0));
            if (blad == 0)
            {
                if (k > maxit) return 1;
                else return 0;
            }
            else return blad;

        }//Koniec Rozwiaz()
        //------------------------------------------
    }//Koniec RowNieLinMetodaNewtonaAbstr
    //----------------------------------------------------------------------
    
    /// <summary>
    ///Klas pochodna względem klasy RowNieLinMetodaNewtonaAbstr
    ///której konstruktor przekazuje egzemplarz delegata FunNieLinDelegate funkcji 
    ///definiującej układ równań nieliniowych
    /// </summary>
    public class RowNieLinMetodaNewtona : RowNieLinMetodaNewtonaAbstr
    {
        FunWektorWektorDelegate FF;
        /// <summary>
        /// Konstruktor metody Newtona
        /// </summary>
        ///  <param name="FNLD">Egzemparz delegata funkcji definiującej układ równań nieliniowych </param>
        /// <param name="N1">Liczba równań</param>
        /// <param name="X0">Wektor warunków początkowych iteracji</param>
        /// <param name="eps">Dokładność obliczeń</param>
        /// <param name="epsr">Względny krok różniczkowania przy obliczaniu numerycznym Jacobiego </param>
        /// <param name="epsg">Dokładność rozróżnienia maksymalnego elementu głównego w metodzie eliminacji Gauus 
        ///  w metodzie Newtona</param>
        /// <param name="maxit1">Maksymalna liczba iteracji</param>
        public RowNieLinMetodaNewtona(FunWektorWektorDelegate FNLD,int N1, double[] X0,
             double eps, double epsr, double epsg, int maxit1): base(N1, X0, eps, epsr, epsg, maxit1)
        {
          FF = FNLD;
        }

        public override double[] FunNieLin(double[] X)
        {
            //FunNieLinDelegate F;
            return FF(X);
        }

    }//Koniec RowNieLinMetodaNewtona
    //---------------------------------------------------------------------------

    /// <summary>
    /// Klas abstrakcyjna zawierająca metodę Rozwiaz() implementującą metodę gradientu sprzężonego
    /// </summary>
    public abstract class RowNieLinMetodaGradientuSprzezonegoAbstr : RowNieLinAbstr
    {
        /// <summary>
        /// Konstruktor metody gradientu sprzężonego
        /// </summary>
        /// <param name="N1">Liczba równań</param>
        /// <param name="X0">Wektor warunków początkowych iteracji</param>
        /// <param name="eps">Dokładność obliczeń</param>
        /// <param name="epsr">Względny krok różniczkowania przy obliczaniu numerycznym Jacobiego </param>
        /// <param name="maxit1">Maksymalna liczba iteracji</param>
        public RowNieLinMetodaGradientuSprzezonegoAbstr(int N1, double[] X0, double eps, double epsr, double epsg, int maxit1) :
            base(N1, X0, eps, epsr, epsg, maxit1)
        { }
        //----------------------------------
        public override int Rozwiaz()
        {
            int i, j, k, l, Blad;
            double s, s1, mi, ni, beta, beta1, beta2, xx;
            N = X.Length - 1;
            double[,] MacJac = new double[M + 1, N + 1];
            double[] dX = new double[N + 1];
            double[] dX1 = new double[N + 1];
            double[] Y = new double[M + 1];
            double[] G = new double[M + 1];
            k = 0; l = 0;
            Blad = GeneracjaMacierzyJacobiego(MacJac, X, M, epsr);
            if (Blad == 0)
            {
                ni = 0;
                Y=FunNieLin(X);
                for (i = 1; i <= N; i++)
                {
                    s = 0;
                    for (j = 1; j <= M; j++) s += MacJac[j, i] * Y[j];
                    dX[i] = 2 * s;      //Wyznaczanie składowych gradientu
                }//i
                for (i = 1; i <= N; i++) ni += Math.Abs(dX[i]);
                beta2 = 0;
                for (i = 1; i <= N; i++) { xx = dX[i]; beta2 += xx * xx; }
            }
            else return 11;

            while (!(ni < eps || l > maxit || Blad != 0))
            {
                ni = 0;
                k++; l++;
                for (i = 1; i <= M; i++)
                {
                    s = 0;
                    for (j = 1; j <= N; j++) s += MacJac[i, j] * dX[j];
                    G[i] = s;
                }//i
                s = 0; s1 = 0;
                for (i = 1; i <= M; i++)
                {
                    s += Y[i] * G[i]; s1 += G[i] * G[i];
                }
                mi = s / s1;
                for (i = 1; i <= N; i++) X[i] -= mi * dX[i];   //Minimalizacja w kierunku
                for (i = 1; i <= N; i++) ni += Math.Abs(dX[i]);
                //Wyznaczanie kierunku sprzeżonego
                Y=FunNieLin(X);
                Blad = GeneracjaMacierzyJacobiego(MacJac, X, M, epsr);
                for (i = 1; i <= N; i++)
                {
                    s = 0;
                    for (j = 1; j <= M; j++) s += MacJac[j, i] * Y[j];
                    dX1[i] = 2.0 * s;   //Wyznaczanie składowych gradientu
                }//i
                beta1 = 0;
                for (i = 1; i <= N; i++) { xx = dX1[i]; beta1 += xx * xx; }
                beta = beta1 / beta2;
                for (i = 1; i <= N; i++) dX[i] = dX1[i] + beta * dX[i];
                beta2 = beta1;
                if (k > N)
                {
                    k = 1;
                    for (i = 1; i <= N; i++) dX[i] = dX1[i];
                }
            }//Koniec  while
            if (l > maxit) return 3;
            else
            {
                if (Blad != 0) return 23;
                else return 0;
            }

        }//Koniec Rozwiaz()
        //------------------------------------------
    }//Koniec RowNieLinMetodaGradientuSprzezonegoAbstr
    //----------------------------------------------------------------------

    /// <summary>
    ///Klas pochodna względem klasy RowNieLinMetodaGradientuSprzezonegoAbstr
    ///której konstruktor przekazuje egzemplarz delegata FunNieLinDelegate funkcji 
    ///definiującej układ równań nieliniowych
    /// </summary>
    public class RowNieLinMetodaGradientuSprzezonego : RowNieLinMetodaGradientuSprzezonegoAbstr
    {
        FunWektorWektorDelegate FF;
        /// <summary>
        /// Konstruktor metody gradientu sprzężonego
        /// </summary>
        /// <param name="FNLD">Egzemparz delegata funkcji definiującej układ równań nieliniowych </param>
        /// <param name="N1">Liczba równań</param>
        /// <param name="X0">Wektor warunków początkowych iteracji</param>
        /// <param name="eps">Dokładność obliczeń</param>
        /// <param name="epsr">Względny krok różniczkowania przy obliczaniu numerycznym Jacobiego </param>
        /// <param name="maxit1">Maksymalna liczba iteracji</param>
        public RowNieLinMetodaGradientuSprzezonego(FunWektorWektorDelegate FNLD, int N1, double[] X0,
             double eps, double epsr, double epsg, int maxit1)
            : base(N1, X0, eps, epsr, epsg, maxit1)
        {
            FF = FNLD;
        }

        public override double[] FunNieLin(double[] X)
        {
           return FF(X);
        }

    }//Koniec RowNieLinMetodaGradientuSprzezonego
    //---------------------------------------------------------------------------

    /// <summary>
    ///Klas abstrakcyjna zawierająca metodę Rozwiaz() implementującą metodę zmiennej metryki BFGS
    ///wg. Broydena - Fletchera - Goldfarba  - Shanno
    /// </summary>
    public abstract class RowNieLinMetodaZmiennejMetrykiBGFSAbstr : RowNieLinAbstr
    {
        /// <summary>
        /// Konstruktor metody zmiennej metryki BFGS
        /// </summary>
        /// <param name="N1">Liczba równań</param>
        /// <param name="X0">Wektor warunków początkowych iteracji</param>
        /// <param name="eps">Dokładność obliczeń</param>
        /// <param name="epsr">Względny krok różniczkowania przy obliczaniu numerycznym Jacobiego </param>
        /// <param name="maxit1">Maksymalna liczba iteracji</param>
        public RowNieLinMetodaZmiennejMetrykiBGFSAbstr(int N1, double[] X0, double eps, double epsr, double epsg, int maxit1) :
            base(N1, X0, eps, epsr, epsg, maxit1)
        { }
        //----------------------------------
        public override int Rozwiaz()
        {
            int i, j, k, l, Blad;
            double s, s1, mi, ni, suma, m, m1;
            double[,] MacJac = new double[M + 1, N + 1];
            double[,] V = new double[N + 1, N + 1];
            double[] WekD = new double[N + 1];
            double[] X0 = new double[N + 1];
            double[] gradF = new double[N + 1];
            double[] gradF0 = new double[N + 1];
            double[] Y = new double[N + 1];
            double[] G = new double[N + 1];
            double[] Z1 = new double[N + 1];
            double[] S1 = new double[N + 1];
            double[] Y1 = new double[N + 1];
            for (i = 1; i <= N; i++)
                for (j = 1; j <= N; j++) if (i == j) V[i, j] = 1.0; else V[i, j] = 0;
            k = 0; l = 0;
            Blad = GeneracjaMacierzyJacobiego(MacJac, X, M, 1e-10);
            if (Blad == 0)
            {
                ni = 0;
                Y=FunNieLin(X);
                for (i = 1; i <= N; i++)
                {
                    s = 0;
                    for (j = 1; j <= M; j++) s += MacJac[j, i] * Y[j];
                    gradF0[i] = 2.0 * s;      //Wyznaczanie składowych gradientu
                }//i
                for (i = 1; i <= N; i++) ni += gradF0[i] * gradF0[i];
                ni = Math.Sqrt(ni);
            }
            else return 11;
            while (!(ni < eps || l > maxit || Blad != 0))
            {
                ni = 0;
                k++; l++;
                for (i = 1; i <= N; i++)
                {
                    suma = 0;
                    for (j = 1; j <= N; j++) suma += V[i, j] * gradF0[j];
                    WekD[i] = suma;
                }
                //Minimalizacja w kierunku
                for (i = 1; i <= M; i++)
                {
                    s = 0;
                    for (j = 1; j <= N; j++) s += MacJac[i, j] * WekD[j];
                    G[i] = s;
                }//i
                s = 0; s1 = 0;
                for (i = 1; i <= M; i++)
                {
                    s += Y[i] * G[i]; s1 += G[i] * G[i];
                }
                mi = s / s1;
                for (i = 1; i <= N; i++) X[i] -= mi * WekD[i];
                //Koniec minimalizacji w kierunku
                //for (i=1; i<=N; i++) ni+=gradF0[i]*gradF0[i];
                //ni=Math.Sqrt(ni);
                for (i = 1; i <= N; i++) ni += Math.Abs(gradF0[i]);

                //Korekcja macierzy odwrotnej Hesjanu V 
                Y=FunNieLin(X);
                Blad = GeneracjaMacierzyJacobiego(MacJac, X, M, 1e-10);
                for (i = 1; i <= N; i++)
                {
                    s = 0;
                    for (j = 1; j <= M; j++) s += MacJac[j, i] * Y[j];
                    gradF[i] = 2.0 * s;   //Wyznaczanie składowych gradientu
                    S1[i] = X[i] - X0[i];
                    Y1[i] = gradF[i] - gradF0[i];
                }//i
                for (i = 1; i <= N; i++)
                {
                    suma = 0;
                    for (j = 1; j <= N; j++) suma += V[i, j] * Y1[j];
                    Z1[i] = suma;
                }
                m = 0;
                m1 = 0;
                for (i = 1; i <= N; i++)
                {
                    m += Y1[i] * S1[i];
                    m1 += (S1[i] + Z1[i]) * Y1[i];
                }
                for (i = 1; i <= N; i++)
                    for (j = 1; j <= N; j++) V[i, j] += m1 * S1[i] * S1[j] / (m * m) - (S1[i] * Z1[j] + Z1[i] * S1[j]) / m;
                for (i = 1; i <= N; i++)
                {
                    X0[i] = X[i];
                    gradF0[i] = gradF[i];
                }
                if (k > N) //to pozostaw za V bmacierz jednostkową
                {
                    k = 1;
                    for (i = 1; i <= N; i++)
                        for (j = 1; j <= N; j++) if (i == j) V[i, j] = 1.0; else V[i, j] = 0;
                }
            }
            if (l > maxit) return 3;
            else
            {
                if (Blad != 0) return 23;
                else return 0;
            }
  
        }//Koniec Rozwiaz()
        //------------------------------------------
    }//Koniec RowNieLinMetodaZmiennejMetrykiBGFSAbstr
    //----------------------------------------------------------------------

    /// <summary>
    ///Klas pochodna względem klasy RowNieLinMetodaZmiennejMetrykiBGFSAbstr
    ///której konstruktor przekazuje egzemplarz delegata FunNieLinDelegate funkcji 
    ///definiującej układ równań nieliniowych
    /// </summary>
    public class RowNieLinMetodaZmiennejMetrykiBGFS : RowNieLinMetodaZmiennejMetrykiBGFSAbstr
    {
        FunWektorWektorDelegate FF;
        /// <summary>
        /// Konstruktor metody zmiennej metryki BFGS
        /// wg. Broydena - Fletchera - Goldfarba  - Shanno
        /// </summary>
        /// <param name="FNLD">Egzemparz delegata funkcji definiującej układ równań nieliniowych </param>
        /// <param name="N1">Liczba równań</param>
        /// <param name="X0">Wektor warunków początkowych iteracji</param>
        /// <param name="eps">Dokładność obliczeń</param>
        /// <param name="epsr">Względny krok różniczkowania przy obliczaniu numerycznym Jacobiego </param>
        /// <param name="maxit1">Maksymalna liczba iteracji</param>
        public RowNieLinMetodaZmiennejMetrykiBGFS(FunWektorWektorDelegate FNLD, int N1, double[] X0,
             double eps, double epsr, double epsg, int maxit1)
            : base(N1, X0, eps, epsr, epsg, maxit1)
        {
            FF = FNLD;
        }

        public override double[] FunNieLin(double[] X)
        {
           return FF(X);
        }

    }//Koniec RowNieLinMetodaZmiennejMetrykiBGF
    //---------------------------------------------------------------------------

  

    
    public class RowNieLin
    {
        public double[] X;
        private double[] F;
        private int N, M, maxit;
        private double eps, epsr, epsg;

        //public delegate void FunNieLin(double[] F, double[] X);
        //public delegate double[] FunNieLin(double[] X);

        public RowNieLin(int N1, double[] X0, double eps1, int maxit1)
        {
            N = N1;
            M = N1;
            //X = X0;
            eps = eps1;
            epsr = 1e-9;
            epsg = 1e-20;
            maxit = maxit1;
            F = new double[N + 1];
            X = new double[N + 1];
            for (int i = 0; i <= N; i++)
            {
                F[i] = 0; X[i] = X0[i];
            }

        }
        public RowNieLin(int N1,int M1, double[] X0, double eps1, int maxit1)
        {
            N = N1;
            M = M1;
            //X = X0;
            eps = eps1;
            epsr = 1e-9;
            epsg = 1e-20;
            maxit = maxit1;
            F = new double[N + 1];
            X = new double[N + 1];
            for (int i = 0; i <= N; i++)
            {
                F[i] = 0; X[i] = X0[i];
            }

        }

        public int GeneracjaMacierzyJacobiego(FunWektorWektorDelegate FF, double[,] MacJac,
                                double[] WekX, int M, double eps2)
        {
            int i, j;
            double h, r;
            double[] X1 = new double[N + 1];
            double[] Y1 = new double[M + 1];
            double[] WekY = new double[M + 1];
            //Y1(M),WekY(M);
            //FF(WekX);
            WekY=FF(WekX);
            try
            {
                for (i = 1; i <= N; i++)
                {
                    r = Math.Abs(WekX[i]);
                    if (r > eps2) h = eps2 * r;
                    else h = eps2;
                    //X1=Kopiuj(WekX); 
                    for (j = 1; j <= N; j++) X1[j] = WekX[j];
                    X1[i] = WekX[i] + h;
                    //FF(Y1, X1);
                    Y1=FF(X1);
                    for (j = 1; j <= M; j++) MacJac[j, i] = (Y1[j] - WekY[j]) / h;
                }//i
                return 0;
            }
            catch
            {
                return 23;
            }
        }//GeneracjaMacierzyJacobiego

        //------------------------------------------------------------------
        //-----------------------------------------------------------------------
        public int MetodaNewtona(FunWektorWektorDelegate FF)
        {
            int i, k, blad = 0;
            double[] dX = new double[N + 1];
            double[] XX = new double[N + 1];
            double[] Y = new double[N + 1];
            double[,] MacJac = new double[N + 1, N + 1];
            double ni;
            k = 0;
            do
            {//Konstrukcja procesu iteracyjnego 
                ni = 0;
                k++;
                //Konstrukcja macierzy Jacobiego MacJac 
                blad = GeneracjaMacierzyJacobiego(FF, MacJac, X, N, epsr);
                //FF(Y, X);
                Y=FF(X);
                for (i = 1; i <= N; i++) XX[i] = Y[i];
                //Rozwiązanie układu )
                blad = MetodaGaussa.RozRowMacGaussa(MacJac, XX, dX, epsg);
                if (blad == 0)
                {
                    for (i = 1; i <= N; i++)
                    {
                        ni += Math.Abs(dX[i]);
                        X[i] -= dX[i];
                    }
                }
                else return 2;
            }
            while (!(ni < eps || k > maxit || blad != 0));
            if (blad == 0)
            {
                if (k > maxit) return 1;
                else return 0;
            }
            else return blad;
        }
        //----------------------------------------------------------------

        //Wyznaczanie rozwiązania układu równań nieliniowych metodą 
        //gradientu sprzężonego wg. Fletchera i Revvsa
        public int RozwiazMetodaGradientuSprzezonegoFR(FunWektorWektorDelegate FF)
        {
            int i, j, k, l, Blad;
            double s, s1, mi, ni, beta, beta1, beta2, xx;
            N = X.Length - 1;
            double[,] MacJac = new double[M + 1, N + 1];
            double[] dX = new double[N + 1];
            double[] dX1 = new double[N + 1];
            double[] Y = new double[M + 1];
            double[] G = new double[M + 1];
            k = 0; l = 0;
            Blad = GeneracjaMacierzyJacobiego(FF, MacJac, X, M, epsr);
            if (Blad == 0)
            {
                ni = 0;
                //FF(Y, X);
                Y=FF(X);
                for (i = 1; i <= N; i++)
                {
                    s = 0;
                    for (j = 1; j <= M; j++) s += MacJac[j, i] * Y[j];
                    dX[i] = 2 * s;      //Wyznaczanie składowych gradientu
                }//i
                for (i = 1; i <= N; i++) ni += Math.Abs(dX[i]);
                beta2 = 0;
                for (i = 1; i <= N; i++) { xx = dX[i]; beta2 += xx * xx; }
            }
            else return 11;

            while (!(ni < eps || l > maxit || Blad != 0))
            {
                ni = 0;
                k++; l++;
                for (i = 1; i <= M; i++)
                {
                    s = 0;
                    for (j = 1; j <= N; j++) s += MacJac[i, j] * dX[j];
                    G[i] = s;
                }//i
                s = 0; s1 = 0;
                for (i = 1; i <= M; i++)
                {
                    s += Y[i] * G[i]; s1 += G[i] * G[i];
                }
                mi = s / s1;
                for (i = 1; i <= N; i++) X[i] -= mi * dX[i];   //Minimalizacja w kierunku
                for (i = 1; i <= N; i++) ni += Math.Abs(dX[i]);
                //Wyznaczanie kierunku sprzeżonego
                //FF(Y, X);
                Y=FF(X);
                Blad = GeneracjaMacierzyJacobiego(FF, MacJac, X, M, epsr);
                for (i = 1; i <= N; i++)
                {
                    s = 0;
                    for (j = 1; j <= M; j++) s += MacJac[j, i] * Y[j];
                    dX1[i] = 2.0 * s;   //Wyznaczanie składowych gradientu
                }//i
                beta1 = 0;
                for (i = 1; i <= N; i++) { xx = dX1[i]; beta1 += xx * xx; }
                beta = beta1 / beta2;
                for (i = 1; i <= N; i++) dX[i] = dX1[i] + beta * dX[i];
                beta2 = beta1;
                if (k > N)
                {
                    k = 1;
                    for (i = 1; i <= N; i++) dX[i] = dX1[i];
                }
            }//Koniec  while
            if (l > maxit) return 3;
            else
            {
                if (Blad != 0) return 23;
                else return 0;
            }
        }//RozwiazMetodaGradientuSprzezonegoFR

        //--------------------------------------------------------------------------------------

        //Wyznaczanie rozwiązania układu równań nieliniowych metodą zmiennej metryki
        //wg. Broydena -Goldfarba - Fletchera - Shanno
        public int MetodaZmiennejMetrykiBGFS(FunWektorWektorDelegate FF)
        {
            int i, j, k, l, Blad;
            double s, s1, mi, ni, suma, m, m1;
            double[,] MacJac = new double[M + 1, N + 1];
            double[,] V = new double[N + 1, N + 1];
            double[] WekD = new double[N + 1];
            double[] X0 = new double[N + 1];
            double[] gradF = new double[N + 1];
            double[] gradF0 = new double[N + 1];
            double[] Y = new double[M + 1];
            double[] G = new double[M + 1];
            double[] Z1 = new double[N + 1];
            double[] S1 = new double[N + 1];
            double[] Y1 = new double[N + 1];
            for (i = 1; i <= N; i++)
                for (j = 1; j <= N; j++) if (i == j) V[i, j] = 1.0; else V[i, j] = 0;
            k = 0; l = 0;
            Blad = GeneracjaMacierzyJacobiego(FF, MacJac, X, M, 1e-10);
            if (Blad == 0)
            {
                ni = 0;
                //FF(Y, X);
                Y=FF(X);
                for (i = 1; i <= N; i++)
                {
                    s = 0;
                    for (j = 1; j <= M; j++) s += MacJac[j, i] * Y[j];
                    gradF0[i] = 2.0 * s;      //Wyznaczanie składowych gradientu
                }//i
                for (i = 1; i <= N; i++) ni += gradF0[i] * gradF0[i];
                ni = Math.Sqrt(ni);
            }
            else return 11;
            while (!(ni < eps || l > maxit || Blad != 0))
            {
                ni = 0;
                k++; l++;
                for (i = 1; i <= N; i++)
                {
                    suma = 0;
                    for (j = 1; j <= N; j++) suma += V[i, j] * gradF0[j];
                    WekD[i] = suma;
                }
                //Minimalizacja w kierunku
                for (i = 1; i <= M; i++)
                {
                    s = 0;
                    for (j = 1; j <= N; j++) s += MacJac[i, j] * WekD[j];
                    G[i] = s;
                }//i
                s = 0; s1 = 0;
                for (i = 1; i <= M; i++)
                {
                    s += Y[i] * G[i]; s1 += G[i] * G[i];
                }
                mi = s / s1;
                for (i = 1; i <= N; i++) X[i] -= mi * WekD[i];
                //Koniec minimalizacji w kierunku
                for (i=1; i<=N; i++) ni+=gradF0[i]*gradF0[i];
                //ni=Math.Sqrt(ni);
                //for (i = 1; i <= N; i++) ni += Math.Abs(gradF0[i]);
                //Korekcja macierzy odwrotnej Hesjanu V 
                //FF(Y, X);
                Y=FF(X);
                Blad = GeneracjaMacierzyJacobiego(FF, MacJac, X, M, 1e-10);
                for (i = 1; i <= N; i++)
                {
                    s = 0;
                    for (j = 1; j <= M; j++) s += MacJac[j, i] * Y[j];
                    gradF[i] = 2.0 * s;   //Wyznaczanie składowych gradientu
                    S1[i] = X[i] - X0[i];
                    Y1[i] = gradF[i] - gradF0[i];
                }//i
                for (i = 1; i <= N; i++)
                {
                    suma = 0;
                    for (j = 1; j <= N; j++) suma += V[i, j] * Y1[j];
                    Z1[i] = suma;
                };
                m = 0;
                m1 = 0;
                for (i = 1; i <= N; i++)
                {
                    m += Y1[i] * S1[i];
                    m1 += (S1[i] + Z1[i]) * Y1[i];
                }
                for (i = 1; i <= N; i++)
                    for (j = 1; j <= N; j++) V[i, j] += m1 * S1[i] * S1[j] / (m * m) - (S1[i] * Z1[j] + Z1[i] * S1[j]) / m;
                for (i = 1; i <= N; i++)
                {
                    X0[i] = X[i];
                    gradF0[i] = gradF[i];
                }
                if (k > N) //to pozostaw za V bmacierz jednostkową
                {
                    k = 1;
                    for (i = 1; i <= N; i++)
                        for (j = 1; j <= N; j++) if (i == j) V[i, j] = 1.0; else V[i, j] = 0;
                }
            }
            if (l > maxit) return 3;
            else
            {
                if (Blad != 0) return 23;
                else return 0;
            }
        }//Koniec MetodaZmiennejMetrykiBGFS
        //--------------------------------------------------------------------

    }//Koniec public class RowNieLin

}
