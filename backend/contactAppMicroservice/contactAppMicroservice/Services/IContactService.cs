using contactAppMicroservice.DTO.Response;

namespace contactAppMicroservice.Services
{
    public interface IContactService
    {
        Task<List<ContactResponse>?> getAllContacts();
        Task<ContactDetailsResponse?> getContactById(Guid contactId);
        Task<bool> deleteContact(Guid contactId, string password);
    }
}
