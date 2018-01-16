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
        ListOfWishLists listOfWishListsObjectUsable;

        public NewWishList(LogIn logInUserObject, ListOfWishLists listOfWishListsObject)
        {
            InitializeComponent();
            logInObjectUsable = logInUserObject;
            listOfWishListsObjectUsable = listOfWishListsObject;

            Dispatcher.Invoke(() =>
            {
                LabelUserName.Content = logInObjectUsable.user.username;
            });
        }

        private void ButtonAddNewList_Click(object sender, RoutedEventArgs e)
        {
            if (UserInputValidation.ValidCharacters(TextBoxWishListName.Text) && !SeeIfWishListNameExists())
            {
                Task.Run(() =>
                {
                    //WebReq.WebRq("un=" + logInObjectUsable.user.username, "POST", "NewWishList.php");
                    Dispatcher.Invoke(() =>
                    {
                        LabelUserInputvalidation.Foreground = Brushes.LightGreen;
                        LabelUserInputvalidation.Content = "Successful";
                        this.Close();
                    });
                });
            }
            else if (SeeIfWishListNameExists())
            {
                Dispatcher.Invoke(() =>
                {
                    LabelUserInputvalidation.Foreground = Brushes.LightPink;
                    LabelUserInputvalidation.Content = $"A list by the name {TextBoxWishListName.Text} already exists";
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
            foreach (var item in listOfWishListsObjectUsable.wishLists)
            {
                if (TextBoxWishListName.Text.ToLower() == item.wishListName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
