using AutoMapper;
using contactAppMicroservice.DTO.Response;
using contactAppMicroservice.Entities;

namespace contactAppMicroservice.AutoMapper
{
    public class ContactMappingProfile: Profile
    {
        public ContactMappingProfile() {
            CreateMap<Contact, ContactResponse>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<Subcategory, SubcategoryResponse>();
            CreateMap<Contact, ContactDetailsResponse>();
        }
    }
}
