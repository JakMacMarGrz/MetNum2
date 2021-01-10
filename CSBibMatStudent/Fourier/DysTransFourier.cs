using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSBibMatStudent.Complex;

namespace CSBibMatStudent.Fourier
{
    enum TypDTF
    {  
      CooleyTukey=1,
      SandeTukey=2,
      Horner=3
    }
    public class DysTransFourier
    {
        /// <summary>
        ///Odwrotne uporządkowanie bitów wektora próbek X[j]
        /// (j=0,1,2,...,N-1 np.  dla p=4  probka  nr. k=5=(0101)
        /// zostaje zamieniona z l=(1010)=10
        /// </summary>
        /// <param name="p">Potega dla N=2^p</param>
        /// <param name="X">Wektor próbek zespolonych</param>
        /// <returns>Zwraca numer błędu</returns>
        public static int  InversBit(int p, Complex.Complex[]  X)
        {
           int i,j,k,N,NP2;
           Complex.Complex T;
           N=1<<p;     //dwa do potęgi p
           NP2=N>>1;  //dzielenie N przez dwa
           if (X.Length-1==N && p>3)
             {
               i=0;
               for (j=0; j<=N-2; j++)
               {
                    if (j<i)
                      { T=X[i]; X[i]=X[j]; X[j]=T ; };
                    k=NP2;
                    while (k<=i)
                      { i-=k;  k=k>>1 ; }        //k=k>>1;  //dzielenie przez dwa
                    i+=k;
               }
               return 0;
             } else return 1;
        }// InversBit
        //-----------------------------------------------------------------------------

        /// <summary>
        /// Wyznacznie wektora ExpW o elementach zespolonych ExpW[k]=Exp(znak*j*2*PI*k/N);
        /// </summary>
        /// <param name="znak">znak =-1 -transformacja prosta
        ///                    znak =+1 -transformacja odwrotna</param>
        /// <param name="N">liczba probek</param>
        /// <param name="ExpW">ExpW referencja do wektora o elementach zespolonych</param>
        /// <returns>funkcja zwraca 0 gdy błąd nie wystapil, w przeciwnym
        /// razie zwraca numer błędu</returns>
        public static int WekExp(int znak ,int N , Complex.Complex[] ExpW)
        {
          int k;
          double fi,s,c ;
          Complex.Complex W, W1;
          W1 =new Complex.Complex(1.0, 0);
          if (ExpW.Length-1==N)
             {
               fi=2*Math.PI/N;  s=Math.Sin(fi); c=Math.Cos(fi);
               if (znak >= 0) W = new Complex.Complex(c, s);
               else W = new Complex.Complex(c, -s); 
               ExpW[0]=W1;
               for (k=1; k<=N-1; k++)
                     ExpW[k]=ExpW[k-1]*W;
               return 0;
            } else return 2;
        }// WekExpW
        //---------------------------------------------------------------------------

        /// <summary>
        /// Metoda wg algorytmu FFT Cooleya-Tukeya
        /// </summary>
        /// <param name="znak">znak =-1 -transformacja prosta
        ///                    znak =+1 -transformacja odwrotna</param>
        /// <param name="p">wykładnik potęgi przy obliczaniu N dwa do potęgi p</param>
        /// <param name="X">wektor zespolony próbek wejściowych o elementach X[j]</param>
        /// <returns>funkcja zwraca 0 gdy błąd nie wystapil, w przeciwnym
        /// razie zwraca numer błędu</returns>
        public static int FFTCooleyTukey(int znak ,int p,Complex.Complex[] X)
        {
          Complex.Complex T ;  
          Complex.Complex[] ExpW;
          int i,l,k,m,r,s,N,MP=0,MP1,Blad=0 ;
          N=1<<p;     //dwa do potęgi p
          //InicjacjaWektora(ExpW,N);
          ExpW = new Complex.Complex[N + 1];
          if (p<3 || p>15) Blad=3 ;
             else if (X.Length-1!=N) Blad=5 ;
          if (Blad==0)
            {
              Blad=InversBit(p,X);
              if (Blad==0)
                {
                  WekExp(znak,N,ExpW);
                  r=N;
                  for (m=1; m<=p; m++)
                  {
                       if (m>1) MP=MP<<1; else MP=2;        //MP=MP<<1;  mnożenie przez dwa
                       MP1=MP>>1;    r= r>>1;               //r=r>>1; dzielenie przez dwa
                       for (k=0; k<=MP1-1; k++)
                         {
                           i=k; s=r*k;
                           while (i<N)
                           {
                             l=i+MP1;
                             T=X[l]*ExpW[s];
                             X[l]=X[i]-T;   X[i]+=T;
                             i+=MP;
                           }
                         }
                   }
                   return 0;
                 } else return Blad;
            }  else return Blad;
        }//FFTCooleyTukey
        //------------------------------------------------------------------------------
  
        /// <summary>
        /// Metoda wg algorytmu FFT Sandego-Tukeya
        /// </summary>
        /// <param name="znak">znak =-1 -transformacja prosta
        ///                    znak =+1 -transformacja odwrotna</param>
        /// <param name="p">wykładnik potęgi przy obliczaniu N dwa do potęgi p</param>
        /// <param name="X">wektor zespolony próbek wejściowych o elementach X[j]</param>
        /// <returns>funkcja zwraca 0 gdy błąd nie wystapil, w przeciwnym
        /// razie zwraca numer błędu</returns>
        public static int FFTSandeTukey(int znak , int p, Complex.Complex[] X)
        {
           Complex.Complex T ;  
           Complex.Complex[] ExpW;
           int  i,l,k,m,r=0,s,N,MP=0,MP1,Blad=0 ;
           N=1<<p;     //dwa do potęgi p
           //InicjacjaWektora(ExpW,N);
           ExpW = new Complex.Complex[N + 1];
           if (p<3 || p>15) Blad=4 ;
              else if (X.Length-1!=N) Blad=6 ;
           if (Blad==0)
             {
               WekExp(znak,N,ExpW);
               MP1=N;
               for (m=1; m<=p; m++)
               {
                   MP1=MP1>>1;   MP=MP1<<1;
                   if (m==1)  r=1; 
                   else r=r<<1;      //r=r<<1;  mnożenie przez dwa
                   for (i=0; i<=MP1-1;i++)
                     {
                        l=i;  s=i*r;
                        while (l<N)
                         {
                           k=l+MP1;
                           T=X[l]-X[k];
                           X[l]+=X[k];  
                           X[k]=T*ExpW[s];
                           l+=MP;
                         }
                      }//i
               }//m
               InversBit(p,X);   return 0;
            }else return Blad;
        }//FFTSandeTukey
        //------------------------------------------------------------------------------

        /// <summary>
        /// Metoda wg algorytmu Hornera
        /// </summary>
        /// <param name="znak">znak =-1 -transformacja prosta
        ///                    znak =+1 -transformacja odwrotna</param>
        /// <param name="N">liczba elementów wektora X</param>
        /// <param name="X">wektor zespolony próbek wejściowych o elementach X[j]</param>
        /// <returns>funkcja zwraca 0 gdy błąd nie wystapil, w przeciwnym
        /// razie zwraca numer błędu</returns>
        public static int DTFHorner(int znak ,int N,Complex.Complex[] X)
        {
          int i,k,Blad=0 ;
          Complex.Complex ZX, Wk;
          Complex.Complex[] ExpW=new Complex.Complex[N+1];
          //InicjacjaWektora(ExpW,N);
          //vector<complex<Typ> > WekC(X);
          Complex.Complex[] WekC=new Complex.Complex[N+1];
          for (k=0; k<=N-1; k++) WekC[k]=X[k];
          //WekC=X;
          if (X.Length-1!=N) Blad=7 ;
           if (Blad==0)
             {
               //InicjacjaWektora(ExpW,N);
               WekExp(znak,N,ExpW);
               for (k=0; k<=N-1; k++)
               {
                    Wk=ExpW[k];  ZX=WekC[N-1];
                    for (i=N-1; i>=1; i--) {  ZX*=Wk;  ZX+=WekC[i-1]; }
                    X[k]=ZX;
               }
               return 0;
            } else return Blad;
        }//DTFHorner
        //-------------------------------------------------------------------------

        /// <summary>
        /// Metoda do wyznaczanie zespolonych współczynników  szeregu Fouriera
        /// funkcji okresowej
        /// </summary>
        /// <param name="Metoda">Rodzaj algorytmu stosowanego do wyznaczania 
        /// odwrotnej transformaty  1 - FFT Cooleya-Tukeya 
        ///                         2 - FFT Sandego-Tukeya
        ///                         3 - DTF Hornera</param>
        /// <param name="p">wykładnik potęgi przy obliczaniu 
        /// N dwa do potęgi p dla algorytmu</param>
        /// <param name="T0">Chwila początkowa całkowania po okresie T</param>
        /// <param name="T">Okres badanej funkcji</param>
        /// <param name="X">Wektor zespolony współczynników szeregu Fouriera X[j]</param>
        /// <param name="U">Egzemplarz delegata FunkcjaRealeReale funkcji okresowej </param>
        /// <returns>funkcja zwraca 0 gdy błąd nie wystapil, w przeciwnym
        /// razie zwraca numer błędu</returns>
        public static int WspolczynnikiSzereguFouriera(int Metoda, 
                                int p, double T0,double T,
                                Complex.Complex[] X, FunkcjaRealeReale U)
        {
          int k,N=0,Blad=0 ;
          double la,h,tt,aa ;
          if (Metoda==3) { N=p;}
            else { if (p<3|| p>15)  Blad=9 ; 
                   else N=1<<p; 
                 }
          if (Blad==0)
            {
              h=T/N; la=2.0/N;
              for (k=0; k<=N-1; k++)
              {
                  tt=T0+k*h;  aa=U(tt)*la;
                  X[k] =new Complex.Complex(aa, 0);
              }
              switch (Metoda)
              {
                   case 1 : Blad=FFTCooleyTukey(-1,p,X);
                                      break;
                   case 2 :  Blad=FFTSandeTukey(-1,p,X);
                                      break;
                   case 3:       Blad=DTFHorner(-1 ,N,X);
                                      break;
                   default: Blad = 8; break;
              }
              if (Blad==0) return 0;
                  else return Blad;
            } else return Blad;
        }//WspolczynnikiSzereguFouriera
        //---------------------------------------------------------------------------
        //Wzorzec funkcji do wyznaczania odwrotnej transformacji Fouriera
        //Metoda - rodzaj algorytmu stosowanego do wyznaczania odwrotnej transformaty
        //p - wykładnik potęgi przy obliczaniu N dwa do potęgi p dla metod
        // Cooleya-Tukeya i Sandego-Tukeya oraz liczbę próbek dla metody Hornera
        //F - transformacja Fouriera jako egzemplarz delegata FunkcjaComplexReale
        //w0- określa przedział całkowania ze względu na zmienną w<=(-w0,+w0)
        //Ex - wektor próbek rozwiązania Ex[i] odpowiadający chwilom czasowym Tx[i]
        public static int OdwrotnaTransformacjaFouriera(int Metoda ,int p,
                FunkcjaComplexReale F, double w0,  double[] Tx, double[] Ex)
        {
          int N,k,N1,Blad=0;   
          double w,dw,la,dt;
          if (Metoda==3) N=p;
             else N=1<<p;
          if (N!=(int)Tx.Length-1 || N!=(int)Ex.Length-1) Blad=10;
            else
            {
              //vector<complex<Typ> > Gp;
              //InicjacjaWektora(Gp,N);
              Complex.Complex[] Gp=new Complex.Complex[N+1]; 
              //vector<Typ> Ep,El;
              //InicjacjaWektora(Ep,N);   InicjacjaWektora(El,N);
              //InicjacjaWektora(Tx,N);   InicjacjaWektora(Ex,N);
              double [] Ep= new double[N+1];
              double [] El= new double[N+1];
              //Ex= new double[N+1];
              //Tx= new double[N+1];
               N1=N>>1 ;
               dt=Math.PI/w0;
               dw=w0/N1; la=1/(N*dt);
               for (k=0; k<=N1; k++)
               {
                  Tx[N1-k]=-k*dt; Tx[N1+k]=k*dt;
               }
               for (k=0; k<=N1-1; k++)
               {
                   w=k*dw; Gp[k]=F(w)*la;
                   if (k>0) Gp[N-k]=F(-w)*la;
               }
               w=N1*dw;
               Gp[N1]=F(w)*la+F(-w)*la;
               //vector<complex<Typ> > Gl(Gp);
               Complex.Complex[] Gl=new Complex.Complex[N+1];
               for (k = 0; k <= N; k++) Gl[k] = Gp[k];
               switch (Metoda)
               {
                   case 1 : Blad=FFTCooleyTukey(1 ,p,Gp);
                                      if (Blad==0) Blad=FFTCooleyTukey(-1 ,p,Gl) ;
                                      break;
                   case 2 :  Blad=FFTSandeTukey(1 ,p,Gp);
                                      if (Blad==0) Blad=FFTSandeTukey(-1 ,p,Gl);
                                      break;
                   case 3:       Blad=DTFHorner(1 ,N,Gp) ;
                                      if (Blad==0) Blad=DTFHorner(-1 ,N,Gl);
                                      break;
                   default: Blad=11; break;
               }
               if (Blad==0)
               {
                   for (k=0; k<=N-1; k++)
                   {  
                       Ep[k]=Gp[k].Re;  
                       El[k]=Gl[k].Re; 
                   }
                   for (k=0; k<=N-1; k++)
                      if (k<N1) Ex[k]=El[N1-k] ;
                      else  Ex[k]=Ep[k-N1];
               }
            }
          return Blad;
        }




    }
}
