using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WishMeLuck
{
    /// <summary>
    /// Interaction logic for NewItem.xaml
    /// </summary>
    public partial class NewItem : Window
    {
        LogIn logInObjectUsable;
        ObjectOfWishLists listOfWishListsObjectUsable;

        public NewItem(LogIn logInUserObject, ObjectOfWishLists listOfWishListsObject, string selectedList)
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

            if (UserInputValidation.InputValidator(wishName, true).ValidInput)
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
