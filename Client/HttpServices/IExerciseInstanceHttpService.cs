using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;

namespace CogniFitRepo.Client.HttpServices
{
    public interface IExerciseInstanceHttpService
    {
        Task<DataResponse<List<ExerciseInstanceDto>>> GetExerciseInstances();
        Task<DataResponse<ExerciseInstanceDto>> GetExerciseInstance(long? id);
        Task<DataResponse<ExerciseInstanceDto>> CreateExerciseInstance(ExerciseInstanceDto exerciseInstance);
        Task<DataResponse<ExerciseInstanceDto>> CreateExerciseInstanceForWorkout(long? id, ExerciseInstanceDto exerciseInstance);
        Task<DataResponse<ExerciseInstanceDto>> UpdateExerciseInstance(long? id, ExerciseInstanceDto exerciseInstance);
        Task<DataResponse<ExerciseInstanceDto>> ToggleExerciseInstanceCompleted(long? id);
        Task<DataResponse<ExerciseInstanceDto>> DeleteExerciseInstance(long? id);
    }
}
