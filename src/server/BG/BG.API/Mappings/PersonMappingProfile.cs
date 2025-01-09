using AutoMapper;
using BG.Application.DTOs.Person;
using BG.Core.Entities;

namespace BG.API.Mappings;

public class PersonMappingProfile : Profile
{
    public PersonMappingProfile()
    {
        CreateMap<Person, PersonDto>();
        CreateMap<CreatePersonDto, Person>();
        CreateMap<UpdatePersonDto, Person>();
    }
}