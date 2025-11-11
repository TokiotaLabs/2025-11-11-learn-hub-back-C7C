using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Back.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerTag("Enrollment Management")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EnrollmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all enrollments
        /// </summary>
        /// <returns>List of all enrollments</returns>
        /// <response code="200">Returns the list of enrollments</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Gets all enrollments",
            Description = "Retrieves a complete list of all enrollments in the system")]
        [ProducesResponseType(typeof(List<EnrollmentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EnrollmentDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllEnrollmentsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Gets a specific enrollment by its ID
        /// </summary>
        /// <param name="id">Enrollment ID</param>
        /// <returns>Enrollment data</returns>
        /// <response code="200">Returns the requested enrollment</response>
        /// <response code="404">Enrollment not found</response>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Gets an enrollment by ID",
            Description = "Retrieves the data of a specific enrollment")]
        [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnrollmentDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetEnrollmentByIdQuery { Id = id });
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Creates a new enrollment
        /// </summary>
        /// <param name="command">Enrollment data to create</param>
        /// <returns>Created enrollment</returns>
        /// <response code="201">Enrollment created successfully</response>
        /// <response code="400">Invalid data</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new enrollment",
            Description = "Registers a new enrollment in the system")]
        [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(CreateEnrollmentCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing enrollment
        /// </summary>
        /// <param name="id">ID of the enrollment to update</param>
        /// <param name="command">New enrollment data</param>
        /// <response code="204">Update successful</response>
        /// <response code="400">ID doesn't match or invalid data</response>
        /// <response code="404">Enrollment not found</response>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Updates an enrollment",
            Description = "Updates the data of a specific enrollment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid id, UpdateEnrollmentCommand command)
        {
            if (id != command.Id)
                return BadRequest("The route ID does not match the enrollment ID");

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes an enrollment
        /// </summary>
        /// <param name="id">ID of the enrollment to delete</param>
        /// <response code="204">Deletion successful</response>
        /// <response code="404">Enrollment not found</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletes an enrollment",
            Description = "Permanently deletes an enrollment from the system")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteEnrollmentCommand { Id = id });
            return NoContent();
        }
    }
}