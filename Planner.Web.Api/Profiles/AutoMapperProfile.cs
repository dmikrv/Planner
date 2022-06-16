using AutoMapper;
using Planner.Data.Entities;
using Planner.Web.Api.Models;
using Action = Planner.Data.Entities.Action;

namespace Planner.Web.Api.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Tag, LabelTagModel>()
            .ForMember(d => d.Color, o 
                => o.PreCondition(s => s.Color is not null))
            .ForMember(d => d.Color, o 
                => o.MapFrom(s => (ColorModel)s.Color )).ReverseMap();
        
        CreateMap<Area, AreaTagModel>()
            .ForMember(d => d.Color, o 
                => o.PreCondition(s => s.Color is not null))
            .ForMember(d => d.Color, o 
                => o.MapFrom(s => (ColorModel)s.Color )).ReverseMap();
        
        CreateMap<Contact, ContactTagModel>()
            .ForMember(d => d.Color, o 
                => o.PreCondition(s => s.Color is not null))
            .ForMember(d => d.Color, o 
                => o.MapFrom(s => (ColorModel)s.Color )).ReverseMap();

        CreateMap<TrashAction, TrashActionModel>()
            .ForMember(d => d.Text, o 
                => o.MapFrom(s => s.Name )).ReverseMap();

        CreateMap<Action, ActionModel>()
            .ForMember(d => d.Energy, o 
                => o.PreCondition(s => s.Energy is not null))
            .ForMember(d => d.Energy, o 
                => o.MapFrom(s => (ActionModel.EnergyLevelModel)s.Energy ))
            
            .ForMember(d => d.AreaTags, o 
                => o.MapFrom(s => s.Areas ))
            
            .ForMember(d => d.ContactTags, o 
                => o.MapFrom(s => s.Contacts ))
            
            .ForMember(d => d.LabelTags, o 
                => o.MapFrom(s => s.Tags ))
            .ReverseMap();
        
        CreateMap<Project, ProjectModel>().ReverseMap();
    }
}