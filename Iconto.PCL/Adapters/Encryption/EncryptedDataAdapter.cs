using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Adapters.Encryption
{
    public class EncryptedDataAdapter : IEncryptedDataAdapter
    {
        public byte[] Encrypt(byte[] value, byte[] optionalEntropy = null)
        {
            return ProtectedData.Protect(value, optionalEntropy);
        }

        public byte[] Decrypt(byte[] value, byte[] optionalEntropy = null)
        {
            return ProtectedData.Unprotect(value, optionalEntropy);
        }
    }
}
