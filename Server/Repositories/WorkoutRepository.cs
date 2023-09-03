using CogniFitRepo.Server.Controllers;
using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models;
using CogniFitRepo.Server.Models.ExerciseModels;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static CogniFitRepo.Client.Pages.Profile;

namespace CogniFitRepo.Server.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<WorkoutController> _logger;

        public WorkoutRepository(ApplicationDbContext context, 
                                 UserManager<ApplicationUser> userManager,
                                 ILogger<WorkoutController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IEnumerable<WorkoutDto> GetWorkouts()
        {
            if (_context.Workouts is null)
            {
                _logger.LogError("Workouts in the Context were Null on {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new MissingMemberException();
            }

            var retVal = _context.Workouts
                                 .Include(w => w.ExerciseInstances)
                                     .ThenInclude(ei => ei.Exercise)
                                 .Include(w => w.ExerciseInstances) // Repeat like so
                                     .ThenInclude(ei => ei.ExerciseInstance_ExerciseProperties)
                                         .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                                 .Select(w => w.ToDto())
                                 .ToList();

            return retVal;
        }

        public WorkoutDto GetWorkout(long id)
        {
            if (_context.Workouts is null)
            {
                _logger.LogError("Workout Request for {id} in the Context were Null at {Placeholder:MMMM dd, yyyy}", id, DateTimeOffset.UtcNow);
                throw new MissingMemberException();
            }
            var retVal = _context.Workouts
                                       .Where(w => w.Id == id)
                                       .Include(w => w.ExerciseInstances)
                                           .ThenInclude(ei => ei.Exercise)
                                       .Include(w => w.ExerciseInstances) // Repeat like so
                                           .ThenInclude(ei => ei.ExerciseInstance_ExerciseProperties)
                                               .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                                       .Select(w => w.ToDto())
                                       .FirstOrDefault();
            if (retVal is not null)
            {
                return retVal;
            }
            else
            {
                _logger.LogError("GetWorkout(long id) Query returned a Null on {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
        }

        public void PutWorkout(long id, [FromBody] WorkoutDto workoutDto)
        {
            if (_context.Workouts is null ||
                _context.WorkoutPrograms is null)
            {
                _logger.LogError("PutWorkout() _context.Workouts or _context.WorkoutPrograms is Null on {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }


            if (workoutDto.Id is null ||
                id != workoutDto.Id)
            {
                _logger.LogError("PutWorkout() workoutDto.Id is null or id != workoutDto.Id on {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }

            Workout? toUpdate = _context.Workouts?.FirstOrDefault(w => w.Id == id);
            if (toUpdate is null)
            {
                _logger.LogError("PutWorkout() Query _context.Workouts?.FirstOrDefault() is null on {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }

            WorkoutProgram? workoutProgram = null;
            if (workoutDto.WorkoutProgramId is not null)
            {
                workoutProgram = _context.WorkoutPrograms.FirstOrDefault(wp => wp.Id == workoutDto.WorkoutProgramId);
                if (workoutProgram is null)
                {
                    _logger.LogError("PutWorkout() Query _context.WorkoutPrograms.FirstOrDefault() is null on {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                    throw new ArgumentNullException();
                }
            }

            // ignore WorkoutProgramId

            // Check if new date is correct
            if (workoutProgram is not null &&
                (workoutDto.WorkoutDateTime.Date < workoutProgram.StartDate.Date ||
                 workoutDto.WorkoutDateTime.Date > workoutProgram.EndDate.Date))
            {
                _logger.LogError("PutWorkout() workoutDto.WorkoutDateTime is not between the start and end dates of the workout program {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentException();
            }
            else
            {
                toUpdate.WorkoutDateTime = workoutDto.WorkoutDateTime;
            }

            // Update Notes
            if (workoutDto.Notes is not null)
            {
                toUpdate.Notes = workoutDto.Notes;
            }

            toUpdate.IsComplete = workoutDto.IsComplete;

            _context.SaveChanges();
        }

        public void DeleteWorkout(long id)
        {
            if (_context.Workouts is null)
            {
                _logger.LogError("DeleteWorkout(long id) _context.Workouts was Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }

            // Use includes to cascade deletion????
            var workout =  _context.Workouts?
                                        .Where(w => w.Id == id)
                                        .Include(w => w.ExerciseInstances)
                                           .ThenInclude(ei => ei.ExerciseInstance_ExerciseProperties)
                                        .FirstOrDefault();

            if (workout is null)
            {
                _logger.LogError("DeleteWorkout(long id) Query: workout was Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
            _context.Workouts.Remove(workout);
            _context.SaveChanges();
        }

        public WorkoutDto GetWorkoutsByDateForCurrentUser(string dateStr, ClaimsPrincipal user)
        {
            if (_context.Workouts is null)
            {
                _logger.LogError("GetWorkoutsByDateForCurrentUser() _context.Workouts is Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }

            DateTime desiredDateTime;
            if (!DateTime.TryParse(dateStr, out desiredDateTime))
            {
                _logger.LogError("GetWorkoutsByDateForCurrentUser() dateStr was not able to be converted to DateTime at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentException();
            }
            if(user is null)
            {
                _logger.LogError("GetWorkoutsByDateForCurrentUser() user was null");
                throw new ArgumentNullException();
            }

            //string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // does not work
            string? currentUserId = _userManager.GetUserId(user); // works

            if (currentUserId is null)
            {
                _logger.LogError("GetWorkoutsByDateForCurrentUser() _userManager.GetUserId() is Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }

            var retVal = _context.Workouts
                                       .Where(w => w.WorkoutProgram != null)
                                       .Where(w => w.WorkoutProgram!.UserId == currentUserId)
                                       .Where(w => w.WorkoutDateTime.Date == desiredDateTime.Date)
                                       .Include(w => w.ExerciseInstances)
                                           .ThenInclude(ei => ei.Exercise)
                                       .Include(w => w.ExerciseInstances) // Repeat like so
                                           .ThenInclude(ei => ei.ExerciseInstance_ExerciseProperties)
                                               .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                                       .Select(w => w.ToDto())
                                       .FirstOrDefault();
            if (retVal is null)
            {
                _logger.LogWarning("GetWorkoutsByDateForCurrentUser() Query retVal returned Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
            return retVal;
        }

        public IEnumerable<WorkoutDto> GetWorkoutsByWorkoutProgramId(long workoutProgramId)
        {
            if (_context.Workouts is null)
            {
                _logger.LogError("GetWorkoutsByWorkoutProgramId() _context.Workouts is Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }

            if (_context.WorkoutPrograms.Find(workoutProgramId) is null)
            {
                _logger.LogError("GetWorkoutsByWorkoutProgramId() _context.WorkoutPrograms.Find() is Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }

            // want to avoid extra query by looking at workout programs to see if this id exists????

            var retVal = _context.Workouts
                           .Where(w => w.WorkoutProgramId == workoutProgramId)
                           .Include(w => w.ExerciseInstances)
                               .ThenInclude(ei => ei.Exercise)
                           .Include(w => w.ExerciseInstances) // Repeat like so
                               .ThenInclude(ei => ei.ExerciseInstance_ExerciseProperties)
                                   .ThenInclude(ei_ep => ei_ep.ExerciseProperty)
                           .Select(w => w.ToDto())
                           .ToList();

            return retVal;
        }

        public void PostWorkouts(long workoutProgramId, [FromBody] IEnumerable<WorkoutDto> workoutDtos)
        {
            if (_context.WorkoutPrograms is null)
            {
                _logger.LogError("PostWorkouts() _context.WorkoutPrograms is Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
            if (_context.Workouts is null)
            {
                _logger.LogError("PostWorkouts() _context.Workouts is Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
            if (_context.ExerciseInstances is null)
            {
                _logger.LogError("PostWorkouts() _context.ExerciseInstances is Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
            if (_context.Exercises is null)
            {
                _logger.LogError("PostWorkouts() _context.Exercises is Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
            if (_context.ExerciseInstance_ExerciseProperties is null)
            {
                _logger.LogError("PostWorkouts() _context.ExerciseInstance_ExerciseProperties is Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
            if (_context.ExerciseProperties is null)
            {
                _logger.LogError("PostWorkouts() _context.ExerciseProperties is Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }

            WorkoutProgram? workoutProgram = _context.WorkoutPrograms?.Find(workoutProgramId);
            if (workoutProgram is null)
            {
                _logger.LogError("PostWorkouts() Query: workoutProgram is Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }

            foreach (WorkoutDto workoutDto in workoutDtos)
            {
                // Check that dates are in right range for each workout
                if (workoutDto.WorkoutDateTime.Date < workoutProgram.StartDate.Date ||
                    workoutDto.WorkoutDateTime.Date > workoutProgram.EndDate.Date)
                {
                    _logger.LogError("PostWorkouts() workoutDtos.Date is outside the workoutProgram start and end Date at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                    throw new ArgumentOutOfRangeException();
                }

                // prevent error if there are no exercise instances for this workout
                if (workoutDto.ExerciseInstanceDtos is null)
                {
                    continue;
                }

                foreach (ExerciseInstanceDto exerciseInstanceDto in workoutDto.ExerciseInstanceDtos)
                {
                    // check that the exercise being referred to exists
                    if (!(_context.Exercises.Any(e => e.Id == exerciseInstanceDto.ExerciseId)))
                    {
                        _logger.LogError("PostWorkouts() exerciseInstanceDto.ExerciseId doesn't exist. At {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                        throw new ArgumentException();
                    }

                    // prevent error if there are no joining table entries for this exercise instance
                    if (exerciseInstanceDto.ExerciseInstance_ExercisePropertyDtos is null)
                    {
                        continue;
                    }

                    // check that all exercise properties exist
                    foreach (ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto in exerciseInstanceDto.ExerciseInstance_ExercisePropertyDtos)
                    {
                        if (!(_context.ExerciseProperties.Any(ep => ep.Id == exerciseInstance_ExercisePropertyDto.ExercisePropertyId)))
                        {
                            _logger.LogError("PostWorkouts() exerciseInstance_ExercisePropertyDto.ExercisePropertyId doesn't exist. At {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                            throw new ArgumentException();
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
                        _logger.LogError("PostWorkouts() Your sequence of WorkoutSequenceNumbers for the exercise instances of at least one exercise do not range between 1 and the number of exercise instances for that workout. {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }

            // Create all the Workouts
            foreach (WorkoutDto workoutDto in workoutDtos)
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
                    _context.SaveChanges(); // Save here????

                }
            }
        }
    }
}
