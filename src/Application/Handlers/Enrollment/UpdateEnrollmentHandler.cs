using AutoMapper;
using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class UpdateEnrollmentHandler : IRequestHandler<UpdateEnrollmentCommand, Unit>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateEnrollmentHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _context.Enrollments.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            _mapper.Map(request, enrollment);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}