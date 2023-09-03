using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CogniFitRepo.Server.Repositories
{
    public interface IWorkoutProgramRepository
    {
        // Get all of them
        IEnumerable<WorkoutProgramDto> GetWorkoutPrograms();

        // Get one workout program
        WorkoutProgramDto GetWorkoutProgram(long id);

        // This will not affect the workouts owned by this workout program
        // Any workout DTOs in the workout program DTO passed in will be ignored
        // This will not move workout programs between users
        void PutWorkoutProgram(long id, WorkoutProgramDto workoutProgramDto, string currentUserId);

        // Post a workout program based on a DTO. Post all workouts contained within
        void PostWorkoutProgram(WorkoutProgramDto workoutProgramDto, string currentUserId);

        void DeleteWorkoutProgram(long id, string currentUserId);

        // gets all workout programs belonging to the specified user
        IEnumerable<WorkoutProgramDto> GetWorkoutProgramsForUser(string currentUserId);

        // gets all completed workout programs belonging to a specified user
        IEnumerable<WorkoutProgramDto> GetCompletedWorkoutProgramsForUser(string currentUserId);


        IEnumerable<WorkoutProgramDto> GetIncompleteWorkoutProgramsForUser(string currentUserId);
    }
}