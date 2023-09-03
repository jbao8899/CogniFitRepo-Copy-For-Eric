using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;

namespace CogniFitRepo.Client.HttpServices
{
    public interface IExerciseHttpService
    {
        Task<DataResponse<List<ExerciseDto>>> GetExercises();
        Task<DataResponse<ExerciseDto>> GetExercise(int id);
        Task<DataResponse<ExerciseDto>> CreateExercise(ExerciseDto exercise);
        Task<DataResponse<ExerciseDto>> UpdateExercise(int id, ExerciseDto exercise);
        Task<DataResponse<ExerciseDto>> DeleteExercise(int id);
    }
}
