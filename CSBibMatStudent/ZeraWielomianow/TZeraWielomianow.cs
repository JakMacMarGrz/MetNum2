using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSBibMatStudent.Complex;

namespace CSBibMatStudent.ZeraWielomianow
{
    public static class TZeraWielomianow
    {

        public static string[] Komunikat = 
        { /*0*/ "Brak błędu",
          /*1 8*/  "W metodzie Bairstow stopień wielomianu jest mniejszy od N",
          /*2 9*/  "Metoda Bairstow nie znajduje dzielnika w postaci trójmianu kwadratowego",
          /*3 10*/ "Przekroczono zadaną liczbę iteracji w metodzie Laguerre nie osiągnąwszy żądanej dokładności iteracji eps",
          /*4 11*/ "Niedozwolony argument w metodzie DIV1"
        };

        public static void PiszKomunikat(int k)
        {
            MessageBox.Show(Komunikat[k]);
        }
   
        /// <summary>
        /// Div2 wykonuje dzielenie wielomianu
        /// FW[0]*X^N+FW[1]*X^(N-1)+..+FW[N] przez trójmian
        /// kwadratowy X^2-FP*X+FQ i pozostawia obliczone współczynniki
        /// ilorazu FW[0],FW[1],...,FW[N-2] rownież w tablicy  FW[i]
        /// </summary>
        /// <param name="FW">Tablica współczyników wielomianu</param>
        /// <param name="FP">Pierwszy współczynik trójmianu X^2-FP*X+FQ</param>
        /// <param name="FQ">Drugi współczynik trójmianu X^2-FP*X+FQ</param>
        /// <param name="N">Obniżony o dwa stopień wielomianu
        ///                 pozostawia także pod zmienna N</param>
        public static void Div2(double[] FW, double FP, double FQ, ref int N)
        {
           FW[1]+=FP*FW[0];
           for (int k=2;k<=N-2; k++)
                      FW[k]+=FP*FW[k-1]-FQ*FW[k-2];
           N-=2;
        }// Div2
        //-------------------------------------------------------------------
                                                             
        /// <summary>
        /// Poprawia czynnik kwadratowy X^2-FP*x+FQ będący dzielnikiem
        ///wielomianu  FW[0]*X^N+FW[1]*X^(N-1)+...+FW[N] pozostawiając
        /// poprawione wartości jako zmienne FP,FQ
        /// </summary>
        /// <param name="FW">Tablica współczyników wielomianu</param>
        /// <param name="eps">Dokładność iteracji</param>
        /// <param name="mega"></param>
        /// <param name="FP">Zwracany pierwszy współczynik trójmianu X^2-FP*X+FQ</param>
        /// <param name="FQ">Zwracany drugi współczynik trójmianu X^2-FP*X+FQ</param>
        /// <param name="N">Stopień wielomianu</param>
        /// <returns>Wartość funkcji nr błędu; wartość 0 - brak błędu</returns>
        public static int Bairstow(double[] FW, double eps,double mega, ref double FP, ref double FQ, int N)
        {
          double R,R1,R2,R3,R4,P1,Q1,S,LL,MM,XX,YY ;
          int j,Blad=0 ;
          double[] B=new double[N+1]; 
          double[] C=new double[N+1]; 
          if (FW[0]==0) return 1;
          if (FP==0 && FQ==0) FP=0.1;
          do
          {
               B[0]=FW[0];
               B[1]=FW[1]+FP*B[0];
               for (j=2; j<=N; j++) B[j]=FW[j]+FP*B[j-1]-FQ*B[j-2];
               C[0]=B[0];
               C[1]=B[1]+FP*C[0];
               for (j=2; j<=N-2; j++) C[j]=B[j]+FP*C[j-1]-FQ*C[j-2];
               R1=C[N-2]; R2=C[N-3]; R3=B[N-1]; R4=B[N];
               P1=FP; Q1=FQ;    R=FP*R1-FQ*R2;   S=R1*R1-R*R2;
               FP-=(R3*R1-R4*R2)/S;     FQ-=(R3*R-R1*R4)/S;
               XX=FP-P1;  YY=FQ-Q1;
               LL=Math.Sqrt(XX*XX+YY*YY);    MM=Math.Sqrt(P1*P1+Q1*Q1);
               R=LL/MM;
               if (R>mega) Blad=2;
           } while (!(R<eps || Blad!=0));
           return Blad;
        }//Bairstow
        //------------------------------------------------------------------------
  
        /// <summary>
        ///RKW rozwiązuje równanie kwadratowe X^2-FP*X+FQ = 0
        ///pozostawiając jako pierwiastki zmienne X,Y (rzeczywiste) oraz
        ///zmienną boolowską Re ,przy czym X,Y są rzeczywiste jesli Re=True
        ///lub X jest częścią rzeczywistą,a Y-urojoną zespolonych
        ///pierwiastków jesli Re=False
        /// </summary>
        /// <param name="FP"></param>
        /// <param name="FQ"></param>
        /// <param name="X">Pierwiastek X równanie kwadratowe X^2-FP*X+FQ</param>
        /// <param name="Y">Pierwiastek Y równanie kwadratowe X^2-FP*X+FQ</param>
        /// <param name="Re">true jeżeli X i Y rzeczywiste
        ///                  false jeżeli pierwiatek jest zespolony</param>
        private static void RKW(double FP, double FQ, ref double X, ref double Y, ref bool Re)
        {
           double D;
           X=FP/2; D=X*X-FQ; Re=D>=0;
           D=Math.Abs(D);  D=Math.Sqrt(D);
           if (Re) { Y=X+D; X-=D;}
           else Y=D;
        }// RKW
        
        /// <summary>
        /// Metoda dokonuje podstawienia kolejnych pierwiastków
        ///wielomianu pod wektor zespolony FZ
        ///FZ.B[k] -k-ty pierwiastek wielomianu
        /// </summary>
        /// <param name="FZ">Wektor zespolony pierwiastków</param>
        /// <param name="FP">Pierwszy współczynik trójmianu X^2-FP*X+FQ</param>
        /// <param name="FQ">Drugi współczynik trójmianu X^2-FP*X+FQ</param>
        /// <param name="k">Numer k-tego pierwiastka wielomianu</param>
        private static void PRW2(Complex.Complex[] FZ, double FP, double FQ, ref int k)
        {
          double X=0,Y=0;
          bool Re=false;
          RKW(FP,FQ,ref X,ref Y,ref Re) ;
          if (Re)
            {
              k++; FZ[k] = new Complex.Complex(X, 0);
              k++; FZ[k] = new Complex.Complex(Y, 0);
            }
          else
            {
              k++; FZ[k] = new Complex.Complex(X, Y);
              k++; FZ[k] = new Complex.Complex(X, -Y);
            }
        }// PRW2
        //----------------------------------------------------------------------------
       
        /// <summary>
        /// Przestawianie pierwiastków wg wzrastajacych modułów
        /// </summary>
        /// <param name="FZ">Wektor zespolony zer wielomianu</param>
        /// <param name="N">Stopień wielomianu</param>
        public static void PrzestawPierwiastki(Complex.Complex[] FZ, int N)
        {
           int i,j,k;  
           double X1,X;
           Complex.Complex Z;
           for (i=1; i<=N; i++)
             {
               X=ComplexMath.Abs(FZ[i]); k=i;
               for (j=i+1; j<=N; j++)
                 {
                   X1 = ComplexMath.Abs(FZ[j]);
                   if (X1<X) { X=X1; k=j ;}
                 }
               if (k != i)
               {
                 Z = FZ[i]; FZ[i] = FZ[k]; FZ[k] = Z;
               }
                   //swap(FZ[i],FZ[k]);
             }
        }//PrzestawPierwiastki
        //-----------------------------------------------------------------------
        /// <summary>
        /// Metoda Bairstowa obliczania pierwiastków wielomianu (4.57) stopnia N
        /// </summary>
        /// <param name="FW">Wektor wpółczynników wielomianu inicjowany na zewnątrz klasy</param>
        /// <param name="ZZ">Wektor zespolony zer wielomianu inicjowany na zewnątrz klasy</param>
        /// <param name="P">Początkowe przybliżenie do dzielenia wielomianu przez
        ///      trójmian kwadratowy X^2-FP*X+FQ np. P:=1</param>
        /// <param name="Q">Początkowe przybliżenie do dzielenia wielomianu przez
        ///      trójmian kwadratowy X^2-FP*X+FQ np. Q:=1</param>
        /// <param name="eps">dokładność bezwzględna iteracji  np. eps=1E-16</param>
        /// <param name="mega">próg poszukiwania dzielnika np.mega:=1E+3</param>
        /// <param name="maxit">ustalona maksymalna liczba iteracji kończąca obliczenia</param>
        /// <returns>Funkcja zwraca numer błędu; wartość 0 - brak błędu</returns>
        public static int ZeraWielBairstow(double[] FW, Complex.Complex[] ZZ,
                         double P,double Q,double eps, double mega, int maxit)
        {
           double FP,FQ;
           int k,N,N1,Blad;
           Blad=0;
           N=FW.Length-1;   N1=N;
           //InicjacjaWektora(ZZ,N);
           FP=P; FQ=Q;
           if (FW[0]==0) return 1;
           k=0;
           while (N>2)
              {
                Blad=Bairstow(FW,eps,mega,ref FP,ref FQ,N) ;
                if (Blad==0)
                  { 
                    PRW2(ZZ,FP,FQ,ref k) ;
                    Div2(FW,FP,FQ,ref N);
                  }
              }
           if (N==2)
              {
                FP=-FW[1]/FW[0]; FQ=FW[2]/FW[0];
                PRW2(ZZ,FP,FQ,ref k) ;
              }
           else
              {
                  k++; ZZ[k] = new Complex.Complex(-FW[1] / FW[0], 0);
              }
           PrzestawPierwiastki(ZZ,N1) ;
           return Blad;
        }// ZeraWielBairstow

        /// <summary>
        /// Div1 wykonuje dzielenie wielomianu
        /// FW.B[0]*X^N+FW.B[1]*X^(N-1)+..+FW.B[N] przez czynnik liniowy X-X0
        /// dla rzeczywistego X0 wg sklejanego algorytmu Hornera
        ///(4.49)(4.50)(4.51)  i pozostawia obliczone współczynniki ilorazu
        /// rownież w tablicy FW.B[i]
        /// </summary>
        /// <param name="FW">Wektor współczynników wielomianu</param>
        /// <param name="N">Obniżony o jeden stopień
        /// wielomianu pozostawia także pod zmienną N</param>
        /// <param name="X0">Występuje w czynniku liniowym X-X0</param>
        /// <returns>Funkcja zwraca numer błędu; wartość 0 - brak błędu</returns>
        private static int Div1(double[] FW, ref int N, double X0)
        {
            int i, k, Blad;
            double X, DX, M, L;
            double[] QP = new double[N + 1];
            double[] QB = new double[N + 1];
            Blad = 0;
            if (X0 == 0) Blad = 4;
            else
            {
                QP[0] = FW[0];
                for (i = 1; i <= N; i++) QP[i] = FW[i] + X0 * QP[i - 1]; //wzór (4.49)
                QB[N] = 0;
                for (i = N; i >= 1; i--) QB[i - 1] = (QB[i] - FW[i]) / X0;  //wzór (4.50)
                X = 1E+30; k = 0;
                for (i = 1; i <= N - 1; i++)
                {
                    L = Math.Abs(QP[i] - QB[i]);
                    M = Math.Abs(FW[i]) + Math.Abs(QP[i]);
                    if (M > 0)
                    {
                        DX = L / M;  // wzór (4.52)
                        if (DX < X)
                        { X = DX; k = i; }
                    }
                }
                for (i = 0; i <= k; i++) FW[i] = QP[i];
                for (i = k + 1; i <= N; i++) FW[i] = QB[i];
                N--;  // Dec(N)
            }
            return Blad;
        }
 
        /// <summary>
        /// Wyznacza pierwiastek Z wielomianu stopnia N o współczynnikach
        /// zapisanych w wektorze FW[i]
        /// </summary>
        /// <param name="N">Stopień wielomianu </param>
        /// <param name="FW">Wektor współczynników wielomianu</param>
        /// <param name="Z">Pierwiastek wielomianu</param>
        /// <param name="eps"> dokładność bezwzględna iteracji  np. eps=1E-16</param>
        /// <param name="maxit">ustalona maksymalna liczba iteracji kończąca obliczenia</param>
        /// <returns>return - nr błędu; 0 - brak błędu.</returns>
        private static int Laguerre(int N, double[] FW, ref Complex.Complex Z, double eps, int maxit)
        {
           Complex.Complex B, C, D, S, H, M1, M2;
           double MM1,MM2,Alfa,Nx,LL,MM ;
           int N1,i,k,Blad;
           Blad=0;
           if (FW[0]==0) Blad=1;
            else
             {
                if (FW[N] == 0) Z =new Complex.Complex(0, 0);
                else
                 {
                   N1=N-1;   k=0;
                   do
                     { //Wyznaczanie wartości wielomianu i ich pochodnych pierwszego
                       // i drugiego rodzaju wg algorytmu Hornera (4.76)
                       B =new Complex.Complex(FW[0], 0);
                       C=B; D=B;
                       for (i=1; i<=N; i++)
                        {
                          if (i<=N-2)
                            {
                               S=B*Z;   S+=FW[i];    B=S;
                               C*=Z;  C+=B;
                               D*=Z;  D+=C;
                            }
                          else
                            {
                              if (i<=N-1)
                                {
                                  S=B*Z;  S+=FW[i];  B=S;
                                  C*=Z;   C+=B;
                                }
                              else
                                { S=B*Z;   S+=FW[i];  B=S; }
                             }
                         }
                        D*=2.0;
                        //Konstukcja wzoru rekurencyjnego (4.74)
                        Nx=N1;
                        H=C*C;  H*=Nx;
                        Nx=N;
                        S=B*D  ;S*=Nx ;
                        // H - wg wzoru (4.75)
                        Nx=N1;
                        H-=S;  H*=Nx;
                        H=ComplexMath.Sqrt(H);
                        M1=B/(C+H) ;  M2=B/(C-H) ;
                        MM1 = ComplexMath.Abs(M1); MM2 = ComplexMath.Abs(M2); Nx = N;
                        if (MM1>MM2) H=M2*Nx;
                        else H=M1*Nx;
                        Z-=H;   k++;
                        LL = ComplexMath.Abs(H); MM = ComplexMath.Abs(Z);
                        Alfa=LL/MM;
                     }
                   while (!(Alfa<eps || k>maxit) );
                   if (k>maxit) Blad=3;
                 }
             }
           return Blad;
        }//Laguerre
        //----------------------------------------------------------------------------
        
        /// <summary>
        /// Metoda Laguerre'a obliczania pierwiastków wielomianu(4.73) stopnia N
        /// </summary>
        /// <param name="FW">Wektor wpółczynników wielomianu inicjowany na zewnątrz klasy</param>
        /// <param name="FZ">Wektor zespolony zer wielomianu inicjowany na zewnątrz klasy</param>
        /// <param name="eps">Dokładność bezwzględna iteracji  np. eps=1E-16</param>
        /// <param name="maxit">Ustalona maksymalna liczba iteracji kończąca obliczenia</param>
        /// <returns></returns>
        public static int ZeraWielLaguerre(double[] FW, Complex.Complex[] FZ,
                             double eps, int maxit)
        {
           int M,Blad,N1;
           double X1,X2,delta,Y1,Y2,FP,FQ;
           Complex.Complex ZZ;
           M=FW.Length-1;   N1=M;
           //InicjacjaWektora(FZ,M);
           Blad=0;
           if (FW[0]==0) Blad=1;
            else
             {
               while (M>2)
               {
                     ZZ =new Complex.Complex(0, 0);
                  if (FW[M]==0)
                     {
                         FZ[M] = new Complex.Complex(0, 0); M--;
                     }
                    else
                      {
                        Blad=Laguerre(M,FW, ref ZZ,eps, maxit);
                        if (Blad==0)
                          {
                            Y1=ZZ.Re; Y2=ZZ.Im;
                            if (Math.Abs(Y2) <= 1e-38)
                              {
                                FZ[M] = new Complex.Complex(Y1, 0);
                                Blad=Div1(FW,ref M,Y1) ;
                                if (Blad!=0) return Blad;
                              }
                            else
                              {
                                FZ[M]=ZZ;
                                FZ[M-1]=ZZ.Conjugate;
                                FP=2*Y1;
                                FQ = Y1*Y1+Y2*Y2;
                                Div2(FW,FP,FQ,ref M);
                              }
                         }
                        else return Blad;
                       }
                 }
                if (M==2)
                  {
                    if (FW[M]==0)
                    { FZ[M] = new Complex.Complex(0, 0); M -= 1; }
                    else
                       {
                         delta=FW[1]*FW[1]-4*FW[0]*FW[2];
                         if (delta<0)
                           {
                             X1=-0.5*FW[1]/FW[0];
                             X2 = 0.5 * Math.Sqrt(-delta) / FW[0];
                             FZ[M] = new Complex.Complex(X1, X2);
                             FZ[M - 1] = new Complex.Complex(X1, -X2);
                           }
                         else
                           {  X1=-0.5*FW[1]/FW[0];
                              X2=0.5*Math.Sqrt(delta)/FW[0];
                              FZ[M] = new Complex.Complex(X1 + X2, 0);
                              FZ[M - 1] = new Complex.Complex(X1 - X2, 0);
                           }
                       }
                    }
                  else
                    {
                        FZ[1] = new Complex.Complex(-FW[1] / FW[0], 0);
                    }
                //Przestawianie pierwiastków wg wzrastających modułów
                PrzestawPierwiastki(FZ,N1);
           }
          return Blad;
        }//ZeraWielLaguerre};
        //------------------------------------------------------------------------------


     }
}
