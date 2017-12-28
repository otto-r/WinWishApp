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
using Newtonsoft.Json;


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
            //Dispatcher.Invoke(() =>
            //{
            //    MainLogIn mainLogInWindow = new MainLogIn(logInUserObject);
            //    mainLogInWindow.Show();
            //    this.Close();
            //});
        }

        public void HttpRequest(string userName, string password)
        {
            Task.Run(() =>
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
                string jsonStr = sr.ReadToEnd();

                LogIn logInUserObject = JsonConvert.DeserializeObject<LogIn>(jsonStr);

                //MessageBox.Show(jsonStr);
                //Clipboard.SetText(jsonStr);
                //MessageBox.Show($"Success or not = {logInUserObject.success}\nMsg: {logInUserObject.msg}");
                //Dispatcher.Invoke(() => { TextBoxUserName.Text = jsonStr; });

                int testInt = logInUserObject.success;
                sr.Close();
                stream.Close();

                if (testInt == 1)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MainLogIn mainLogInWindow = new MainLogIn(logInUserObject);
                        mainLogInWindow.Show();
                        this.Close();
                    });
                }
            });
        }
    }
}
