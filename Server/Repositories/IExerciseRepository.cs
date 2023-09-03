using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Server.Models.ExerciseModels;

namespace CogniFitRepo.Server.Repositories
{
    public interface IExerciseRepository
    {
        List<ExerciseDto> GetExercises();
        ExerciseDto? GetExerciseById(int id);
        List<ExerciseDto> GetExerciseByPrimaryMuscle(string primaryMuscleName);
        List<ExerciseDto> GetExercisesByAnyMuscle(string muscleName);
        List<ExerciseDto> GetExercisesByEquipment(string equipmentName);
        List<ExerciseDto> GetExercisesByLevel(string levelName);
        List<ExerciseDto> GetExercisesByCategory(string categoryName);
        List<ExerciseDto> GetExercisesByForce(string forceName);
        List<ExerciseDto> GetExercisesByMechanic(string mechanicName);
        List<ExerciseImage> GetExerciseImages(int id);
        List<ExerciseDto> GetExercisesByMultipleAttributes(string primaryMuscle, string anyMuscle,
             string equipment, string force, string level, string category, string mechanic);
    }
}
