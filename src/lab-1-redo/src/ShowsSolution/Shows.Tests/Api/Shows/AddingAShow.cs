using Alba;
using Shows.Api.Api.Shows;
using Shows.Tests.Api.Fixtures;

namespace Shows.Tests.Api.Shows;

[Collection("SystemTestFixture")]
[Trait("Category", "SystemTest")]
public class AddingAShow(SystemTestFixture fixture)
{
    private readonly IAlbaHost _host = fixture.Host;

    [Fact]
    public async Task AddShow()
    {
        var response = await _host.Scenario(_ =>
        {
            _.Post.Json(new
            {
                Name = "Test Show",
                Description = "This is a test show",
                StreamingService = "HBO Max"
            }).ToUrl("/api/shows");
            _.StatusCodeShouldBeOk();
        });
        

    }
    [Fact]
    public async Task AddShowFromModel()
    {
        var showRequest = new ShowRequest
        {
            Name = "Severance",
            Description = "Idk",
            StreamingService = "Apple TV"
        };
        var postResponse = await _host.Scenario(_ =>
        {
            _.Post.Json(showRequest).ToUrl("/api/shows");
            _.StatusCodeShouldBeOk();
        });
        var showAdded = postResponse.ReadAsJson<Show>();

        var responseToCheckGET = await _host.Scenario(_ =>
        {
            _.Get.Url($"/api/shows/{showAdded.Id}");
            _.StatusCodeShouldBeOk();
        });
    }
    
}