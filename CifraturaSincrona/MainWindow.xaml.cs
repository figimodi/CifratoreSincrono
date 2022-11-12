using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CifraturaSincrona
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}
		private void Esplora(object sender, RoutedEventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			fileDialog.RestoreDirectory = true;
			if (fileDialog.ShowDialog() == true)
			{
				txtPlainFile.Text = fileDialog.FileName;
				string[] appo = fileDialog.FileName.Split('.');
				txtCipherFile.Text = appo[0] + ".cfr";
			}
		}
		private void CifraDecifra(object sender, RoutedEventArgs e)
		{
			string content;
			if (btnCifraDecifra.Content.ToString() == "Decifra") content = "Cifra";
			else content = "Decifra";

			lblHeader1.Content = btnCifraDecifra.Content.ToString() + " un file";
			lblHeader2.Content = btnCifraDecifra.Content.ToString() + " un testo";
			btnCifraDecifra.Content = content;
		}

		#region Eventi TextBox
		private void txtGotFocus(object sender, RoutedEventArgs e)
		{
			((TextBox)sender).Text = "";
			if (((TextBox)sender).Name.ToString() == "txtPlainFile") txtCipherFile.Text = "";
			if (((TextBox)sender).Name.ToString() == "txtPlainText") txtCipherText.Text = "";
		}
		private void TextChanged(object sender, TextChangedEventArgs e)
		{
			string[] appo = txtPlainFile.Text.Split('\\');
			StringBuilder s = new StringBuilder();
			for (int i = 0; i < appo.Length - 1; i++)
			{
				s.Append(appo[i] + "\\");
			}
			txtCipherFile.Text = s.ToString();
		}
		#endregion

		#region Cifra File
		private void Invio(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return) CifraFile();
		}

		private void Avvia(object sender, RoutedEventArgs e)
		{
			CifraFile();
		}

		private void CifraFile()
		{
			try
			{
				string password;
				bool mod;
				if (File.Exists(txtCipherFile.Text))
					throw new Exception("Esiste già un file con lo stesso nome \"" + txtCipherFile.Text + "\"");
				if ((txtPassword.Visibility == Visibility.Hidden && pswPassword.Password.Trim() == "") || (pswPassword.Visibility == Visibility.Hidden && txtPassword.Text.Trim() == ""))
					throw new Exception("La password non può essere vuota");

				if (txtPassword.Visibility == Visibility.Hidden)
					password = pswPassword.Password.Trim();
				else
					password = txtPassword.Text.Trim();
				if (btnCifraDecifra.Content.ToString() == "Cifra")
					mod = false;
				else
					mod = true;

				switch (cmbCrittografia.SelectedIndex)
				{
					case 0:
						var des = new DES(password, mod);
						des.CifraFileStream(txtPlainFile.Text.Trim(), txtCipherFile.Text.Trim());
						break;
					case 1:
						var tripledes = new TripleDES(password, mod);
						tripledes.CifraFileStream(txtPlainFile.Text.Trim(), txtCipherFile.Text.Trim());
						break;
					case 2:
						var aes = new AES(password, mod);
						aes.CifraFileStream(txtPlainFile.Text.Trim(), txtCipherFile.Text.Trim());
						break;
				}

				if (btnCifraDecifra.Content.ToString() == "Cifra")
					lblError1.Content = "Il file è stato decifrato correttamente";
				else
					lblError1.Content = "Il file è stato cifrato correttamente";
			}
			catch (Exception ex)
			{
				lblError1.Content = ex.Message.ToString();
			}
		}
		#endregion

		#region Cifra Memory
		private void Invio2(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				CifraMemory();
			}
		}
		private void Avvia2(object sender, RoutedEventArgs e)
		{
			CifraMemory();
		}

		private void CifraMemory()
		{
			try
			{
				string password;
				bool mod;
				if ((txtPassword2.Visibility == Visibility.Hidden && pswPassword2.Password.Trim() == "") || (pswPassword2.Visibility == Visibility.Hidden && txtPassword2.Text.Trim() == ""))
					throw new Exception("La password non può essere vuota");

				if (txtPassword2.Visibility == Visibility.Hidden)
					password = pswPassword2.Password.Trim();
				else
					password = txtPassword2.Text.Trim();
				if (btnCifraDecifra.Content.ToString() == "Cifra")
					mod = false;
				else
					mod = true;

				switch (cmbCrittografia.SelectedIndex)
				{
					case 0:
						var des = new DES(password, mod);
						txtCipherText.Text = des.CifraMemoryStream(txtPlainText.Text);
						break;
					case 1:
						var tripledes = new TripleDES(password, mod);
						txtCipherText.Text = tripledes.CifraMemoryStream(txtPlainText.Text);
						break;
					case 2:
						var aes = new AES(password, mod);
						txtCipherText.Text = aes.CifraMemoryStream(txtPlainText.Text);
						break;
				}
			}
			catch (Exception ex)
			{
				lblError2.Content = ex.Message.ToString();
			}
		}
		#endregion

		#region Bottone Occhio
		private void ShowPassword(object sender, RoutedEventArgs e)
		{
			if (txtPassword.Visibility == Visibility.Hidden)
			{
				txtPassword.Text = pswPassword.Password;
				txtPassword.Visibility = Visibility.Visible;
				pswPassword.Visibility = Visibility.Hidden;
				txtPassword.Focus();
			}
			else
			{
				pswPassword.Password = txtPassword.Text;
				pswPassword.Visibility = Visibility.Visible;
				txtPassword.Visibility = Visibility.Hidden;
				pswPassword.Focus();
			}
		}
		private void ShowPassword2(object sender, RoutedEventArgs e)
		{
			if (txtPassword2.Visibility == Visibility.Hidden)
			{
				txtPassword2.Text = pswPassword2.Password;
				txtPassword2.Visibility = Visibility.Visible;
				pswPassword2.Visibility = Visibility.Hidden;
				txtPassword2.Focus();
			}
			else
			{
				pswPassword2.Password = txtPassword2.Text;
				pswPassword2.Visibility = Visibility.Visible;
				txtPassword2.Visibility = Visibility.Hidden;
				pswPassword2.Focus();
			}
		}
		#endregion
	}
}
