using contactAppMicroservice.Entities;

namespace contactAppMicroservice.DTO.Response
{
    public class ContactDetailsResponse
    {
        public Guid ContactId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public CategoryResponse Category { get; set; }
        public SubcategoryResponse? Subcategory { get; set; }
    }
}
