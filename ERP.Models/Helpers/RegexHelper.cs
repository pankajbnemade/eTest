using System.Text.RegularExpressions;

namespace ERP.Models.Helpers
{
    /// <summary>
    /// Class for regular expression helper.
    /// </summary>
    public class RegexHelper
    {
        public const string AlphabetOnly = @"^[a-zA-Z ]+$";
        public const string AlphaNumeric = "^[0-9A-Za-z ]+$";
        public const string AlphaNumericNoSpace = "^[0-9A-Za-z]+$";
        public const string NumericOnly = "^[0-9]*$";
        public const string Decimal2Digit = @"^\d*(\.\d{0,2})?$";
        public const string Decimal2DigitMessage = "2 Decimal Only";
        public const string Decimal4Digit = @"^\d*(\.\d{0,4})?$";
        public const string Decimal4DigitMessage = "4 Decimal Only";
        public const string Decimal6Digit = @"^\d*(\.\d{0,6})?$";
        public const string Decimal6DigitMessage = "6 Decimal Only";

        public const string InvalidEmailAddress = @"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$";
        public const string PhoneNumber = "^([0-9]{10})$";
        public const string InvalidDate = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\/\d{4}$";
        public const string MMyyyy = @"^((0[1-9])|(1[0-2]))\/(\d{4})$";

        /// <summary>
        /// Function to check if string is matching with specific regular expression or not.
        /// </summary>
        /// <param name="stringToCompare"></param>
        /// <param name="regularExpresssion"></param>
        /// <returns></returns>
        public static bool IsValidString(string stringToCompare, string regularExpresssion)
        {
            return Regex.Match(stringToCompare, regularExpresssion).Success;
        }
    }
}
