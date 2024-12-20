using Microsoft.AspNetCore.Http;

namespace Core
{
    public class AddTrashBin
    {
        public IFormFile? Image { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Organic { get; set; }
        public bool Paper { get; set; }
        public bool Plastic { get; set; }
        public bool Glass { get; set; }
        public string UserId { get; set; }
        public bool SuggestedBin { get; set; }
    }
}
