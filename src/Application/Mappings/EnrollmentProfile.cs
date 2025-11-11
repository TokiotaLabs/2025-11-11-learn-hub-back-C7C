using AutoMapper;
using LearnHub.Back.Domain;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Mappings;

public class EnrollmentProfile : Profile
{
    public EnrollmentProfile()
    {
        CreateMap<Enrollment, EnrollmentDto>()
            .ReverseMap()
            .ForMember(dest => dest.Course, opt => opt.Ignore())
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.Payment, opt => opt.Ignore());
    }
}