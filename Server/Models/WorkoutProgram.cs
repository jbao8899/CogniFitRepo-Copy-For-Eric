using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Shared.DataTransferObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.ExerciseModels;

namespace CogniFitRepo.Server.Models
{
    public class WorkoutProgram
    {
        [Key]
        [Column(TypeName = "BIGINT")]
        public long Id { get; set; }

        [ForeignKey("ApplicationUser")]
        [Column(TypeName = "NVARCHAR(450)")]
        public string? UserId { get; set; } = null;

        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "DATE")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Notes { get; set; } = null;

        [Column(TypeName = "BIT")]
        public bool IsComplete { get; set; }

        //Navigation
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
        public ApplicationUser? ApplicationUser { get; set; } = null;

        public WorkoutProgramDto ToDto()
        {
            return new WorkoutProgramDto()
            {
                Id = Id,
                UserId = UserId,
                Name = Name,
                StartDate = StartDate,
                EndDate = EndDate,
                Notes = Notes,
                IsComplete = IsComplete,
                WorkoutDtos = Workouts.Select(w => w.ToDto())
            };
        }

        // Construct from DTO
        // No error checking is done
        // Ignore Id as that is not known ahead of time by the front end
        // Ignore UserId
        // Ignore WorkoutDto objects inside workoutProgramDto
        public WorkoutProgram(WorkoutProgramDto workoutProgramDto)
        {
            Name = workoutProgramDto.Name;
            StartDate = workoutProgramDto.StartDate;
            EndDate = workoutProgramDto.EndDate;
            Notes = workoutProgramDto.Notes;
            IsComplete = workoutProgramDto.IsComplete;
        }

        public WorkoutProgram() { }
    }
}
