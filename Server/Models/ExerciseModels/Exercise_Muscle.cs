using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogniFitRepo.Server.Models.ExerciseModels
{
    public class Exercise_Muscle
    {
        [ForeignKey("Exercise")] //, Column(Order = 0)]
        public int ExerciseId { get; set; }

        [ForeignKey("Muscle")] //, Column(Order = 1)]
        public int MuscleId { get; set; }

        [Column(TypeName = "BIT")]
        public bool IsPrimary { get; set; }

        public Muscle Muscle { get; set; } = null!;
        public Exercise Exercise { get; set; } = null!;

    }
}
