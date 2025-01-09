using AutoMapper;
using BG.Application.DTOs;
using BG.Core.Entities;

namespace BG.API.Mappings;

public class IdentificationTypeMappingProfile : Profile
{
    public IdentificationTypeMappingProfile()
    {
        CreateMap<IdentificationType, IdentificationTypeDto>();
        CreateMap<IdentificationTypeDto, IdentificationType>();
    }
}
