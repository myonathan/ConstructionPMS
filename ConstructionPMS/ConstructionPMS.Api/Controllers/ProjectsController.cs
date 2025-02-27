using Confluent.Kafka; // Assuming you are using Confluent.Kafka for Kafka integration
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Services;
using ConstructionPMS.Services.NotificationService;
using Microsoft.AspNetCore.Authorization; // Import the authorization namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Exceptions;

namespace ConstructionPMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IProjectService _projectService;
        private readonly INotificationService _notificationService;
        private readonly IProducer<Null, string> _kafkaProducer; // Kafka producer for sending messages

        public ProjectsController(IConfiguration configuration, IProjectService projectService, INotificationService notificationService, IProducer<Null, string> kafkaProducer)
        {
            _configuration = configuration;
            _projectService = projectService;
            _notificationService = notificationService;
            _kafkaProducer = kafkaProducer;
        }

        // GET: api/projects
        [Authorize] // Require authentication
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects()
        {
            // Fetch projects from Elasticsearch for list view
            var projects = await _projectService.GetAllProjectsFromElasticSearchAsync();
            return Ok(projects);
        }

        // GET: api/projects/{id}
        [Authorize] // Require authentication
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            // Fetch project details from Elasticsearch
            var project = await _projectService.GetProjectByIdFromElasticSearchAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        // POST: api/projects
        [Authorize(Roles = "Admin")] // Require authentication and Admin role
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject([FromBody] Project project)
        {
            if (project == null)
            {
                return BadRequest("Project cannot be null.");
            }

            try
            {
                var createdProject = await _projectService.CreateProjectAsync(project);

                // Send a notification after creating a project
                await _notificationService.SendNotificationAsync($"Project '{createdProject.ProjectName}' has been created.");

                // Publish to Kafka topic for storing in Elasticsearch
                var message = $"Created project: {createdProject.ProjectId}";
                await _kafkaProducer.ProduceAsync(_configuration["Kafka:Topic"], new Message<Null, string> { Value = message });

                return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.ProjectId }, createdProject);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/projects/{id}
        [Authorize(Roles = "Admin")] // Require authentication and Admin role
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest("Project ID mismatch.");
            }

            try
            {
                await _projectService.UpdateProjectAsync(project);

                // Send a notification after updating a project
                await _notificationService.SendNotificationAsync($"Project '{project.ProjectName}' has been updated.");

                // Publish to Kafka topic for updating in Elasticsearch
                var message = $"Updated project: {project.ProjectId}";
                await _kafkaProducer.ProduceAsync(_configuration["Kafka:Topic"], new Message<Null, string> { Value = message });

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/projects/{id}
        [Authorize(Roles = "Admin")] // Require authentication and Admin role
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            await _projectService.DeleteProjectAsync(id);

            // Send a notification after deleting a project
            await _notificationService.SendNotificationAsync($"Project '{project.ProjectName}' has been deleted.");

            // Publish to Kafka topic for deletion in Elasticsearch
            var message = $"Deleted project: {project.ProjectId}";
            await _kafkaProducer.ProduceAsync(_configuration["Kafka:Topic"], new Message<Null, string> { Value = message });

            return NoContent();
        }
    }
}