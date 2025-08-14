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

        var show = response.ReadAsJson<ShowResponse>();
        var showId = show.Id; 

        var getResponse = await _host.Scenario(_ =>
        {
            _.Get.Url($"/api/shows/{showId}");
            _.StatusCodeShouldBeOk();
        });

        var retrievedShow = getResponse.ReadAsJson<ShowResponse>();
        Assert.Equal("Test Show", retrievedShow.Name);
        Assert.Equal("This is a test show", retrievedShow.Description);
        Assert.Equal("HBO Max", retrievedShow.StreamingService);
        Assert.Equal(showId, retrievedShow.Id);

    }


    
}