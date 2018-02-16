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
    /// Interaction logic for NewWishList.xaml
    /// </summary>
    public partial class NewWishList : Window
    {
        LogIn logInObjectUsable;
        ObjectOfWishLists objectOfWishLits;

        public NewWishList(LogIn logInUserObject, ObjectOfWishLists listOfWishListsObject)
        {
            InitializeComponent();
            logInObjectUsable = logInUserObject;
            objectOfWishLits = listOfWishListsObject;

            Dispatcher.Invoke(() =>
            {
                LabelUserName.Content = logInObjectUsable.user.username;
            });
        }

        private void ButtonAddNewList_Click(object sender, RoutedEventArgs e)
        {
            string userWishListNameInput = TextBoxWishListName.Text.Trim();
            if (UserInputValidation.InputValidator(userWishListNameInput, true) && !SeeIfWishListNameExists())
            {
                Task.Run(() =>
                {
                    string postData = "un=" + logInObjectUsable.user.username + "&wln=" + userWishListNameInput;
                    string method = "POST";
                    string phpFileName = "addWishList.php";

                    string jsonStr = WebReq.WebRq(postData, method, phpFileName);

                    var addNewWishListObj = JsonConvert.DeserializeObject<AddNewWishListObj>(jsonStr);

                    if (addNewWishListObj.success == 1)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            LabelUserInputvalidation.Foreground = Brushes.LightGreen;
                            LabelUserInputvalidation.Content = "Successful";

                            this.Close();
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            LabelUserInputvalidation.Foreground = Brushes.LightPink;
                            LabelUserInputvalidation.Content = $"Error: {addNewWishListObj.msg}";
                        });
                    }
                });
            }
            else if (SeeIfWishListNameExists())
            {
                Dispatcher.Invoke(() =>
                {
                    LabelUserInputvalidation.Foreground = Brushes.LightPink;
                    LabelUserInputvalidation.Content = $"A list by the name {userWishListNameInput} already exists";
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    LabelUserInputvalidation.Foreground = Brushes.LightPink;
                    LabelUserInputvalidation.Content = "Invalid characters";
                });
            }
        }

        private bool SeeIfWishListNameExists()
        {
            string userWishListNameInput = TextBoxWishListName.Text.Trim();

            if (objectOfWishLits.wishLists != null)
            {
                foreach (var item in objectOfWishLits.wishLists)
                {
                    if (userWishListNameInput.ToLower() == item.wishListName.ToLower())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
