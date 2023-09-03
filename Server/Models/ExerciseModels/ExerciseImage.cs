using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CogniFitRepo.Server.Models.ExerciseModels
{
    public class ExerciseImage
    {
        [Key]
        [Column(TypeName = "SMALLINT")]
        public int Id { get; set; }

        [Column(TypeName = "SMALLINT")]
        [ForeignKey("Exercise")]
        public int ExerciseId { get; set; }

        [Column(TypeName = "VARCHAR(75)")]
        public string Path { get; set; } = null!;

        // navigation property
        public Exercise Exercise { get; set; } = null!;
    }
}
