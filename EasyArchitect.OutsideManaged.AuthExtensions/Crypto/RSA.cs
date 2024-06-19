using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyArchitect.OutsideManaged.AuthExtensions.Crypto
{
    /// <summary>
    /// RSACrypto 加密方法
    /// </summary>
    public class RSA
    {
        private static RSACryptoServiceProvider _rsa;

        private static string _privateKey = string.Empty;

        private static string _publicKey = string.Empty;

        private static void Initialize()
        {
            _rsa = new RSACryptoServiceProvider();
            _privateKey = "<RSAKeyValue><Modulus>3VUwkkRtc5g5SVsYFKWjV83RwATF+TOHSFN2fxCwcBlzQGc7T0t0N/9tyCb6M/SNqeBl/eE42yWkVZ1NFpC9d1nJg40ivS8LMIQHRg1VmAmwbkkfo/aj952cE8KeHgZumg9iN6z24Gi8ybPdWnDppLOw1Tmi237YDbrN4cq/q48=</Modulus><Exponent>AQAB</Exponent><P>6bZeCkZES9ZJQSoTw118CnJEr0sd/jLybzdA02GPn7etnfjxcdd+rE4Rxjc2oJrOjdKgACllFLRHpsTnDTC2yw==</P><Q>8nCZOoNt9DDg6fXTiWoIUu/grOSx90vJf9mi39aYa5uaKjy02aewONlwZhQ2p6FBFokUQMuHcd3pWRNyFsOBzQ==</Q><DP>fDfq2ckpKam2e8UyhecdM6wyZ30kbuSDSKt0cCVtofWNeOZE5j4kXM6N0e2swkYlvOmTEyLtT8jWQIRtTexzaw==</DP><DQ>VVmOmCrs6qrKg5MnhZjulUQdtMBOZuEnbvsPe/3wavG8tGHqyTVftKPYDhfPpfP/Fg/sMWN1q4CUReeyopDxgQ==</DQ><InverseQ>SzLpPHp7L7gNnNGzBm2LbTnxeZJkFmkCzT84RSFAZBvsHB6whOBTnrPDPsavOfYjaPO94vsl3G1FAV+PMPOeCA==</InverseQ><D>LMPZIgwy8uk71OlWsn5Zzh1zqdmNTPyuBOPUmJiAhvuuwrzeUwhYxiakRpITksSIjm4zUqjlvfUcCrDY8ZJurz+sy6CqWbrD3qDGkvRxB+uWnctGKmPc6vg/cIX8wkZ6XSbGDdVfxwhs/d2elw/eI8LJHfUOAMI4Jw+tL2lExsk=</D></RSAKeyValue>";
            _publicKey = "<RSAKeyValue><Modulus>3VUwkkRtc5g5SVsYFKWjV83RwATF+TOHSFN2fxCwcBlzQGc7T0t0N/9tyCb6M/SNqeBl/eE42yWkVZ1NFpC9d1nJg40ivS8LMIQHRg1VmAmwbkkfo/aj952cE8KeHgZumg9iN6z24Gi8ybPdWnDppLOw1Tmi237YDbrN4cq/q48=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        }

        private static void Checked2Initial()
        {
            if (_privateKey == string.Empty || _publicKey == string.Empty)
            {
                Initialize();
            }
        }

        public static string EncryptString(string original)
        {
            Checked2Initial();
            _rsa.FromXmlString(_privateKey);
            byte[] inArray = _rsa.Encrypt(Encoding.ASCII.GetBytes(original), false);
            return Convert.ToBase64String(inArray);
        }

        public static string DecryptString(string encryptString)
        {
            Checked2Initial();
            _rsa.FromXmlString(_privateKey);
            byte[] bytes = _rsa.Decrypt(Convert.FromBase64String(encryptString), false);
            return Encoding.Default.GetString(bytes);
        }
    }
}
