using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionPMS.Domain.Entities;
using ConstructionPMS.Domain.Interfaces;
using ConstructionPMS.Infrastructure.Repositories;
using ConstructionPMS.Shared.Exceptions;

namespace ConstructionPMS.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<ProjectTask>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async Task<ProjectTask> GetTaskByIdAsync(Guid taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                throw new CustomException($"Task with ID {taskId} not found.");
            }
            return task;
        }

        public async Task<ProjectTask> CreateTaskAsync(ProjectTask task)
        {
            if (task == null)
            {
                throw new CustomException("Task cannot be null.");
            }
            return await _taskRepository.AddAsync(task);
        }

        public async Task UpdateTaskAsync(ProjectTask task)
        {
            if (task == null)
            {
                throw new CustomException("Task cannot be null.");
            }
            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(Guid taskId)
        {
            await _taskRepository.DeleteAsync(taskId);
        }
    }
}