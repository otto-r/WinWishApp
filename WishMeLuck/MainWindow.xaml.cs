using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;


namespace WishMeLuck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool showRegistrationFields = false;
        bool loadingLabelOn = false;
        string userName;
        string password;
        string reTypePassword;
        string eMail;
        string reTypeEMail;
        string errorMessage = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {
            if (showRegistrationFields == true)
            {
                showRegistrationFields = false;
                ButtonLogIn.Content = "Login";
                LabelPassword.SetValue(Grid.RowProperty, 7);        // 7=>1
                PasswordBox.SetValue(Grid.RowProperty, 1);          // 1=>2
                PasswordBoxRetype.SetValue(Grid.RowProperty, 6);    // 6=>3
                ButtonLogIn.SetValue(Grid.RowProperty, 2);          // 2=>7
                ButtonRegister.SetValue(Grid.RowProperty, 2);       // 2=>7
            }
            else
            {
                loadingLabelOn = true;
                Task.Run(() =>
                {
                    LoadingLabelAnimated();
                });
                userName = TextBoxUserName.Text;
                password = PasswordBox.Password;
                HttpRequest(userName, password);
            }
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            userName = TextBoxUserName.Text;
            password = PasswordBox.Password;
            reTypePassword = PasswordBoxRetype.Password;
            eMail = TextBoxEmail.Text;
            reTypeEMail = TextBoxEmailRetype.Text;

            if (showRegistrationFields == false)
            {
                showRegistrationFields = true;
                ShowRegistration();
            }
            else
            {
                if (UserInputValidation.ValidCharacters(userName, false))
                {
                    Task.Run(() =>
                    {
                        string postData = "un=" + userName + "&pw=" + password + "&email=" + eMail;
                        string method = "POST";
                        string phpFileName = "regUser.php";
                        //WebReq.WebRq(postData, method, phpFileName);
                        string jsonStr = "";
                        string error = "";

                        if (eMail == reTypeEMail && password == reTypePassword)
                        {
                            try
                            {
                                jsonStr = WebReq.WebRq(postData, method, phpFileName);
                            }
                            catch (System.Exception err)
                            {
                                error = err.ToString();
                                Task.Run(() =>
                                {
                                    InfoBarAsync("", error);
                                    errorMessage = error;
                                    loadingLabelOn = false;
                                });
                            }

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
                            else if (logInUserObject.success != 1)
                            {
                                Task.Run(() =>
                                {
                                    InfoBarAsync("", logInUserObject.msg);
                                    loadingLabelOn = false;
                                });
                            }
                        }
                        else
                        {
                            if (eMail != reTypeEMail)
                            {
                                InfoBarAsync("", "E-mails do not match.");
                                errorMessage = "E-mails do not match.";
                            }
                            else
                            {
                                InfoBarAsync("","Passwords do not match.");
                                errorMessage = "Passwords do not match.";
                            }
                        }

                    });
                }
            }
        }

        private void ShowRegistration()
        {
            Dispatcher.Invoke(() =>
            {
                ButtonLogIn.Content = "Back";
                LabelPassword.SetValue(Grid.RowProperty, 1);        // 7=>1
                PasswordBox.SetValue(Grid.RowProperty, 2);          // 1=>2
                PasswordBoxRetype.SetValue(Grid.RowProperty, 3);    // 6=>3
                ButtonLogIn.SetValue(Grid.RowProperty, 7);          // 2=>7
                ButtonRegister.SetValue(Grid.RowProperty, 7);       // 2=>7

                //Application.Current.MainWindow.Height = 275;

                LabelEmail.Visibility = Visibility.Visible;
                PasswordBoxRetype.Visibility = Visibility.Visible;
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

                string jsonStr = "";
                string error = "";
                try
                {
                    jsonStr = WebReq.WebRq(postData, method, phpFileName);
                }
                catch (System.Exception e)
                {
                    error = e.ToString();
                    Task.Run(() =>
                    {
                        InfoBarAsync("", error);
                        errorMessage = error;
                        loadingLabelOn = false;
                    });
                }

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
                else if (logInUserObject.success != 1)
                {
                    Task.Run(() =>
                    {
                        InfoBarAsync("", logInUserObject.msg);
                        loadingLabelOn = false;
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

        private async Task InfoBarAsync(string symbol, string message)
        {

            int infoBarHeight = 0;
            Dispatcher.Invoke(() =>
            {
                string shortenedMessage = message.Substring(0, 20) + "...";
                ButtonSeeError.Visibility = Visibility.Visible;
                InfoBarBG.Visibility = Visibility.Visible;
                LabelSymbol.Foreground = Brushes.Green;

                LabelSymbol.Content = symbol;
                LabelText.Content = shortenedMessage;
                LabelSymbol.Visibility = Visibility.Visible;
                LabelText.Visibility = Visibility.Visible;
            });

            async Task PutTaskDelay()
            {
                await Task.Delay(15);
            }


            for (int i = 0; i < 6; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    ButtonSeeError.Height = infoBarHeight;
                    InfoBarBG.Height = infoBarHeight;
                    LabelSymbol.Height = infoBarHeight;
                    LabelText.Height = infoBarHeight;
                });
                infoBarHeight += 5;
                await PutTaskDelay();
            }
            await Task.Delay(3000);
            Dispatcher.Invoke(() =>
            {
                InfoBarBG.Visibility = Visibility.Hidden;
                LabelSymbol.Foreground = Brushes.Green;

                ButtonSeeError.Visibility = Visibility.Hidden;
                LabelSymbol.Content = "";
                LabelText.Content = "";
                LabelSymbol.Visibility = Visibility.Hidden;
                LabelText.Visibility = Visibility.Hidden;
            });
        }

        private void ButtonLogIn_DragEnter(object sender, DragEventArgs e)
        {

        }
        async Task LoadingLabelAnimated()
        {
            while (loadingLabelOn)
            {
                Dispatcher.Invoke(() => { LabelLoading.Visibility = Visibility.Visible; });
                await Dispatcher.Invoke(async () =>
                {
                    await Task.Delay(500);
                    LabelLoading.Content = "Loading";
                    await Task.Delay(500);
                    LabelLoading.Content = "Loading.";
                    await Task.Delay(500);
                    LabelLoading.Content = "Loading..";
                    await Task.Delay(500);
                    LabelLoading.Content = "Loading...";
                });
            }
            Dispatcher.Invoke(() => { LabelLoading.Visibility = Visibility.Hidden; });

            return;
        }

        private void ButtonSeeError_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(errorMessage);
        }
    }
}