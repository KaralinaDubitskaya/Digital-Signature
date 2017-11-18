using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital_Signature
{
    public class SHA_1
    {

        #region Fields 
        //   private int HashBitCount = 62; //number of significant bits in the hash

           private byte[] _Buffer;
           private long _Count; //number of bytes in the text
           private uint[] _StateSHA1; //hash
           private uint[] _ExpandedBuffer;         
        #endregion

        #region Constructor
        public SHA_1()
        {
            _StateSHA1 = new uint[5]; //length of hash = 5 32-bit integers = 160 bit
            _Buffer = new byte[64]; //length of block of the text = 64 bytes = 512 bit
            _ExpandedBuffer = new uint[80]; 
        }
        #endregion

        #region Methods
        #region public GetHash(string text). Return SHA-1 hash for string
        public SHA_1_Hash GetHash(string text)
        {
            SHA_1_Hash result = new SHA_1_Hash();

            byte[] buffer = Encoding.GetEncoding("utf-8"/*1251*/).GetBytes(text);

            Initialize(); 

            Hash
        }
        #endregion

        #region private void Initialize(). Initialize _StateSHA1.
        private void Initialize()
        {
            _Count = 0;

            _StateSHA1[0] = 0x67452301;
            _StateSHA1[1] = 0xefcdab89;
            _StateSHA1[2] = 0x98badcfe;
            _StateSHA1[3] = 0x10325476;
            _StateSHA1[4] = 0xc3d2e1f0;
        }
        #endregion
        #region private unsafe void HashData(byte[] partIn, int ibStart, int cbSize). Return hash of main part of the text
        private unsafe void HashData(byte[] partIn, int ibStart, int cbSize)
        {
            int bufferLen = (int) _Count & 0x3f; //max length of the text 2^64 bits (0x3f = 00111111 = 63)
            int partInLen = cbSize; //number of bytes
            int partInBase = ibStart; //first byte

            _Count += partInLen; //update number of bytes

            fixed (uint* stateSHA1 = _StateSHA1)
            {
                fixed (byte* buffer = _Buffer)
                {
                    fixed (uint* expandedBuffer = _ExpandedBuffer)
                    {
                        if ((bufferLen > 0) && (bufferLen + partInLen >= 64))
                        {

                        }
                    } 
                }
            }
        }
        #endregion
        #endregion

    }
}
