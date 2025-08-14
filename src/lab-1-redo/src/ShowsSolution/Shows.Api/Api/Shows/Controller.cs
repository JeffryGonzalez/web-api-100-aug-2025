using Marten;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Shows.Api.Api.Shows;

[Route("/api/shows")]
[ApiController]
public class Controller : ControllerBase
{
    private readonly IDocumentSession _documentSession;
    public Controller(IDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }
    [HttpPost]
    public async Task<ActionResult> AddShow([FromBody] ShowRequest request)
    {
        var show = new Show
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            StreamingService = request.StreamingService,
            CreatedAt = DateTime.UtcNow
        };

        _documentSession.Store(show);
        await _documentSession.SaveChangesAsync();
        return Ok(show);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult> GetShow(Guid id)
    {
        var getShow = await _documentSession.LoadAsync<Show>(id);
        return Ok(getShow);
    }
    [HttpGet]
    public async Task<IEnumerable<Show>> GetAllShows()
    {
        var shows = await _documentSession.Query<Show>()
                                  .OrderByDescending(s => s.CreatedAt)
                                  .ToListAsync();

        return shows;
    }
}
public class ShowRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StreamingService { get; set; } = string.Empty;
}
public class Show
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StreamingService { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
}
