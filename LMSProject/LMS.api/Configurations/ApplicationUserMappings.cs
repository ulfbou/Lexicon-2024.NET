using AutoMapper;
using LMS.api.DTO;
using LMS.api.Model;

namespace LMS.api.Configurations
{
    public class ApplicationUserMappings : Profile
    {
        public ApplicationUserMappings()
        {
            CreateMap<ApplicationUser, UserWithRolesDto>().ReverseMap();
        }
    }
}
