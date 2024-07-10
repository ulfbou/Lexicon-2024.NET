using AutoMapper;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenantManagement.DTOs;
using TenantManagement.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TenantManagement.Profiles
{
    // Automapper profile for mapping between DTOs and entities
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<Course, CreateCourseDto>().ReverseMap();
            CreateMap<Course, ReadCourseDto>().ReverseMap();
            CreateMap<Course, UpdateCourseDto>().ReverseMap();
        }
    }
}