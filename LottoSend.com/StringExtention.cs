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
    }
}
