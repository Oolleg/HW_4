using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace HW_4.Services.Validation
{
    public class UserValidation : IValidationService
    {
        public bool IsEmailValid(string text)
        {
            if (text == "" || text == null)
            {
                return false;
            }

            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
        }

        public bool IsFullnameValid(String text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return false;
            }

            string pattern = @"^[A-ZА-ЯЁЇІЄҐ'][a-zА-Яа-яЁёЇїІіЄєҐґ']*$";

            return Regex.IsMatch(text, pattern);
        }

        public bool IsLoginValid(string text)
        {
            if(String.IsNullOrEmpty(text))
            {
                return false;
            }
           
            return true;
        }

        public bool IsPasswordValid(string text)
        {
            if (String.IsNullOrEmpty(text) || text.Length < 5)
            {
                return false;
            }
           
           return true;
        }

        public bool IsPhoneValid(string text)
        {
            if (text == "" || text == null)
            {
                return true;
            }

            string pattern = @"^0(\d{2})-?\d{3}(-?\d{2}){2}$";

            return Regex.IsMatch(text, pattern);
        }

        public bool IsAvatarValid(string ext)
        {

            String extUpper = ext.Substring(1).ToUpper();

            if (extUpper == "PNG" || extUpper == "JPG")
            {
                return true;
            }

            return false;
        }
    }
}
