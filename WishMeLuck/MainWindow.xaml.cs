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

        private void ButtonShowRegister_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                PasswordBoxRetype.Margin = new Thickness(10, 68, 0, 0);
                ButtonLogIn.Margin = new Thickness(10, 93, 0, 0);
                ButtonRegister.Margin = new Thickness(68, 93, 0, 0);
                ButtonShowRegister.Margin = new Thickness(68, 93, 0, 0);
                PasswordBoxRetype.Visibility = Visibility.Visible;
                Application.Current.MainWindow.Height = 160;
                ButtonShowRegister.Visibility = Visibility.Hidden;
                ButtonRegister.Visibility = Visibility.Visible;
                PasswordBoxRetype.Focus();
            });
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

        public void PasswordSecurityColoring()
        {

        }

        private void PasswordBoxRetype_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void PasswordBoxRetype_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (PasswordBoxRetype.Password.Length == 1)
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFCDCD"));
                }
                else if (PasswordBoxRetype.Password.Length == 2)
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFD7CD"));
                }
                else if (PasswordBoxRetype.Password.Length == 3)
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFE1CD"));
                }
                else if (PasswordBoxRetype.Password.Length == 4)
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFEBCD"));
                }
                else if (PasswordBoxRetype.Password.Length == 5)
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFF5CD"));
                }
                else if (PasswordBoxRetype.Password.Length == 6)
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFCD"));
                }
                else if (PasswordBoxRetype.Password.Length == 7)
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF5FFCD"));
                }
                else if (PasswordBoxRetype.Password.Length == 8)
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFEBFFCD"));
                }
                else if (PasswordBoxRetype.Password.Length == 9)
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE1FFCD"));
                }
                else if (PasswordBoxRetype.Password.Length == 10)
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFD7FFCD"));
                }
                else if (PasswordBoxRetype.Password.Length > 10)
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCDFFCD"));
                }
                else
                {
                    PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFFFF"));
                }
            });
        }
    }
}