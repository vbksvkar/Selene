namespace Models.Extensions
{
    public static class EpochConverter
    {
        private static DateTime epochStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        public static DateTime UtcFromEpochTime(this long epochsSeconds)
        {
            var dateTime = DateTime.UtcNow + TimeSpan.FromSeconds(epochsSeconds);
            return dateTime;
        }

        public static DateTime FromEpochTime(this double epochsSeconds)
        {
            var dateTime = epochStartTime.AddSeconds(epochsSeconds);
            return dateTime;
        }
    }
}