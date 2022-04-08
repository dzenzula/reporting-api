using AutoMapper;
using ScalesMWebAPI.Dtos;
using ScalesMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScalesMWebAPI.MappingProfiles
{
    public class DtoMapping : Profile
    {
        public DtoMapping() 
        {
            CreateMap<SensorCapture, AddSensorValueDto>().ReverseMap();
            CreateMap<WeightSensor, AddWeightSensorDto>().ReverseMap();
            CreateMap<WeightPlatform, AddWeightPlatformDto>().ReverseMap();
            CreateMap<LogErrorMessage, AddLogErrorMessageDto>().ReverseMap();
            CreateMap<AssigmentPoint, AddAssigmentPointDto>().ReverseMap();
            CreateMap<WeightPlc, AddWeightPlcDto>().ReverseMap();
            CreateMap<TypePlc, AddTypePlcDto>().ReverseMap();
            CreateMap<LocationPoint, AddLocationPointDto>().ReverseMap();
            CreateMap<WeightPoint, AddWeightPointDto>().ReverseMap();
            
            CreateMap<GetAssigmentPointDto, AssigmentPoint> ().ReverseMap();

            CreateMap<PlatformSensorValueDto, SensorCapture>().ReverseMap();
            CreateMap<UpdateLocationPoint, LocationPoint>().ReverseMap();
            CreateMap<UpdateWeightSensorDataDto,WeightSensor>().ReverseMap();
            CreateMap<UpdateWeightPlatformDto, WeightPlatform>().ReverseMap();
            CreateMap<UpdateWeightPointDto, WeightPoint>().ReverseMap();
            CreateMap<UpdateWeightPlcDto, WeightPlc>().ReverseMap();
        }
    }
}
