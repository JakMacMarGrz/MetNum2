using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSBibMatStudent.Complex
{
    /// <summary>
    /// Zawiera zbiór funkcji matematycznych operujących na liczbach zespolonych.
    /// </summary>
    public static partial class ComplexMath
    {
        /// <summary>
        /// Zwraca liczbę sprzężoną do danej liczby zespolonej.
        /// </summary>
        /// <param name="complex">Liczba zespolona (z)</param>
        /// <returns>Liczba sprzężona lespolona (z*)</returns>
        public static Complex Conjugate(Complex z)
        {
            return new Complex(z.Re, -z.Im);
        }

        /// <summary>
        /// Zwraca moduł liczby zespolonej.
        /// </summary>
        /// <param name="z">Liczba zespolona z</param>
        /// <returns>Moduł liczby zespolonej |z|</returns>
        public static double Abs(Complex z)
        {
            if (Complex.IsNaN(z)) return double.NaN;
            if (Complex.Isinfinity(z)) return double.PositiveInfinity;

            double reAbs = Math.Abs(z.Re);
            double imAbs = Math.Abs(z.Im);

            if (z.Re == 0) return imAbs;
            if (z.Im == 0) return reAbs;

            if (reAbs >= imAbs)
            {
                double div = imAbs / reAbs;
                return reAbs * Math.Sqrt(1.0 + div * div);
            }
            else
            {
                double div = reAbs / imAbs;
                return imAbs * Math.Sqrt(1.0 + div * div);
            }
        }
        //----------------------------------------------------------
        public static Complex Sin(Complex z)
        {
           double x = z.Re;
           double y = z.Im;
           return new Complex(Math.Sin(x) * Math.Cosh(y), Math.Cos(x) * Math.Sinh(y));
        }
        //----------------------------------------------------------
        public static Complex Cos(Complex z)
        {
            double x = z.Re;
            double y = z.Im;
            return new Complex(Math.Cos(x) * Math.Cosh(y), -Math.Sin(x) * Math.Sinh(y));
        }
        //-----------------------------------------------------------
        public static Complex Tan(Complex z)
        {
            double x = 2.0*z.Re;
            double y = 2.0*z.Im;
            double m = Math.Cosh(y) + Math.Cos(x);
            return new Complex(Math.Sin(x)/m, Math.Sinh(y)/m);
        }
        //-----------------------------------------------------------
        public static Complex Exp(Complex z)
        {
            double x = z.Re;
            double y = z.Im;
            return new Complex(Math.Exp(x) * Math.Cos(y), Math.Exp(x) * Math.Sin(y));
        }
        //-----------------------------------------------------------
     
        public static Complex Sinh(Complex z)
        {
            double x = z.Re;
            double y = z.Im;
            return new Complex(Math.Cos(y) * Math.Sinh(x), Math.Sin(y) * Math.Cosh(x));
            //return 0.5 * (Exp(z) - Exp(-z));
        }
        //----------------------------------------------------------
        public static Complex Cosh(Complex z)
        {
            double x = z.Re;
            double y = z.Im;
            return new Complex(Math.Cos(y) * Math.Cosh(x), Math.Sin(y) * Math.Sinh(x));
            //return 0.5 * (Exp(z)+ Exp(-z));
        }
        //----------------------------------------------------------
        public static Complex Tanh(Complex z)
        {
            double x = 2.0 * z.Re;
            double y = 2.0 * z.Im;
            double m = Math.Cosh(x) + Math.Cos(y);
            return new Complex(Math.Sinh(x) / m, Math.Sin(y) / m);
        }
        //-----------------------------------------------------------
        
          public static Complex Sqrt(Complex z)
        {
            double r = Abs(z);
            double f = 0.5*Math.Atan2(z.Im,z.Re);
            return new Complex(r * Math.Cos(f), r * Math.Sin(f));
        }
        //-----------------------------------------------------------
        public static Complex Pow(Complex z,int n)
        {
            Complex zz=new Complex(z.Re,z.Im);
            for (int i = 1; i <= n-1; i++) zz *= z;
            if (n > 0) return zz;
            else return Complex.One;
        }
        //-----------------------------------------------------------
        public static Complex Pow(double a,Complex z)
        {
            return Exp(z*Math.Log(a));
        }
        //------------------------------------------
        public static Complex Log(Complex z)
        {
            double r = Abs(z);
            double f =  Math.Atan2(z.Im, z.Re);
            return new Complex(Math.Log(r), f);
        }       
        /// <summary>
        /// Funkcji Bessela argumentu zespolonego
        /// </summary>
        /// <param name="n">Rząd funkcji</param>
        /// <param name="x">Argument zespolony</param>
        /// <returns>Wartość zespoloną</returns>
        public static Complex Jn(int n, Complex x)
        {
          Complex AZ, Z, S, WYR, SIL, Y, a, xn;
          int i;
          double NX,pot,i1,w,N2;
          NX=n;
          a = new Complex(1,0);
          if (Abs(x)<10.0)
            {
              SIL = new Complex(1, 0);
              for (i=1; i<=n; i++) SIL*=i;
              Z=x*x;
              pot=Math.Pow(2.0,NX);
              WYR=a/(pot*SIL);
              S=WYR; i=1;
              while (Abs(WYR)>1e-20)
                {  i1=i;
                   Y=4.0*i1*(NX+i1);
                   WYR*=-Z/Y; S+=WYR;  i++;
                }
              //xn=pow(x,NX);
              //xn = new Complex(1, 0);
              //for (i=1; i<=n; i++) xn*=x;
              xn = Pow(x, n);
              return  S*xn ;       
            } else
               {
                 w=64.0;
                 N2=4.0*n*n;
                 Z=w*x;
                 Z*=x;
                 w=-(N2-1.0)*(N2-9.0)/2.0;
                 AZ=w/Z;   w=1.0;
                 S=a+AZ;   w=12.0;
                 AZ/=w*Z;
                 AZ*=-(N2-25.0)*(N2-49.0);
                 S+=AZ;  w=30.0;  AZ/=w*Z;
                 AZ*=-(N2-81)*(N2-121);
                 S+=AZ;  w=(N2-1)/8.0;  WYR=w/x;
                 SIL=WYR; w=6.0;  WYR/=w*Z;
                 WYR*=-(N2-9.0)*(N2-25.0);
                 SIL+=WYR;  w=20.0;  WYR/=w*Z;
                 WYR*=-(N2-49.0)*(N2-81.0);
                 SIL+=WYR; w=42.0;  WYR/=w*Z;
                 WYR*=-(N2-121.0)*(N2-169.0);
                 SIL+=WYR;   w=0.25*Math.PI;
                 Z = x - w; w = 0.5 * n * Math.PI;
                 Z-=w;
                 S *= ComplexMath.Cos(Z); SIL *= ComplexMath.Sin(Z);
                 w=2.0/Math.PI;
                 return  Sqrt(w/x)*(S-SIL);
               }
        }//JN
        //---------------------------------------------------------------------------
        public static Complex dJn(int n, Complex x)
        {
          int k,N2;
          Complex J0, J1, JN, JNP;
          if (x == Complex.Zero)
            {
              if (n==1) return 0.5;
              else return Complex.Zero;
            }
          else
            {
              if (n==0)
                {
                 J1=Jn(1,x);  return -J1;
                }
              else
                {   J0=Jn(0,x) ; J1=Jn(1,x);
                    for (k=2; k<=n; k++)
                      { N2=2*k-2;
                        JN=J1/x;  JN*=N2;  JN-=J0;
                        J0=J1; J1=JN ;
                      }//k;
                    JNP=J1/x;  JNP*=n;
                    return J0-JNP;
                 }
            }
        }//dJn;
        //------------------------------------------
        //Definicja zmodyfikowanej funkcji Bessela
        public static Complex In(int n ,Complex x)
        {
          Complex Z,S,WYR,SIL,a,z0,xn,Y;
          int l,N2,i;
          double Mx,potc,i1,l1,NX,NC,Rx,c8;
          z0 = Complex.Zero;
          a = Complex.One;
          NX=n;
          Mx=Abs(x);
          if (x!=z0)
           {
            if (Mx<15.0)
             {
                SIL = Complex.One;
                for (i=1; i<=n; i++) SIL*=i;
                //potc=1<<n;     //dwa do potęgi N
                potc=Math.Pow(2.0,n);
                Z=x*x;
                WYR=a;   WYR/=SIL;  WYR/=potc;
                S=WYR; i=1;
                while (Abs(WYR)>1E-30)
                  {
                    i1=i;
                    Rx=i1*(NX+i1)*4.0;
                    Y=Z;
                    Y/=Rx;
                    WYR*=Y;
                    S+=WYR;
                    i++;
                  } ;
                xn=Pow(x,n);
                return S*xn ;
               } else
                  {
                     N2=(int)4*n*n;
                     NC=N2;
                     c8=8.0;
                     Z=x;
                     Z*=c8;
                     WYR=a/Z;
                     WYR*=-(NC-1.0);
                     S=a+WYR;
                     for (i=2; i<=10; i++)
                       {
                         l=2*i-1; l*=l;
                         l1=l;  i1=i;
                         Y=-(NC-l1); Y/=Z; Y/=i1;
                         WYR*=Y;
                         S+=WYR;
                       } ;
                     Rx=2.0;
                     Rx*=Math.PI;
                     Y=x;
                     Y*=Rx;
                     Rx=Abs(Y);
                     Rx=Math.Sqrt(Rx);
                     Y=Exp(x);
                     Y*=S;
                     Y/=Rx;
                     return Y;
                   }
           }
          else
           {
             if (n==0)  return a;
             else return z0;
           }
        }//In
        //--------------------------------------------------------------------
        public static Complex dIn(int n, Complex X)
        {
          Complex A0,A2,AN;
          double N2;
          if (n==0)
            {
              A2=In(1,X);
              return A2;
            }
          else
            {
              N2=0.5;
              A0=In(n-1,X);
              A2=In(n+1,X);
              AN=A0+A2;
              AN*=N2;
              return AN;
            }
        }//dIn

        //Definicja funkcji MacDonalda
        public static Complex Kn(int n ,Complex x)
        {
          Complex S1,SIL,R1,Z,Y,WYR,S,S2,SL,SP,S3,a,xn,a2;
          int N2,i,j,l;
          double NX,l1,l2,i1,j1,NC,Mx,potc,Rx;
          S2=Complex.Zero;// Typ(0);
          a = Complex.One; //Typ(1);
          a2 = Complex.One; //Typ(1);
          NX=n;
          Mx=Abs(x);
          if (Mx<4.0)
            {
              SIL=Complex.One;//Typ(1);
              for (i=1; i<=n; i++) SIL*=i;
              Z=x*x;
              //potc=1<<N;     //dwa do potęgi N
              potc=Math.Pow(2.0,n);
              a2/=potc;
              WYR=a2/SIL;
              //WYR=a/(potc*SIL);
              S=WYR;
              if (n == 0) S3 = Complex.Zero;//Typ(0);
              else
              {
                  SP = Complex.Zero;//Typ(0);
                  for (j = 1; j <= n; j++)
                  {
                      j1 = j;
                      a2 = Complex.One;//Typ(1);
                      a2 /= j1;
                      SP += a2;
                      //SP+=a/j1;
                  }
                  S3 = WYR * SP;
              };
              i=1;
              SL=Complex.One;//Typ(1);
              SP=Complex.Zero;//Typ(0);
              for (j=1; j<=n+1; j++)
                {
                   j1=j;  a2=Complex.One;//Typ(1);
                   a2/=j1;
                   SP+=a2;
                   //SP+=a/j1;
                }
              while (Abs(WYR)>1E-30)
              {
                   i1=i;
                   Rx=i1*(NX+i1)*4.0;
                   Y=Z;
                   Y/=Rx ;
                   WYR*=Y;
                   //WYR*=Z/(i1*(NX+i1)*4.0);
                   if (i>1)
                    {
                      a2=Complex.One;//Typ(1); 
                      a2/=i1;
                      SL+=a2;
                      //SL+=a/i1;
                      a2=Complex.One;//Typ(1);  
                      a2/=NX+i1;
                      SP+=a2 ;
                      //SP+=a/(NX+i1) ;
                    };
                   S3+=WYR*(SL+SP); 
                  S+=WYR; i++;
              }
              l1=-1.0;
              for (j=0; j<=n; j++) l1=-l1;
              l2=l1;
              xn=Pow(x,n);
              l2*=0.5;
              Y=xn;
              Y*=l2;
              //Y=xn*l1;
              //S3*=xn*l1;
              S3*=Y;
              S*=xn;
              Y=x;
              Y*=0.5;
              S1=Log(Y);
              S1+=0.5772156649;
              //S1=0.577156649+log(0.5*x);
              S1*=S;
              S1*=-l1;
              //S1=-l1*S*(0.577156649+log(0.5*x));
              if (n==0) S2=Complex.Zero;//Typ(0);
              if (n==1) S2=a/x;
              if (n>1)
                {
                  Z=x*x;
                  Z*=0.25;
                  WYR=-Z;
                  WYR/=NX-1.0;
                  //WYR=-Z/(NX-a);
                  S=a+WYR;
                  for (i=2; i<=n-1; i++)
                    {
                      i1=i;
                      Rx=i1*(NX-i1);
                      Y=-Z;
                      Y/=Rx;
                      WYR*=Y;
                      //WYR*=-Z/(i1*(NX-i1));
                      S+=WYR  ;
                    };
                  SIL/=NX;
                  Y=2.0; Y/=x;
                  R1=Pow(Y,n);
                  R1*=SIL;
                  R1*=0.5;
                  //R1=0.5*SIL*powrz(Y,N);
                  //R1=0.5*SIL*pow(2.0/x,NX);
                  S2=R1*S ;
                };
               return S1+S2+S3 ;
            } else
                {
                   N2=4*n;
                   N2*=n;
                   NC=N2;
                   Z=x;
                   Z*=8.0;
                   //Z=8.0*x;
                   WYR=-a;
                   WYR+=NC;
                   WYR/=Z;
                   //WYR=(NC-a)/Z;
                   S=a+WYR;
                   if ((Mx>=2.0)&&(Mx<=3.0)) j=3;
                    else j=4;
                   for (i=2; i<=j; i++)
                     {
                       l=2*i-1; l*=l;
                       l1=l;   i1=i;
                       Y=NC-l1;
                       Y/=Z;
                       Y/=i1;
                       WYR*=Y;
                       //WYR*=(NC-l1)/(i1*Z);
                       S+=WYR  ;
                     };
                  Y=Math.PI;
                  Y*=0.5;
                  Y/=x;
                  //S1=M_PI*0.5/x;
                  S1=Sqrt(Y);
                  S1*=Exp(-x);
                  S*=S1;
                  //S*=sqrt(S1)*exp(-x);
                  //return sqrt(S1)*exp(-x)*S;
                  return S;
               }
        }//KN
        //--------------------------------------------------------------------
        public static Complex dKn(int n, Complex x)
        {
          double N2=0.5;
          Complex K0, K2, KN;
          if (n==0)
          {
              K2=Kn(1,x); 
              return -K2;
          }
          else
          {
              K0=Kn(n-1,x);   K2=Kn(n+1,x);
              KN=K0+K2;
              KN*=-N2;
              return KN;
          }
        }
        //------------------------------------------------------
        public static Complex Yn(int N, Complex X)
        {
           Complex  LOG,S1,SIL,R1,Z,WYR,S,S2,SL,SP,S3,a,xn,xn1,zx;
           int I,J;
           double N2,MX,NX,pot,I1,J1,a2,a64,a1,a9,a25,a49,a12,a4,
                  a81,a121,a30,a8,a20,a6,a169,a42,a025,a05;
           a2 = 2.0; a4 = 4.0; a64 = 64.0; a1 = 1.0; a9 = 9.0; a25 = 25.0; a49 = 49.0; a12 = 12.0;
           a25=25.0; a81=81.0; a121=121.0; a30=30.0; a8=8.0; a20=20.0;
           a6=6.0; a169=169.0; a42=42.0;  a025=0.25;  a05=0.5;
           a = Complex.One;// Typ(1);
           MX=Abs(X);
           NX=N;
           S2=0;
           if (MX<10)
             {
               SIL = Complex.One;//Typ(1);
               for (I=1; I<=N; I++) SIL*=I;
               Z=X*X;
               pot=Math.Pow(2.0,NX);
               WYR=a/(pot*SIL);
               S=WYR;
               if (N==0) S3=0;
                 else
                   {  SP=0;
                      for (J=1; J<=N; J++) { J1=J; SP+=a/J1;}
                      S3=WYR*SP ;
                   }
               I=1; SL=a; SP=Complex.Zero;//Typ(0);
               for (J=1; J<=N+1; J++) {J1=J; SP+=a/J1; }
               while (Abs(WYR)>1E-20)
                 {
                   I1=I;
                   WYR=-WYR*Z/(I1*(NX+I1)*4.0);
                   if (I>1)
                   {
                      SL+=a/I1;   SP+=a/(NX+I1) ;
                   }
                   S3+=WYR*(SL+SP); S+=WYR;  I++;  //I:=I+1
                  }
               //S3:=-POTEGA(N,X)*S3/PI; S:=POTEGA(N,X)*S;
               //xn=pow(X,NX);
               xn=Pow(X,N);
               S3*=-xn/Math.PI;
               S*=xn;
               LOG=Log(a05*X);
               S1=a2*(0.577156649+LOG)*S/Math.PI;
               if (N==0)  S2=0;
               if (N==1)  S2=-a2/(Math.PI*X);
               if (N>1)
                {
                   Z=a025*X*X; WYR=Z/(NX-a1); S=a+WYR;
                   for (I=2; I<=N-1; I++)
                   {
                     I1=I;
                     WYR*=Z/(I1*(NX-I1)); S+=WYR ;
                   }
                   SIL/=NX;
                   //R1=-SIL*POTEGA(N,2.0/X)/PI;
                   zx=a2/X;
                   //xn1=pow(2.0/X,NX);
                   xn1=Pow(zx,N);
                   R1=-SIL*xn1/Math.PI;
                   S2=R1*S ;
                 }
               return S1+S2+S3  ;
             } else
                   {
                       N2=a4*NX*NX; Z=a64*X*X; WYR=-(N2-a1)*(N2-a9)/(a2*Z);
                       S=1.0+WYR;
                       WYR=-WYR*(N2-a25)*(N2-a49)/(a12*Z);
                       S+=WYR;
                       WYR*=-(N2-a81)*(N2-a121)/(a30*Z);
                       S+=WYR;
                       WYR=(N2-a1)/(a8*X);
                       SIL=WYR;
                       WYR*=-(N2-a9)*(N2-a25)/(a6*Z);
                       SIL+=WYR;
                       WYR*=-(N2-a49)*(N2-a81)/(a20*Z);
                       SIL+=WYR;
                       WYR*=-(N2-a121)*(N2-a169)/(a42*Z);
                       SIL+=WYR;
                       Z=X-a025*Math.PI-a05*NX*Math.PI;
                       S *= ComplexMath.Sin(Z); SIL *= ComplexMath.Cos(Z);
                       return Sqrt(a2/(Math.PI*X))*(S+SIL);
                    }
        }
        //--------------------------------------------------------------------
        public static Complex dYn(int N, Complex X)
        {
          int k;
          double N2;
          Complex Y0,Y1,YN;
          if (N==0)
            {
              Y1=Yn(1,X);  return -Y1;
            }
          else
            {
              Y0=Yn(0,X);   Y1=Yn(1,X);
              for (k=2; k<=N; k++)
                {
                  N2=2.0*k-2.0;
                  YN=Y1/X;
                  YN*=N2;  YN-=Y0;
                  Y0=Y1;   Y1=YN;
                }
              YN=Y1/X;   N2=N;   YN*=N2;
              return Y0-YN;
            }
        }//dYn

    }
}
