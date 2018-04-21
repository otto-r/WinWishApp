using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            FriendListRequest friendListRequest = new FriendListRequest
            {
                un = logInObject.user.username,
                wln = ComboBoxSelectWishList.SelectedItem.ToString(),
                sun = new List<string>()
            };
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
                    list += " " + friend.msg;
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

            //if (sharedListObject.friends == null)
            //{
            //    //this.Close();
            //    Dispatcher.Invoke(() =>
            //    {
            //        ListBoxFriends.Items.Add("null");
            //    });
            //}
            //else
            //{
            //    foreach (var friend in sharedListObject.friends)
            //    {
            //        Dispatcher.Invoke(() =>
            //        {
            //            ListBoxFriends.Items.Add(friend);
            //        });
            //    }
            //}
        }

        private void ComboBoxSelectWishList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxFriends.Items.Clear();
            string selectedItem = ComboBoxSelectWishList.SelectedItem.ToString();

            List<string> otherLists = new List<string>();
            List<string> sharedWith = new List<string>();
            List<string> fillShareList = new List<string>();


            foreach (var list in objectOfWishLists.wishLists)
            {
                if (list.wishListName != selectedItem)
                {
                    foreach (var friend in list.friendList)
                    {
                        if (!otherLists.Contains(friend) || otherLists != null)
                        {
                            otherLists.Add(friend);
                        }
                    }
                }
                else
                {
                    foreach (var friend in list.friendList)
                    {
                        sharedWith.Add(friend);
                    }
                }
            }

            foreach (var friend in otherLists)
            {
                if (!sharedWith.Contains(friend) && !fillShareList.Contains(friend))
                {
                    fillShareList.Add(friend);
                }
            }

            foreach (var friend in fillShareList)
            {
                Dispatcher.Invoke(() =>
                {
                    ListBoxFriends.Items.Add(friend);
                });
            }
        }
    }
}