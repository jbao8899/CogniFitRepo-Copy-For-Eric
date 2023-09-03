using CogniFitRepo.Shared.DataTransferObjects;

namespace CogniFitRepo.Server.Repositories
{
    public interface IExercisePropertyRepository
    {
        public List<ExercisePropertyDto> GetExerciseProperties();
    }
}
