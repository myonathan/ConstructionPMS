using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Services;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Services.NotificationService;
using System.ComponentModel.DataAnnotations;

namespace ConstructionPMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly INotificationService _notificationService;

        public ProjectsController(IProjectService projectService, INotificationService notificationService)
        {
            _projectService = projectService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

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

                return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.ProjectId }, createdProject);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

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

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

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

            return NoContent();
        }
    }
}