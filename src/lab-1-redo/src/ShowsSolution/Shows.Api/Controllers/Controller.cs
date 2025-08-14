using Marten;
using Microsoft.AspNetCore.Mvc;
using Riok.Mapperly.Abstractions;
using static Shows.Api.Models.Models;

namespace Shows.ApiShows;

public class Controller(IDocumentSession session,TimeProvider clock): ControllerBase
{
    [HttpPost("/api/shows")]
    public async Task<ActionResult> AddAShow(   
        [FromBody] ShowCreateRequest request    // Create a ShowCreateRequest object from the body
        )
    {
        var entity = request.MapToEntity(Guid.NewGuid(), clock.GetLocalNow());
        session.Store(entity);
        await session.SaveChangesAsync();
        ShowDetailsResponse response = entity.MapToResponse();
        return Ok(response);
    }

    [HttpGet("/api/shows")]
    public async Task<ActionResult> GetAllShows(CancellationToken token)
    {
        var response = await session.Query<ShowEntity>()
          
            .ProjectToResponse()   // select from the entities a bunch of ShowDetailResponse   
            .ToListAsync(token);

        return Ok(response.OrderByDescending(s => s.CreatedAt));
    }
}






[Mapper]    // Read up on Mappers
// Read up on partial classes
public static partial class ShowMappers
{
    public static partial ShowEntity MapToEntity(this ShowCreateRequest request, Guid Id, DateTimeOffset createdAt);
    public static partial ShowDetailsResponse MapToResponse(this ShowEntity entity);

    public static partial IQueryable<ShowDetailsResponse> ProjectToResponse(this IQueryable<ShowEntity> q);
}


