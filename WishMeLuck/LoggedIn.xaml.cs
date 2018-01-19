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
    /// <summary>
    /// Interaction logic for MainLogIn.xaml
    /// </summary>
    public partial class MainLogIn : Window
    {
        LogIn LogInObj;
        ListOfWishLists listOfWishLists;
        string selectedItemAvailableAt;

        public MainLogIn(LogIn logInUserObject)
        {
            LogInObj = logInUserObject;
            InitializeComponent();
            Dispatcher.Invoke(() =>
            {
                UserName.Content = logInUserObject.user.username;

            });
            listOfWishLists = GetListofLists(logInUserObject.user);

            if (listOfWishLists != null)
            {
                FillListOfLists(listOfWishLists);
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    LabelErrorMessage.Content = "ERROR: No lists returned";
                });
            }
        }

        private ListOfWishLists GetListofLists(User user)
        {
            string postData = "un=" + user.username;
            string method = "POST";
            string phpFileName = "getLists.php";

            string jsonStr = WebReq.WebRq(postData, method, phpFileName);

            ListOfWishLists ListOfLists = JsonConvert.DeserializeObject<ListOfWishLists>(jsonStr);

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
                FillWishList(listOfWishLists, selectedWishList);
            }
            Task.Run(() =>
            {

            });
        }

        private void FillListOfLists(ListOfWishLists ListOfLists)
        {
            Dispatcher.Invoke(() =>
            {
                //WishListOfLists.SelectedItem = -1;
                WishListOfLists.Items.Clear();
                foreach (var item in ListOfLists.wishLists)
                {
                    WishListOfLists.Items.Add(item.wishListName);
                }
            });
        }

        private void FillWishList(ListOfWishLists ListOfLists, string wishListName)
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
                foreach (var wishList in listOfWishLists.wishLists)
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
            NewWishList newWishList = new NewWishList(LogInObj, listOfWishLists);
            newWishList.Show();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            listOfWishLists = GetListofLists(LogInObj.user);

            if (listOfWishLists != null)
            {
                FillListOfLists(listOfWishLists);
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
            NewItem newItem = new NewItem(LogInObj, listOfWishLists, selectedWishListString);
            newItem.Show();
        }

        private void ButtonDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            string selectedItem = WishListBox.SelectedItem.ToString();

            foreach (var wishList in listOfWishLists.wishLists)
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

        private void ButtonAvailableAt_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(selectedItemAvailableAt);
        }
    }
}