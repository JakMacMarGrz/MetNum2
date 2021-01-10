using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSBibMatStudent.MinFunBO
{
    public interface IMinFunF
    {
        double F(double[] X);
    }
    //----------------------------------------------------
    public interface IMinFunH
    {
        double[] H(double[] X);
    }
    //----------------------------------------------------
    public interface IMinFunG
    {
        double[] G(double[] X);
    }
    //----------------------------------------------------
    public abstract class MinFunAbstBO : IMinFunF
    {
        public double[] X;
        protected object[] par;
        abstract public double F(double[] X);
        abstract public double ObliczMinimumFunkcji();
    }
    //------------------------------------------------------
    public abstract class MinFunAbstBOBG : MinFunAbstBO
    {

        private double MIN3(double q1, double q2, double q3)
        {
            if (q1 <= q2 && q1 <= q3) return q1;
            else if (q2 <= q1 && q2 <= q3) return q2;
            else return q3;
        }//MIN3

        /// <summary>
        /// Poszukiwanie minimum funkcji FF w punkcie X w kierunku wektora D
        /// metodą złotego podziału odcinka
        /// </summary>
        /// <param name="FF">Referencje do badanej funkcji</param>
        /// <param name="X">Wektor punktu startowego iteracji oraz rozwiązania</param>
        /// <param name="D">Wektor w kierunku którego poszukuje się minimum</param>
        /// <param name="?">taumin - współczynnik kroku odpowiadający minimum funkcji w kierunku wektora D</param>
        /// <param name="?">qmin minimu </param>
        /// <param name="par">par[0]=taumax -początkowy punkt graniczny przedziału</param>
        /// <param name="par">par[1]=del -dopuszczalna minimalna wartość wspołczynnika kroku np.del:=1e-20</param>
        /// <param name="par">par[2]=maxob -maksymalna ilość obliczeń fukcji FF(X)</param>
        /// <param name="par">par[3]=wariant - 0-minimum poszukiwane jest w przedziale otwartym (-taumax,taumax)</param>
        /// <param name="par">par[3]=wariant - 1-minimum poszukiwane jest w przedziale domkniętym [-taumax,taumax]</param>
        /// <returns>Minimum funkcji</returns>
        public double MinimumKierZlotegoPodzialuOdcinka(ref double[] D,
                     ref double taumin, object[] par)
        //vector<Typ> &FWekD, Typ FTaumax,Typ &FTaumin,Typ FDel,Typ &FQmin,
        //int FMaxob,  int Wariant=0)
        {
            int j, l, i, N;
            double q1, q2, q3, q4, q, tau0, tau1, tau2, tau3, tau4, deltau, alfa, qmin = 0;
            bool Tak1, Koniec;
            double taumax = (double)par[0];
            double del = (double)par[1];
            int maxob = (int)par[2];
            int wariant = (int)par[3];
            // Początek MinimumKierZlotegoPodzialuOdcinka
            q1 = 0; q2 = 0; q3 = 0; q4 = 0;
            alfa = (Math.Sqrt(5.0) - 1) / 2.0;
            deltau = 2 * (1 - alfa) * taumax;
            tau1 = -taumax; tau4 = taumax;
            tau2 = tau1 + deltau; tau3 = tau4 - deltau;
            tau0 = tau1;
            j = 0;
            Koniec = false;
            N = X.Length;
            double[] Y = new double[N];
            N = N - 1;
            do
            {
                for (i = 1; i <= N; i++) Y[i] = X[i] + tau0 * D[i];
                q = F(Y);
                //q=QQ(tau0);
                Tak1 = false;
                if (tau0 != tau1)
                {
                    Tak1 = true;
                    if (tau0 == tau2) { q2 = q; }
                    else
                    {
                        if (tau0 == tau3) { q3 = q; }
                        else { q4 = q; }
                    }
                }
                else
                {
                    q1 = q;
                    if (j < 4)
                    {
                        qmin = q1; taumin = tau0; tau0 = tau2;
                        j = 1; Koniec = false;
                    }
                }
                if (j >= 4 || Tak1)
                {
                    if (q < qmin)
                    {
                        qmin = q;
                        taumin = tau0;
                    }
                    j++;//Inc(j);
                    if (j < 4)
                    {
                        if (tau0 == tau2) tau0 = tau3; else tau0 = tau4;
                        Koniec = false;
                    }
                    else
                    {
                        if (j == maxob)
                        {
                            taumin = 0; Koniec = true;
                        }
                        else
                        {
                            if (Math.Abs(deltau) <= del) Koniec = true;
                            else
                            {
                                if (wariant == 0)
                                {
                                    if (q1 < MIN3(q2, q3, q4))
                                    {
                                        tau3 = tau2; q3 = q2;
                                        tau2 = tau1; q2 = q1;
                                        deltau = (1 + alfa) * deltau;
                                        tau1 = tau1 - deltau;
                                        tau0 = tau1;
                                        Koniec = false;
                                    }
                                    else
                                    {
                                        if (q4 < MIN3(q1, q2, q3))
                                        {
                                            tau2 = tau3; q2 = q3;
                                            tau3 = tau4; q3 = q4;
                                            deltau = (1 + alfa) * deltau;
                                            tau4 = tau4 + deltau;
                                            tau0 = tau4;
                                            Koniec = false;
                                        }
                                        else
                                        {
                                            if (q2 <= q3)
                                            {
                                                tau4 = tau3; q4 = q3;
                                                tau3 = tau2; q3 = q2;
                                                deltau = alfa * del;
                                                tau2 = tau1 + del;
                                                tau0 = tau2;
                                                Koniec = false;
                                            }
                                            else
                                            {
                                                tau1 = tau2; q1 = q2;
                                                tau2 = tau3; q2 = q3;
                                                deltau = alfa * del;
                                                tau3 = tau4 - del;
                                                tau0 = tau3;
                                                Koniec = false;
                                            }
                                        }
                                    }
                                } //(Wariant==0)
                                else
                                {
                                    if (q2 <= q3)
                                    {
                                        tau4 = tau3; q4 = q3;
                                        tau3 = tau2; q3 = q2;
                                        deltau = alfa * del;
                                        tau2 = tau1 + del;
                                        tau0 = tau2;
                                        Koniec = false;
                                    }
                                    else
                                    {
                                        tau1 = tau2; q1 = q2;
                                        tau2 = tau3; q2 = q3;
                                        deltau = alfa * del;
                                        tau3 = tau4 - del;
                                        tau0 = tau3;
                                        Koniec = false;
                                    }
                                } //!(wariant==0)
                            }// !(Abs(deltau)<=del)
                        }// !(j==maxob)
                    }// !(j<4)
                }//(j>=4 || Tak1)
            }
            while (!(Koniec));
            for (l = 1; l <= N; l++) X[l] += taumin * D[l];
            return qmin;
        }//MinimumKierZlotegoPodzialuOdcinka
        //-------------------------------------------------------------------

    }

    public abstract class MinFunAbstBOZG : MinFunAbstBO
    {
 
        /// <summary>
        /// Norma maksimum 
        /// </summary>
        /// <param name="X">wektora X</param>
        /// <returns>Zwraca maksimum modułu składowych wektora</returns>
        private double NormaWek(ref double[] X)
        {
            double r, s;
            int N = X.Length - 1;
            s = 0;
            for (int i = 1; i <= N; i++)
            {
                r = Math.Abs(X[i]);
                if (r > s) s = r;
            }
            return s;
        }//NormaWek

        /// <summary>
        /// Obliczanie gradientu funkcji FF(X) w punkcie X metodą rzędu drugiego
        /// </summary>
        /// <param name="FF">Delegat do funkcji rzeczywistej wielu zmiennych</param>
        /// <param name="X">Punkt obliczenia gradientu</param>
        /// <param name="gradF">Wektor gradientu</param>
        /// <param name="a">względny krok różniczkowania  h=a*NormaWek(X)</param>
        public virtual void GradientFun2R(ref double[] gradF, double a)
        {
            int i, N;
            double FX1, FX2, h, g;
            N = X.Length - 1;
            //vector<Typ> X1(N);
            double[] X1 = new double[N + 1];
            X1 = (double[])X.Clone();
            g = NormaWek(ref X);
            if (g > a) h = a * g; else h = a;
            for (i = 1; i <= N; i++)
            {
                //X1=Kopiuj(WekX);
                X1 = X;
                X1[i] = X[i] + h; FX1 = F(X1);
                X1[i] = X[i] - h; FX2 = F(X1);
                gradF[i] = 0.5 * (FX1 - FX2) / h;
            }
        }//GradientFun

        /// <summary>
        /// Obliczanie gradientu funkcji FF(X) w punkcie X metodą rzędu czwartego
        /// </summary>
        /// <param name="FF">Delegat do funkcji rzeczywistej wielu zmiennych</param>
        /// <param name="X">Punkt obliczenia gradientu</param>
        /// <param name="gradF">Wektor gradientu</param>
        /// <param name="a">względny krok różniczkowania  h=a*NormaWek(X)</param>
        public virtual void GradientFun4R(ref double[] gradF, double a)
        {
            int i, N;
            double FX1, FX2, FX3, FX4, h, h2, g;
            N = X.Length - 1;
            //vector<Typ> X1(N);
            double[] X1 = new double[N + 1];
            X1 = (double[])X.Clone();
            g = NormaWek(ref X);
            if (g > a) h = a * g; else h = a;
            for (i = 1; i <= N; i++)
            {
                //X1=Kopiuj(WekX);
                X1 = X;
                X1[i] = X[i] + h; FX1 = F(X1);
                X1[i] = X[i] - h; FX2 = F(X1);
                h2 = 2.0 * h;
                X1[i] = X[i] + h2; FX3 = F(X1);
                X1[i] = X[i] - h2; FX4 = F(X1);
                gradF[i] = (8.0 * FX1 - 8.0 * FX2 - FX3 + FX4) / (12.0 * h);
            }
        }//GradientFun

        /// <summary>
        /// Poszukiwanie minimum funkcji FF(X) w kierunku wektora D
        /// metodą ekspansji i kontrakcji geometrycznej
        /// </summary>
        /// <param name="FF">Minimalizowana funkcja wielu zmiennych</param>
        /// <param name="X">Punkt początkowy iteracji oraz rozwiązania </param>
        /// <param name="D">Wektor określający kierunek poszukiwania minimum</param>
        /// <param name="tau">Początkowy współczynnik kroku,może przyjmować wartości z poprzedniej
        ///                   minimalizacji w danym kierunku</param>
        /// <param name="par[0]">współczynnik testu kroku 0<bet<0,5</param>
        /// <param name="par[1]">względny krok różniczkowania  h=par[1]*Norma(X)</param>
        /// <param name="par[2]">współczynnik ekspansji kroku kap>1</param>
        /// <param name="par[3]">dopuszczalna minimalna wartość wspołczynnika kroku np.Fdel:=1e-20</param>
        /// <param name="par[4]">maksymalna liczba wszystkich obliczeń</param>
        /// <param name="par[5]">maksymalna liczba obliczeń korzystnych wartosci funkcji</param>
        /// <returns>Minimum funkcji</returns>
        public double MiniKierEkspKontr(ref double[] D,
                                       ref double tau, object[] par)
        {
            int i, j, k, l, N;
            double Fq, q0, q1, q2, dq0, tau1;
            bool Tak, Tak1, Tak2;
            double bet = (double)par[0];
            double ni = (double)par[1];
            double kap = (double)par[2];
            double del = (double)par[3];
            int maxob = (int)par[4];
            int maxkoob = (int)par[5];
            if (bet <= 0 || bet > 0.5) throw new Exception("Współczynnik testu kroku w metodzie minimalizacji kierumkowej nie jest w przedziale 0<bet<0,5");
            if (kap <= 1) throw new Exception("Współczynnik ekspansji kroku w metodzie minimalizacji kierumkowej nie jest kap>1");
            N = X.Length - 1;
            q0 = F(X);
            //q0 - wartość funkcji FF w punkcie początkowym X
            double[] gradF = new double[N + 1];
            GradientFun4R(ref gradF, ni);
            //dq0 - pochodna kierunkowa czyli różniczka Gateaux w punkcie X
            dq0 = 0;
            for (j = 1; j <= N; j++) dq0 += gradF[j] * D[j];
            q1 = q0; j = 0;
            k = 0; tau1 = 0;
            Tak1 = false; Tak2 = false; Tak = false;
            double[] Y = new double[N + 1];
            do
            {
                for (i = 1; i <= N; i++) Y[i] = X[i] + tau * D[i];
                Fq = F(Y);
                if (Fq < q1) { q1 = Fq; tau1 = tau; }
                if (Fq <= q0) j++; else k++;
                if ((k + j) >= maxob)
                {
                    if (j == 0) throw new Exception("Algorytm minimalizacji kierunkowej wykonał zbyt dużo niekorzystnych obliczeń funkcji takich, ze Q(tau)>q0. Być może niekorzystny dobór tau lub za mała liczba maxob niekorzystnych obliczeń funkcji.");
                    else { Tak = true; }
                }
                else
                {
                    if (j < maxkoob)
                    {
                        q2 = q0 + bet * dq0 * tau;
                        if (Fq < q2)
                        {
                            if (Tak1) { Tak = true; }
                            else
                            { Tak2 = true; tau *= kap; }
                        }
                        else
                        {
                            if (Tak2) Tak = true;
                            else
                            { Tak1 = true; tau /= kap; }
                        }
                    }
                }
            }
            while (!(Tak));
            Fq = q1; tau = tau1;
            for (l = 1; l <= N; l++) X[l] = X[l] + tau * D[l];
            if (Fq > q0) throw new Exception("Algorytm minimalizacji kierunkowej MiniKierEkspKontr klasy MetodyMinFunBO nie znalazł wartości funkcji Q(tau) mniejszej niż początkowa q0 - zwiększ dokładność obliczeń.");
            else if (Math.Abs(tau) <= del) throw new Exception("Algorytm minimalizacji kierunkowej MiniKier1 klasy MetodyMinFunBO znalazł zbyt mały wspołczynnik kroku tau grożący błędami zaokrągleń ");
            return Fq;
        }//MiniKierEkspKontr
        //-------------------------------------------------------------------------------------------------------------------------
 
    }
   
}
