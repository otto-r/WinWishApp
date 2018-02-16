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
    public partial class ShareWindow : Window
    {
        LogInObject logInObject;
        ObjectOfWishLists objectOfWishLists;

        public ShareWindow(LogInObject logInObjectIn, ObjectOfWishLists objectOfWishListsIn, string selectedList)
        {
            logInObject = logInObjectIn;
            objectOfWishLists = objectOfWishListsIn;

            InitializeComponent();
            Dispatcher.Invoke(() =>
            {
                //for testing

                FillComboBox(objectOfWishListsIn, selectedList);
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

        private void ComboBoxSelectWishList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxFriends.Items.Clear();
            string selectedItem = ComboBoxSelectWishList.SelectedItem.ToString();

            List<string> comboBoxFriendsAdd = new List<string>();
            List<string> toAddList = new List<string>();
            foreach (var list in objectOfWishLists.wishLists)
            {
                if (list.wishListName != selectedItem)
                {
                    foreach (var friend in list.friendList)
                    {
                        if (!comboBoxFriendsAdd.Contains(friend))
                        {
                            comboBoxFriendsAdd.Add(friend);
                        }
                    }
                }
                else
                {
                    foreach (var friend in list.friendList)
                    {
                        toAddList.Add(friend);
                    }
                }
            }

            foreach (var friend in comboBoxFriendsAdd)
            {
                foreach (var friend2 in toAddList)
                {
                    if (friend2.Equals(friend))
                    {
                        ListBoxFriends.Items.Add(friend);
                    }
                }
            }
        }
    }
}