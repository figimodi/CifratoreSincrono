using System.Security.Cryptography;

namespace CifraturaSincrona
{
    public class DES : CifraturaBase
    {
        public DES(string password, bool cifra) : base(password, cifra)
        {
            var pdb = new PasswordDeriveBytes(password, null);
            byte[] k = pdb.CryptDeriveKey("DES", null, 0, IVPassword);
            var csp = new DESCryptoServiceProvider();

            if (cifra)
                enc = csp.CreateEncryptor(k, IVAlgoritmo);
            else
                enc = csp.CreateDecryptor(k, IVAlgoritmo);
        }
    }
}