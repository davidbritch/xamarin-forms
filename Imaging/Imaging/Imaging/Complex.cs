using System;

namespace Imaging
{
    public struct Complex
    {
        public double Re; // Real
        public double Im; // Imaginary

        public double Magnitude
        {
            get { return Math.Sqrt(Re * Re + Im * Im); }
        }

        public double Phase
        {
            get { return Math.Atan2(Im, Re); }
        }

        public Complex(double re, double im)
        {
            Re = re;
            Im = im;
        }

        public Complex(Complex c)
        {
            Re = c.Re;
            Im = c.Im;
        }
    }
}
