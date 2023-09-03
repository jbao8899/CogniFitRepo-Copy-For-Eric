using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CogniFitRepo.Server.Models.ExerciseModels
{
    public class Equipment
    {
        [Key]
        [Column(TypeName = "SMALLINT")]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string Name { get; set; } = null!;
        
        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
