using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models;
using CogniFitRepo.Shared.DataTransferObjects;
using System.Security.Claims;
using CogniFitRepo.Server.Models.ExerciseModels;
using Microsoft.AspNetCore.Identity;
using CogniFitRepo.Server.Models.UserModels;
using NuGet.Protocol.Core.Types;
using CogniFitRepo.Server.Repositories;

namespace CogniFitRepo.Server.Controllers
{
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWorkoutRepository _repository;

        public WorkoutController(ApplicationDbContext context,
                                 UserManager<ApplicationUser> userManager,
                                 IWorkoutRepository repository)
        {
            _context = context;
            _userManager = userManager;
            _repository = repository;
        }

        // default endpoints, created by Visual Studio:

        // Get all workouts
        [HttpGet]
        [Route("api/workout")]
        public async Task<ActionResult<IEnumerable<Workout>>> GetWorkouts()
        {
            try
            {
                return Ok(await Task.Run(() => _repository.GetWorkouts()));
            }
            catch (MissingMemberException e)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/workout/id/5
        [HttpGet]
        [Route("api/workout/id/{id}")]
        public async Task<ActionResult<WorkoutDto>> GetWorkout(long id)
        {
            try
            {
                var retVal = await Task.Run(() => _repository.GetWorkout(id));

                return Ok(retVal);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //var workout = await _context.Workouts.FindAsync(id);

            //if (workout == null)
            //{
            //    return NotFound();
            //}

            //return workout;
        }

        // PUT: api/Workout/5
        // This will not affect the exercise instances owned by this workout
        // Any exercies instance DTOs in the workout DTO passed in will be ignored
        // This will not move workouts between workout programs.
        // Any WorkoutProgramId in the workout DTO passed in will be ignored
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("api/workout/{id}")]
        public async Task<IActionResult> PutWorkout(long id, [FromBody] WorkoutDto workoutDto)
        {
            try
            {
                await Task.Run(() => _repository.PutWorkout(id, workoutDto));

                return NoContent();
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            // default stuff
            //_context.Entry(workout).State = EntityState.Modified;
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!WorkoutExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
        }

        // DELETE: api/Workout/5
        [HttpDelete]
        [Route("api/workout/{id}")]
        public async Task<IActionResult> DeleteWorkout(long id)
        {
            try
            {
                await Task.Run(() => _repository.DeleteWorkout(id));

                return NoContent();
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Our specific controllers:

        // Get all workouts a user has planned for a certain date
        [HttpGet]
        [Route("api/workout/date/{dateStr}")]
        public async Task<ActionResult<Workout>> GetWorkoutsByDateForCurrentUser(string dateStr)
        {
            try
            {
                var retVal = await Task.Run(() => _repository.GetWorkoutsByDateForCurrentUser(dateStr, User));
                return Ok(retVal);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Get all workouts in some workout program
        [HttpGet]
        [Route("api/workout/workout-program-id/{workoutProgramId}")]
        public async Task<ActionResult<WorkoutDto>> GetWorkoutsByWorkoutProgramId(long workoutProgramId)
        {
            try
            {
                var retVal = await Task.Run(() => _repository.GetWorkoutsByWorkoutProgramId(workoutProgramId));

                return Ok(retVal);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }


            // Distinguishing between an invalid workout program id,
            // and a simple lack of workouts for that program????
            //if (workouts.Count() == 0)
            //{
            //    return NotFound();
            //} 

            //List<Workout> workouts = await (from workout in _context.Workouts
            //                                where workout.WorkoutProgramId == workoutProgramId
            //                                select workout
            //                    ).ToListAsync();

            //return Ok(workouts); // get empty list for nonexistent workout id???
        }

        // Post a list of workouts for a certain workout program
        // To post a single workout, just post a list containing a single workout
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("api/workout/workout-program-id/{workoutProgramId}")]
        public async Task<ActionResult<Workout>> PostWorkouts(long workoutProgramId, [FromBody] IEnumerable<WorkoutDto> workoutDtos)
        {
            await Task.Run(() => _repository.PostWorkouts(workoutProgramId, workoutDtos));

            return NoContent();

            //_context.Workouts.Add(workout);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetWorkout", new { id = workout.Id }, workout);
        }


        //Default helper method
        private bool WorkoutExists(long id)
        {
            return (_context.Workouts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
