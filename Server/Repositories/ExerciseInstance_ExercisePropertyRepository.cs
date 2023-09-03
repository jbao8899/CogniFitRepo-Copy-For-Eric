using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.ExerciseModels;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CogniFitRepo.Server.Repositories
{
    public class ExerciseInstance_ExercisePropertyRepository : IExerciseInstance_ExercisePropertyRepository
    {
        private readonly ApplicationDbContext _context;

        public ExerciseInstance_ExercisePropertyRepository(
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        public ExerciseInstance_ExercisePropertyDto GetByBothIds(long exerciseInstanceId, int exercisePropertyId)
        {
            if (_context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            if (!_context.ExerciseInstances.Any(ei => ei.Id == exerciseInstanceId))
            {
                throw new KeyNotFoundException($"There is no exercise instance with an ID of {exerciseInstanceId}.");
            }

            if (!_context.ExerciseProperties.Any(ep => ep.Id == exercisePropertyId))
            {
                throw new KeyNotFoundException($"There is no exercise property with an ID of {exercisePropertyId}.");
            }

            var retVal = _context.ExerciseInstance_ExerciseProperties
                                 .Where(ei_ep => ei_ep.ExerciseInstanceId == exerciseInstanceId)
                                 .Where(ei_ep => ei_ep.ExercisePropertyId == exercisePropertyId)
                                 .Include(ei_ep => ei_ep.ExerciseProperty)
                                 .Select(ei_ep => ei_ep.ToDto())
                                 .FirstOrDefault();

            if (retVal is null)
            {
                throw new KeyNotFoundException($"The exercise instance with an ID of {exerciseInstanceId} does not have a property with an ID of {exercisePropertyId}.");
            }

            return retVal;
        }

        public IEnumerable<ExerciseInstance_ExercisePropertyDto> GetByExerciseInstanceId(long exerciseInstanceId)
        {
            if (_context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseInstances is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            if (!_context.ExerciseInstances.Any(ei => ei.Id == exerciseInstanceId))
            {
                throw new KeyNotFoundException($"There is no exercise instance with an ID of {exerciseInstanceId}.");
            }

            var retVal = _context.ExerciseInstance_ExerciseProperties
                                 .Where(ei_ep => ei_ep.ExerciseInstanceId == exerciseInstanceId)
                                 .Include(ei_ep => ei_ep.ExerciseProperty)
                                 .Select(ei_ep => ei_ep.ToDto())
                                 .ToList();

            //if (retVal is null)
            //{
            //    throw new KeyNotFoundException($"The exercise instance with an ID of {exerciseInstanceId} has no properties associated with it.");
            //}

            return retVal;
        }

        public void Post(ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto, string currentUserId)
        {
            if (_context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseInstances is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            // Could have used ArgumentNullException, but did it this way for consistency
            if (exerciseInstance_ExercisePropertyDto.ExerciseInstanceId is null)
            {
                throw new ArgumentException("You must provide the ID of an exercise instance");
            }

            // check that the current user owns the exercise instance to which a
            // property will be added
            ExerciseInstance? ownsThisProperty = _context.ExerciseInstances
                                                        .Where(ei => ei.Id == exerciseInstance_ExercisePropertyDto.ExerciseInstanceId)
                                                        .Include(ei => ei.Workout)
                                                            .ThenInclude(w => w.WorkoutProgram)
                                                        .FirstOrDefault();
            if (ownsThisProperty is null)
            {
                throw new KeyNotFoundException($"There is no exercise instance with an ID of {exerciseInstance_ExercisePropertyDto.ExerciseInstanceId}.");
            }
            // Check that you aren't adding a property to someone else's exercise instance
            if (ownsThisProperty.Workout is not null &&
                ownsThisProperty.Workout.WorkoutProgram is not null &&
                ownsThisProperty.Workout.WorkoutProgram.UserId != currentUserId)
            {
                throw new NotSupportedException($"The exercise instance with an ID of {exerciseInstance_ExercisePropertyDto.ExerciseInstanceId} belongs to another user!");
            }

            if (!_context.ExerciseProperties.Any(ep => ep.Id == exerciseInstance_ExercisePropertyDto.ExercisePropertyId))
            {
                throw new KeyNotFoundException($"There is no exercise property with an ID of {exerciseInstance_ExercisePropertyDto.ExercisePropertyId}.");
            }

            // check that this doesn't already exist
            bool connectionAlreadyExists =
                (from ExerciseInstance_ExerciseProperty exerciseInstance_ExercisePropertyLinq in _context.ExerciseInstance_ExerciseProperties
                 where exerciseInstance_ExercisePropertyLinq.ExerciseInstanceId == exerciseInstance_ExercisePropertyDto.ExerciseInstanceId
                 where exerciseInstance_ExercisePropertyLinq.ExercisePropertyId == exerciseInstance_ExercisePropertyDto.ExercisePropertyId
                 select exerciseInstance_ExercisePropertyLinq).Any();

            if (connectionAlreadyExists)
            {
                throw new ArgumentException($"There is already an ExerciseInstance_ExerciseProperty with an ExerciseInstanceId of {exerciseInstance_ExercisePropertyDto.ExerciseInstanceId} and an ExercisePropertyId of {exerciseInstance_ExercisePropertyDto.ExercisePropertyId}.");
            }

            ExerciseInstance_ExerciseProperty exerciseInstance_ExerciseProperty =
                new ExerciseInstance_ExerciseProperty(exerciseInstance_ExercisePropertyDto);
            exerciseInstance_ExerciseProperty.ExerciseInstanceId =
                (int)exerciseInstance_ExercisePropertyDto.ExerciseInstanceId;
            _context.ExerciseInstance_ExerciseProperties.Add(exerciseInstance_ExerciseProperty);

            _context.SaveChanges();

            return;
        }

        public void UpdatePropertyAmount(ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto, string currentUserId)
        {
            if (_context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseInstances is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            ExerciseInstance_ExerciseProperty? toUpdate =
                _context.ExerciseInstance_ExerciseProperties
                        .Where(ei_ep => ei_ep.ExerciseInstanceId == exerciseInstance_ExercisePropertyDto.ExerciseInstanceId)
                        .Where(ei_ep => ei_ep.ExercisePropertyId == exerciseInstance_ExercisePropertyDto.ExercisePropertyId)
                        .Include(ei_ep => ei_ep.ExerciseInstance)
                            .ThenInclude(ei => ei.Workout)
                                .ThenInclude(w => w.WorkoutProgram)
                        .FirstOrDefault();

            //(from exerciseInstance_ExerciseProperty in _context.ExerciseInstance_ExerciseProperties
            //where exerciseInstance_ExerciseProperty.ExerciseInstanceId == exerciseInstance_ExercisePropertyDto.ExerciseInstanceId
            //where exerciseInstance_ExerciseProperty.ExercisePropertyId == exerciseInstance_ExercisePropertyDto.ExercisePropertyId
            //select exerciseInstance_ExerciseProperty).FirstOrDefault();


            if (toUpdate is null)
            {
                throw new KeyNotFoundException($"There is no join table entry with an ExerciseInstanceId of {exerciseInstance_ExercisePropertyDto.ExerciseInstanceId} and an ExercisePropertyId of {exerciseInstance_ExercisePropertyDto.ExercisePropertyId}.");
            }

            if (toUpdate.ExerciseInstance is not null &&
                toUpdate.ExerciseInstance.Workout is not null &&
                toUpdate.ExerciseInstance.Workout.WorkoutProgram is not null &&
                toUpdate.ExerciseInstance.Workout.WorkoutProgram.UserId != currentUserId)
            {
                throw new NotSupportedException($"The join table entry connecting the exercise instance with an ID of {exerciseInstance_ExercisePropertyDto.ExerciseInstanceId} and the exercise property with an ID of {exerciseInstance_ExercisePropertyDto.ExercisePropertyId} belongs to another user!");
            }

            toUpdate.Amount = exerciseInstance_ExercisePropertyDto.Amount;

            _context.SaveChanges();

            return;
        }

        public void Delete(long exerciseInstanceId, int exercisePropertyId, string currentUserId)
        {
            if (_context.ExerciseInstance_ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            ExerciseInstance_ExerciseProperty? toDelete =
                _context.ExerciseInstance_ExerciseProperties
                        .Where(ei_ep => ei_ep.ExerciseInstanceId == exerciseInstanceId)
                        .Where(ei_ep => ei_ep.ExercisePropertyId == exercisePropertyId)
                        .Include(ei_ep => ei_ep.ExerciseInstance)
                            .ThenInclude(ei => ei.Workout)
                                .ThenInclude(w => w.WorkoutProgram)
                        .FirstOrDefault();

            //(from ExerciseInstance_ExerciseProperty exerciseInstance_ExerciseProperty in _context.ExerciseInstance_ExerciseProperties
            // where exerciseInstance_ExerciseProperty.ExerciseInstanceId == exerciseInstanceId
            // where exerciseInstance_ExerciseProperty.ExercisePropertyId == exercisePropertyId
            // select exerciseInstance_ExerciseProperty).FirstOrDefault();

            if (toDelete is null)
            {
                throw new KeyNotFoundException($"There is no ExerciseInstance_ExerciseProperty with an ExerciseInstanceId of {exerciseInstanceId} and an ExercisePropertyId of {exercisePropertyId}.");
            }

            if (toDelete.ExerciseInstance is not null &&
                toDelete.ExerciseInstance.Workout is not null &&
                toDelete.ExerciseInstance.Workout.WorkoutProgram is not null &&
                toDelete.ExerciseInstance.Workout.WorkoutProgram.UserId != currentUserId)
            {
                throw new NotSupportedException($"The join table entry connecting the exercise instance with an ID of {exerciseInstanceId} and the exercise property with an ID of {exercisePropertyId} belongs to another user!");
            }

            _context.ExerciseInstance_ExerciseProperties.Remove(toDelete);
            _context.SaveChanges();

            return;
        }
    }
}
