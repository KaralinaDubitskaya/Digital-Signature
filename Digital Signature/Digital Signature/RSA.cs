using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Digital_Signature
{
    public class RSA
    {

        #region Fields

        private BigInteger _P;
        private BigInteger _Q;
        private BigInteger _N;
        private BigInteger _E;

        #endregion

        #region Properties

        // secret key (p, q)
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }

        // public key (n, p)
        public BigInteger N { get; set; }
        public BigInteger E { get; set; }

        #endregion

        #region Constructor
        public RSA()
        {

        }
        #endregion

    }
}
