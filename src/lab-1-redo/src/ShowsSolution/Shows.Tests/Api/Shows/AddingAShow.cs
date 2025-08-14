using Alba;
using Shows.Tests.Api.Fixtures;
using Xunit;

namespace Shows.Tests.Api.Shows;

[Collection("SystemTestFixture")]
[Trait("Category", "SystemTest")]
public class AddingAShow(SystemTestFixture fixture)
{
    private readonly IAlbaHost _host = fixture.Host;

    public record ShowResponse(
        Guid Id,
        string Name,
        string Description,
        string StreamingService,
        DateTimeOffset CreatedAt
    );

    [Fact]
    public async Task AddShow()
    {
        var request = new
        {
            name = "Twin Peaks the Return",
            description = "David Lynch at his best",
            streamingService = "Amazon Prime"
        };

        var postResponse = await _host.Scenario(s =>
        {
            s.Post.Json(request).ToUrl("/api/shows");
            s.StatusCodeShouldBe(200);
        });

        var show = postResponse.ReadAsJson<ShowResponse>();

        var getResponse = await _host.Scenario(s =>
        {
            s.Get.Url($"/api/shows/{show.Id}");
            s.StatusCodeShouldBe(200);
        });

        var fetchedShow = getResponse.ReadAsJson<ShowResponse>();
        Assert.Equal(show.Id, fetchedShow.Id);
        Assert.Equal(show.Name, fetchedShow.Name);
        Assert.Equal(show.Description, fetchedShow.Description);
        Assert.Equal(show.StreamingService, fetchedShow.StreamingService);
    }
}