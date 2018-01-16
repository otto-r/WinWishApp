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
        LogIn LogInObj;

        ListOfWishLists listOfWishLists;
        public MainLogIn(LogIn logInUserObject)
        {
            LogInObj = logInUserObject;
            InitializeComponent();
            Dispatcher.Invoke(() =>
            {
                UserName.Content = logInUserObject.user.username;
            });
            listOfWishLists = GetListofLists(logInUserObject.user);

            FillListOfLists(listOfWishLists);
            //FillWishList(listOfWishLists,"Djur");
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
                WishListItem.Items.Clear();
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
                WishListItem.Items.Clear();
                foreach (var wishList in listOfWishLists.wishLists)
                {
                    foreach (var item in wishList.wishList)
                    {
                        if (item.wishItemName == itemSelected)
                        {
                            WishListItem.Items.Add("Description: " + item.wishItemDesc);
                            WishListItem.Items.Add("Item id: " + item.wId);
                            WishListItem.Items.Add("Available at: " + item.wishItemAvailableAt);
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
            FillListOfLists(GetListofLists(LogInObj.user));
        }
    }
}