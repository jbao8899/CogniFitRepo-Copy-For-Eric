using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogniFitRepo.Server.Models.ExerciseModels
{
    // Go through this!!! Nullable foreign keys??? null! ???
    public class Exercise
    {
        [Key]
        [Column(TypeName = "SMALLINT")]
        public int Id { get; set; }

        [Column(TypeName = "TINYINT")]
        [ForeignKey("ExerciseLevel")]
        public int? ExerciseLevelId { get; set; } = null!;

        [Column(TypeName = "TINYINT")]
        [ForeignKey("ExerciseCategory")]
        public int? ExerciseCategoryId { get; set; } = null!;

        [Column(TypeName = "TINYINT")]
        [ForeignKey("Force")]
        public int? ForceId { get; set; } = null!;

        [Column(TypeName = "TINYINT")]
        [ForeignKey("Mechanic")]
        public int? MechanicId { get; set; } = null!;

        [Column(TypeName = "SMALLINT")]
        [ForeignKey("Equipment")]
        public int? EquipmentId { get; set; } = null!;

        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "TEXT")]
        public string Instructions { get; set; } = null!;

        // Navigation Properties
        public ExerciseLevel ExerciseLevel { get; set; } = null!;
        public Equipment Equipment { get; set; } = null!;
        public ExerciseCategory ExerciseCategory { get; set; } = null!;
        public Force Force { get; set; } = null!;
        public Mechanic Mechanic { get; set; } = null!;
        public ICollection<ExerciseImage> ExerciseImages { get; set; } = new List<ExerciseImage>();
        public ICollection<Exercise_Muscle> Exercise_Muscles { get; set; } = new List<Exercise_Muscle>();

        //public ICollection<ExerciseInstance> ExerciseInstances { get; set; } = new List<ExerciseInstance>(); //unneccesary????


        public ExerciseDto ToDto()
        {
            return new ExerciseDto()
            {
                Id = Id,
                ExerciseLevel = ExerciseLevel.Name,
                ExerciseCategory = ExerciseCategory.Name,
                Force = Force.Name,
                Mechanic = Mechanic.Name,
                Equipment = Equipment.Name,
                Name = Name,
                Instructions = Instructions,
                PrimaryMuscles = Exercise_Muscles.Where(em => em.IsPrimary).Select(em => em.Muscle.Name).ToList(),
                SecondaryMuscles = Exercise_Muscles.Where(em => !em.IsPrimary).Select(em => em.Muscle.Name).ToList(),
                ImagePaths = ExerciseImages.Select(ei => ei.Path).ToList()
            };
        }
    }
}
