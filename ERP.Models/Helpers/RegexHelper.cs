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
        public const string InvalidEmailAddress = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public const string PhoneNumber = "^([0-9]{10})$";
        public const string UnitNumber = "(([A-Z]+[0-9]*|[0-9]+[A-Z]*)([1357]/[248])?)|.{0}";
        public const string InvalidDate = @"(?=\d)^(?:(?!(?:10\D(?:0?[5-9]|1[0-4])\D(?:1582))|(?:0?9\D(?:0?[3-9]|1[0-3])\D(?:1752)))((?:0?[13578]|1[02])|(?:0?[469]|11)(?!\/31)(?!-31)(?!\.31)|(?:0?2(?=.?(?:(?:29.(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:(?:\d\d)(?:[02468][048]|[13579][26])(?!\x20BC))|(?:00(?:42|3[0369]|2[147]|1[258]|09)\x20BC))))))|(?:0?2(?=.(?:(?:\d\D)|(?:[01]\d)|(?:2[0-8])))))([-.\/])(0?[1-9]|[12]\d|3[01])\2(?!0000)((?=(?:00(?:4[0-5]|[0-3]?\d)\x20BC)|(?:\d{4}(?!\x20BC)))\d{4}(?:\x20BC)?)(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$";
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
