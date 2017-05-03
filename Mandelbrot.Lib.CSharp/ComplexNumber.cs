using System;

namespace Mandelbrot.Lib.CSharp
{
    /// <summary>
    /// Number that can be expressed in form (A + Bi) where A is the 
    ///  'Real' part and B is the 'Imaginary' part and i represents 
    ///  the square root of -1.
    /// See https://en.wikipedia.org/wiki/Complex_number for more.
    /// </summary>
    public struct ComplexNumber : IEquatable<ComplexNumber>
    {
        /// <summary>
        /// 'Real' part of the complex number.
        /// </summary>
        public readonly float Re;

        /// <summary>
        /// 'Imaginary' part of the complex number.
        /// </summary>
        public readonly float Im;

        public ComplexNumber(float re, float im)
        {
            Re = re;
            Im = im;
        }

        /// <summary>
        /// Whether this Complex Number is equal to another Complex Number.
        /// </summary>
        public bool Equals(ComplexNumber other)
        {
            return Re.Equals(other.Re) && Im.Equals(other.Im);
        }

        /// <summary>
        /// Whether this Complex Number object instance is equal to 
        ///  another Complex Number object instance.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is ComplexNumber && Equals((ComplexNumber) obj);
        }

        /// <summary>
        /// Generates a hash code of this Complex Number value.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Re.GetHashCode() * 397) ^ Im.GetHashCode();
            }
        }

        /// <summary>
        /// Zero value.
        /// </summary>
        public static readonly ComplexNumber Zero = new ComplexNumber(0.0f, 0.0f);

        /// <summary>
        /// Add together two Complex Numbers.
        /// </summary>
        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
        {
            return new ComplexNumber(a.Re + b.Re, a.Im + b.Im);
        }

        /// <summary>
        /// Multiple together two Complex Numbers.
        /// </summary>
        public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
        {
            var realPart = (a.Re * b.Re) - (a.Im * b.Im);
            var imagPart = (a.Im * b.Re) + (a.Re * b.Im);

            return new ComplexNumber(realPart, imagPart);
        }

        /// <summary>
        /// Squares the given Complex Number.
        /// </summary>
        public static ComplexNumber Square(ComplexNumber a)
        {
            return a * a;
        }

        /// <summary>
        /// Absolute value of the given Complex Number.
        /// </summary>
        public static float Abs(ComplexNumber a)
        {
            return (float) Math.Sqrt((a.Re * a.Re) + (a.Im * a.Im));
        }
    }
}
