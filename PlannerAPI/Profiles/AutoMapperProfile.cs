using System.Drawing;
using AutoMapper;
using PlannerAPI.Database.Entities;
using PlannerAPI.Models;
using Color = PlannerAPI.Database.Entities.Color;

namespace PlannerAPI.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Tag, LabelTagModel>()
            .ForMember(d => d.Color, o 
                => o.PreCondition(s => s.Color is not null))
            .ForMember(d => d.Color, o 
                => o.MapFrom(s => (ColorModel)s.Color )).ReverseMap();
        
        
        // CreateMap<Color, ColorModel>();
        // CreateMap<Color, Colors>()
        //     .ForMember(d => d, o => o.MapFrom(s => (Colors) s.Id));

        // CreateMap<Colors, Color>()
        //     .ForMember(d => d.Id, o => o.MapFrom(s => (int) s)).ReverseMap();
        //
        // CreateMap<ActionStates, ActionState>()
        //     .ForMember(d => d.Id, o => o.MapFrom(s => (int) s));
        //
        // CreateMap<ActionEnergies, Energy>()
        //     .ForMember(d => d.Id, o => o.MapFrom(s => (int) s));
        //
        // CreateMap<ProjectStates, ProjectState>()
        //     .ForMember(d => d.Id, o => o.MapFrom(s => (int) s));

        // CreateMap<Tag, TagModel>()
        //     .ForMember(d => d.Type, o => o.MapFrom(x => TagTypes.LABEL))
        //     .ForMember(d => d.Color, o => o.MapFrom(x => (TagTypes)x.Color.Id));
    }
}