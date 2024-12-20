using Core;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDBContext _context;

        public FeedbackController(AppDBContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddFeedback([FromBody] FeedBackDto dto)
        {
            try
            {
                var feedback = new Feedback
                {
                    Comment = dto.Comment,
                    Ratings = dto.Ratings,
                    LatestFeedback = dto.LatestFeedback,
                    TrashBinId = dto.TrashBinId,
                    UserId = dto.UserId,
                    CreatedDate = DateTime.UtcNow,
                };

                await _context.Feedbacks.AddAsync(feedback);
                await _context.SaveChangesAsync();

                return Ok(feedback);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
