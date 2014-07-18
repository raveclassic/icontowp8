using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Adapters.Encryption
{
    public interface IEncryptedDataAdapter
    {
        byte[] Encrypt(byte[] value, byte[] optionalEntropy = null);
        byte[] Decrypt(byte[] value, byte[] optionalEntropy = null);
    }
}
