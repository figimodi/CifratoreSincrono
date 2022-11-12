using System.Security.Cryptography;

namespace CifraturaSincrona
{
    public class AES : CifraturaBase
    {
        public AES(string password, bool cifra) : base(password, cifra)
        {
            var pdb = new Rfc2898DeriveBytes(password, IVPassword);
            byte[] k = pdb.GetBytes(32);       
            byte[] IVAlgoritmoAES = pdb.GetBytes(16); 
            var csp = new AesCryptoServiceProvider();

            if (cifra)
                enc = csp.CreateEncryptor(k, IVAlgoritmoAES);
            else
                enc = csp.CreateDecryptor(k, IVAlgoritmoAES);
        }
    }
}