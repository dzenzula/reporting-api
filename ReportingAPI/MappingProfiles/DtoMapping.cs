using AutoMapper;
using ReportingApi.Dtos;
using ReportingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingApi.MappingProfiles
{
    public class DtoMapping : Profile
    {
        public DtoMapping()
        {
            CreateMap<UpdateCategory, Category>().ReverseMap();
        }
    }
}
