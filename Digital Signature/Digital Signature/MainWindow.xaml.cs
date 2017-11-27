using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Numerics;
using System.Threading;
using System.Security.Cryptography;

namespace Digital_Signature
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Consts
        const int DIGITS_DIFFERENCE = 30; // p ~ q
        const int MIN = 1000000; // min value of p and q
        #endregion

        #region Fields
        private string _FileName; // path to file with message
        private byte[] _Message; // original message
        #endregion

        #region Properties
        public string FileName { get; set; }
        public byte[] Message { get; set; }
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        #region btnFile_Click. Open OpenFileDialog, set file location and load file in the array of bytes
        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.ShowDialog();

            _FileName = dlg.FileName;
            

            if (_FileName == "")
                System.Windows.MessageBox.Show("Error: couldn't load file.");
            else
            {
                //cut file name
                if (_FileName.Length > (int)lblFileName.Width - 20)
                    lblFileName.Content = "..." + _FileName.Substring(_FileName.Length - (int) lblFileName.Width - 20);
                else
                    lblFileName.Content = _FileName;

                //load file as an array of bytes
                byte[] _Message = new byte[File.ReadAllBytes(_FileName).Length];
                _Message = File.ReadAllBytes(_FileName);

                this.Message = new byte[_Message.Length];
                this.Message = _Message;
                                
                //tbSource.Text = BitConverter.ToString(arr);
                tbMessage.Text = string.Join(" ", _Message.Select(b => b.ToString()));
            }
        }
        #endregion 
        #region btnSign_Click. A signing algorithm that, given a message and a private key, produces a signature
        private void btnSign_Click(object sender, RoutedEventArgs e)
        {
            if (_FileName == "")
            {
                MessageBox.Show("Error: choose file");
            }
            else
            {
                BigInteger p, q, eps; // secret key
                if (IsSecretKeyValid(out p, out q, out eps))
                {
                    BigInteger r;
                    r = p * q;
                    tbR.Text = r.ToString();
                    Digital_Signature.RSA RSA = new Digital_Signature.RSA(p, q, eps, r);
                    
                    Signature Signature = new Signature();

                    //byte[] signature = new byte[20];
                    //signature = Signature.Create(Message, RSA);

                    SHA_1 SHA1 = new SHA_1();

                    byte[] SHAHash = new byte[20];
                    SHAHash = SHA1.GetHash(Message).Value;

                    Array.Reverse(SHAHash);

                    byte[] temp = new byte[SHAHash.Length];
                    Array.Copy(SHAHash, temp, SHAHash.Length);
                    SHAHash = new byte[temp.Length + 1];
                    Array.Copy(temp, SHAHash, temp.Length);

                    BigInteger BI_Hash = new BigInteger(SHAHash);


                    tbHashDecimal.Text = BitConverter.ToString(SHAHash);

                    string signature = RSA.EncryptHash(BI_Hash).ToString();

                    tbDigitalSignature.Text = signature;

                    File.WriteAllText(@"C:\Users\Ирина\Desktop\Foo.txt", signature);
                }
            }
        }
        #endregion
        private void btnVerify_Click(object sender, RoutedEventArgs e)
        {
            if (_FileName == "")
            {
                MessageBox.Show("Error: choose file");
            }
            else
            {
                BigInteger eps, r; // public key
                if (IsPublicKeyValid(out eps, out r))
                {
                    Digital_Signature.RSA RSA = new Digital_Signature.RSA(eps, r);

                    Signature Signature = new Signature();
                    SHA_1 SHA1 = new SHA_1();
                    byte[] realHash = new byte[20];
                    realHash = SHA1.GetHash(Message).Value;

                    Array.Reverse(realHash);

                    byte[] temp = new byte[realHash.Length];
                    Array.Copy(realHash, temp, realHash.Length);
                    realHash = new byte[temp.Length + 1];
                    Array.Copy(temp, realHash, temp.Length);


                    string cryptedHash = File.ReadAllText(@"C:\Users\Ирина\Desktop\Foo.txt");
                    //cryptedHash = File.ReadAllBytes(@"C:\Users\Ирина\Desktop\Foo.txt");

                    /*RSA Rsa = new RSA(eps, r);
                    Array.Reverse(cryptedHash);
           
                    byte[] temp = new byte[cryptedHash.Length];
                    Array.Copy(cryptedHash, temp, cryptedHash.Length);
                    cryptedHash = new byte[temp.Length + 1];
                    Array.Copy(temp, cryptedHash, temp.Length);

                    BigInteger BI_Hash = new BigInteger(cryptedHash);*/

                    BigInteger BI_Hash = BigInteger.Parse(cryptedHash);

                    byte[] checkedHash = new byte[20];
                    RSA Rsa = new RSA(eps, r);
                    checkedHash = Rsa.DecryptHash(BI_Hash).ToByteArray();

                    //if (Equals(checkedHash, realHash))
                    if (checkedHash.SequenceEqual(realHash))
                    {
                        MessageBox.Show("urraaaa");
                    }
                    else
                    {
                        MessageBox.Show("plak");
                    }

                }
            }
        }
        #region
        #endregion

        #region NumberValidationTextBox. User can input only digits
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion

        #endregion

        #region Methods
        #region private bool IsSecretKeyValid(out BigInteger p, out BigInteger q, out BigInteger eps). Check input
        private bool IsSecretKeyValid(out BigInteger p, out BigInteger q, out BigInteger eps)
        {

            p = q = eps = 0;

            if (!BigInteger.TryParse(tbP.Text, out p))
            {
                MessageBox.Show("Error: Invalid value of p.");
            }
            else if (!BigInteger.TryParse(tbQ.Text, out q))
            {
                MessageBox.Show("Error: Invalid value of q.");
            }
            else if (!BigInteger.TryParse(tbE.Text, out eps))
            {
                MessageBox.Show("Error: Invalid value of e.");
            }
            else if ((!(p.IsProbablyPrime())) || (!(q.IsProbablyPrime())))// || (!(eps.IsProbablyPrime())))
            {
                MessageBox.Show("Error: Values of p and q and eps must be prime.");
            }
            else if (Math.Abs(p.ToString().Length - q.ToString().Length) > DIGITS_DIFFERENCE)
            {
                MessageBox.Show("Error: P and Q must be comparable.");
            }
            else if (p == q)
            {
                MessageBox.Show("Error: P and Q couldn't be the same.");
            }
            else if ((p < MIN) || (q < MIN))
            {
                MessageBox.Show("Error: Value of p and q must be greater then 1000000");
            }
            else if (eps >= p * q)
            {
                MessageBox.Show("Error: Eps must be less than P * Q.");
            }
            else
            {
                return true;
            }

            return false;
        }
        #endregion
        #region private bool IsPublicKeyValid(out BigInteger eps, out BigInteger r). Check input
        private bool IsPublicKeyValid(out BigInteger eps, out BigInteger r)
        {

            r = eps = 0;

            if (!BigInteger.TryParse(tbR.Text, out r))
            {
                MessageBox.Show("Error: Invalid value of r.");
            }
            else if (!BigInteger.TryParse(tbE.Text, out eps))
            {
                MessageBox.Show("Error: Invalid value of e.");
            }
            else if (eps >= r)
            {
                MessageBox.Show("Error: Eps must be less than R.");
            }
            else if (r < 1000000000000) 
            {
                MessageBox.Show("Error: Value of r must be greater then 1000000000000");
            }
            else
            {
                return true;
            }

            return false;
        }
        #endregion

        #endregion


    }
}
