using Newtonsoft.Json;
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
using System.Windows.Shapes;

namespace WishMeLuck
{
    /// <summary>
    /// Interaction logic for MainLogIn.xaml
    /// </summary>
    public partial class MainLogIn : Window
    {
        public MainLogIn(LogIn logInUserObject)
        {
            InitializeComponent();
            Dispatcher.Invoke(() =>
            {
                UserName.Content = logInUserObject.user.username;
            });
            GetListofLists(logInUserObject.user);
        }

        private void GetListofLists(User user)
        {
            string postData = "un=" + user.username;
            string method = "POST";
            string phpFileName = "getLists.php";

            string jsonStr = WebReq.WebRq(postData, method, phpFileName);

            ListOfLists ListOfLists = JsonConvert.DeserializeObject<ListOfLists>(jsonStr);

            Dispatcher.Invoke(() =>
            {
                foreach (var item in ListOfLists.WishLists)
                {
                    WishListOfLists.Items.Add(item);
                }
            });
        }
    }
}
