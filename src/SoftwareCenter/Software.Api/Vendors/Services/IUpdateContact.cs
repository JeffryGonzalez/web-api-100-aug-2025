namespace Software.Api.Vendors.Services;

public interface IUpdateContact
{
    Task<PointOfContact> UpdateContactAsync(VendorEntity vendor, PointOfContact request);
}
