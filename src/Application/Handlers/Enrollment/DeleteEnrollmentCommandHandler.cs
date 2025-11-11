using LearnHub.Back.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Back.Application.Handlers.Enrollment
{
    public class DeleteEnrollmentCommandHandler : IRequestHandler<DeleteEnrollmentCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public DeleteEnrollmentCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (enrollment == null)
                throw new KeyNotFoundException($"Enrollment with ID {request.Id} not found");

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}