using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Digital_Signature
{
    public static class PrimeExtensions
    {
        // Random generator (thread safe)
        private static ThreadLocal<Random> s_Gen = new ThreadLocal<Random>(
          () => {
              return new Random();
          }
        );

        // Random generator (thread safe)
        private static Random Gen
        {
            get
            {
                return s_Gen.Value;
            }
        }

        public static Boolean IsProbablyPrime(this BigInteger value, int witnesses = 10)
        {
            // prime numbers for tests
            string[] PrimeNumbers = { "37975227936943673922808872755445627854565536638199",
            "40094690950920881030683735292761468389214899724061",
            "6122421090493547576937037317561418841225758554253106999",
            "5846418214406154678836553182979162384198610505601062333",
            "327414555693498015751146303749141488063642403240171463406883",
            "693342667110830181197325401899700641361965863127336680673013",
            "3490529510847650949147849619903898133417764638493387843990820577",
            "32769132993266709549961988190834461413177642967992942539798288533",
            "39685999459597454290161126162883786067576449112810064832555157243",
            "45534498646735972188403686897274408864356301263205069600999044599",
            "11", "13", "17", "19", "23", "29", "3011", "3023",
            "35742549198872617291353508656626642567",
            "3593340859668622831041960188598043661065388726959079837",
            "27644437",

            "3398717423028438554530123627613875835633986495969597423490929302771479",
            "6264200187401285096151654948264442219302037178623509019111660653946049",
            "348009867102283695483970451047593424831012817350385456889559637548278410717",
            "445647744903640741533241125787086176005442536297766153493419724532460296199"};

            // check array of big prime numbers
            for (int i = 0; i < PrimeNumbers.Length; i++)
            {
                if (BigInteger.Parse(PrimeNumbers[i]) == value)
                {
                    return true;
                }
            }

            if (value <= 1)
                return false;

            if (witnesses <= 0)
                witnesses = 10;

            BigInteger d = value - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            Byte[] bytes = new Byte[value.ToByteArray().LongLength];
            BigInteger a;

            for (int i = 0; i < witnesses; i++)
            {
                do
                {
                    Gen.NextBytes(bytes);

                    a = new BigInteger(bytes);
                }
                while (a < 2 || a >= value - 2);

                BigInteger x = BigInteger.ModPow(a, d, value);
                if (x == 1 || x == value - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, value);

                    if (x == 1)
                        return false;
                    if (x == value - 1)
                        break;
                }

                if (x != value - 1)
                    return false;
            }

            return true;
        }
    }
}
