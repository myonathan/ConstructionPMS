using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ConstructionPMS.Domain.Entities
{
    public class Project
    {
        private DateTime _constructionStartDate;

        [Key]
        [Required]
        public int ProjectId { get; set; } // 6-digit unique number

        [Required(ErrorMessage = "Project Name is required.")]
        [MaxLength(200, ErrorMessage = "Project Name cannot exceed 200 characters.")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Project Location is required.")]
        [MaxLength(500, ErrorMessage = "Project Location cannot exceed 500 characters.")]
        public string ProjectLocation { get; set; }

        [Required(ErrorMessage = "Project Stage is required.")]
        public ProjectStage ProjectStage { get; set; } // Enum for Project Stage

        [Required(ErrorMessage = "Project Category is required.")]
        public ProjectCategory ProjectCategory { get; set; } // Enum for Project Category

        public string OtherCategory { get; set; } // For user-defined categories (optional)

        [Required(ErrorMessage = "Construction Start Date is required.")]
        [DataType(DataType.Date)]
        public DateTime ConstructionStartDate
        {
            get => _constructionStartDate.ToUniversalTime();
            set => _constructionStartDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        [Required(ErrorMessage = "Project Details/Description is required.")]
        [MaxLength(2000, ErrorMessage = "Project Details/Description cannot exceed 2000 characters.")]
        public string ProjectDetails { get; set; }

        [Required(ErrorMessage = "Project Creator ID is required.")]
        public Guid ProjectCreatorId { get; set; } // UserID

        // Constructor for creating a new project
        public Project()
        {
        }

        // Constructor for editing an existing project
        public Project(int projectId, string projectName, string projectLocation, ProjectStage projectStage,
                       ProjectCategory projectCategory, string otherCategory, DateTime constructionStartDate,
                       string projectDetails, Guid projectCreatorId)
        {
            ProjectId = projectId; // This will invoke the validation logic
            ProjectName = projectName;
            ProjectLocation = projectLocation;
            ProjectStage = projectStage;
            ProjectCategory = projectCategory;
            OtherCategory = otherCategory; // This can be null or empty
            ConstructionStartDate = constructionStartDate;
            ProjectDetails = projectDetails;
            ProjectCreatorId = projectCreatorId;
        }

        // Method to validate the project based on the specified rules
        public void Validate()
        {
            // Validate ProjectId
            if (!IsValidProjectId(ProjectId.ToString()))
            {
                throw new ValidationException("Project ID must be a 6-digit unique number.");
            }

            // Validate Construction Start Date
            if (IsFutureConstructionDateRequired())
            {
                if (ConstructionStartDate <= DateTime.UtcNow)
                {
                    throw new ValidationException("Construction Start Date must be in the future for the selected project stage.");
                }
            }

            // Validate Project Creator ID
            if (ProjectCreatorId == Guid.Empty)
            {
                throw new ValidationException("Project Creator ID is required.");
            }
        }

        // Method to check if the ProjectId is a valid 6-digit number using regex
        private bool IsValidProjectId(string projectId)
        {
            // Regex to check if the projectId is exactly 6 digits
            return Regex.IsMatch(projectId, @"^\d{6}$");
        }

        // Method to check if the construction date must be in the future based on the project stage
        private bool IsFutureConstructionDateRequired()
        {
            return ProjectStage == ProjectStage.Concept ||
                   ProjectStage == ProjectStage.DesignAndDocumentation ||
                   ProjectStage == ProjectStage.PreConstruction;
        }

        // Method to generate a unique 6-digit Project ID
        public void GenerateUniqueProjectId()
        {
            // Logic to generate a unique 6-digit number
            // This is a placeholder; implement your own logic to ensure uniqueness
            Random random = new Random();
            ProjectId = random.Next(100000, 999999);
        }
    }

    // Enum for Project Stage
    public enum ProjectStage
    {
        Concept,
        DesignAndDocumentation,
        PreConstruction,
        Construction
    }

    // Enum for Project Category
    public enum ProjectCategory
    {
        Education,
        Health,
        Office,
        Others
    }
}