namespace Helper
{
    public static class StringHelper
    {
        public static int ToInt(this string str, int defaultValue = 0)
        {
            return int.TryParse(str, out int result) ? result : defaultValue;
        }
    }
}
