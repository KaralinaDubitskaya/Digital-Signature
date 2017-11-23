using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital_Signature
{
    public class SHA_1_Hash
    {
        #region Fields and Consts
        private const int BYTE_COUNT = 20;
        private byte[] _Value = null;
        #endregion

        #region Properties
        public byte[] Value { get; set; }
        public string Text
        {
            get
            {
                if (Value != null)
                {
                    string sHash = BitConverter.ToString(Value).Remove('-');
                    return sHash;
                }
                else
                    return String.Empty;
                
            }
        }
        public int ByteCount { get; }
        #endregion

        #region Constructor
        public SHA_1_Hash(): this(null)
        { }

        public SHA_1_Hash(byte[] Value)
        {
            this.Value = Value;
            this.ByteCount = BYTE_COUNT;
        }
        #endregion

        #region Methods
        #region public SHA_1_Hash Clone(). Return copy of the hash.
        public  SHA_1_Hash Clone()
        {
            SHA_1_Hash result = new SHA_1_Hash();

            if (this.Value != null)
            {
                result.Value = new byte[this.Value.Length];
                this.Value.CopyTo(result.Value, 0);
            }

            return result;
        }
        #endregion
        #endregion
    }
}
