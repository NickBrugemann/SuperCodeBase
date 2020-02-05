using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SuperCodeBase.strings
{
    /// <summary>
    /// StringMaster contains various methods regarding string manipulation.
    /// </summary>
    static class StringMaster
    {
        /// <summary>
        /// Determines the separator on the basis of the given string.
        /// </summary>
        /// <param name="stringToCheck">The string to retrieve the separator from.</param>
        /// <returns>Returns the separator as char.</returns>
        public static char FindSeparator(string stringToCheck)
        {
            int mostOccurrence = -1;
            char mostOccurringChar = ' ';
            foreach (char currentChar in stringToCheck)
            {
                int foundCharOccurrence = 0;
                foreach (char charToBeMatch in stringToCheck)
                {
                    if (currentChar == charToBeMatch)
                    {
                        if (!Char.IsLetterOrDigit(currentChar))
                        {
                            foundCharOccurrence++;
                        }
                    }
                }
                if (mostOccurrence < foundCharOccurrence)
                {
                    mostOccurrence = foundCharOccurrence;
                    mostOccurringChar = currentChar;
                }
            }
            return mostOccurringChar;
        }

        /// <summary>
        /// Counts a certain character in a string.
        /// </summary>
        /// <param name="stringToCheck">The string to count characters in.</param>
        /// <param name="charToCount">The character to count.</param>
        /// <returns>Returns the amount of a certain character in a string.</returns>
        public static int CountCharacterIn(string stringToCheck, char charToCount)
        {
            int count = 0;

            foreach (char c in stringToCheck)
            {
                if (c == charToCount)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Counts the amount of words in a string.
        /// </summary>
        /// <param name="fullString">The string to count the words in.</param>
        /// <returns>Returns the amount of words in a string.</returns>
        public static int CountWordsIn(string fullString)
        {
            System.Text.RegularExpressions.MatchCollection a = System.Text.RegularExpressions.Regex.Matches(fullString, @"[\S]+");
            return a.Count;
        }

        /// <summary>
        /// Retrieves a string starting from the end of a string.
        /// ("potato", 4) would retrieve "tato".
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <param name="amount">The amount of characters to retrieve.</param>
        /// <returns>Returns a string.</returns>
        public static string GetCharactersFromEnd(string stringToCheck, int amount)
        {
            try
            {
                return stringToCheck.Substring(stringToCheck.Length - amount, amount);
            }

            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a string starting from the beginning of a string.
        /// ("potato", 4) would retrieve "pota".
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <param name="amount">The amount of characters to retrieve.</param>
        /// <returns>Returns a string.</returns>
        public static string GetCharactersFromBeginning(string stringToCheck, int amount)
        {
            try
            {
                return stringToCheck.Substring(0, amount);
            }

            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves a string starting in the middle of a string.
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <param name="startIndex">The index of the first letter to retrieve.</param>
        /// <param name="amount">The amount of characters to retrieve.</param>
        /// <returns>Returns a string.</returns>
        public static string GetCharactersFromMiddle(string stringToCheck, int startIndex, int amount)
        {
            try
            {
                return stringToCheck.Substring(startIndex, amount);
            }

            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if a string contains ASCII characters only.
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <returns>Returns true if the string contains ASCII characters only.</returns>
        public static bool IsAscii(string stringToCheck)
        {
            return Encoding.UTF8.GetByteCount(stringToCheck) == stringToCheck.Length;
        }

        /// <summary>
        /// Removes non-ASCII characters from a string.
        /// </summary>
        /// <param name="stringToCheck">The string to be stripped.</param>
        /// <returns>Returns a stirng with ASCII characters only.</returns>
        public static string RemoveNonAscii(string stringToCheck)
        {
            stringToCheck = Regex.Replace(stringToCheck, @"[^\u0000-\u007F]+", string.Empty);
            return stringToCheck;
        }

        /// <summary>
        /// Checks if a string is in a valid time format.
        /// Can be used for DateTime objects for example.
        /// </summary>
        /// <param name="time">The string with a time format.</param>
        /// <returns>Returns true if the string contains a valid time format.</returns>
        public static bool IsValidTime(string time)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
            return r.IsMatch(time);
        }

        /// <summary>
        /// Sets the first letter of a string to the uppercase variant.
        /// </summary>
        /// <param name="value">The string to edit.</param>
        /// <returns>Returns the string with the first letter as uppercase.</returns>
        public static string FirstCharToUpper(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new NullReferenceException();
            }

            return value.First().ToString().ToUpper() + value.Substring(1);
        }

        /// <summary>
        /// Compresses a string containing enters to a single line.
        /// </summary>
        /// <param name="value">The string to be compressed to a single line.</param>
        /// <returns>Returns the given string in a single line.</returns>
        public static string ToOneLine(string value)
        {
            value = value.Replace("\r\n", " ");
            return value;
        }

        /// <summary>
        /// Creates a string from the current date.
        /// Format: [dd-MM-YYYY]
        /// Example: 21 april 2002 = 21-04-2002
        /// </summary>
        /// <returns>Returns the current date as a string.</returns>
        public static string DateToString()
        {
            string day = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();

            if (day.Length != 2)
            {
                day = "0" + day;
            }

            if (month.Length != 2)
            {
                month = "0" + month;
            }

            return day + "-" + month + "-" + year;
        }

        /// <summary>
        /// Creates a string from the current time.
        /// Format: [HH:mm:ss]
        /// Example: half 4 's ochtends = 03:30:00
        /// </summary>
        /// <returns>Returns the current time as a string.</returns>
        public static string TimeToString()
        {
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();

            if (hour.Length != 2)
            {
                hour = "0" + hour;
            }

            if (minute.Length != 2)
            {
                minute = "0" + minute;
            }

            if (second.Length != 2)
            {
                second = "0" + second;
            }

            return hour + ":" + minute + ":" + second;
        }
    }
}
