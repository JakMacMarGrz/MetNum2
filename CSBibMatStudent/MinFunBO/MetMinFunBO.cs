using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSBibMatStudent.MinFunBO
{
    public abstract class MetMinFunAbstHookeaJeevsa : MinFunAbstBOBG
    {

        public MetMinFunAbstHookeaJeevsa(double[] X0,object[] p)
        {
            par = p;
            X = X0;
        }
        /// <summary>
        /// Metoda Hooke'a Jeevsa
        /// </summary>
        /// <param name="FF">Funkcja minimalizowana typu FunctionDelegate</param>
        /// <param name="X">War. początkowe i wektor rozwiązania</param>
        /// <param name="par">Parametry metody: par[0] - 
        /// eps(double); par[1] - tau(double);
        /// par[2] - maxit(int)</param>
        /// <returns></returns>
        public override double  ObliczMinimumFunkcji()
        {
            double MinF = 0.0;
            bool czy = false;
            double eps = (double)par[0];
            double tau = (double)par[1];
            int maxit = (int)par[2];

            double F1, tau1;
            int k;
            int N = X.Length;

            double[] X1 = new double[N];
            double[] X0 = (double[])X.Clone();
            N = N - 1;

            double F2 = F(X);
            double F0 = F2;

            int m = 0;
            double ni = tau;

            do
            {
                m++;
                tau1 = ni;
                for (k = 1; k <= N; k++)
                { //Krok(1);
                    //X1=Kopiuj(WekX);
                    X1 = (double[])X.Clone();
                    X1[k] = X[k] + ni;
                    F1 = F(X1);
                    if (F1 < F2) { X[k] = X1[k]; F2 = F1; }
                    else
                    { //  Krok(-1);
                        //X1=Kopiuj(WekX);
                        X1 = (double[])X.Clone();
                        X1[k] = X[k] - ni;
                        F1 = F(X1);
                        if (F1 < F2) { X[k] = X1[k]; F2 = F1; };
                    }
                }//k
                if (F2 < F0)
                {
                    for (k = 1; k <= N; k++)
                    {
                        X1[k] = X0[k]; X0[k] = X[k];
                        X[k] = X0[k] + 1.4 * (X[k] - X1[k]);
                    }
                    F0 = F2; F2 = F(X);
                    ni *= 1.2; czy = true;
                }
                else
                {
                    if (czy)
                    {
                        //WekX=Kopiuj(X0);
                        X = (double[])X0.Clone();
                        F2 = F0; czy = false;
                    }
                    else { if (tau1 > eps) ni *= 0.2; }
                }
            } while (!(tau1 <= eps || m > maxit));

            MinF = F(X);

            if (m > maxit) throw new Exception("Przkroczono zadaną liczbę iteracji maxit="+maxit.ToString());

            return MinF;
        }
    }
    //------------------------------------------------------------------------
    public class MetMinFunHookeaJeevsa : MetMinFunAbstHookeaJeevsa
    {
        public Global.FunctionDelegate FF;
        public MetMinFunHookeaJeevsa(Global.FunctionDelegate F1, double[] X0, object[] p1):base(X0,p1)
        //public FunRealDelegate FF;
        //public MetMinFunHookeaJeevsa(FunRealDelegate F1, double[] X0, object[] p1)
         //: base(X0, p1)
        {
            //X = X0;
            FF = F1;
            //par = p1;
        }
        override public double F(double[] X)
        {
            return FF(X);
        }
    }
    //--------------------------------------------------------------------
    public abstract class MetMinFunAbstPowella : MinFunAbstBOBG
    {
        public MetMinFunAbstPowella(double[] X0, object[] p)
        {
            par = p;
            X = X0;
        }
        
        /// <summary>
        /// Metoda Powella wyznaczanie minimum funkcji FF(X) N zmiennych
        /// </summary>
        /// <param name="FF">FF - Referencje do badanej funkcji</param>
        /// <param name="X">X - wektor punktu startowego iteracji oraz rozwiazania</param>
        /// <param name="par[0]">Dkładność rozwiązania </param>
        /// <param name="par[1]">początkowy punkt graniczny przedziału </param>
        ///<param name="par[2]">dopuszczalna minimalna wartość współczynnika kroku np.del:=1e-20</param>
        ///<param name="par[3]">ustalona maksymalna liczba iteracji kończąca obliczenia</param>
        ///<param name="par[4]">dopuszczalna liczba obliczeń wartości funkcji</param>
        /// <returns> minimum funkcji</returns>
        public override double ObliczMinimumFunkcji()
        {
            int i, j, k, m, N;
            double wyznacznik, alfa, delta, c, s, modul, taumin = 1e-6, Qmin;
            double eps = (double)par[0];
            double taumax = (double)par[1];
            double del = (double)par[2];
            int maxit = (int)par[3];
            int maxob = (int)par[4];
            object[] par1 = { taumax, del, maxob, 1 };
            //FMaxob=Maxob; FTaumax=Taumax; FDel=Del;
            // MacB-baza początkowa jako wersory kartezjańskiego ukladu współrzędnych
            //if (taumax<=0) return 2;
            if (taumax <= 0) throw new Exception("W metodzie MinFunPow klasy zadano nieprawidłowy parametr taumax");
            //if (maxob<=5) return 4;
            if (maxob <= 5) throw new Exception("W metodzie MinFunPow klasy przekroczono maksymalną ilość obliczeń funkcji maxob");
            N = X.Length - 1;
            //matrix<Typ> MacB(N,N) ;
            //vector<Typ> Xp(N),Tau(N),WekD(N) ;
            double[,] B = new double[N + 1, N + 1];
            double[] Xp = new double[N + 1];
            double[] Tau = new double[N + 1];
            double[] D = new double[N + 1];
            m = 0;
            //MacierzJednostkowa<Typ>(MacB);
            //MacB.Unitary(N);
            for (i = 1; i <= N; i++) B[i, i] = 1.0;
            k = 0; wyznacznik = 1;
            //Xp=Kopiuj(X);
            Xp = (double[])X.Clone();
            do
            {
                k++;
                for (i = 1; i <= N; i++)
                { //Minimalizacja w kierunku D[i] metodą złotego podziału
                    for (j = 1; j <= N; j++) D[j] = B[j, i];
                    Qmin = MinimumKierZlotegoPodzialuOdcinka(ref D, ref taumin, par1);
                    Tau[i] = taumin;
                }//i
                c = 0;
                for (i = 1; i <= N; i++)
                {
                    modul = Math.Abs(X[i] - Xp[i]);
                    if (c < modul) c = modul;
                }//i
                for (i = 1; i <= N; i++) D[i] = (X[i] - Xp[i]) / c;
                Qmin = MinimumKierZlotegoPodzialuOdcinka(ref D, ref taumin, par1);
                //MinimumKierZlotegoPodzialuOdcinka(FF,WekX,WekD,Taumax,Taumin,Del,Qmin,maxob,1);
                alfa = 0;
                for (i = 1; i <= N; i++)
                {
                    modul = Math.Abs(X[i] - Xp[i]);
                    if (alfa < modul) alfa = modul;
                }
                if (alfa > eps)
                {
                    //Xp=Kopiuj(WekX);  
                    Xp = (double[])X.Clone();
                    s = 0;
                    for (i = 1; i <= N; i++)
                    {
                        modul = Math.Abs(Tau[i]);
                        if (s < modul) { s = Math.Abs(Tau[i]); m = i; };
                    }//i
                    delta = s * wyznacznik / alfa;
                    if (delta >= 0.8) // Modyfikacja bazy
                    {
                        for (i = 1; i <= N; i++) B[i, m] = D[i];
                        wyznacznik = delta;
                    }
                }
            }
            while (alfa > eps);
            if (k > maxit) throw new Exception("W metodzie MinFunPow  przekroczono maksymalną ilość iteracji maxit"+maxit.ToString());
            return F(X);
        }//MinFunPow
     }
    //----------------------------------------------------------------------------
    public class MetMinFuntPowella : MetMinFunAbstPowella
    {   //FunRealDelegate
        //public Global.FunctionDelegate FF;
        //public MetMinFuntPowella(Global.FunctionDelegate F1,
        //                         double[] X0, object[] p1)
        //    : base(X0, p1)
        public FunRealDelegate FF;
        public MetMinFuntPowella(FunRealDelegate F1, double[] X0, object[] p1)
            : base(X0, p1)
        {
            //X = X0;
            FF = F1;
            //par = p1;
        }
        override public double F(double[] X)
        {
            return FF(X);
        }
    }
    //---------------------------------------------------------------------------
    
    public abstract class MetMinFunAbstGradSprzFletcheraRevvsa1 : MinFunAbstBOZG
    {
        public MetMinFunAbstGradSprzFletcheraRevvsa1(double[] X0, object[] p)
        {
            par = p;
            X = X0;
        }
        /// <summary>
        ///Wyznaczanie minimum funkcji FF(X) metodą gradientu sprzężonego metodą Fletchera i Revvsa
        ///z minimalizacją kierunkową MiniKierEkspKontr tj. metodą ekspansji  i kontrakcji geometrycznej
        /// </summary>
        /// <param name="FF">minimalizowana funkcja wielu zmiennych </param>
        /// <param name="X">wektor punktu początkowego i rozwiązania</param>
        /// <param name="par[0]">dokładność iteracji</param>
        /// <param name="par[1]">współczynnik testu kroku 0<bet<0,5</param>
        /// <param name="par[2]">początkowy współczynnik kroku,może przyjmować wartości 
        /// z poprzedniej minimalizacji w danym kierunku</param>

        /// <param name="par[3]">dopuszczalna minimalna wartość wspołczynnika kroku np.del=1e-20</param>
        /// <param name="par[4]">współczynnik ekspansji kroku kap>1</param>
        /// <param name="par[5]">względny krok różniczkowania do obliczenia gradientu</param>
        /// <param name="par[6]">maksymalna liczba iteracji</param>
        /// <param name="par[7]">maksymalna liczba wszystkich obliczeń</param>
        /// <param name="par[8]">maksymalna liczba obliczeń korzystnych wartości funkcji</param>

        /// <returns>minimum funkcji</returns>
        public override double ObliczMinimumFunkcji()
        {
            int k, i;
            double alfa, Fq = 0, tau, beta1, beta2, beta, G1;
            int N = X.Length - 1;
            double[] gradF = new double[N + 1];
            double[] D = new double[N + 1];

            double eps = (double)par[0];
            double bet = (double)par[1];
            double tau1 = (double)par[2];
            double del = (double)par[3];
            double kap = (double)par[4];
            double ni = (double)par[5];
            int maxit = (int)par[6];
            int maxob = (int)par[7];
            int maxkoob = (int)par[8];
            tau = tau1;
            GradientFun4R(ref gradF, ni);
            alfa = 0;
            beta1 = 0;
            for (i = 1; i <= N; i++)
            {
                G1 = gradF[i]; alfa += G1 * G1; D[i] = -G1;
            }
            beta1 = alfa;
            k = 0;
            object[] par1 = new object[6];
            par1[0] = bet; ;
            par1[1] = ni;
            par1[2] = kap;
            par1[3] = del;
            par1[4] = maxob;
            par1[5] = maxkoob;
            while (!(alfa < eps || k > maxit))
            {
                k++;
                Fq = MiniKierEkspKontr(ref D, ref tau, par1);
                //Oblicz gradient i wyznacz kierunek sprzężony D
                GradientFun4R(ref gradF, ni);
                alfa = 0;
                for (i = 1; i <= N; i++)
                {
                    G1 = gradF[i]; alfa += G1 * G1;
                };
                beta2 = alfa;
                //alfa = Math.Sqrt(alfa);
                beta = beta2 / beta1;
                for (i = 1; i <= N; i++) D[i] = beta * D[i] - gradF[i];
                if (k > N)
                {
                    k = 1;
                    for (i = 1; i <= N; i++) D[i] = -gradF[i];
                }
            }
            if (k > maxit) throw new Exception("Przekroczona zadana liczba iteracji w klasie MinFunAbstGradSprzFletcheraRevvsa1 maxit = "+maxit.ToString());
            return Fq;
        }
    }//MinFunAbstGradSprzFletcheraRevvsa1
    //-------------------------------------------------------------
    public class MetMinFunGradSprzFletcheraRevvsa1 : MetMinFunAbstGradSprzFletcheraRevvsa1
    {
        public Global.FunctionDelegate FF;
        public MetMinFunGradSprzFletcheraRevvsa1(Global.FunctionDelegate F1,
                                 double[] X0, object[] p1) : base(X0, p1)
        {
            FF = F1;
        }
        override public double F(double[] X)
        {
            return FF(X);
        }
    }//MetMinFunGradSprzFletcheraRevvsa1
    //---------------------------------------------------------------------------
    
    public abstract class MetMinFunAbstBroydenaGoldfarbaFletcheraShanno1 : MinFunAbstBOZG
    {
        public MetMinFunAbstBroydenaGoldfarbaFletcheraShanno1(double[] X0, object[] p)
        {
            par = p;
            X = X0;
        }
        /// <summary>
        /// Wyznaczanie minimum funkcji F(X) metodą zmiennej metryki 
        /// wg Broydena -Goldfarba -Fletchera - Shanno  
        //// z minimalizacją kierunkową MiniKierEkspKontr tj. metodą ekspansji
        //// i kontrakcji geometrycznej
        /// </summary>
        /// <param name="par[0]">dokładność iteracji</param>
        /// <param name="par[1]">współczynnik testu kroku 0<bet<0,5</param>
        /// <param name="par[2]">początkowy współczynnik kroku,może przyjmować wartości 
        /// z poprzedniej minimalizacji w danym kierunku</param>

        /// <param name="par[3]">dopuszczalna minimalna wartość wspołczynnika kroku np.del=1e-20</param>
        /// <param name="par[4]">współczynnik ekspansji kroku kap>1</param>
        /// <param name="par[5]">względny krok różniczkowania do obliczenia gradientu</param>
        /// <param name="par[6]">maksymalna liczba iteracji</param>
        /// <param name="par[7]">maksymalna liczba wszystkich obliczeń</param>
        /// <param name="par[8]">maksymalna liczba obliczeń korzystnych wartości funkcji</param>
        /// <returns>Minimum funkcji</returns>
        public override double ObliczMinimumFunkcji()
        {
            int k, l, i, j;
            double alfa, Fq = 0, tau, suma, G1, m, m1;
            int N = X.Length - 1;
            double[] gradF0 = new double[N + 1];
            double[] gradF = new double[N + 1];
            double[] D = new double[N + 1];
            double[] X0 = new double[N + 1];
            double[] Z = new double[N + 1];
            double[] Y = new double[N + 1];
            double[] S = new double[N + 1];
            double[,] V = new double[N + 1, N + 1];

            double eps = (double)par[0];
            double bet = (double)par[1];
            double tau1 = (double)par[2];
            double del = (double)par[3];
            double kap = (double)par[4];
            double ni = (double)par[5];
            int maxit = (int)par[6];
            int maxob = (int)par[7];
            int maxkoob = (int)par[8];

            object[] par1 = new object[6];
            par1[0] = bet; ;
            par1[1] = ni;
            par1[2] = kap;
            par1[3] = del;
            par1[4] = maxob;
            par1[5] = maxkoob;

            for (i = 1; i <= N; i++)
                for (j = 1; j <= N; j++) if (i == j) V[i, j] = 1.0; else V[i, j] = 0;
            tau = tau1;
            for (i = 1; i <= N; i++) X0[i] = X[i];
            GradientFun4R(ref gradF0, ni);
            alfa = 0;
            for (i = 1; i <= N; i++)
            {
                G1 = gradF0[i]; alfa += G1 * G1;
            }
            k = 0; l = 0;
            while (!(alfa < eps || l > maxit))
            {
                k++; l++;
                for (i = 1; i <= N; i++)
                {
                    suma = 0;
                    for (j = 1; j <= N; j++) suma += V[i, j] * gradF0[j];
                    D[i] = -suma;
                }
                //alfa=sqrt(alfa);
                //Minimalizacja funkcji w punkcie X w kierunku wektora D
                //Wektor X przyjmuje nową wartość po minimalizacji funkcji
                Fq = MiniKierEkspKontr(ref D, ref tau, par1);
                //Blad=MiniKierEkspKontr(GradF0,WekX,WekD,Fbet,Ftau ,kap,  Fdel, Fq, maxob, maxkoob);
                //Oblicz gradient w wyznaczonym punkcie X
                GradientFun4R(ref gradF, ni);
                alfa = 0;
                for (i = 1; i <= N; i++)
                {
                    G1 = gradF[i]; alfa += G1 * G1;
                    S[i] = X[i] - X0[i];
                    Y[i] = gradF[i] - gradF0[i];
                }
                //alfa = Math.Sqrt(alfa);
                for (i = 1; i <= N; i++)
                {
                    suma = 0;
                    for (j = 1; j <= N; j++) suma += V[i, j] * Y[j];
                    Z[i] = suma;
                }
                m = 0;
                m1 = 0;
                for (i = 1; i <= N; i++)
                {
                    m += Y[i] * S[i];
                    m1 += (S[i] + Z[i]) * Y[i];
                }
                for (i = 1; i <= N; i++)
                    for (j = 1; j <= N; j++) V[i, j] += m1 * S[i] * S[j] / (m * m) - (S[i] * Z[j] + Z[i] * S[j]) / m;
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
            if (l > maxit) throw new Exception("Przekroczona zadana liczba iteracji w klasie MetMinFunZmiennaMetrykaBroydenaGoldfarbaFletcheraShanno1 metit= " + maxit.ToString());
            return Fq;
        }
    }//MetMinFunAbstBroydenaGoldfarbaFletcheraShanno1
    //-------------------------------------------------------------
    public class MetMinFunBroydenaGoldfarbaFletcheraShanno1 : MetMinFunAbstBroydenaGoldfarbaFletcheraShanno1
    {
        public Global.FunctionDelegate FF;
        public MetMinFunBroydenaGoldfarbaFletcheraShanno1(Global.FunctionDelegate F1,
                                 double[] X0, object[] p1) : base(X0, p1)
        {
            //X = X0;
            FF = F1;
            //par = p1;
        }
        override public double F(double[] X)
        {
            return FF(X);
        }
    }//MetMinFunGradSprzFletcheraRevvsa1
    //---------------------------------------------------------------------------   

}//koniec namespace CSBibMat.MinFun1

