using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;

namespace CogniFitRepo.Client.HttpServices

{
    public interface IWorkoutProgramHttpService
    {
        Task<DataResponse<List<WorkoutProgramDto>>> GetWorkoutPrograms();
        Task<DataResponse<List<WorkoutProgramDto>>> GetCurrentUsersWorkoutPrograms();
        Task<DataResponse<WorkoutProgramDto>> GetWorkoutProgram(long? id);
        Task<DataResponse<WorkoutProgramDto>> CreateWorkoutProgram(WorkoutProgramDto workoutProgram);
        Task<DataResponse<WorkoutProgramDto>> UpdateWorkoutProgram(long? id, WorkoutProgramDto workoutProgram);
        Task<DataResponse<WorkoutProgramDto>> DeleteWorkoutProgram(long? id);
    }
}
