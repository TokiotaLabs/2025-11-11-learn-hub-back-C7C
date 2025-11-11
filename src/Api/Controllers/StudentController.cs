using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Student;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Back.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerTag("Student Management")]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all registered students
        /// </summary>
        /// <returns>List of all students</returns>
        /// <response code="200">Returns the list of students</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Gets all students",
            Description = "Retrieves a complete list of all students registered in the system")]
        [ProducesResponseType(typeof(List<StudentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<StudentDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllStudentsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Gets a student by their ID
        /// </summary>
        /// <param name="id">Unique ID of the student</param>
        /// <returns>Student data</returns>
        /// <response code="200">Returns the requested student</response>
        /// <response code="404">Student not found</response>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Gets a specific student",
            Description = "Retrieves the data of a specific student based on their ID")]
        [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetStudentByIdQuery { Id = id });
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Creates a new student
        /// </summary>
        /// <param name="command">Student data to create</param>
        /// <returns>Created student</returns>
        /// <response code="201">Student created successfully</response>
        /// <response code="400">Invalid data</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registers a new student",
            Description = "Creates a new student record in the system")]
        [ProducesResponseType(typeof(StudentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(CreateStudentCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing student's data
        /// </summary>
        /// <param name="id">ID of the student to update</param>
        /// <param name="command">New student data</param>
        /// <response code="204">Update successful</response>
        /// <response code="400">ID doesn't match object or invalid data</response>
        /// <response code="404">Student not found</response>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Updates an existing student",
            Description = "Updates the data of a specific student")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid id, UpdateStudentCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes a student from the system
        /// </summary>
        /// <param name="id">ID of the student to delete</param>
        /// <response code="204">Deletion successful</response>
        /// <response code="404">Student not found</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletes a student",
            Description = "Permanently deletes a student from the system")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteStudentCommand { Id = id });
            return NoContent();
        }
    }
}