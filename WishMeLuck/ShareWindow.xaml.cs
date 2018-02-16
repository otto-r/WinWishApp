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
    /// Interaction logic for Share.xaml
    /// </summary>
    public partial class ShareWindow : Window
    {
        public ShareWindow(LogInObject logInObject, ObjectOfWishLists objectOfWishLists, string selectedList)
        {
            InitializeComponent();
            Dispatcher.Invoke(() =>
            {
                FillComboBox(objectOfWishLists, selectedList);
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

        }
    }
}
