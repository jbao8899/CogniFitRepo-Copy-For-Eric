using CogniFitRepo.Server.Models.ExerciseModels;
using CogniFitRepo.Shared.DataTransferObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogniFitRepo.Server.Models
{
    public class Workout
    {
        [Key]
        [Column(TypeName = "BIGINT")]
        public long Id { get; set; }

        [ForeignKey("WorkoutProgram")]
        [Column(TypeName = "BIGINT")]
        public long? WorkoutProgramId { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime WorkoutDateTime { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Notes { get; set; } = null;

        [Column(TypeName = "BIT")]
        public bool IsComplete { get; set; }

        //Navigation
        public ICollection<ExerciseInstance> ExerciseInstances { get; set; } = new List<ExerciseInstance>();
        public WorkoutProgram? WorkoutProgram { get; set; } = null;

        public WorkoutDto ToDto()
        {
            WorkoutDto retVal = new WorkoutDto()
            {
                Id = Id,
                WorkoutProgramId = WorkoutProgramId,
                WorkoutDateTime = WorkoutDateTime,
                Notes = Notes,
                IsComplete = IsComplete
            };

            List<ExerciseInstanceDto> exerciseInstanceDtos = new List<ExerciseInstanceDto>();

            foreach (ExerciseInstance exerciseInstance in ExerciseInstances)
            {
                exerciseInstanceDtos.Add(exerciseInstance.ToDto());
            }

            retVal.ExerciseInstanceDtos = exerciseInstanceDtos;

            return retVal;
        }

        public Workout() { }

        // Construct from DTO
        // No error checking is done
        // Ignore Id as that is not known ahead of time by the front end
        // Ignore WorkoutProgramId as that is not known ahead of time by the front end
        // (at least in the method for posting WorkoutProgram objects using a DTO)
        // Ignore ExerciseInstanceDto objects inside workoutDto
        public Workout(WorkoutDto workoutDto)
        {
            WorkoutDateTime = workoutDto.WorkoutDateTime;
            Notes = workoutDto.Notes;
            IsComplete = workoutDto.IsComplete;
        }
    }
}
