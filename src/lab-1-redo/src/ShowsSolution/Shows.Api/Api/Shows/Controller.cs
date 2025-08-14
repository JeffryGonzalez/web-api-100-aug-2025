using Microsoft.AspNetCore.Mvc;
using Marten;
using System;
using System.Threading.Tasks;

namespace Shows.Api.Api.Shows
{
    [Route("/api/shows")]
    [ApiController]
    public class Controller(IDocumentSession session) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AddShow([FromBody] ShowRequest request)
        {
            var show = new Show
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                StreamingService = request.StreamingService,
                CreatedAt = DateTimeOffset.UtcNow
            };

            session.Store(show);
            await session.SaveChangesAsync();


            return Ok(show);
        }

        [HttpGet]
        public async Task<IEnumerable<Show>> GetAllShows()
        {
            var shows = await session.Query<Show>()
                                      .OrderByDescending(s => s.CreatedAt)
                                      .ToListAsync();

            return shows; 
        }
    }



    public record ShowRequest
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string StreamingService { get; init; } = string.Empty;
    }

    public class Show
    {
        public Guid Id { get; init; } = Guid.Empty;
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string StreamingService { get; init; } = string.Empty;
        public DateTimeOffset CreatedAt { get; init; }
    }

    public class ShowResponse
    {
        public Guid Id { get; init; } = Guid.Empty;
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string StreamingService { get; init; } = string.Empty;
        public DateTimeOffset CreatedAt { get; init; }
    }
}