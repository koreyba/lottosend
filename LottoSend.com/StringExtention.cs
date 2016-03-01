using System;
using System.Text.RegularExpressions;

namespace LottoSend.com
{
    public static class StringExtention
    {
        /// <summary>
        /// Delets all spaces in a string
        /// </summary>
        /// <returns>String without spaces</returns>
        public static string DeleteAllSpaces(this string str)
        {
            string stringWithoutSpaces = "";
            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] != ' ')
                {
                    stringWithoutSpaces += str[i];
                }
            }

            return stringWithoutSpaces;
        }
        
        /// <summary>
        /// Parses time and return TimeSpan object
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static TimeSpan ParseTimeSpan(this string str)
        {
            string[] substrings = Regex.Split(str, " ");

            TimeSpan timeSpan = new TimeSpan();

            foreach(string time in substrings)
            {
                Match m = Regex.Match(time, "^(?:(?:([01]?[0-9]|2[0-3]):)?([0-5]?[0-9]):)?([0-5]?[0-9])$");
                if(m.Success)
                {
                    timeSpan = TimeSpan.Parse(time);
                }
            }

            return timeSpan;
        }

        /// <summary>
        /// Parce double from a string or int if there is no dot. If there is several digits then parce the first one
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ParseDouble(this string str)
        {
            bool isFirstFound = false;
            bool isLastFound = false;
            string number = "";

            foreach(char letter in str)
            {
                if(Char.IsDigit(letter) || letter.Equals('.'))
                {
                    number += letter;
                    isFirstFound = true;
                }
                else if (letter.Equals(','))
                {
                    number += "";
                }
                else
                {
                    if (isFirstFound == true)
                    {
                        isLastFound = true;
                    }
                }

                if (isLastFound == true)
                    break;
            }

            try
            {
                return Convert.ToDouble(number, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "Can't parce double from the next string:" + number + ". String was generated from the next string: " + str + " ");
            }
        }
    }
}
