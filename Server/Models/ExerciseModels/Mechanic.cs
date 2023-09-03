using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CogniFitRepo.Server.Models.ExerciseModels
{
    public class Mechanic
    {
        [Key]
        [Column(TypeName = "TINYINT")]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(25)")]
        public string Name { get; set; } = null!;

        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
