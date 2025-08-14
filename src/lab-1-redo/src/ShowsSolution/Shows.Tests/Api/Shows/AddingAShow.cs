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
        var show = new Show
        {
            Id = Guid.NewGuid(),
            Name = "Severance",
            Description = "Idk",
            StreamingService = "Apple TV",
            CreatedAt = DateTime.UtcNow
        };
        var postResponse = await _host.Scenario(_ =>
        {
            _.Post.Json(show).ToUrl("/api/shows");
            _.StatusCodeShouldBeOk();
        });
        //var postStatusCode = postResponse.StatusCode();

        /*var checkShow = response.ReadAsJson<Show>();
        Assert.NotNull(checkShow);
        Assert.Equal(show.Name, checkShow.Name);
        Assert.Equal(show.Description, checkShow.Description);
        Assert.Equal(show.StreamingService, checkShow.StreamingService);*/

        /*var responseToCheckGET = await _host.Scenario(_ =>
        {
            _.Get.Json(show.Id).ToUrl("/api/shows/{id}");
            _.StatusCodeShouldBeOk();
        });*/
    }
    
}