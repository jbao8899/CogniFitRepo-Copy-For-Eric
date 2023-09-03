using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.ExerciseModels;
using CogniFitRepo.Server.Models;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CogniFitRepo.Server.Repositories
{
    public class ExerciseInstanceRepository : IExerciseInstanceRepository
    {
        private readonly ApplicationDbContext _context;

        public ExerciseInstanceRepository(
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        public IEnumerable<ExerciseInstanceDto> GetAll()
        {
            if (_context.ExerciseInstances is null ||
                _context.Exercises is null ||
                _context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            var retVal = _context.ExerciseInstances
                                 .Include(ei => ei.Exercise)
                                 .Include(ei => ei.ExerciseInstance_ExerciseProperties)
                                     .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                                 .Select(ei => ei.ToDto())
                                 .ToList();

            return retVal;
        }

        public ExerciseInstanceDto GetById(long id)
        {
            if (_context.ExerciseInstances is null ||
                _context.Exercises is null ||
                _context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            var retVal = _context.ExerciseInstances
                                 .Where(ei => ei.Id == id)
                                 .Include(ei => ei.Exercise)
                                 .Include(ei => ei.ExerciseInstance_ExerciseProperties)
                                     .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                                 .Select(ei => ei.ToDto())
                                 .FirstOrDefault();

            if (retVal is null)
            {
                throw new KeyNotFoundException($"There is no exercise instance with an id of {id}.");
            }

            return retVal;
        }

        // Code reuse????
        public void PostToWorkout(long workoutId, ExerciseInstanceDto exerciseInstanceDto, string currentUserId)
        {
            // Properties should be added separately
            // WorkoutSequenceNumber will be set to 1 + the highest WorkoutSequenceNumber
            // for the workout referred to by WorkoutId, or 1 if WorkoutId is null.

            if (_context.Workouts is null ||
                _context.ExerciseInstances is null ||
                _context.Exercises is null ||
                _context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            Workout? parentWorkout = _context.Workouts
                                             .Include(w => w.ExerciseInstances)
                                             .Include(w => w.WorkoutProgram)
                                             .FirstOrDefault(w => w.Id == workoutId);

            if (parentWorkout is null)
            {
                throw new KeyNotFoundException($"There is no workout with an ID of {workoutId}");
            }

            if (parentWorkout.WorkoutProgram is not null &&
                parentWorkout.WorkoutProgram.UserId != currentUserId)
            {
                throw new NotSupportedException($"User {currentUserId} does not have permission to add an exercise instance to the workout with an ID of {workoutId}.");
            }

            int setWorkoutSequenceNumber = 1;
            if (parentWorkout.ExerciseInstances.Count > 0)
            {
                // if adding to an existing workout with exercise instances,
                // add this after the last exercise instance which is already there
                // by default
                setWorkoutSequenceNumber = parentWorkout.ExerciseInstances.Select(ei => ei.WorkoutSequenceNumber).Max() + 1;
            }

            if (!_context.Exercises.Any(e => e.Id == exerciseInstanceDto.ExerciseId))
            {
                throw new KeyNotFoundException($"There is no exercise with an ID of {exerciseInstanceDto.ExerciseId}.");
            }

            if (exerciseInstanceDto.ExerciseInstance_ExercisePropertyDtos is not null)
            {
                foreach (ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto in exerciseInstanceDto.ExerciseInstance_ExercisePropertyDtos)
                {
                    if (!_context.ExerciseProperties.Any(ep => ep.Id == exerciseInstance_ExercisePropertyDto.ExercisePropertyId))
                    {
                        throw new KeyNotFoundException($"There is no exercise property with an ID of {exerciseInstance_ExercisePropertyDto.ExercisePropertyId}.");
                    }
                }
            }

            ExerciseInstance exerciseInstance = new ExerciseInstance(exerciseInstanceDto);
            exerciseInstance.WorkoutId = workoutId;
            _context.ExerciseInstances.Add(exerciseInstance);
            _context.SaveChanges(); // Do this to populate exerciseInstance.Id?????

            if (exerciseInstanceDto.ExerciseInstance_ExercisePropertyDtos is not null)
            {
                // create all the joining table entries
                foreach (ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto in exerciseInstanceDto.ExerciseInstance_ExercisePropertyDtos)
                {
                    ExerciseInstance_ExerciseProperty exerciseInstance_ExerciseProperty = new ExerciseInstance_ExerciseProperty(exerciseInstance_ExercisePropertyDto);
                    exerciseInstance_ExerciseProperty.ExerciseInstanceId = exerciseInstance.Id;
                    _context.ExerciseInstance_ExerciseProperties.Add(exerciseInstance_ExerciseProperty);
                }
                _context.SaveChanges();
            }

            return;
        }

        public void Delete(long id, string currentUserId)
        {
            if (_context.ExerciseInstances is null ||
                _context.ExerciseInstance_ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            // Use includes to cascade deletion????
            var exerciseInstance = _context.ExerciseInstances
                                           .Where(ei => ei.Id == id)
                                           .Include(ei => ei.ExerciseInstance_ExerciseProperties)
                                           .Include(ei => ei.Workout)
                                               .ThenInclude(w => w.WorkoutProgram)
                                           .FirstOrDefault();

            if (exerciseInstance is null)
            {
                throw new KeyNotFoundException($"There is no exercise instance with an id of {id}.");
            }

            if (exerciseInstance.Workout is not null &&
                exerciseInstance.Workout.WorkoutProgram is not null &&
                exerciseInstance.Workout.WorkoutProgram.UserId != currentUserId)
            {
                throw new NotSupportedException($"The exercise instance with an ID of {id} belongs to another user!");
            }

            _context.ExerciseInstances.Remove(exerciseInstance);
            _context.SaveChanges();

            return;
        }

        public IEnumerable<ExerciseInstanceDto> GetByWorkoutId(long workoutId)
        {
            if (_context.ExerciseInstances is null ||
                _context.Exercises is null ||
                _context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            // check if workoutId is valid
            if (_context.Workouts.FirstOrDefault(w => w.Id == workoutId) == null)
            {
                throw new KeyNotFoundException($"There is no workout with an ID of {workoutId}.");
            }

            var retVal = _context.ExerciseInstances
                                 .Where(ei => ei.WorkoutId == workoutId)
                                 .Include(ei => ei.Exercise)
                                 .Include(ei => ei.ExerciseInstance_ExerciseProperties)
                                     .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                                 .Select(ei => ei.ToDto())
                                 .ToList();

            return retVal;
        }

        public void ChangeExercise(long id, int newExerciseId, string currentUserId)
        {
            if (_context.ExerciseInstances is null ||
                _context.Exercises is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            ExerciseInstance? toUpdate = _context.ExerciseInstances
                                                 .Where(ei => ei.Id == id)
                                                 .Include(ei => ei.Workout)
                                                     .ThenInclude(w => w.WorkoutProgram)
                                                 .FirstOrDefault();
            if (toUpdate == null)
            {
                throw new KeyNotFoundException($"There is no exercise instance with an id of {id}.");
            }
            else
            {
                if (toUpdate.Workout is not null &&
                    toUpdate.Workout.WorkoutProgram is not null &&
                    toUpdate.Workout.WorkoutProgram.UserId != currentUserId)
                {
                    throw new NotSupportedException($"The exercise instance with an ID of {id} belongs to another user!");
                }


                if (!_context.Exercises.Any(e => e.Id == newExerciseId))
                {
                    throw new KeyNotFoundException($"There is no exercise with an ID of {newExerciseId}.");
                }

                toUpdate.ExerciseId = newExerciseId;

                _context.SaveChanges();

                return;
            }
        }

        public void SwapPositions(long firstId, long secondId, string currentUserId)
        {
            if (_context.ExerciseInstances is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            ExerciseInstance? firstExerciseInstance = _context.ExerciseInstances
                                                              .Where(ei => ei.Id == firstId)
                                                              .Include(ei => ei.Workout)
                                                                  .ThenInclude(w => w.WorkoutProgram)
                                                              .FirstOrDefault();
            if (firstExerciseInstance == null)
            {
                throw new KeyNotFoundException($"There is no exercise instance with an id of {firstId}.");
            }

            ExerciseInstance? secondExerciseInstance = _context.ExerciseInstances.FirstOrDefault(ei => ei.Id == secondId);
            if (secondExerciseInstance == null)
            {
                throw new KeyNotFoundException($"There is no exercise instance with an id of {secondId}.");
            }

            if (firstExerciseInstance.WorkoutId == null ||
                secondExerciseInstance.WorkoutId == null)
            {
                throw new ArgumentException("One or both of these exercise instances does not belong to a workout, so their positions cannot be swapped.");
            }

            if (firstExerciseInstance.WorkoutId != secondExerciseInstance.WorkoutId)
            {
                throw new ArgumentException("These exercise instances do not belong to the same workout, so their positions cannot be swapped.");
            }
            else
            {
                // first and second will come from same workout, so we do not need
                // to check for second
                if (firstExerciseInstance.Workout is not null &&
                    firstExerciseInstance.Workout.WorkoutProgram is not null &&
                    firstExerciseInstance.Workout.WorkoutProgram.UserId != currentUserId)
                {
                    throw new NotSupportedException($"The exercise instances with IDs of {firstId} and {secondId} belong to another user!");
                }

                int temp = firstExerciseInstance.WorkoutSequenceNumber;
                firstExerciseInstance.WorkoutSequenceNumber = secondExerciseInstance.WorkoutSequenceNumber;
                secondExerciseInstance.WorkoutSequenceNumber = temp;

                _context.SaveChanges();

                return;
            }
        }

        public void ToggleCompletion(long id, string currentUserId)
        {
            if (_context.ExerciseInstances is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            ExerciseInstance? toUpdate = _context.ExerciseInstances
                                                 .Where(ei => ei.Id == id)
                                                 .Include(ei => ei.Workout)
                                                     .ThenInclude(w => w.WorkoutProgram)
                                                 .FirstOrDefault();
            if (toUpdate == null)
            {
                throw new KeyNotFoundException($"There is no exercise instance with an id of {id}.");
            }
            else
            {
                if (toUpdate.Workout is not null &&
                    toUpdate.Workout.WorkoutProgram is not null &&
                    toUpdate.Workout.WorkoutProgram.UserId != currentUserId)
                {
                    throw new NotSupportedException($"The exercise instance with an ID of {id} belongs to another user!");
                }

                toUpdate.IsComplete = !toUpdate.IsComplete;

                _context.SaveChanges();

                return;
            }

        }
    }
}
