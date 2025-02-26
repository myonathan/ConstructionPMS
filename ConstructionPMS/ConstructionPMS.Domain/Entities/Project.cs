using System;
using System.Collections.Generic;

namespace ConstructionPMS.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public List<ProjectTask> Tasks { get; private set; }

        public Project(string name, string description, DateTime startDate, DateTime endDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Tasks = new List<ProjectTask>();
        }

        // Additional methods for business logic can be added here
    }
}