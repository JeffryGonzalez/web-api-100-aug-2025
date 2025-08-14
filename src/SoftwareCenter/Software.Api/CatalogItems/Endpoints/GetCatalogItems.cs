using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Software.Api.CatalogItems.Representations;

namespace Software.Api.CatalogItems.Endpoints;

public static class GetCatalogItems
{
    public static async Task<Results<Ok<IReadOnlyList<CatalogItemResponse>>, NotFound>> Handle(
        Guid vendorId,
        IDocumentSession session,
        CancellationToken token)
    {
        var catalogItems = await session.Query<CatalogItemResponse>().Where(x => x.Id == vendorId).ToListAsync(token);

        return TypedResults.Ok(catalogItems);
    }
}