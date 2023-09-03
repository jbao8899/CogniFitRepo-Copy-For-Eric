using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CogniFitRepo.Server.Models.ExerciseModels
{
    public class Muscle
    {
        [Key]
        //[Required]
        [Column(TypeName = "SMALLINT")]
        public int Id { get; set; }

        //[Required]
        [Column(TypeName = "VARCHAR(50)")]
        public string Name { get; set; } = null!;

        public ICollection<Exercise_Muscle> Exercise_Muscles { get; set; } = new List<Exercise_Muscle>();
    }
}
