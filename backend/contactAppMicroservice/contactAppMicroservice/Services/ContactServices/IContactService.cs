using contactAppMicroservice.DTO.Request;
using contactAppMicroservice.DTO.Response;
using contactAppMicroservice.Entities;

namespace contactAppMicroservice.Services.ContactServices
{
    public interface IContactService
    {
        Task<List<ContactResponse>?> getAllContactsAsync();
        Task<ContactDetailsResponse?> getContactByIdAsync(Guid contactId);
        Task<bool> deleteContactAsync(Guid contactId, string password);
        Task<ContactDetailsResponse> addNewContactAsync(ContactRequest contactRequest);
        Task updateContactAsync(Guid contactId, ContactRequest contactRequest);
    }
}
