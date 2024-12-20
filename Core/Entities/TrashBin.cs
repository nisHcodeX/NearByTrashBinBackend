namespace Core.Entities
{
    public class TrashBin
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageUrl { get; set; }
        public bool Organic { get; set; }
        public bool Paper { get; set; }
        public bool Plastic { get; set; }
        public bool Glass { get; set; }
        public bool SuggestedBin { get; set; }
        public DateTime CreatedDate { get; set; }
        public TrashBinStatus TrashBinStatus { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
