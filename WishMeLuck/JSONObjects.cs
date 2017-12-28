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

    public class ListOfLists
    {
        public int success { get; set; }
        public string msg { get; set; }
        public List<string> WishLists { get; set; }
    }

    public class WishList
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string WishListName { get; set; }
        public WishListItem WshListItem{ get; set; }
    }

    public class WishListItem
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string WishListName { get; set; }
        public string WishItemName { get; set; }
        public string WishItemDesc { get; set; }
        public string WishItemAvailableAt { get; set; }
    }
}
