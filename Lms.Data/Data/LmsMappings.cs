using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class LmsMappings : Profile
    {

        public LmsMappings()
        { 



        CreateMap<Course, CourseDto>().ReverseMap(); 
        CreateMap<Module, ModuleDto>().ReverseMap();
        CreateMap<Module, ModulePostDto>().ReverseMap();
        }
    }
}
