using System;
using System.ComponentModel.DataAnnotations;

namespace ConstructionPMS.Domain.Entities
{
    public class Project
    {
        private DateTime _constructionStartDate;

        [Key]
        [Required]
        [Range(100000, 999999, ErrorMessage = "Project ID must be a 6-digit unique number.")]
        public int ProjectId { get; private set; } // 6-digit unique number

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

        public string OtherCategory { get; set; } // For user-defined categories

        [Required(ErrorMessage = "Construction Start Date is required.")]
        [DataType(DataType.Date)]
        public DateTime ConstructionStartDate
        {
            get { return _constructionStartDate.ToUniversalTime(); }
            set { _constructionStartDate = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        [Required(ErrorMessage = "Project Details/Description is required.")]
        [MaxLength(2000, ErrorMessage = "Project Details/Description cannot exceed 2000 characters.")]
        public string ProjectDetails { get; set; }

        [Required(ErrorMessage = "Project Creator ID is required.")]
        public Guid ProjectCreatorId { get; set; } // UserID

        // Constructor to generate a unique Project ID
        public Project()
        {
            ProjectId = GenerateUniqueProjectId();
        }

        // Method to validate the project based on the specified rules
        public void Validate()
        {
            if (ProjectStage == ProjectStage.Concept ||
                ProjectStage == ProjectStage.DesignAndDocumentation ||
                ProjectStage == ProjectStage.PreConstruction)
            {
                if (ConstructionStartDate <= DateTime.UtcNow)
                {
                    throw new ValidationException("Construction Start Date must be in the future for the selected project stage.");
                }
            }
        }

        // Method to generate a unique 6-digit Project ID
        private int GenerateUniqueProjectId()
        {
            // Logic to generate a unique 6-digit number
            // This is a placeholder; implement your own logic to ensure uniqueness
            Random random = new Random();
            return random.Next(100000, 999999);
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