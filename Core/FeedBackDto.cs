namespace Core
{
    public class FeedBackDto
    {
        public string Comment { get; set; } = string.Empty;
        public int Ratings { get; set; }
        public TrashBinLatestFeedback LatestFeedback { get; set; }
        public int TrashBinId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
