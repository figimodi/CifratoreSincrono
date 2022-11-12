using System.Security.Cryptography;

namespace CifraturaSincrona
{
    public class TripleDES : CifraturaBase
    {
        public TripleDES(string password, bool cifra) : base(password, cifra)
        {
            var pdb = new PasswordDeriveBytes(password, null);
            byte[] k = pdb.CryptDeriveKey("3DES", null, 0, IVPassword);
            var csp = new TripleDESCryptoServiceProvider();

            if (cifra)
                enc = csp.CreateEncryptor(k, IVAlgoritmo);
            else
                enc = csp.CreateDecryptor(k, IVAlgoritmo);
        }
    }
}