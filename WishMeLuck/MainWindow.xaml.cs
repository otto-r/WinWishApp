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
            if (UserInputValidation.ValidCharacters(TextBoxUserName.Text))
            {
                //Register method here
            }
            else
            {
                MessageBox.Show("No special characters allowed.\n- Allowed: A-Z, a-z and 0-9");
            }
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

        private void PasswordBoxRetype_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                PasswordBoxRetype.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(UserInputValidation.PasswordStrengthTest(PasswordBoxRetype.Password)));
            });
        }
    }
}