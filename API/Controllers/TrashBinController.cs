using Core;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrashBinController : ControllerBase
    {
        private readonly AppDBContext context;

        public TrashBinController(AppDBContext context)
        {
            this.context = context;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddTrashBin([FromForm] AddTrashBin bin)
        {
            try
            {
                if (bin.Image != null && bin.Image.Length > 0)
                {
                    var directory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\images";
                    var fileName = $"{Guid.NewGuid()}.png";
                    var fullPath = Path.Combine(directory, fileName);

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    using (var stream = System.IO.File.Create(fullPath))
                    {
                        bin.Image.CopyTo(stream);
                        stream.Flush();
                    }

                    var imageUrl = $"https://localhost:7279/images/{fileName}";

                    var trashBin = new TrashBin
                    {
                        Latitude = bin.Latitude,
                        Longitude = bin.Longitude,
                        ImageUrl = imageUrl,
                        Organic = bin.Organic,
                        Paper = bin.Paper,
                        Plastic = bin.Plastic,
                        Glass = bin.Glass,
                        CreatedDate = DateTime.UtcNow,
                        TrashBinStatus = TrashBinStatus.PENDING,
                        AppUserId = bin.UserId,
                        SuggestedBin = bin.SuggestedBin,
                    };

                    await context.TrashBins.AddAsync(trashBin);
                    await context.SaveChangesAsync();

                    return Ok(trashBin);
                }
                else
                {
                    throw new Exception("Please upload image");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] double lat, double lon, double radius)
        {
            try
            {
                const double earthRadius = 3959;

                var nearestLocations = await context.TrashBins
                     .Where(l => l.Latitude != null && l.Longitude != null)
                     .Include(l => l.Feedbacks.OrderByDescending(x=> x.CreatedDate))
                     .Select(l => new
                     {
                         Location = l,
                         Distance = earthRadius * Math.Acos(
                             Math.Cos(Math.PI * lat / 180) * Math.Cos(Math.PI * l.Latitude / 180) *
                             Math.Cos(Math.PI * lon / 180 - Math.PI * l.Longitude / 180) +
                             Math.Sin(Math.PI * lat / 180) * Math.Sin(Math.PI * l.Latitude / 180))
                     })
                     .Where(l => l.Distance < radius)
                     .OrderBy(l => l.Distance)
                     .Select(l => l.Location)
                     .ToListAsync();

                return Ok(nearestLocations);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTrashBinById([FromRoute] string userId)
        {
            var trashbins = await context.TrashBins.Where(x => x.AppUserId == userId).ToListAsync();
            return Ok(trashbins);
        }
    }
}
