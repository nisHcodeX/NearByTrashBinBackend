namespace Core.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Ratings { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int TrashBinId { get; set; }
        public TrashBin TrashBin { get; set; }
        public TrashBinLatestFeedback LatestFeedback { get; set; }
    }
}