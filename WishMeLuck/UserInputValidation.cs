using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WishMeLuck
{
    public class UserInputValidation
    {
        public static bool ValidCharacters(string userInput, bool allowSpace)
        {
            Match matchInput;
            if (allowSpace)
            {
                matchInput = Regex.Match(userInput, @"^[a-zA-ZåäöÅÄÖ0-9 ]*$");
            }
            else
            {
                matchInput = Regex.Match(userInput, @"^[a-zA-ZåäöÅÄÖ0-9]*$");
            }

            if (matchInput.Success && !string.IsNullOrWhiteSpace(userInput))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string PasswordStrengthTest(string password)  //Returns HEX color code
        {

            if (password.Length == 1)
            {
                return ("#FFFFCDCD");
            }
            else if (password.Length == 2)
            {
                return ("#FFFFD7CD");
            }
            else if (password.Length == 3)
            {
                return ("#FFFFE1CD");
            }
            else if (password.Length == 4)
            {
                return ("#FFFFEBCD");
            }
            else if (password.Length == 5)
            {
                return ("#FFFFF5CD");
            }
            else if (password.Length == 6)
            {
                return ("#FFFFFFCD");
            }
            else if (password.Length == 7)
            {
                return ("#FFF5FFCD");
            }
            else if (password.Length == 8)
            {
                return ("#FFEBFFCD");
            }
            else if (password.Length == 9)
            {
                return ("#FFE1FFCD");
            }
            else if (password.Length == 10)
            {
                return ("#FFD7FFCD");
            }
            else if (password.Length > 10)
            {
                return ("#FFCDFFCD");
            }
            else
            {
                return ("#FFFFFFFF");
            }
        }
    }
}
