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
    }
}
