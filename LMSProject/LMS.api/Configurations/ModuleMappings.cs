using AutoMapper;
using LMS.api.Model;

namespace LMS.api.Configurations
{
    public class ModuleMappings : Profile
    {
        public ModuleMappings()
        {
            CreateMap<Module, ModuleDTO>().ReverseMap();
        }
    }
}
