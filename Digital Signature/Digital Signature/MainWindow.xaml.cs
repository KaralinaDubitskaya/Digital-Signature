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
        const int DIGITS_DIFFERENCE = 20; // p ~ q
        BigInteger MIN = new BigInteger(10000000000) * new BigInteger(10000000000); // min value of p * q
        #endregion

        #region Properties
        public string FileName { get; set; }
        public byte[] Message { get; set; }
        public string Signature { get; set; }
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        #region btnFile_Click. Open OpenFileDialog, set file location and load file in the array of bytes.
        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.ShowDialog();

            FileName = dlg.FileName;
            FileName = FileName;
            

            if (FileName == "")
                System.Windows.MessageBox.Show("Error: couldn't load file.");
            else
            {
                //cut file name
                if (FileName.Length > (int)lblFileName.Width - 20)
                    lblFileName.Content = "..." + FileName.Substring(FileName.Length - (int) lblFileName.Width - 20);
                else
                    lblFileName.Content = FileName;

                //load file as an array of bytes
                byte[] _Message = new byte[File.ReadAllBytes(FileName).Length];
                _Message = File.ReadAllBytes(FileName);

                this.Message = new byte[_Message.Length];
                this.Message = _Message;
                                
                //tbSource.Text = BitConverter.ToString(arr);
                tbMessage.Text = string.Join(" ", _Message.Select(b => b.ToString()));
            }
        }
        #endregion 
        #region private void btnSignFile_Click(object sender, RoutedEventArgs e). Load digital signature from file.
        private void btnSignFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.ShowDialog();

            Signature = dlg.FileName;


            if (Signature == "")
                System.Windows.MessageBox.Show("Error: couldn't load file.");
            else
            {
                //cut file name
                if (Signature.Length > (int)lblSignFileName.Width - 20)
                    lblSignFileName.Content = "..." + Signature.Substring(Signature.Length - (int)lblSignFileName.Width - 20);
                else
                    lblSignFileName.Content = Signature;

                //load file as a string
                Signature = File.ReadAllText(Signature);

                tbDigitalSignature.Text = Signature;
            }
        }
        #endregion

        #region btnSign_Click. A signing algorithm that, given a message and a private key, produces a signature.
        private void btnSign_Click(object sender, RoutedEventArgs e)
        {
            if ((FileName == null) || (FileName == ""))
            {
                MessageBox.Show("Error: choose file");
            }
            else if ((Message == null) || (Message.Length == 0))
            {
                MessageBox.Show("Error: empty file");
                return;
            }
            else
            {
                BigInteger p, q, eps, d; // secret key
                if (IsSecretKeyValid(out p, out q, out eps, out d))
                {
                    // modulo
                    BigInteger r;
                    r = p * q;
                    tbR.Text = r.ToString();

                    RSA RSA = new RSA(p, q, eps, d, r);
                    SHA_1 SHA1 = new SHA_1();

                    byte[] SHAHash = new byte[20];
                    SHAHash = SHA1.GetHash(Message).Value;

                    // BigInteger values are represented in little endian
                    Array.Reverse(SHAHash);

                    // sure that a positive value is not incorrectly instantiated as a negative value 
                    // by adding a byte whose value is zero to the end of the array
                    byte[] temp = new byte[SHAHash.Length];
                    Array.Copy(SHAHash, temp, SHAHash.Length);
                    SHAHash = new byte[temp.Length + 1];
                    Array.Copy(temp, SHAHash, temp.Length);

                    // convert hash array to BigInteger
                    BigInteger BI_Hash = new BigInteger(SHAHash);

                    if (BI_Hash > r)
                    {
                        MessageBox.Show("Error: Hash is greater than R.");
                        return;
                    }
                    
                    // decimal representation of hash
                    tbHashDecimal.Text = BI_Hash.ToString();

                    //hexadecimal representation of hash
                    string hexHash = BI_Hash.ToString("X");
                    if (hexHash[0] == '0')
                    {
                        hexHash = hexHash.Substring(1);
                    }
                    tbHashHex.Text = hexHash;

                    // encrypt hash
                    string signature = RSA.EncryptHash(BI_Hash).ToString();
                    
                    tbD.Text = RSA.D.ToString();
                    tbE.Text = RSA.E.ToString();

                    tbDigitalSignature.Text = signature;

                    // save signature to file 
                    File.WriteAllText(FileName.Substring(0, FileName.IndexOf('.')) + "_Sign.txt", signature);
                }
            }
        }
        #endregion
        private void btnVerify_Click(object sender, RoutedEventArgs e)
        {
            if ((FileName == null) || (FileName == ""))
            {
                MessageBox.Show("Error: Choose file.");
            } 
            else if ((Signature == null) || (Signature == ""))
            {
                MessageBox.Show("Error: Choose file with signature.");
            }
            else
            {
                BigInteger eps, r; // public key
                if (IsPublicKeyValid(out eps, out r))
                {
                    RSA RSA = new RSA(eps, r);
                    SHA_1 SHA1 = new SHA_1();

                    byte[] realHash = new byte[20];
                    realHash = SHA1.GetHash(Message).Value;

                    // BigInteger values are represented in little endian
                    Array.Reverse(realHash);

                    // sure that a positive value is not incorrectly instantiated as a negative value 
                    // by adding a byte whose value is zero to the end of the array
                    byte[] temp = new byte[realHash.Length];
                    Array.Copy(realHash, temp, realHash.Length);
                    realHash = new byte[temp.Length + 1];
                    Array.Copy(temp, realHash, temp.Length);

                    // decimal representation of hash
                    tbHashDecimal.Text = new BigInteger(realHash).ToString();

                    //hexadecimal representation of hash
                    string hexHash = new BigInteger(realHash).ToString("X");
                    if (hexHash[0] == '0')
                    {
                        hexHash = hexHash.Substring(1);
                    }
                    tbHashHex.Text = hexHash;

                    // convert signature to BigInteger
                    BigInteger BI_Hash = BigInteger.Parse(Signature);

                    //encrypt hash from file using public key
                    byte[] checkedHash = new byte[20];
                    RSA Rsa = new RSA(eps, r);
                    checkedHash = Rsa.DecryptHash(BI_Hash).ToByteArray();

                    if (new BigInteger(realHash) > RSA.R)
                    {
                        MessageBox.Show("Error: Hash is greater than R.");
                        return;
                    }

                    // output encrypted hash from file
                    tbCheckedHash.Text = new BigInteger(checkedHash).ToString();

                    if (checkedHash.SequenceEqual(realHash))
                    {
                        MessageBox.Show("Digital signature is correct. File is authentic.");
                    }
                    else
                    {
                        MessageBox.Show("Digital signature isn't correct!");
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
        #region private bool IsSecretKeyValid(out BigInteger p, out BigInteger q, out BigInteger eps). Check input.
        private bool IsSecretKeyValid(out BigInteger p, out BigInteger q, out BigInteger eps, out BigInteger d)
        {
            // initialization
            p = q = eps = d = 0;

            BigInteger.TryParse(tbE.Text, out eps);
            BigInteger.TryParse(tbD.Text, out d);

            if (!BigInteger.TryParse(tbP.Text, out p))
            {
                MessageBox.Show("Error: Invalid value of P.");
            }
            else if (!BigInteger.TryParse(tbQ.Text, out q))
            {
                MessageBox.Show("Error: Invalid value of Q.");
            }
            else if ((!(p.IsProbablyPrime())) || (!(q.IsProbablyPrime())))
            {
                MessageBox.Show("Error: Values of P and Q must be prime.");
            }
            else if (Math.Abs(p.ToString().Length - q.ToString().Length) > DIGITS_DIFFERENCE)
            {
                MessageBox.Show("Error: P and Q must be comparable.");
            }
            else if (p == q)
            {
                MessageBox.Show("Error: P and Q couldn't be the same.");
            }
            else if (eps >= p * q)
            {
                MessageBox.Show("Error: Eps must be less than P * Q.");
            }
            else if (d >= p * q)
            {
                MessageBox.Show("Error: D must be less than P * Q.");
            }
            else if ((eps == 0) && (d == 0))
            {
                MessageBox.Show("Error: Enter value of E or D.");
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
                MessageBox.Show("Error: Invalid value of R.");
            }
            else if (!BigInteger.TryParse(tbE.Text, out eps))
            {
                MessageBox.Show("Error: Invalid value of E.");
            }
            else if (eps >= r)
            {
                MessageBox.Show("Error: E must be less than R.");
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
