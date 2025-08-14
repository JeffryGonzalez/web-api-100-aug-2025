
using System.Net.Http.Json;
using System.Security.Claims;
using Alba;
using Alba.Security;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Software.Api.Vendors;

namespace Software.Tests.Vendors;

[Trait("Category", "SystemsTest")]
public class ManagersCanUpdateVendors
{

    [Fact]

    public async Task CanUpdateAVendorContactInformation()
    {

        var pointOfContact = new PointOfContact
        {
                Name = "Derwyn",
                Email = "test@microsoft.com",
                Phone = "876-5309"
        };
        var host = await AlbaHost.For<Program>(config =>
        {
            config.ConfigureTestServices(services =>
            {
            });
        }, new AuthenticationStub().WithName("sarah@company.com"));

        var postResponse = await host.Scenario(api =>
        {
            api.Post.Json(pointOfContact).ToUrl("/vendors/61ebdca2-fedd-49a3-9aca-9366f8b9a49d/contact");
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.WithClaim(new Claim(ClaimTypes.Role, "Manager"));
            api.StatusCodeShouldBe(201);
        });

        var postResponseBody = await postResponse.ReadAsJsonAsync<VendorDetailsModel>();

        Assert.NotNull(postResponseBody);
        Assert.Equal("sarah@company.com", postResponseBody.CreatedBy);

        var location = postResponse.Context.Response.Headers.Location;
       
        var getResponse = await host.Scenario(api =>
        {
            api.Get.Url(location!);
            api.StatusCodeShouldBeOk();
        });

        var getResponseBody = await getResponse.ReadAsJsonAsync<VendorDetailsModel>();
        Assert.NotNull(getResponse);

        Assert.Equal(postResponseBody, getResponseBody);

    }

    [Fact]
    public async Task InvalidVendorContactUpdateRequestsReturnBadRequest()
    {
        var host = await AlbaHost.For<Program>((_) => { }, new AuthenticationStub());

        var badRequest = new PointOfContact { Name = "", Phone = "", Email = "" };

        await host.Scenario(api =>
        {
            api.Post.Json(badRequest).ToUrl("/vendors/61ebdca2-fedd-49a3-9aca-9366f8b9a49d/contact");
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.WithClaim(new Claim(ClaimTypes.Role, "Manager"));
            api.StatusCodeShouldBe(400);
        });

    }
    [Fact]
    public async Task UnauthorizedWithBadRequestGetsA403()
    {
        var host = await AlbaHost.For<Program>((_) => { }, new AuthenticationStub());

        var badRequest = new PointOfContact { Name = "", Phone = "", Email = "" };

        await host.Scenario(api =>
        {
            api.Post.Json(badRequest).ToUrl("/vendors/61ebdca2-fedd-49a3-9aca-9366f8b9a49d/contact");
            api.WithClaim(new Claim(ClaimTypes.Role, "NotSoftwareCenter"));
            api.WithClaim(new Claim(ClaimTypes.Role, "NotManager"));
            api.StatusCodeShouldBe(403);
        });

    }

    [Fact]
    public async Task UnathenticatedGetsA400()
    {
        var host = await AlbaHost.For<Program>();

        var badRequest = new PointOfContact { Name = "", Phone = "", Email = "" };

        await host.Scenario(api =>
        {
            api.Post.Json(badRequest).ToUrl("/vendors/61ebdca2-fedd-49a3-9aca-9366f8b9a49d/contact");
            api.StatusCodeShouldBe(401);
        });
    }
}

