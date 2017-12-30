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
        public static bool ValidCharacters(string userInput)
        {
            Match matchInput = Regex.Match(userInput, @"^[a-zA-ZåäöÅÄÖ0-9]*$");
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

        }
    }
}
