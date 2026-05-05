using Microsoft.AspNetCore.Mvc;
using Record_Shop.Data;

namespace Record_Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly RecordDbContext _recordDbContext;

        public HealthController(RecordDbContext recordDbContext)
        {
            _recordDbContext = recordDbContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetHealth()
        {
            try
            {
                // checks if the database is reachable
                await _recordDbContext.Database.CanConnectAsync();
                return Ok(new
                {
                    status = "Healthy",
                    database = "Connected",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(503, new
                {
                    status = "Unhealthy",
                    database = "Unreachable",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
