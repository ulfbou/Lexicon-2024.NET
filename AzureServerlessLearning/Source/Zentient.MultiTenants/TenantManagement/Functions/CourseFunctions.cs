using AutoMapper;
using Azure.Core;
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
        ITenantService<Course> service,
        ILogger<CourseFunctions> logger)
    {
        private readonly ITenantService<Course> _service = service ?? throw new ArgumentNullException(nameof(_service));
        private readonly ILogger<CourseFunctions> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Post: api/courses
        [Function("CreateCourse")]
        public async Task<IActionResult> CreateCourse(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "courses")] HttpRequest request)
        {
            if (request == null)
            {
                // TenantService logs the exception
                return new BadRequestResult();
            }

            try
            {
                var dto = await request.ReadFromJsonAsync<CreateCourseDto>();
                if (dto == null) return new BadRequestResult();

                string? tenantId = request?.Headers["X-Tenant-Id"];

                var course = await _service.CreateAsync(tenantId ?? Tenant.DefaultTenantKey, dto, CancellationToken.None);
                if (course == null) return new StatusCodeResult(StatusCodes.Status404NotFound);

                return new CreatedResult($"/api/courses/{course.Id}", dto);
            }
            catch (Exception)
            {
                // TenantService logs the exception
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // Get: api/courses
        [Function("GetCourses")]
        public async Task<IActionResult> GetCourses([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "courses")] HttpRequest request,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            [FromQuery] string? filter)
        {
            try
            {
                // Generate an expression for pagination and filtering
                Expression<Func<Course, bool>> expression = course => course.PartitionKey == Tenant.DefaultTenantKey;

                string? tenantId = request?.Headers["X-Tenant-Id"];

                var dtos = await _service.GetBulkAsync<ReadCourseDto>(tenantId ?? Tenant.DefaultTenantKey, null, CancellationToken.None);
                return new OkObjectResult(dtos);
            }
            catch (Exception)
            {
                // TenantService logs the exception
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
                string? tenantId = request?.Headers["X-Tenant-Id"];

                var course = await _service.GetAsync<ReadCourseDto>(tenantId ?? Tenant.DefaultTenantKey, id, CancellationToken.None);
                if (course == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(course);
            }
            catch (Exception)
            {
                // TenantService logs the exception
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

                string? tenantId = request?.Headers["X-Tenant-Id"];

                UpdateCourseDto createdDto = await _service.UpdateAsync(tenantId ?? Tenant.DefaultTenantKey, dto, CancellationToken.None);

                return new OkObjectResult(createdDto);
            }
            catch (Exception)
            {
                // TenantService logs the exception
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
                string? tenantId = request?.Headers["X-Tenant-Id"];

                await _service.DeleteAsync(tenantId ?? Tenant.DefaultTenantKey, id, CancellationToken.None);

                return new OkResult();
            }
            catch (Exception)
            {
                // TenantService logs the exception
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}