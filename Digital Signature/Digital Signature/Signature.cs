using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Digital_Signature
{
    public class Signature
    {

        #region Constructor
        public Signature()
        {

        }
        #endregion

        #region Methods
        #region public byte[] Create(byte[] text, RSA RSA). Encrypt hash using RSA algorithm.
        public byte[] Create(byte[] text, RSA RSA)
        {
            SHA_1 SHA1 = new SHA_1();

            byte[] SHAHash = new byte[20];
            SHAHash = SHA1.GetHash(text).Value;

            BigInteger BI_Hash = new BigInteger(SHAHash);

            

            byte[] signature = RSA.EncryptHash(BI_Hash).ToByteArray();

            return signature;
        }
        #endregion
        #endregion
    }
}
