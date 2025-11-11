using MediatR;
using LearnHub.Back.Application.DTOs;
using System.Collections.Generic;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class GetAllEnrollmentsQuery : IRequest<List<EnrollmentDto>>
    {
    }
}