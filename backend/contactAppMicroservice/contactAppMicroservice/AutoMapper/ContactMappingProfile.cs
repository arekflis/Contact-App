using AutoMapper;
using contactAppMicroservice.DTO.Request;
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

            CreateMap<ContactRequest, Contact>()
                .ForMember(dest => dest.ContactId, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Subcategory, opt => opt.Ignore())
                .ForMember(dest => dest.SubcategoryId, opt => opt.Ignore());
                    
        }
    }
}
