using AutoMapper;
using LMS.api.DTO;
using LMS.api.Model;

namespace LMS.api.Configurations
{
    public class ActivityMappings : Profile
    {
        public ActivityMappings()
        {
            CreateMap<ActivityCreateDTO, Activity>();

            CreateMap<ActivityUpdateDTO, Activity>();
        }
    }
}
