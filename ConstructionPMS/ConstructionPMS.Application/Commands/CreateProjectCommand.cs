using System;

namespace ConstructionPMS.Application.Commands
{
    public class CreateProjectCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}