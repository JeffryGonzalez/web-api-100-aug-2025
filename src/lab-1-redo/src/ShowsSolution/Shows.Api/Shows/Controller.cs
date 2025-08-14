using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shows.Api.Shows
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private static readonly ConcurrentDictionary<Guid, ShowResponse> _shows = new();

        [HttpPost]
    public IActionResult AddShow([FromBody] AddShowRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var show = new ShowResponse(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.StreamingService,
            DateTimeOffset.UtcNow
        );

        _shows[show.Id] = show;
        return Ok(show);
    }
        [HttpGet("{id}")]
        public IActionResult GetShow(Guid id)
        {
            if (_shows.TryGetValue(id, out var show))
            {
                return Ok(show);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetAllShows()
        {
            var ordered = _shows.Values.OrderByDescending(s => s.CreatedAt).ToList();
            return Ok(ordered);
        }
    }

    public record AddShowRequest(
        [Required, MinLength(3), MaxLength(100)] string Name,
        [Required, MinLength(10), MaxLength(500)] string Description,
        [Required] string StreamingService
    );

    public record ShowResponse(
        Guid Id,
        string Name,
        string Description,
        string StreamingService,
        DateTimeOffset CreatedAt
    );
}