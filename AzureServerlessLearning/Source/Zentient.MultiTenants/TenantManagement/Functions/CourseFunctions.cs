using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TenantManagement.DTOs;
using TenantManagement.Entities;
using TenantManagement.Services;

namespace TenantManagement.Functions
{
    public class CourseFunctions(
        IService<Course> service,
        ILogger<CourseFunctions> logger)
    {
        private readonly IService<Course> _service = service ?? throw new ArgumentNullException(nameof(_service));
        private readonly ILogger<CourseFunctions> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Post: api/courses
        [Function("CreateCourse")]
        public async Task<IActionResult> CreateCourse(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "courses")] HttpRequest request)
        {
            try
            {
                var dto = await request.ReadFromJsonAsync<CreateCourseDto>();
                if (dto == null) return new BadRequestResult();

                var course = await _service.CreateAsync(dto, CancellationToken.None);
                if (course == null) return new StatusCodeResult(StatusCodes.Status404NotFound);

                return new CreatedResult($"/api/courses/{course.Id}", dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // Get: api/courses
        [Function("GetCourses")]
        public async Task<IActionResult> GetCourses([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "courses")] HttpRequest req)
        {
            try
            {
                var dtos = await _service.GetBulkAsync<ReadCourseDto>(null, CancellationToken.None);
                return new OkObjectResult(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // Get: api/courses/{id}
        [Function("GetCourse")]
        public async Task<IActionResult> GetCourse(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "courses/{id}")] HttpRequest request, string id)
        {
            try
            {
                var course = await _service.GetAsync<ReadCourseDto>(id, CancellationToken.None);
                if (course == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // Put: api/courses/{id}
        [Function("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "courses/{id}")] HttpRequest request, string id)
        {
            try
            {
                var dto = await request.ReadFromJsonAsync<UpdateCourseDto>();
                if (dto == null)
                {
                    return new BadRequestResult();
                }

                UpdateCourseDto createdDto = await _service.UpdateAsync(dto, CancellationToken.None);

                return new OkObjectResult(createdDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // Delete: api/courses/{id}
        [Function("DeleteCourse")]
        public async Task<IActionResult> DeleteCourse(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "courses/{id}")] HttpRequest request, string id)
        {
            try
            {
                await _service.DeleteAsync(id, CancellationToken.None);

                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}