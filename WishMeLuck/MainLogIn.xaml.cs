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
            GetListofLists(logInUserObject.user);
        }

        private void GetListofLists(User user)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "un=" + user.username;
            byte[] data = encoding.GetBytes(postData);

            WebRequest request = WebRequest.Create("http://192.168.10.191/test/webservice/getLists.php");
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
