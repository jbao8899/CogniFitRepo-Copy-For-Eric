using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CogniFitRepo.Server.Repositories
{
    public interface IWorkoutRepository
    {
        public IEnumerable<WorkoutDto> GetWorkouts();
        public WorkoutDto GetWorkout(long id);
        public void PutWorkout(long id, [FromBody] WorkoutDto workoutDto);
        public void DeleteWorkout(long id);
        public WorkoutDto GetWorkoutsByDateForCurrentUser(string dateStr, ClaimsPrincipal user);
        public IEnumerable<WorkoutDto> GetWorkoutsByWorkoutProgramId(long workoutProgramId);
        public void PostWorkouts(long workoutProgramId, [FromBody] IEnumerable<WorkoutDto> workoutDtos);
    }
}
