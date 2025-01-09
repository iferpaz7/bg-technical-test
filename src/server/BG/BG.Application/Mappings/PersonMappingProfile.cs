using BG.Application.DTOs.Person;

namespace BG.Application.Mappings;

public class PersonMappingProfile: Profile
{
    public PersonMappingProfile()
    {
        CreateMap<Person, PersonDto>();
        CreateMap<CreatePersonDto, Person>();
        CreateMap<UpdatePersonDto, Person>();
    }
}