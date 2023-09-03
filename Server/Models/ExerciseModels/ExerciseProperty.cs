using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Humanizer;
using CogniFitRepo.Shared.DataTransferObjects;

namespace CogniFitRepo.Server.Models.ExerciseModels
{
    public class ExerciseProperty
    {
        [Key]
        [Column(TypeName = "TINYINT")]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string Name { get; set; } = null!;

        // Navigation Property
        // Not needed????
        //public ICollection<ExerciseInstance_ExerciseProperty> ExeciseInstance_ExerciseProperties { get; set; } = null!;

        public ExercisePropertyDto ToDto()
        {
            return new ExercisePropertyDto
            {
                Id = this.Id,
                Name = this.Name,
            };
        }
    }
}
