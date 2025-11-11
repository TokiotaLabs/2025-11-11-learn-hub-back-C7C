using AutoMapper;
using LearnHub.Back.Domain;
using LearnHub.Back.Application.DTOs;

namespace LearnHub.Back.Application.Mappings;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentDto>()
            .ReverseMap()
            .ForMember(dest => dest.Enrollments, opt => opt.Ignore());
    }
}