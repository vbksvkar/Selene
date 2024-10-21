using System.Net;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Response;

namespace Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedditController : ControllerBase
    {
        private readonly IRedditService redditService;

        public RedditController(IRedditService redditService)
        {
            this.redditService = redditService;
        }

        [HttpGet]
        public async Task<ActionResult<StatsResponse>> GetStats(string subRedditName)
        {
            var stats = await this.redditService.GetStats(subRedditName);
            return this.HandleResponse(stats);            
        }

        private ActionResult HandleResponse(StatsResponse stats)
        {
            if (stats.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return this.NotFound();
            }

            return this.Ok(stats);
        }
    }
}