using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Course;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace LearnHub.Back.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerTag("Course Management")]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all available courses
        /// </summary>
        /// <returns>List of courses</returns>
        /// <response code="200">Returns the list of courses</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Gets all courses",
            Description = "Retrieves a complete list of all available courses")]
        [ProducesResponseType(typeof(List<CourseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CourseDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllCoursesQuery());
            return Ok(result);
        }

        /// <summary>
        /// Gets a specific course by its ID
        /// </summary>
        /// <param name="id">Course ID</param>
        /// <returns>Detailed course information</returns>
        /// <response code="200">Returns the requested course</response>
        /// <response code="404">Course not found</response>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Gets a course by ID",
            Description = "Retrieves detailed information about a specific course")]
        [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CourseDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCourseByIdQuery { Id = id });
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Creates a new course
        /// </summary>
        /// <param name="command">Course data to create</param>
        /// <returns>Created course</returns>
        /// <response code="201">Course created successfully</response>
        /// <response code="400">Invalid data</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new course",
            Description = "Registers a new course in the system")]
        [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create(CreateCourseCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing course
        /// </summary>
        /// <param name="id">ID of the course to update</param>
        /// <param name="command">New course data</param>
        /// <response code="204">Update successful</response>
        /// <response code="400">ID doesn't match or invalid data</response>
        /// <response code="404">Course not found</response>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Updates a course",
            Description = "Updates the information of an existing course")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid id, UpdateCourseCommand command)
        {
            if (id != command.Id)
                return BadRequest("The route ID does not match the course ID");
            
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes a course
        /// </summary>
        /// <param name="id">ID of the course to delete</param>
        /// <response code="204">Deletion successful</response>
        /// <response code="404">Course not found</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletes a course",
            Description = "Permanently deletes a course from the system")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteCourseCommand { Id = id });
            return NoContent();
        }
    }
}