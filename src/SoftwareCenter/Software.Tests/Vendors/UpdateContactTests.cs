using System.Security.Claims;
using Alba;
using Alba.Security;
using Software.Api.Vendors;

namespace Software.Tests.Vendors;

[Trait("Category", "SystemsTest")]
public class UpdateContactTests
{
    [Fact]
    public async Task UpdateContactAsCreatorSucceeds()
    {
        var host = await AlbaHost.For<Program>(new AuthenticationStub());

        var vendorToPost = new VendorCreateModel
        {
            Name = "Microsoft",
            Contact = new PointOfContact
            {
                Name = "Satya",
                Email = "satya@microsoft.com",
                Phone = "555-1212"
            },
            Url = "https://microsoft.com"

        };

        var postResponse = await host.Scenario(api =>
        {
            api.Post.Json(vendorToPost).ToUrl("/vendors");
            api.WithClaim(new Claim(ClaimTypes.Name, "sarah@company.com"));
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.WithClaim(new Claim(ClaimTypes.Role, "Manager"));
            api.IgnoreStatusCode();
        });

        var postResponseBody = await postResponse.ReadAsJsonAsync<VendorDetailsModel>();

        var newContact = new PointOfContact()
        {
            Name = "Sarah Hacker",
            Email = "sarah@company.com"
        };

        var updateResponse = await host.Scenario(api =>
        {
            api.Post.Json(newContact).ToUrl($"/vendors/{postResponseBody.Id}/contact");
            api.WithClaim(new Claim(ClaimTypes.Name, "sarah@company.com"));
            api.StatusCodeShouldBeOk();
        });

        Assert.NotNull(updateResponse);
        var updateResponseBody = await updateResponse.ReadAsJsonAsync<PointOfContact>();

        Assert.Equal(newContact, updateResponseBody);
    }

    [Fact]
    public async Task UpdateContactAsCeoSucceeds()
    {
        var host = await AlbaHost.For<Program>(new AuthenticationStub());

        var vendorToPost = new VendorCreateModel
        {
            Name = "Microsoft",
            Contact = new PointOfContact
            {
                Name = "Satya",
                Email = "satya@microsoft.com",
                Phone = "555-1212"
            },
            Url = "https://microsoft.com"

        };

        var postResponse = await host.Scenario(api =>
        {
            api.Post.Json(vendorToPost).ToUrl("/vendors");
            api.WithClaim(new Claim(ClaimTypes.Name, "sarah@company.com"));
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.WithClaim(new Claim(ClaimTypes.Role, "Manager"));
            api.IgnoreStatusCode();
        });

        var postResponseBody = await postResponse.ReadAsJsonAsync<VendorDetailsModel>();

        var newContact = new PointOfContact()
        {
            Name = "Sarah Hacker",
            Email = "sarah@company.com"
        };

        var updateResponse = await host.Scenario(api =>
        {
            api.Post.Json(newContact).ToUrl($"/vendors/{postResponseBody.Id}/contact");
            api.WithClaim(new Claim(ClaimTypes.Name, "bob@company.com"));
            api.WithClaim(new Claim(ClaimTypes.Role, "CEO"));
            api.StatusCodeShouldBeOk();
        });

        Assert.NotNull(updateResponse);
        var updateResponseBody = await updateResponse.ReadAsJsonAsync<PointOfContact>();

        Assert.Equal(newContact, updateResponseBody);
    }

    [Fact]
    public async Task UpdateContactAsOtherFails()
    {
        var host = await AlbaHost.For<Program>(new AuthenticationStub());

        var vendorToPost = new VendorCreateModel
        {
            Name = "Microsoft",
            Contact = new PointOfContact
            {
                Name = "Satya",
                Email = "satya@microsoft.com",
                Phone = "555-1212"
            },
            Url = "https://microsoft.com"

        };

        var postResponse = await host.Scenario(api =>
        {
            api.Post.Json(vendorToPost).ToUrl("/vendors");
            api.WithClaim(new Claim(ClaimTypes.Name, "sarah@company.com"));
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.WithClaim(new Claim(ClaimTypes.Role, "Manager"));
            api.IgnoreStatusCode();
        });

        var postResponseBody = await postResponse.ReadAsJsonAsync<VendorDetailsModel>();

        var newContact = new PointOfContact()
        {
            Name = "Sarah Hacker",
            Email = "sarah@company.com"
        };

        var updateResponse = await host.Scenario(api =>
        {
            api.Post.Json(newContact).ToUrl($"/vendors/{postResponseBody.Id}/contact");
            api.WithClaim(new Claim(ClaimTypes.Name, "bob@company.com"));
            api.StatusCodeShouldBe(401);
        });
    }

    [Fact]
    public async Task UpdateContactVendorNotFound()
    {
        var host = await AlbaHost.For<Program>(new AuthenticationStub());

        var newContact = new PointOfContact()
        {
            Name = "Sarah Hacker",
            Email = "sarah@company.com"
        };

        var updateResponse = await host.Scenario(api =>
        {
            api.Post.Json(newContact).ToUrl($"/vendors/{Guid.NewGuid()}/contact");
            api.WithClaim(new Claim(ClaimTypes.Name, "sarah@company.com"));
            api.StatusCodeShouldBe(404);
        });
    }

    [Fact]
    public async Task UpdateContactEmptyNameFails()
    {
        var host = await AlbaHost.For<Program>(new AuthenticationStub());

        var vendorToPost = new VendorCreateModel
        {
            Name = "Microsoft",
            Contact = new PointOfContact
            {
                Name = "Satya",
                Email = "satya@microsoft.com",
                Phone = "555-1212"
            },
            Url = "https://microsoft.com"

        };

        var postResponse = await host.Scenario(api =>
        {
            api.Post.Json(vendorToPost).ToUrl("/vendors");
            api.WithClaim(new Claim(ClaimTypes.Name, "sarah@company.com"));
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.WithClaim(new Claim(ClaimTypes.Role, "Manager"));
            api.IgnoreStatusCode();
        });

        var postResponseBody = await postResponse.ReadAsJsonAsync<VendorDetailsModel>();

        var newContact = new PointOfContact()
        {
            Name = "",
            Email = "sarah@company.com"
        };

        var updateResponse = await host.Scenario(api =>
        {
            api.Post.Json(newContact).ToUrl($"/vendors/{postResponseBody.Id}/contact");
            api.WithClaim(new Claim(ClaimTypes.Name, "sarah@company.com"));
            api.StatusCodeShouldBe(400);
        });
    }

    [Fact]
    public async Task UpdateContactEmptyEmailPhoneFails()
    {
        var host = await AlbaHost.For<Program>(new AuthenticationStub());

        var vendorToPost = new VendorCreateModel
        {
            Name = "Microsoft",
            Contact = new PointOfContact
            {
                Name = "Satya",
                Email = "satya@microsoft.com",
                Phone = "555-1212"
            },
            Url = "https://microsoft.com"

        };

        var postResponse = await host.Scenario(api =>
        {
            api.Post.Json(vendorToPost).ToUrl("/vendors");
            api.WithClaim(new Claim(ClaimTypes.Name, "sarah@company.com"));
            api.WithClaim(new Claim(ClaimTypes.Role, "SoftwareCenter"));
            api.WithClaim(new Claim(ClaimTypes.Role, "Manager"));
            api.IgnoreStatusCode();
        });

        var postResponseBody = await postResponse.ReadAsJsonAsync<VendorDetailsModel>();

        var newContact = new PointOfContact()
        {
            Name = "Sarah Hacker"
        };

        var updateResponse = await host.Scenario(api =>
        {
            api.Post.Json(newContact).ToUrl($"/vendors/{postResponseBody.Id}/contact");
            api.WithClaim(new Claim(ClaimTypes.Name, "sarah@company.com"));
            api.StatusCodeShouldBe(400);
        });
    }
}
