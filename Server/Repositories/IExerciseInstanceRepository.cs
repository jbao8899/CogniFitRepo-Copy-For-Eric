using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CogniFitRepo.Server.Repositories
{
    public interface IExerciseInstanceRepository
    {
        IEnumerable<ExerciseInstanceDto> GetAll();

        ExerciseInstanceDto GetById(long id);

        // POST an exercise instance to a specified workout
        // The position of the exercise instance within that workout's sequence of events will be at the end
        // The WorkoutSequenceNumber field will be ignored
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        void PostToWorkout(long workoutId, ExerciseInstanceDto exerciseInstanceDto, string currentUserId);

        void Delete(long id, string currentUserId);

        IEnumerable<ExerciseInstanceDto> GetByWorkoutId(long workoutId);

        void ChangeExercise(long id, int newExerciseId, string currentUserId);

        void SwapPositions(long firstId, long secondId, string currentUserId);

        void ToggleCompletion(long id, string currentUserId);
    }
}