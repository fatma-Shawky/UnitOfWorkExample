using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UnitOfWorkExample.core.Entities;
using UnitOfWorkExample.Core.DTOs;
namespace UnitOfWorkExample.Core.Mappings
{
   

    namespace MyProject.Core.Mappings
    {
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Product, ProductDto>().ReverseMap();
                CreateMap<Category, CategoryDto>().ReverseMap();
            }
        }
    }

}
