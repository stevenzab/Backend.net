using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        [HttpGet("PlayerList")]
        public async Task GetPlayerListAsync()
        {
        }

        [HttpGet("PlayerDetails/{id}")]
        public async Task GetPlayerDetailsAsync(string id)
        {
        }

        [HttpGet("PlayerStats")]
        public async Task GetPlayerStatsAsync()
        {
        }

        [HttpPost("CreatePlayer")]
        public async Task CreatePlayerAsync()
        {
        }
    }
}
