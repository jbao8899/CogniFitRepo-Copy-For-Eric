using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CogniFitRepo.Server.Models.ExerciseModels
{
    public class ExerciseInstance
    {
        [Key]
        [Column(TypeName = "BIGINT")]
        public long Id { get; set; }

        [Column(TypeName = "SMALLINT")]
        [ForeignKey("Exercise")]
        public int ExerciseId { get; set; }

        [Column(TypeName = "BIGINT")]
        [ForeignKey("Workout")]
        public long? WorkoutId { get; set; }

        [Column(TypeName = "TINYINT")]
        public int WorkoutSequenceNumber { get; set; }

        [Column(TypeName = "BIT")]
        public bool IsComplete { get; set; }

        // Navigation Properties
        public Exercise Exercise { get; set; } = null!;

        public Workout? Workout { get; set; } = null; // Circular reference

        // want this???
        public ICollection<ExerciseInstance_ExerciseProperty> ExerciseInstance_ExerciseProperties { get; set; } = new List<ExerciseInstance_ExerciseProperty>();

        public ExerciseInstanceDto ToDto()
        {
            ExerciseInstanceDto retval = new ExerciseInstanceDto()
            {
                Id = Id,
                ExerciseId = ExerciseId,
                ExerciseName = Exercise.Name,
                WorkoutId = WorkoutId,
                WorkoutSequenceNumber = WorkoutSequenceNumber,
                IsComplete = IsComplete
            };

            List<ExerciseInstance_ExercisePropertyDto> exerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>();

            foreach (ExerciseInstance_ExerciseProperty exerciseInstance_ExerciseProperty in ExerciseInstance_ExerciseProperties)
            {
                exerciseInstance_ExercisePropertyDtos.Add(exerciseInstance_ExerciseProperty.ToDto());
            }

            retval.ExerciseInstance_ExercisePropertyDtos = exerciseInstance_ExercisePropertyDtos;
            
            return retval;
        }

        public ExerciseInstance() { }

        // Construct from DTO
        // No error checking is done
        // Ignore Id as that is not known ahead of time by the front end
        // Ignore WorkoutId as that is not known ahead of time by the front end
        // Ignore ExerciseInstance_ExercisePropertyDto objects inside exerciseInstanceDto
        public ExerciseInstance(ExerciseInstanceDto exerciseInstanceDto)
        {
            ExerciseId = exerciseInstanceDto.ExerciseId;
            WorkoutSequenceNumber = exerciseInstanceDto.WorkoutSequenceNumber;
            IsComplete = exerciseInstanceDto.IsComplete;
        }
    }
}
