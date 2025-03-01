using Confluent.Kafka; // Assuming you are using Confluent.Kafka for Kafka integration
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Exceptions;
using ConstructionPMS.Services;
using ConstructionPMS.Services.NotificationService;
using Microsoft.AspNetCore.Authorization; // Import the authorization namespace
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConstructionPMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class ProjectsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IProducer<Null, string> _kafkaProducer; // Kafka producer for sending messages

        public ProjectsController(IConfiguration configuration, IProjectService projectService, IUserService userService, INotificationService notificationService, IProducer<Null, string> kafkaProducer)
        {
            _configuration = configuration;
            _projectService = projectService;
            _notificationService = notificationService;
            _kafkaProducer = kafkaProducer;
            _userService = userService;
        }

        [Authorize] // Require authentication
        [HttpGet("GetAllKafkaProjects")]
        public async Task<ActionResult<IEnumerable<Project>>> GetAllKafkaProjects()
        {
            // Fetch projects from Elasticsearch for list view
            var projects = await _projectService.GetAllProjectsFromElasticSearchAsync();
            return Ok(projects);
        }

        [Authorize] // Require authentication
        [HttpGet("GetKafkaProjectById/{id}")]
        public async Task<ActionResult<Project>> GetKafkaProjectById(int id)
        {
            // Fetch project details from Elasticsearch
            var project = await _projectService.GetProjectByIdFromElasticSearchAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }


        // GET: api/projects
        [Authorize] // Require authentication
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        // GET: api/projects/{id}
        [Authorize] // Require authentication
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [Authorize(Roles = "Admin")] // Require authentication and Admin role
        [HttpPost("CreateKafkaProject")]
        public async Task<ActionResult<Project>> CreateKafkaProject([FromBody] Project project)
        {
            if (project == null)
            {
                return BadRequest("Project cannot be null.");
            }

            try
            {
                // Retrieve the user's email from the claims
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var user = await _userService.GetUserByEmailAsync(userEmail);

                project.ProjectCreatorId = user.Id;

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
                // Retrieve the user's email from the claims
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var user = await _userService.GetUserByEmailAsync(userEmail);

                project.ProjectCreatorId = user.Id;

                var createdProject = await _projectService.CreateProjectAsync(project);

                return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.ProjectId }, createdProject);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")] // Require authentication and Admin role
        [HttpPut("UpdateKafkaProject")]
        public async Task<IActionResult> UpdateKafkaProject([FromBody] Project project)
        {
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

        // PUT: api/projects
        [Authorize(Roles = "Admin")] // Require authentication and Admin role
        [HttpPut]
        public async Task<IActionResult> UpdateProject([FromBody] Project project)
        {
            try
            {
                await _projectService.UpdateProjectAsync(project);

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")] // Require authentication and Admin role
        [HttpDelete("DeleteKafkaProject/{id}")]
        public async Task<IActionResult> DeleteKafkaProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);

            await _projectService.DeleteProjectAsync(id);
            var input = project?.ProjectName ?? id.ToString();

            // Send a notification after deleting a project
            await _notificationService.SendNotificationAsync($"Project '{input}' has been deleted.");

            // Publish to Kafka topic for deletion in Elasticsearch
            var message = $"Deleted project: {id}";

            await _kafkaProducer.ProduceAsync(_configuration["Kafka:Topic"], new Message<Null, string> { Value = message });

            return NoContent();
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

            return NoContent();
        }
    }
}