
namespace Software.Api.Vendors;

public interface ICreateVendors
{
    Task<VendorDetailsModel> CreateVendorAsync(VendorCreateModel request);
}

public interface IUpdateVendors
{
    Task<VendorDetailsModel?> UpdateVendorContadctByIdAsync(Guid id, PointOfContact pointOfContact, CancellationToken token);
}