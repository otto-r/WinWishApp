using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class LoggedInWindow : Window
    {
        LogInObject LogInObj;
        ObjectOfWishLists objectOfWishLists;
        FriendRequest friendRequestTop;
        string selectedItemAvailableAt;

        public LoggedInWindow(LogInObject logInUserObject)
        {
            LogInObj = logInUserObject;
            InitializeComponent();
            Dispatcher.Invoke(() =>
            {
                UserName.Content = logInUserObject.user.username;
            });

            objectOfWishLists = GetListofLists(logInUserObject.user);
            FillListOfLists(objectOfWishLists);
            FillFriendList(LogInObj.user);

        }

        private ObjectOfWishLists GetListofLists(User user)
        {
            string postData = "un=" + user.username;
            string method = "POST";
            string phpFileName = "getLists.php";

            string jsonStr = WebReq.WebRq(postData, method, phpFileName, "");

            ObjectOfWishLists ListOfLists = JsonConvert.DeserializeObject<ObjectOfWishLists>(jsonStr);

            return ListOfLists;
        }

        private void WishListOfLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WishListOfLists.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                string selectedWishList = WishListOfLists.SelectedItem.ToString();
                FillWishList(objectOfWishLists, selectedWishList);
            }
            Task.Run(() =>
            {

            });
        }

        public void FillListOfLists(ObjectOfWishLists ListOfLists)
        {
            if (ListOfLists.wishLists != null)
            {
                Dispatcher.Invoke(() =>
                {
                    WishListOfLists.Items.Clear();
                    foreach (var item in ListOfLists.wishLists)
                    {
                        WishListOfLists.Items.Add(item.wishListName);
                    }
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    LabelErrorMessage.Content = "No lists returned";
                });
            }
        }

        public void FillWishList(ObjectOfWishLists ListOfLists, string wishListName)
        {
            Dispatcher.Invoke(() =>
            {
                WishListBox.Items.Clear();
                foreach (var WishList in ListOfLists.wishLists)
                {
                    if (WishList.wishListName == wishListName)
                    {
                        foreach (var item in WishList.wishList)
                        {
                            WishListBox.Items.Add(item.wishItemName);
                        }
                    }
                }
            });
        }

        private void WishListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ButtonAvailableAt.Visibility = Visibility.Visible;
                //WishListItem.Items.Clear();
            });
            if (WishListBox.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                string selectedItem = WishListBox.SelectedItem.ToString();
                FillItemInfo(selectedItem);
            }
        }

        private void FillItemInfo(string itemSelected)
        {
            Dispatcher.Invoke(() =>
            {
                //WishListItem.Items.Clear();
                foreach (var wishList in objectOfWishLists.wishLists)
                {
                    foreach (var item in wishList.wishList)
                    {
                        if (item.wishItemName == itemSelected)
                        {
                            selectedItemAvailableAt = item.wishItemAvailableAt;
                            //Check if URL if yes enable button
                            Match matchInput = Regex.Match(item.wishItemAvailableAt, @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])");
                            if (matchInput.Success)
                            {
                                ButtonAvailableAt.IsEnabled = true;
                            }
                            else
                            {
                                ButtonAvailableAt.IsEnabled = false;
                            }

                            LabelItemName.Content = item.wishItemName;
                            LabelItemDescrition.Text = item.wishItemDesc.ToString();
                            ButtonAvailableAt.Content = item.wishItemAvailableAt;
                        }
                    }
                }
            });
        }

        private void ButtonAddNewWishList_Click(object sender, RoutedEventArgs e)
        {
            NewWishList newWishList = new NewWishList(LogInObj, objectOfWishLists);
            newWishList.Show();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            objectOfWishLists = GetListofLists(LogInObj.user);

            if (objectOfWishLists != null)
            {
                FillListOfLists(objectOfWishLists);
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    LabelErrorMessage.Content = "ERROR: No lists returned";
                });
            }
        }

        private void ButtonAddNewItem_Click(object sender, RoutedEventArgs e)
        {
            string selectedWishListString = "";
            if (WishListOfLists.SelectedIndex != -1)
            {
                selectedWishListString = WishListOfLists.SelectedItem.ToString();
            }
            NewItem newItem = new NewItem(LogInObj, objectOfWishLists, selectedWishListString);
            newItem.Show();
        }

        private void ButtonDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (objectOfWishLists.wishLists != null && WishListBox.SelectedItem != null)
            {
                string selectedItem = WishListBox.SelectedItem.ToString();

                foreach (var wishList in objectOfWishLists.wishLists)
                {
                    foreach (var wishItem in wishList.wishList)
                    {
                        if (wishItem.wishItemName == selectedItem)
                        {
                            DeletePrompt delete = new DeletePrompt(LogInObj, wishItem);
                            delete.Show();
                        }
                    }
                }
            }
        }

        private void ButtonAvailableAt_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(selectedItemAvailableAt);
        }

        private void ButtonDeleteWishList_Click(object sender, RoutedEventArgs e)
        {
            if (objectOfWishLists.wishLists != null && WishListOfLists.SelectedItem != null)
            {
                string selectedItem = WishListOfLists.SelectedItem.ToString();

                foreach (var wishList in objectOfWishLists.wishLists)
                {
                    if (wishList.wishListName == selectedItem)
                    {
                        DeletePromptWishList delete = new DeletePromptWishList(LogInObj, wishList);
                        delete.Show();
                    }
                }
            }
        }

        private void ButtonShareList_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                string selectedList;
                if (WishListOfLists.SelectedIndex == -1)
                {
                    selectedList = "";
                }
                else
                {
                    selectedList = WishListOfLists.SelectedItem.ToString();
                }
                ShareWindow shareWindow = new ShareWindow(LogInObj, objectOfWishLists, selectedList);
                shareWindow.Show();
            });
        }

        private void FillFriendList(User user)
        {
            FriendRequest friendRequest = GetFriendsLists(user);
            if (friendRequest.wishLists != null)
            {
                Dispatcher.Invoke(() =>
                {
                    ListBoxFriendsLits.Items.Clear();
                    foreach (var item in friendRequest.wishLists)
                    {
                        ListBoxFriendsLits.Items.Add(item.wishListName);
                    }
                });
            }
        }

        private FriendRequest GetFriendsLists(User user)
        {
            string postData = "un=" + user.username;
            string method = "POST";
            string phpFileName = "getSharedLists.php";

            string jsonStr = WebReq.WebRq(postData, method, phpFileName, "");

            friendRequestTop = JsonConvert.DeserializeObject<FriendRequest>(jsonStr);

            return friendRequestTop;
        }


        private void ListBoxFriendsLits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxFriendsLits.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                string selectedWishList = ListBoxFriendsLits.SelectedItem.ToString();
                Dispatcher.Invoke(() =>
                {
                    ListBoxFriendsLitsItems.Items.Clear();
                    foreach (var item in friendRequestTop.wishLists)
                    {
                        ListBoxFriendsLitsItems.Items.Add(item.wishListName); //Lägg till name sen när mackan fixar
                    }
                });
            }
        }

        //public void FillFriendWishList(ObjectOfWishLists ListOfLists, string wishListName)
        //{
        //    Dispatcher.Invoke(() =>
        //    {
        //        foreach (var WishList in ListOfLists.wishLists)
        //        {
        //            if (WishList.wishListName == wishListName)
        //            {
        //                foreach (var item in WishList.wishList)
        //                {
        //                    WishListBox.Items.Add(item.wishItemName);
        //                }
        //            }
        //        }
        //    });
        //}
    }
}