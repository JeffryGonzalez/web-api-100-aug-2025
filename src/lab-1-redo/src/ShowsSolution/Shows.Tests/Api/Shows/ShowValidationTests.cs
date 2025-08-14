using Alba;
using Shows.Tests.Api.Fixtures;
using Xunit;

namespace Shows.Tests.Api.Shows;

[Collection("SystemTestFixture")]
[Trait("Category", "SystemTest")]
public class ShowValidationTests(SystemTestFixture fixture)
{
    private readonly IAlbaHost _host = fixture.Host;

    [Fact]
    public async Task RejectsInvalidShowRequests()
    {
        var badRequest = new
        {
            name = "A", // too short
            description = "Short", // too short
            streamingService = "" // missing
        };

        await _host.Scenario(s =>
        {
            s.Post.Json(badRequest).ToUrl("/api/shows");
            s.StatusCodeShouldBe(400);
        });
    }
}