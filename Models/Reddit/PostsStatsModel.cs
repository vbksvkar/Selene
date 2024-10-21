namespace Models.Reddit
{
    public class PostsStatsModel
    {
        public string SubRedditName { get; set; }
        public string StatsText { get; set; }
        public List<Tuple<string, int>> PostsStats { get; set; } = new List<Tuple<string, int>>();
    }
}