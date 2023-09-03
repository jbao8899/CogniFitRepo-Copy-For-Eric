using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models;
using CogniFitRepo.Server.Models.ExerciseModels;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static CogniFitRepo.Client.Pages.Profile;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace CogniFitRepo.Server.Repositories
{
    public class WorkoutProgramRepository : IWorkoutProgramRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkoutProgramRepository(ApplicationDbContext context)
        { 
            _context = context;
        }

        public IEnumerable<WorkoutProgramDto> GetWorkoutPrograms()
        {
            if (_context.WorkoutPrograms is null ||
                _context.Workouts is null ||
                _context.ExerciseInstances is null ||
                _context.Exercises is null ||
                _context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            //return await _context.WorkoutPrograms.ToListAsync();

            var retVal = _context.WorkoutPrograms
                                 .Include(wp => wp.Workouts)
                                     .ThenInclude(w => w.ExerciseInstances)
                                         .ThenInclude(ei => ei.Exercise)
                                 .Include(wp => wp.Workouts)
                                     .ThenInclude(w => w.ExerciseInstances)
                                         .ThenInclude(ei => ei.ExerciseInstance_ExerciseProperties)
                                             .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                                 .Select(wp => wp.ToDto())
                                 .ToList();

            return retVal;
        }

        public WorkoutProgramDto GetWorkoutProgram(long id)
        {
            if (_context.WorkoutPrograms is null ||
                _context.Workouts is null ||
                _context.ExerciseInstances is null ||
                _context.Exercises is null ||
                _context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            var retVal = _context.WorkoutPrograms
                                 .Where(w => w.Id == id)
                                 .Include(wp => wp.Workouts)
                                     .ThenInclude(w => w.ExerciseInstances)
                                         .ThenInclude(ei => ei.Exercise)
                                 .Include(wp => wp.Workouts)
                                     .ThenInclude(w => w.ExerciseInstances)
                                         .ThenInclude(ei => ei.ExerciseInstance_ExerciseProperties)
                                             .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                                 .Select(wp => wp.ToDto())
                                 .FirstOrDefault();

            if (retVal == null)
            {
                throw new KeyNotFoundException($"There is no workout program with an ID of {id}.");
            }

            return retVal;

        }

        public void PutWorkoutProgram(long id, WorkoutProgramDto workoutProgramDto, string currentUserId)
        {
            if (_context.WorkoutPrograms is null ||
                _context.Workouts is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            if (workoutProgramDto.Id is null ||
                id != workoutProgramDto.Id)
            {
                // It is apparently convention to do this, although it is unneccessary
                throw new ArgumentException($"The ID in the API URL must equal the ID in the DTO passed in.");
            }

            // check that the start date is before the end date
            if (workoutProgramDto.StartDate > workoutProgramDto.EndDate)
            {
                throw new ArgumentException("The start date must be before the end date.");
            }

            WorkoutProgram? toUpdate = _context.WorkoutPrograms.FirstOrDefault(wp => wp.Id == id);
            if (toUpdate == null)
            {
                throw new KeyNotFoundException($"Could not find a workout program with an id of {id}.");
            }

            // Check that this workout belongs to the current user
            if (toUpdate.UserId != currentUserId)
            {
                throw new NotSupportedException($"The workout program with an ID of {id} belongs to another user!");
            }

            // The User ID in the input DTO will be ignored

            toUpdate.Name = workoutProgramDto.Name;

            // check that the new date range contains all existing workout dates
            IEnumerable<Workout> workouts = (from workout in _context.Workouts
                                             where workout.WorkoutProgramId == toUpdate.Id
                                             select workout);

            foreach (Workout workout in workouts)
            {
                if (workout.WorkoutDateTime.Date < workoutProgramDto.StartDate.Date ||
                    workout.WorkoutDateTime.Date > workoutProgramDto.EndDate.Date)
                {
                    throw new ArgumentException($"Your new date range does not contain the date for the workout with an ID of {workout.Id}.");
                }
            }

            // update the dates
            toUpdate.StartDate = workoutProgramDto.StartDate.Date;
            toUpdate.EndDate = workoutProgramDto.EndDate.Date;

            if (workoutProgramDto.Notes is not null)
            {
                toUpdate.Notes = workoutProgramDto.Notes;
            }

            toUpdate.IsComplete = workoutProgramDto.IsComplete;

            _context.SaveChanges();

            return;
        }

        // There is a lot of duplicated code here
        public void PostWorkoutProgram(WorkoutProgramDto workoutProgramDto, string currentUserId)
        {
            if (_context.WorkoutPrograms is null ||
                _context.Workouts is null ||
                _context.ExerciseInstances is null ||
                _context.Exercises is null ||
                _context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            // Prevent error
            if (workoutProgramDto.WorkoutDtos is null)
            {
                workoutProgramDto.WorkoutDtos = new List<WorkoutDto>();
            }

            // check that the start date is before the end date
            if (workoutProgramDto.StartDate > workoutProgramDto.EndDate)
            {
                throw new ArgumentException("The start date must be before the end date.");
            }

            foreach (WorkoutDto workoutDto in workoutProgramDto.WorkoutDtos)
            {
                // Check that dates are in right range for each workout
                if (workoutDto.WorkoutDateTime.Date < workoutProgramDto.StartDate.Date ||
                    workoutDto.WorkoutDateTime.Date > workoutProgramDto.EndDate.Date)
                {
                    throw new ArgumentException($"{workoutDto.WorkoutDateTime} is not between the start and end dates of the workout program.");
                }

                // prevent error if there are no exercise instances for this workout
                if (workoutDto.ExerciseInstanceDtos is null)
                {
                    continue;
                }

                foreach (ExerciseInstanceDto exerciseInstanceDto in workoutDto.ExerciseInstanceDtos)
                {
                    // check that the exercise being referred to exists
                    if (!_context.Exercises.Any(e => e.Id == exerciseInstanceDto.ExerciseId))
                    {
                        throw new KeyNotFoundException($"There is no exercise with an ID of {exerciseInstanceDto.ExerciseId}.");
                    }

                    // prevent error if there are no joining table entries for this exercise instance
                    if (exerciseInstanceDto.ExerciseInstance_ExercisePropertyDtos == null)
                    {
                        continue;
                    }

                    // check that all exercise properties exist
                    foreach (ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto in exerciseInstanceDto.ExerciseInstance_ExercisePropertyDtos)
                    {
                        if (!_context.ExerciseProperties.Any(ep => ep.Id == exerciseInstance_ExercisePropertyDto.ExercisePropertyId))
                        {
                            throw new KeyNotFoundException($"There is no exercise property with an ID of {exerciseInstance_ExercisePropertyDto.ExercisePropertyId}.");
                        }
                    }
                }

                // check that, if the workout has exercise instances, their "WorkoutSequenceNumber"s
                // range from 1 to the number of exercise instances
                List<int> workoutSequenceNumbers = (from exerciseInstance in workoutDto.ExerciseInstanceDtos
                                                    orderby exerciseInstance.WorkoutSequenceNumber ascending
                                                    select exerciseInstance.WorkoutSequenceNumber).ToList();
                for (int i = 1; i <= workoutDto.ExerciseInstanceDtos.Count(); i++)
                {
                    if (workoutSequenceNumbers[i - 1] != i)
                    {
                        throw new ArgumentException("Your sequence of WorkoutSequenceNumbers for the exercise instances of at least one exercise do not range between 1 and the number of exercise instances for that workout.");
                    }
                }
            }

            // Create the WorkoutProgram
            WorkoutProgram workoutProgram = new WorkoutProgram(workoutProgramDto);
            workoutProgram.UserId = currentUserId; // the controller passes in the current user's ID
            _context.WorkoutPrograms.Add(workoutProgram);
            _context.SaveChanges(); // Do this to populate workoutProgram.Id?????

            // Create all the Workouts
            foreach (WorkoutDto workoutDto in workoutProgramDto.WorkoutDtos)
            {
                Workout workout = new Workout(workoutDto);
                workout.WorkoutProgramId = workoutProgram.Id;
                _context.Workouts.Add(workout);
                _context.SaveChanges(); // Do this to populate workout.Id????? Expensive????

                // prevent error if there are no exercise instances for this workout
                if (workoutDto.ExerciseInstanceDtos is null)
                {
                    continue;
                }

                // create all the exercise instances
                foreach (ExerciseInstanceDto exerciseInstanceDto in workoutDto.ExerciseInstanceDtos)
                {
                    ExerciseInstance exerciseInstance = new ExerciseInstance(exerciseInstanceDto);
                    exerciseInstance.WorkoutId = workout.Id;
                    _context.ExerciseInstances.Add(exerciseInstance);
                    _context.SaveChanges(); // Do this to populate exerciseInstance.Id?????

                    // prevent error if there are no joining table entries for this exercise instance
                    if (exerciseInstanceDto.ExerciseInstance_ExercisePropertyDtos == null)
                    {
                        continue;
                    }

                    // create all the joining table entries
                    foreach (ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto in exerciseInstanceDto.ExerciseInstance_ExercisePropertyDtos)
                    {
                        ExerciseInstance_ExerciseProperty exerciseInstance_ExerciseProperty = new ExerciseInstance_ExerciseProperty(exerciseInstance_ExercisePropertyDto);
                        exerciseInstance_ExerciseProperty.ExerciseInstanceId = exerciseInstance.Id;
                        _context.ExerciseInstance_ExerciseProperties.Add(exerciseInstance_ExerciseProperty);
                    }
                    _context.SaveChanges(); // Save here???? Will save at new loop anyways

                }
            }

            return; // What to return???
            //return CreatedAtAction("GetWorkoutProgram", new { id = workoutProgram.Id }, workoutProgram);
        }

        public void DeleteWorkoutProgram(long id, string currentUserId)
        {
            if (_context.WorkoutPrograms is null ||
                _context.Workouts is null ||
                _context.ExerciseInstances is null ||
                _context.ExerciseInstance_ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            // Apparently, all these includes are needed for the
            // deletion to work
            var workoutProgram = _context.WorkoutPrograms
                                         .Where(wp => wp.Id == id)
                                         .Include(wp => wp.Workouts)
                                             .ThenInclude(w => w.ExerciseInstances)
                                                 .ThenInclude(ei => ei.ExerciseInstance_ExerciseProperties)
                                         .FirstOrDefault();

            if (workoutProgram == null)
            {
                throw new KeyNotFoundException($"There is no workout program with an ID of {id}.");
            }

            // Check that this workout belongs to the current user
            if (workoutProgram.UserId != currentUserId)
            {
                throw new NotSupportedException($"The workout program with an ID of {id} belongs to another user!");
            }

            _context.WorkoutPrograms.Remove(workoutProgram);
            _context.SaveChanges();

            return;
        }

        public IEnumerable<WorkoutProgramDto> GetWorkoutProgramsForUser(string currentUserId)
        {
            if (_context.WorkoutPrograms is null ||
                _context.Workouts is null ||
                _context.ExerciseInstances is null ||
                _context.Exercises is null ||
                _context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            var retVal = _context.WorkoutPrograms
                                 .Where(wp => wp.UserId == currentUserId)
                                 .Include(wp => wp.Workouts)
                                     .ThenInclude(w => w.ExerciseInstances)
                                         .ThenInclude(ei => ei.Exercise)
                                 .Include(wp => wp.Workouts)
                                     .ThenInclude(w => w.ExerciseInstances)
                                         .ThenInclude(ei => ei.ExerciseInstance_ExerciseProperties)
                                             .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                                 .Select(wp => wp.ToDto())
                                 .ToList();

            return retVal;
        }

        public IEnumerable<WorkoutProgramDto> GetCompletedWorkoutProgramsForUser(string currentUserId)
        {
            if (_context.WorkoutPrograms is null ||
                _context.Workouts is null ||
                _context.ExerciseInstances is null ||
                _context.Exercises is null ||
                _context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            var retVal = _context.WorkoutPrograms
                                 .Where(wp => wp.UserId == currentUserId)
                                 .Where(wp => wp.IsComplete)
                                 .Include(wp => wp.Workouts)
                                     .ThenInclude(w => w.ExerciseInstances)
                                         .ThenInclude(ei => ei.Exercise)
                                 .Include(wp => wp.Workouts)
                                     .ThenInclude(w => w.ExerciseInstances)
                                         .ThenInclude(ei => ei.ExerciseInstance_ExerciseProperties)
                                             .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                                 .Select(wp => wp.ToDto())
                                 .ToList();

            return retVal;
        }

        public IEnumerable<WorkoutProgramDto> GetIncompleteWorkoutProgramsForUser(string currentUserId)
        {
            if (_context.WorkoutPrograms is null ||
                _context.Workouts is null ||
                _context.ExerciseInstances is null ||
                _context.Exercises is null ||
                _context.ExerciseInstance_ExerciseProperties is null ||
                _context.ExerciseProperties is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            var retVal = _context.WorkoutPrograms
                                 .Where(wp => wp.UserId == currentUserId)
                                 .Where(wp => !wp.IsComplete)
                                 .Include(wp => wp.Workouts)
                                     .ThenInclude(w => w.ExerciseInstances)
                                         .ThenInclude(ei => ei.Exercise)
                                 .Include(wp => wp.Workouts)
                                     .ThenInclude(w => w.ExerciseInstances)
                                         .ThenInclude(ei => ei.ExerciseInstance_ExerciseProperties)
                                             .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                                 .Select(wp => wp.ToDto())
                                 .ToList();

            return retVal;
        }
    }
}
