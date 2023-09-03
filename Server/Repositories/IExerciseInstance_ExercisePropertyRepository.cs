using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CogniFitRepo.Server.Repositories
{
    public interface IExerciseInstance_ExercisePropertyRepository
    {
        // Get single object by both IDs.
        ExerciseInstance_ExercisePropertyDto GetByBothIds(long exerciseInstanceId, int exercisePropertyId);

        // Get all join table entries corresponding to one exercise instance
        IEnumerable<ExerciseInstance_ExercisePropertyDto> GetByExerciseInstanceId(long exerciseInstanceId);

        // Add a new property to an exercise instance
        // ExerciseInstanceId, ExercisePropertyId, and Amount are all needed
        // The Name stored in the DTO is ignored.
        void Post([FromBody] ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto, string currentUserId);

        // Change the amount associated with a property that an ExerciseInstance has
        // Because the join table entries are identified only with their foreign keys,
        // this will not let you change which property or exercise instance
        // a join table entry points to
        // The Name in exerciseInstance_ExercisePropertyDto will be ignored
        void UpdatePropertyAmount(ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto, string currentUserId);

        // Delete join table entry with the specified pair of foreign keys
        void Delete(long exerciseInstanceId, int exercisePropertyId, string currentUserId);
    }
}