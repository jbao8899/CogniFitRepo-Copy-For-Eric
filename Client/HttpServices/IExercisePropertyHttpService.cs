using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;

namespace CogniFitRepo.Client.HttpServices
{
    public interface IExercisePropertyHttpService
    {
        Task<DataResponse<List<ExercisePropertyDto>>> GetExerciseProperties();
    }
}
