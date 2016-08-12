using System;

namespace TestFramework
{
    /// <summary>
    /// Contains methods for generatin random strings
    /// </summary>
    public static class RandomGenerator
    {
        static Random r = new Random();

        /// <summary>
        /// Generates string of random symbols (latin) with set length of the string
        /// </summary>
        /// <param name="lenght">String's length</param>
        /// <returns>Random string</returns>
        public static string GenerateRandomString(int lenght)
        {
            string randomString = "";


            for (int i = 0; i < lenght; ++i)
            {
                randomString += GenerateRandomSymbol();
            }

            return randomString;
        }

        public static char GenerateRandomSymbol()
        {
            //Диапазоны: 65-90 - латинский алфавит заглавные буквы
            //97 - 122 - латинский алфавит строчные буквы
            //48-57 - цифры
            //1400 - 1071 - кирилица, заглавные буквы
            //1072 - 1103 - кирилица, строчные буквы

            return (char)r.Next(97, 122);
        }

        public static int GenerateNumber(int minValue, int maxValue)
        {
            return r.Next(minValue, maxValue);
        }
    }
}
