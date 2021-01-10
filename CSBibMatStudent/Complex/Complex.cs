using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace CSBibMatStudent.Complex
{
    /// <summary>
    /// Liczba zespolona
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Complex : IEquatable<Complex>
    {
        #region Pola prywatne -------------------------------------------------

        private static readonly Complex sj = new Complex(0, 1);
        private static readonly Complex sinfinity = new Complex(Double.PositiveInfinity, Double.PositiveInfinity);
        private static readonly Complex sNaN = new Complex(Double.NaN, Double.NaN);
        private static readonly Complex sOne = new Complex(1);
        private static readonly Complex sZero = new Complex(0);

        private readonly double mRe;
        private readonly double mIm;

        #endregion Pola prywatne ----------------------------------------------

        #region Pola publiczne ------------------------------------------------

        /// <summary>
        /// Część rzeczywista liczby zespolonej.
        /// </summary>
        public double Re
        {
            get { return mRe; }
        }

        /// <summary>
        /// Część urojona liczby zespolonej.
        /// </summary>
        public double Im
        {
            get { return mIm; }
        }

        /// <summary>
        /// Stała reprezentująca liczbę zepoloną j.
        /// </summary>
        public static Complex j
        {
            get { return sj; }
        }

        /// <summary>
        /// Stała reprezentująca wartość liczby zespolonej "w nieskończoności".
        /// Gdzie Re = Double.PositiveInfinity; Im = Double.PositiveInfinity
        /// </summary>
        public static Complex Infinity
        {
            get { return sinfinity; }
        }

        /// <summary>
        /// Stała reprezentująca wartość typu "not a number" NaN jako liczba zespolona.
        /// </summary>
        public static Complex NaN
        {
            get { return sNaN; }
        }

        /// <summary>
        /// Stała reprezentująca wartość jednostkową (1) jako liczba zespolona.
        /// </summary>
        public static Complex One
        {
            get { return sOne; }
        }

        /// <summary>
        /// Stała reprezentująca wartość zero (0) jako liczba zespolona.
        /// </summary>
        public static Complex Zero
        {
            get { return sZero; }
        }

        #endregion Pola publiczne ---------------------------------------------

        #region Konstruktory dla argumentów typu Double -----------------------

        /// <summary>
        /// Tworzy liczbę zespoloną dla argumentów re oraz im.
        /// </summary>
        /// <param name="re">Część rzeczywista liczby zespolonej.</param>
        /// <param name="im">Część urojona liczby zespolonej.</param>
        public Complex(double re, double im)
        {
            mRe = re;
            mIm = im;
        }

        /// <summary>
        /// Tworzy liczbę zespoloną dla argumentu re. Część urojona ustawiona jest na 0.
        /// </summary>
        /// <param name="re">Część rzeczywista liczby zespolonej (im = 0)</param>
        public Complex(double re)
            : this(re, 0)
        {
        }

        #endregion Konstruktory dla argumentów typu Double --------------------


        #region Konstruktory dla argumentów typu String -----------------------

        //todo
        public Complex(string s)
        {
            mRe = Parse(s).mIm;
            mIm = Parse(s).mIm;
        }


        #endregion Konstruktory dla argumentów typu String --------------------


        #region IEquatable<Complex> Members -----------------------------------

        /// <summary>
        /// Sprawdza czy dwie liczby zespolone są sobie równe.
        /// </summary>
        /// <param name="other">Liczba zespoloa, z którą będzie wykonane porónanie.</param>
        /// <returns>Zwraca true jeśli dwie liczby zespolone są sobie równe,
        /// w przeciwnym wypadku false.</returns>
        public bool Equals(Complex other)
        {
            return ((Re == other.Re) && (Im == other.Im));
        }

        #endregion IEquatable<Complex> Members --------------------------------

        /// <summary>
        /// Zwraca HashCode instancji liczby zespolonej.
        /// </summary>
        /// <remarks>
        /// HashCode wyznaczany jest następująco: 
        /// this.Re.GetHashCode() ^ this.Im.GetHashCode());
        /// </remarks>        
        public override int GetHashCode()
        {
            return (int)(this.mRe.GetHashCode() ^ this.mIm.GetHashCode());
        }

        /// <summary>
        /// Sprawdza czy dwie liczby zespolone są sobie równe.
        /// </summary>
        /// <param name="obj">Liczba zespoloa, z którą będzie wykonane porónanie.</param>
        /// <returns>Zwraca true jeśli dwie liczby zespolone są sobie równe,
        /// w przeciwnym wypadku false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is Complex)
            {
                Complex other = (Complex)obj;
                return ((Re == other.Re) && (Im == other.Im));
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        #region Metody Statyczne ----------------------------------------------

        public static Complex FromReIm(double re, double im)
        {
            return new Complex(re, im);
        }

        public static Complex FromModArg(double modulus, double argRadians)
        {
            return new Complex(modulus * Math.Cos(argRadians),
                modulus * Math.Sin(argRadians));
        }

        static public double DegToRad(double angleDeg)
        {
            return Math.PI * angleDeg / 180.0;
        }

        public static bool IsNaN(Complex z)
        {
            return (Double.IsNaN(z.mRe) || Double.IsNaN(z.mIm));
        }

        public static bool Isinfinity(Complex z)
        {
            return (!IsNaN(z) && (Double.IsInfinity(z.mRe) || Double.IsInfinity(z.mIm)));
        }

        /// <summary>
        /// Konwertuje liczbę typu double na liczbę typu Complex
        /// </summary>        
        public static Complex ToComplex(double value)
        {
            return new Complex(value);
        }

        #endregion Metody Statyczne -------------------------------------------

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (IsNaN(this))
            {
                return "NaN";
            }
            if (Isinfinity(this))
            {
                return "Infinity";
            }

            string result = mRe.ToString(format, formatProvider);
            if (mIm < 0)
            {
                result += " - j" + (-mIm).ToString(format, formatProvider);
            }
            else
            {
                result += " + j" + (mIm).ToString(format, formatProvider);
            }

            return result;

            //todo
            //Sprawdź czy zastosowanie klasy StringBuilder przyśpieszy działanie.

            //StringBuilder ret = new StringBuilder();
            //ret.Append(mRe.ToString(format, formatProvider));
            //if (mIm < 0) ret.Append(" ");
            //else ret.Append(" + ");
            //ret.Append(mIm.ToString(format, formatProvider)).Append("i");
            //return ret.ToString();
        }

        /// <summary>
        /// Zwraca liczbę sprzężoną liczby zespolonej.
        /// </summary>
        public Complex Conjugate
        {
            get { return ComplexMath.Conjugate(this); }
        }

        /// <summary>
        /// Zwraca moduł liczby zespolonej.
        /// </summary>
        public double Abs
        {
            get { return ComplexMath.Abs(this); }
        }

        #region Operatory -----------------------------------------------------

        /// <summary>
        /// Operator konwesji z typu Double na typ Complex.
        /// </summary>        
        public static implicit operator Complex(double value)
        {
            return new Complex(value);
        }

        public static bool operator ==(Complex z1, Complex z2)
        {
            return z1.Equals(z2);
        }

        public static bool operator !=(Complex z1, Complex z2)
        {
            return !z1.Equals(z2);
        }

        public static Complex operator +(Complex z)
        {
            return z;
        }

        //public static Complex Plus(Complex z)
        //{
        //    return z;
        //}

        public static Complex operator +(Complex z1, Complex z2)
        {
            return new Complex(z1.Re + z2.Re, z1.Im + z2.Im);
        }

        public Complex Add(Complex z)
        {
            return this + z;
        }

        public static Complex operator -(Complex z)
        {
            return new Complex(-z.Re, -z.Im);
        }

        public static Complex operator -(Complex z1, Complex z2)
        {
            return new Complex(z1.Re - z2.Re, z1.Im - z2.Im);
        }

        public Complex Subtract(Complex z)
        {
            return this - z;
        }

        public static Complex operator *(Complex z1, Complex z2)
        {
            //return new Complex((z1.Re * z2.Re) - (z1.Im * z2.Im),
            //    (z1.Im * z2.Re) + (z1.Re * z2.Im));

            //todo
            //sprawdzic wydajnosc
            double ac = z1.Re * z2.Re;
            double bd = z1.Im * z2.Im;

            return new Complex(ac - bd,
                (z1.Re + z1.Im) * (z2.Re + z2.Im) - ac - bd);
        }

        public Complex Multiply(Complex z)
        {
            return this * z;
        }

        public static Complex operator /(Complex z1, Complex z2)
        {
            if ((z2.Re == 0.0) && (z2.Im == 0.0))
            {
                return sNaN;
            }

            if (Isinfinity(z2) && !Isinfinity(z1))
            {
                return sZero;
            }

            double re, im;
            if (Math.Abs(z2.Re) >= Math.Abs(z2.Im))
            {
                double t = z2.Im / z2.Re;
                double denom = z2.Re + z2.Im * t;
                re = (z1.Re + z1.Im * t) / denom;
                im = (z1.Im - z1.Re * t) / denom;
            }
            else
            {
                double t = z2.Re / z2.Im;
                double denom = z2.Re * t + z2.Im;
                re = (z1.Re * t + z1.Im) / denom;
                im = (z1.Im * t - z1.Re) / denom;
            }

            return new Complex(re, im);
        }

        public Complex Divide(Complex z)
        {
            return this / z;
        }

        #endregion Operatory --------------------------------------------------

        #region ParMath.Sing--------------------------------------------------------

        public static Complex Parse(string value)
        {
            return Parse(value, null);
        }

        /// <summary>
        /// Tworzy reprezentację liczby zespolonej na podstawie parametru typu string.
        /// Podana liczba zespolona może przyjmować następujące formaty:
        /// 
        /// </summary>
        /// <param name="value">Ciąg znaków typu string reprezentujący liczbę zepoloną
        /// w danym formacie.</param>
        /// <param name="formatProvider">IFormatProvider</param>
        /// <returns>Liczba zespolona typu Complex</returns>
        public static Complex Parse(string value, IFormatProvider formatProvider)
        {
            if (value == null)
            {
                throw new ArgumentNullException(value);
            }

            value = value.Trim();
            if (value.Length == 0)
            {
                throw new FormatException();
            }

            while (value.IndexOf(" ") != -1)
            {
                value = value.Replace(" ", "");
            }


            if (value.StartsWith("(", StringComparison.Ordinal))
            {
                if (!value.EndsWith(")", StringComparison.Ordinal))
                {
                    throw new FormatException();
                }
                value = value.Substring(1, value.Length - 2);
            }

            if (value.Length == 1)
            {
                if ((String.Compare(value, "i", StringComparison.OrdinalIgnoreCase) == 0)
                    || (String.Compare(value, "j", StringComparison.OrdinalIgnoreCase) == 0))
                {
                    return new Complex(0, 1);
                }
                return new Complex(Double.Parse(value, formatProvider));
            }

            if (value.Equals("-i") || value.Equals("-I")
                || value.Equals("-j") || value.Equals("-J"))
            {
                return new Complex(0, -1);
            }

            double re = 0;
            double im = 0;

            int jIndex = value.IndexOfAny(new char[] { 'i', 'I', 'j', 'J' });

            if (jIndex == -1)
            {
                // Podana jest tylko część rzeczywista liczby zespolonej
                re = double.Parse(value);
                return new Complex(re, im);
            }
            else
            {
                // Jeśli i/j znajduje się na końcu stringa
                if (jIndex == value.Length - 1)
                {
                    int pmIndex = value.LastIndexOfAny(new char[] { '+', '-' });
                    if (pmIndex == -1)
                    {
                        im = double.Parse(value.Substring(0, jIndex));
                    }
                    else
                    {
                        im = double.Parse(value.Substring(pmIndex, jIndex - pmIndex));
                        if (pmIndex > 0)
                            re = double.Parse(value.Substring(0, pmIndex));
                    }
                }
                else
                {
                    if ((jIndex == 0) || (jIndex == 1))
                    {
                        im = double.Parse(value.Remove(jIndex, 1));
                    }
                    else
                    {
                        im = double.Parse(value.Substring(jIndex - 1,
                            value.Length - jIndex + 1).Remove(1, 1));
                        re = double.Parse(value.Substring(0, jIndex - 1));
                    }
                }

            }

            return new Complex(re, im);
        }

        public static bool TryParse(string value, out Complex result)
        {
            return TryParse(value, null, out result);
        }

        public static bool TryParse(string value, IFormatProvider formatProvider, out Complex result)
        {
            bool ret;
            try
            {
                result = Parse(value, formatProvider);
                ret = true;
            }
            catch (ArgumentNullException)
            {
                result = sZero;
                ret = false;
            }
            catch (FormatException)
            {
                result = sZero;
                ret = false;
            }
            return ret;
        }

        #endregion ParMath.Sing-----------------------------------------------------

    }

}
