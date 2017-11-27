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

            if (D < 0)
            {
                D += EulersFuncOfR;
            }

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
            BigInteger q, d2, x2, y2;
            while (d1 > 1)
            {
                q = d0 / d1;
                d2 = d0 % d1;
                x2 = x0 - q * x1;
                y2 = y0 - q * y1;
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


        //------------------------------------------------

        private static BigInteger QuickPower(BigInteger A, BigInteger B, BigInteger M)
        {
            BigInteger Answer = 1;
            while (B != 0)
            {
                while (B % 2 == 0)
                {
                    B = B / 2;
                    A = A * A % M;
                }
                Answer = (Answer * A) % M;
                B = B - 1;
            }
            return Answer;
        }


        public static BigInteger Euclides(BigInteger A, BigInteger B, out BigInteger x1, out BigInteger d1)
        {
            BigInteger d0 = A;
            d1 = B;
            BigInteger x0 = 1;
            x1 = 0;
            BigInteger y0 = 0;
            BigInteger y1 = 1;
            BigInteger d2;
            BigInteger q;
            BigInteger x2 = 0;
            BigInteger y2;
            while (d1 > 1)
            {
                q = d0 / d1;
                d2 = d0 % d1;
                x2 = x0 - q * x1;
                y2 = y0 - q * y1;
                d0 = d1; d1 = d2;
                x0 = x1; x1 = x2;
                y0 = y1; y1 = y2;
            }
            if (y1 < 0) { y1 = y1 + A; }
            return y1;
        }

        public static byte[] Code(byte[] Text, BigInteger key, BigInteger N, out BigInteger[] Arr)
        {
            BigInteger[] Temp = new BigInteger[Text.Length];
            for (int i = 0; i < Text.Length; i++) { Temp[i] = (BigInteger)Text[i]; }
            Temp = CodeEncode(Temp, key, N);
            if (Temp.Length >= 100) { Arr = new BigInteger[100]; } else { Arr = new BigInteger[Temp.Length]; }
            for (int i = 0; i < Arr.Length; i++) { Arr[i] = Temp[i]; }
            return BigIntToBytes(Temp, N);
        }

        public static byte[] Decode(byte[] Text, BigInteger key, BigInteger N, out BigInteger[] Arr)
        {
            BigInteger[] temp = CodeEncode(blocksToBigInt(Text, N), key, N);
            byte[] Answer = new byte[temp.Length];
            for (int i = 0; i < Answer.Length; i++) { Answer[i] = (byte)(temp[i] % 256); }
            if (temp.Length >= 100) { Arr = new BigInteger[100]; } else { Arr = new BigInteger[temp.Length]; }
            for (int i = 0; i < Arr.Length; i++) { Arr[i] = temp[i]; }
            return Answer;
        }

        private static BigInteger[] CodeEncode(BigInteger[] Text, BigInteger key, BigInteger N)
        {
            BigInteger[] Answer = new BigInteger[Text.Length];
            for (int i = 0; i < Text.Length; i++)
            {
                Answer[i] = QuickPower(Text[i], key, N);
            }
            return Answer;
        }


        private static BigInteger BigIntFromArrBytes(byte[] Arr)
        {
            BigInteger Answer = 0;
            BigInteger q = 1;
            for (int i = Arr.Length - 1; i >= 0; i--)
            {
                Answer += Arr[i] * q;
                q *= 256;
            }
            return Answer;
        }

        private static byte[] add(byte[] A, byte B)
        {
            Array.Resize(ref A, A.Length + 1);
            A[A.Length - 1] = B;
            return A;
        }


        private static BigInteger[] blocksToBigInt(byte[] Text, BigInteger N)
        {
            BigInteger[] Answer = new BigInteger[100000];
            int ansPos = 0; int i = 0; int j = 0;
            byte[] temp;
            byte[] Arr_N = BigIntToBytes(N);
            while (i < Text.Length)
            {
                j = 0;
                temp = new byte[Arr_N.Length];
                while ((i < Text.Length) && (j < Arr_N.Length))
                {
                    temp[j++] = Text[i++];
                }
                if (ansPos >= Answer.Length) { Array.Resize(ref Answer, Answer.Length + 100000); }
                Answer[ansPos++] = BigIntFromArrBytes(temp);
            }
            Array.Resize(ref Answer, ansPos);
            return Answer;
        }



        private static byte[] BigIntToBytes(BigInteger A)
        {
            byte[] Answer = new byte[0];

            while (A != 0)
            {
                Array.Resize(ref Answer, Answer.Length + 1);
                Answer[Answer.Length - 1] = (byte)(A % 256);
                A = A / 256;
            }
            return Answer;
        }


        private static byte[] BigIntToBytes(BigInteger[] Text, BigInteger N)
        {
            byte[] N_Arr = BigIntToBytes(N);
            byte[] Answer = new byte[Text.Length * N_Arr.Length];
            byte[] temp;
            int tempPos;
            int pos = 0;
            for (int i = 0; i < Text.Length; i++)
            {
                tempPos = N_Arr.Length - 1;
                temp = new byte[N_Arr.Length];
                while (Text[i] != 0)
                {
                    temp[tempPos--] = (byte)(Text[i] % 256);
                    Text[i] /= 256;
                }
                concat(ref Answer, temp, ref pos);
            }
            return Answer;
        }


        private static void concat(ref byte[] A, byte[] B, ref int pos)
        {
            if (pos + B.Length > A.Length) { Array.Resize(ref A, A.Length + 100000); }
            for (int i = 0; i < B.Length; i++) { A[i + pos] = B[i]; }
            pos += B.Length;
            return;
        }

        private static byte[] reverse(byte[] A)
        {
            byte[] Answer = new byte[A.Length];
            for (int i = 0; i < A.Length; i++) { Answer[i] = A[A.Length - i - 1]; }
            return Answer;
        }


    }
}
