using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WishMeLuck
{
    /// <summary>
    /// Interaction logic for Share.xaml
    /// </summary>
    public partial class ShareWindow : Window
    {
        LogInObject logInObject;
        public ShareWindow(LogInObject logInObjectIn, ObjectOfWishLists objectOfWishLists, string selectedList)
        {
            logInObject = logInObjectIn;
            InitializeComponent();
            Dispatcher.Invoke(() =>
            {
                //for testing

                FillComboBox(objectOfWishLists, selectedList);
                FillFriendList();
                Task.Run(() =>
                {
                });
            });
        }

        private void FillComboBox(ObjectOfWishLists objectOfWishLists, string selectedList)
        {
            if (objectOfWishLists.wishLists != null)
            {
                foreach (var wishList in objectOfWishLists.wishLists)
                {
                    ComboBoxSelectWishList.Items.Add(wishList.wishListName);
                    if (selectedList != "")
                    {
                        ComboBoxSelectWishList.SelectedItem = selectedList;
                    }
                    else
                    {
                        ComboBoxSelectWishList.SelectedIndex = 0;
                    }
                }
            }
        }
        private void ButtonShare_Click(object sender, RoutedEventArgs e)
        {
            FriendListRequest friendListRequest = new FriendListRequest();
            friendListRequest.un = logInObject.user.username;
            friendListRequest.wln = ComboBoxSelectWishList.SelectedItem.ToString();
            friendListRequest.sun = new List<string>();
            foreach (var friend in ListBoxFriends.SelectedItems)
            {
                friendListRequest.sun.Add(friend.ToString());
            }

            string output = JsonConvert.SerializeObject(friendListRequest);

            string postData = output;
            string method = "POST";
            string phpFileName = "shareWishList.php";

            string jsonStr = WebReq.WebRq(postData, method, phpFileName, "json");

            var shareObject = JsonConvert.DeserializeObject<ShareObject>(jsonStr);

            if (shareObject.friends == null)
            {
                this.Close();
            }
            else
            {
                string list = "";
                foreach (var friend in shareObject.friends)
                {
                    list += " " + friend.shareToUser;
                }
                MessageBox.Show(shareObject.msg + "\n" + list);
            }

        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ListBoxFriends.Items.Add(TextBoxUsername.Text);
                TextBoxUsername.Clear();
            });
        }

        private void FillFriendList()
        {
            string postData = "un=" + logInObject.user.username;
            string method = "POST";
            string phpFileName = "getFriendList.php";

            string jsonStr = WebReq.WebRq(postData, method, phpFileName, "");

            var sharedListObject = JsonConvert.DeserializeObject<SharedListObject>(jsonStr);

            if (sharedListObject.friends == null)
            {
                //this.Close();
                Dispatcher.Invoke(() =>
                {
                    ListBoxFriends.Items.Add("null");
                });
            }
            else
            {
                foreach (var friend in sharedListObject.friends)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ListBoxFriends.Items.Add(friend);
                    });
                }
            }
        }

    }
}
