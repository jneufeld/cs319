using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace DREAM.Models
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            /*
            Mapper.CreateMap<int, string>().ConvertUsing(o => o.ToString());
            Mapper.CreateMap<string, int>().ConvertUsing(Convert.ToInt32);
            Mapper.CreateMap<Request, RequestViewModel>()
                .ForMember(dest => dest.RequestID, opt => opt.MapFrom(r => r.ID))
                .ForMember(dest => dest.RequestTypeString, opt => opt.MapFrom(r => r.Type.StringID))
                .ForMember(dest => dest.CallerRegionString, opt => opt.MapFrom(r => r.Caller.Region.StringID))
                .ForMember(dest => dest.PatientGenderString, opt => opt.MapFrom(r => r.Patient.Gender));
            */
            //.ForMember(dest => dest.Questions, opt => opt.UseDestinationValue())
        }
    }
}