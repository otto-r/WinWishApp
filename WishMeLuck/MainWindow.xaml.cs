using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace WishMeLuck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {
            string userName;
            string password;

            userName = TextBoxUserName.Text;
            password = PasswordBox.Password;
            HttpRequest(userName, password);
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
        }

        public void HttpRequest(string userName, string password)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "un=" + userName + "&pw=" + password;
            byte[] data = encoding.GetBytes(postData);

            WebRequest request = WebRequest.Create("http://192.168.10.191/test/webservice/login.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            WebResponse response = request.GetResponse();
            stream = response.GetResponseStream();

            StreamReader sr = new StreamReader(stream);
            MessageBox.Show(sr.ReadToEnd());

            sr.Close();
            stream.Close();
        }
    }
}
