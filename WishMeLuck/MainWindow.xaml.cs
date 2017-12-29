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
            
        }

        public void HttpRequest(string userName, string password)
        {
            Task.Run(() =>
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                string postData = "un=" + userName + "&pw=" + password;
                string method = "POST";
                string phpFileName = "login.php";

                string jsonStr = WebReq.WebRq(postData, method, phpFileName);

                LogIn logInUserObject = JsonConvert.DeserializeObject<LogIn>(jsonStr);

                if (logInUserObject.success == 1)
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
