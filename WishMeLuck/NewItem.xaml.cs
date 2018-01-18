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
    /// Interaction logic for NewItem.xaml
    /// </summary>
    public partial class NewItem : Window
    {
        LogIn logInObjectUsable;
        ListOfWishLists listOfWishListsObjectUsable;

        public NewItem(LogIn logInUserObject, ListOfWishLists listOfWishListsObject, string selectedList)
        {
            logInObjectUsable = logInUserObject;
            listOfWishListsObjectUsable = listOfWishListsObject;

            InitializeComponent();
            FillComboBox(selectedList);
        }

        private void FillComboBox(string selectedList)
        {
            foreach (var item in listOfWishListsObjectUsable.wishLists)
            {
                Dispatcher.Invoke(() =>
                {
                    ComboBoxSelectWishList.Items.Add(item.wishListName);
                    if (selectedList != "")
                    {
                        ComboBoxSelectWishList.SelectedItem = selectedList;
                    }
                    else
                    {
                        ComboBoxSelectWishList.SelectedIndex = 0;
                    }
                });
            }
        }

        private void ButtonAddWish_Click(object sender, RoutedEventArgs e)
        {
            string wishName = TextBoxNameOfWishItem.Text;
            string wishListName = ComboBoxSelectWishList.SelectedItem.ToString();
            string wishDescription = TextBoxItemDescription.Text;
            string wishAvailableAt = TextBoxItemAvailableAt.Text;

            if (UserInputValidation.ValidCharacters(wishName))
            {
                Task.Run(() =>
                {
                    string postData = $"un={logInObjectUsable.user.username}&wln={wishListName}&win={wishName}&widesc={wishDescription}&wiaa={wishAvailableAt}";
                    string method = "POST";
                    string phpFileName = "addWish.php";

                    string jsonStr = WebReq.WebRq(postData, method, phpFileName);

                    var addNewWishObj = JsonConvert.DeserializeObject<AddNewWishObj>(jsonStr);

                    if (addNewWishObj.success == 1)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            LabelUserInteractionFeedBack.Foreground = Brushes.LightGreen;
                            LabelUserInteractionFeedBack.Content = "Successful";

                            this.Close();
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            LabelUserInteractionFeedBack.Foreground = Brushes.LightPink;
                            LabelUserInteractionFeedBack.Content = $"Error: {addNewWishObj.msg}";
                        });
                    }
                });
            }
        }
    }
}
