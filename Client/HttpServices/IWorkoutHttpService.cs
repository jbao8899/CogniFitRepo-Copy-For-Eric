using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;

namespace CogniFitRepo.Client.HttpServices
{
    public interface IWorkoutHttpService
    {
        Task<DataResponse<List<WorkoutDto>>> GetWorkouts();
        Task<DataResponse<WorkoutDto>> GetWorkout(long? id);
        Task<DataResponse<WorkoutDto>> CreateWorkout(WorkoutDto workout);
        Task<DataResponse<List<WorkoutDto>>> CreateWorkoutsForWorkoutProgram(long? id, List<WorkoutDto> workouts);
        Task<DataResponse<WorkoutDto>> UpdateWorkout(long? id, WorkoutDto workout);
        Task<DataResponse<WorkoutDto>> DeleteWorkout(long? id);
    }
}
