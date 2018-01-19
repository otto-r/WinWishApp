using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishMeLuck
{

    public class LogIn
    {
        public int success { get; set; }
        public string msg { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public string username { get; set; }
        public string email { get; set; }
        public string Password { get; set; }
    }


    public class ListOfWishLists
    {
        public int success { get; set; }
        public string msg { get; set; }
        public List<WishList> wishLists { get; set; }
    }

    public class WishList
    {
        public string wishListName { get; set; }
        public List<WishListItem> wishList { get; set; }
    }

    public class WishListItem
    {
        public string wId { get; set; }
        public string username { get; set; }
        public string wishListName { get; set; }
        public string wishItemName { get; set; }
        public string wishItemDesc { get; set; }
        public string wishItemAvailableAt { get; set; }
    }

    public class AddNewWishListObj
    {
        public bool error { get; set; }
        public int success { get; set; }
        public string msg { get; set; }
    }


    public class AddNewWishObj
    {
        public bool error { get; set; }
        public int success { get; set; }
        public string msg { get; set; }
    }

    public class DeleteItemObj
    {
        public bool error { get; set; }
        public int success { get; set; }
        public string msg { get; set; }
    }


}