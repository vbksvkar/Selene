using Models.Extensions;

namespace Models.Response
{
    public class StatsItem
    {
        public string StatValue { get; set; }
        public int Count { get; set; }
        public string StatCount 
        {
            get
            {
                return NumberFormatter.FormatNumber(Count);
            }
        }
    }
}