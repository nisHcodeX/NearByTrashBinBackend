using Core;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly AppDBContext _context;

        public AdminController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("bins")]
        public async Task<IActionResult> GetBins()
        {
            try
            {
                var bins = await _context.TrashBins.Include(x => x.Feedbacks.OrderByDescending(x => x.CreatedDate)).OrderByDescending(x => x.CreatedDate).ToListAsync();
                return Ok(bins);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("update/bin")]
        public async Task<IActionResult> UpdateBinStatus([FromQuery]int id, TrashBinStatus status)
        {
            try
            {
                var bin = await _context.TrashBins.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (bin != null)
                {
                    bin.TrashBinStatus = status;
                    _context.TrashBins.Update(bin);
                    await _context.SaveChangesAsync();
                }

                return Ok(bin);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("feedbacks")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = await _context.Feedbacks.ToListAsync();
                return Ok(feedbacks);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
