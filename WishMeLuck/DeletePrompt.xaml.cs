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
    /// Interaction logic for DeletePrompt.xaml
    /// </summary>
    public partial class DeletePrompt : Window
    {
        LogIn logInObjectUsable;
        WishListItem wishListItem;

        public DeletePrompt(LogIn logIn, WishListItem wishItem)
        {
            logInObjectUsable = logIn;
            wishListItem = wishItem;

            InitializeComponent();

            Dispatcher.Invoke(() =>
            {
                LabelAreYouSure.Content = $"Are you sure you want to delete {wishItem.wishItemName}?";
            });
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                string postData = $"del=1&un={logInObjectUsable.user.username}&wln={wishListItem.wishListName}&win={wishListItem.wishItemName}";
                string method = "POST";
                string phpFileName = "delWish.php";

                string jsonStr = WebReq.WebRq(postData, method, phpFileName);

                var deleteItemObj = JsonConvert.DeserializeObject<DeleteItemObj>(jsonStr);

                if (deleteItemObj.success == 1)
                {
                    Dispatcher.Invoke(() =>
                    {
                        LabelUserInteractionFeedback.Foreground = Brushes.LightGreen;
                        LabelUserInteractionFeedback.Content = "Successful";

                        Close();
                    });
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        LabelUserInteractionFeedback.Foreground = Brushes.LightPink;
                        LabelUserInteractionFeedback.Content = $"Error: {deleteItemObj.msg}";
                    });
                }
            });

        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
