using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CifraturaSincrona
{
	public class CifraturaBase
	{
		protected static readonly byte[] IVPassword = new byte[8] { 10, 02, 20, 16, 10, 48, 00, 00 };
		protected static readonly byte[] IVAlgoritmo = new byte[8] { 10, 02, 20, 16, 10, 56, 00, 00 };

		protected string password;
		protected bool cifra;
		protected ICryptoTransform enc;

		public CifraturaBase(string password, bool cifra)
		{
			this.password = password;
			this.cifra = cifra;
		}

		public void CifraFileStream(string plainFilePath, string cipherFilePath)
		{
			if (!File.Exists(plainFilePath))
				throw new Exception("Il file da cifrare non esiste");
			using (var istream = new FileStream(plainFilePath, FileMode.Open))
			using (var ostream = new FileStream(cipherFilePath, FileMode.Create))
			{
				using (var cstream = new CryptoStream(ostream, enc, CryptoStreamMode.Write)) { istream.CopyTo(cstream); }
			}
		}

		public string CifraMemoryStream(string plainText)
		{
			var istream = new MemoryStream(Convert.FromBase64String(plainText));
			using (var ostream = new MemoryStream())
			{
				using (var cstream = new CryptoStream(ostream, enc, CryptoStreamMode.Write)) { istream.CopyTo(cstream); }

				return Convert.ToBase64String(ostream.ToArray());
			}
		}
	}
}
