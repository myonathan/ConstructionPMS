using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Services;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Services.NotificationService;

namespace ConstructionPMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectTasksController : ControllerBase
    {
        private readonly ITaskService _projectTaskService;
        private readonly INotificationService _notificationService;

        public ProjectTasksController(ITaskService projectTaskService, INotificationService notificationService)
        {
            _projectTaskService = projectTaskService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTask>>> GetAllTasks()
        {
            var tasks = await _projectTaskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTask>> GetTaskById(Guid id)
        {
            var task = await _projectTaskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectTask>> CreateTask([FromBody] ProjectTask task)
        {
            var createdTask = await _projectTaskService.CreateTaskAsync(task);

            // Send a notification after creating a task
            await _notificationService.SendNotificationAsync($"Task '{createdTask.Title}' has been created.");

            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] ProjectTask task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            await _projectTaskService.UpdateTaskAsync(task);

            // Send a notification after updating a task
            await _notificationService.SendNotificationAsync($"Task '{task.Title}' has been updated.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _projectTaskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            await _projectTaskService.DeleteTaskAsync(id);

            // Send a notification after deleting a task
            await _notificationService.SendNotificationAsync($"Task '{task.Title}' has been deleted.");

            return NoContent();
        }
    }
}