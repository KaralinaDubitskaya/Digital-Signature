using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows;

namespace Digital_Signature
{
    public class RSA
    {

        #region Fields

        private BigInteger _P;
        private BigInteger _Q;
        private BigInteger _D;
        private BigInteger _E;
        private BigInteger _R;

        #endregion

        #region Properties
        
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger R { get; set; }
        public BigInteger D { get; set; }
        public BigInteger E { get; set; }

        #endregion

        #region Constructor
        #region public RSA(BigInteger p, BigInteger q, BigInteger e, BigInteger r)
        public RSA(BigInteger p, BigInteger q, BigInteger e, BigInteger r)
        {
            P = p;
            Q = q;
            E = e;
            R = r;
        }
        #endregion
        #region public RSA(BigInteger e, BigInteger r)
        public RSA(BigInteger e, BigInteger r)
        {
            E = e;
            R = r;
        }
        #endregion
        #endregion

        #region Methods
        #region public BigInteger EncryptHash(BigInteger hash). Encrypt hash value using RSA secret key.
        public BigInteger EncryptHash(BigInteger hash)
        {
            BigInteger EulersFuncOfR = (P - 1) * (Q - 1); // P and Q are prime numbers

            if (GCD(E, EulersFuncOfR) != 1)
            {
                MessageBox.Show("Error: Greatest common divisor of E and euler function of R must be equal 1.");
                return 0;
            }

            D = Euclide(E, EulersFuncOfR); // (E * D) mod EuclidesFunc(R) = 1

            return ModPower(hash, D, R); // c = (m^d) mod r
        }
        #endregion
        #region 
        public BigInteger DecryptHash(BigInteger hash)
        {
            return ModPower(hash, E, R);
        }
        #endregion

        #region private BigInteger ModPower(BigInteger a, BigInteger z, BigInteger n). Fast modular exponentiation for BigInteger numbers. x = a^z mod n
        private BigInteger ModPower(BigInteger a, BigInteger z, BigInteger n)
        {
            BigInteger a1 = a, z1 = z, x = 1;
            while (z1 != 0)
            {
                while (z1 % 2 == 0)
                {
                    z1 = z1 / 2;
                    a1 = (a1 * a1) % n;
                }
                z1--;
                x = (x * a1) % n;
            }
            return x;
        }
        #endregion
        #region private static BigInteger GCD(BigInteger a, BigInteger b). Greatest common diviser
        private static BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (a != 0 && b != 0)
            {
                if (a >= b) a = a % b;
                else b = b % a;
            }
            return a + b;
        }
        #endregion
        #region private BigInteger Euclide(BigInteger a, BigInteger b). Calculate gcd(a, b) and x, y, where ax + by = gcd(a, b). Return y.
        private BigInteger Euclide(BigInteger a, BigInteger b)
        {
            BigInteger d0 = a;
            BigInteger d1 = b;
            BigInteger x0 = 1;
            BigInteger x1 = 0;
            BigInteger y0 = 0;
            BigInteger y1 = 1;
            while (d1 > 1)
            {
                BigInteger q = d0 / d1;
                BigInteger d2 = d0 % d1;
                BigInteger x2 = x0 - q * x1;
                BigInteger y2 = y0 - q * y1;
                d0 = d1;
                d1 = d2;
                x0 = x1;
                x1 = x2;
                y0 = y1;
                y1 = y2;
            }
            return y1;
        }
        #endregion
        #endregion

    }
}
