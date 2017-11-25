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

namespace Digital_Signature
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        private string _FileName; // path to file with message
        private byte[] _Message; // original message
        #endregion

        #region Properties
        // public string FileName { get; set; } 
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
                _Message = File.ReadAllBytes(_FileName);
                //tbSource.Text = BitConverter.ToString(arr);
                tbMessage.Text = string.Join(" ", _Message.Select(b => b.ToString()));
            }
        }
        #endregion 
        #region btnSign_Click. A signing algorithm that, given a message and a private key, produces a signature
        private void btnSign_Click(object sender, RoutedEventArgs e)
        {
            Digital_Signature.RSA RSA = new Digital_Signature.RSA();
            BigInteger p, q, n, eps; // secret and public keys

            if (!BigInteger.TryParse(tbP.Text, out p))
            {
                MessageBox.Show("Error: Invalid value of p.");
            }
            else if (!BigInteger.TryParse(tbQ.Text, out p))
            {
                MessageBox.Show("Error: Invalid value of q.");
            }
            else if (!BigInteger.TryParse(tbP.Text, out eps))
            {
                MessageBox.Show("Error: Invalid value of e.");
            }
            else if (!(p.))
        }
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
        #region 
        
    #endregion

    #region

    #endregion

    #endregion
}
}
