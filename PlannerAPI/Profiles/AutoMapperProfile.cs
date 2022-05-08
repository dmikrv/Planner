using System.Drawing;
using AutoMapper;
using PlannerAPI.Entities;
using PlannerAPI.Models;
using Color = PlannerAPI.Entities.Color;

namespace PlannerAPI.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Colors, Color>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int) s));

        CreateMap<ActionStates, ActionState>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int) s));
        
        CreateMap<ActionEnergies, Energy>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int) s));
        
        CreateMap<ProjectStates, ProjectState>()
            .ForMember(d => d.Id, o => o.MapFrom(s => (int) s));

    }
}