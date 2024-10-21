namespace Models.Extensions
{
    public static class NumberFormatter
    {
        public static string FormatNumber(int number)
        {
            if (number >= 1000000) // Million
            {
                return $"{number / 1000000.0:F1}M";
            }
            else if (number >= 1000) // Thousand
            {
                return $"{number / 1000.0:F1}K";
            }
            else
            {
                return number.ToString();
            }
        }
    }
}