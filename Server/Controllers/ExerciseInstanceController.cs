using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models;
using CogniFitRepo.Server.Models.ExerciseModels;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Server.Repositories;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CogniFitRepo.Server.Controllers
{
    [ApiController]
    public class ExerciseInstanceController : ControllerBase
    {
        private readonly ILogger<BiometricInformationRepository> _logger;
        private readonly IExerciseInstanceRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExerciseInstanceController(
            ILogger<BiometricInformationRepository> logger,
            IExerciseInstanceRepository repository,
            UserManager<ApplicationUser> userManager
        )
        {
            _logger = logger;
            _repository = repository;
            _userManager = userManager;
        }

        // Default, boilerplate endpoints:

        [HttpGet]
        [Route("api/exercise-instance")]
        public async Task<ActionResult<IEnumerable<ExerciseInstanceDto>>> GetAll()
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the GET api/exercise-instance endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                var retval = Ok(await Task.Run(() => _repository.GetAll()));

                _logger.LogInformation("The user with an ID of {currentUserId} retrieved all exercise instances at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    DateTimeOffset.UtcNow);

                return retval;
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("api/exercise-instance/{id}")]
        public async Task<ActionResult<ExerciseInstanceDto>> GetById(long id)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the GET api/exercise-instance/" + id + " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                var retval = Ok(await Task.Run(() => _repository.GetById(id)));

                _logger.LogInformation("The user with an ID of {currentUserId} retrieved the exercise instance with an ID of {id} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);

                return retval;
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // POST an exercise instance to a specified workout
        // The position of the exercise instance within that workout's sequence of events will be at the end
        // The WorkoutSequenceNumber field will be ignored
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("api/exercise-instance/workout-id/{workoutId}")]
        public async Task<IActionResult> PostToWorkout(long workoutId, [FromBody] ExerciseInstanceDto exerciseInstanceDto)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the GET api/exercise-instance/workout-id/" + workoutId + " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                await Task.Run(() => _repository.PostToWorkout(workoutId, exerciseInstanceDto, currentUserId));

                _logger.LogInformation("The user with an ID of {currentUserId} posted an exercise instance belonging to the workout with an ID of {workoutId} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    workoutId,
                    DateTimeOffset.UtcNow);

                return NoContent(); // Return value???
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/exercise-instance/workout-id/{workoutId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    workoutId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/exercise-instance/workout-id/{workoutId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    workoutId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/exercise-instance/workout-id/{workoutId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    workoutId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/exercise-instance/workout-id/{workoutId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    workoutId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/exercise-instance/5
        [HttpDelete]
        [Route("api/exercise-instance/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the DELETE api/exercise-instance/" + id + " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                await Task.Run(() => _repository.Delete(id, currentUserId));

                _logger.LogInformation("The user with an ID of {currentUserId} deleted the exercise instance with an ID of {id} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);

                return NoContent();
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/exercise-instance/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/exercise-instance/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/exercise-instance/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/exercise-instance/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // Our endpoints:

        // get all exercise instances in a particular workout
        [HttpGet]
        [Route("api/exercise-instance/workout-id/{workoutId}")]
        public async Task<ActionResult<IEnumerable<ExerciseInstanceDto>>> GetByWorkoutId(long workoutId)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the GET api/exercise-instance/workout-id/" + workoutId + " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                var retval = Ok(await Task.Run(() => _repository.GetByWorkoutId(workoutId)));

                _logger.LogInformation("The user with an ID of {currentUserId} retrieved all exercise instances belonging to the workout with an ID of {workoutId} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    workoutId,
                    DateTimeOffset.UtcNow);

                return retval;
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance/workout-id/{workoutId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    workoutId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance/workout-id/{workoutId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    workoutId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance/workout-id/{workoutId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    workoutId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance/workout-id/{workoutId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    workoutId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // Change which exercise an ExerciseInstance points to
        [HttpPut]
        [Route("api/exercise-instance/id/{id}/new-exercise-id/{newExerciseId}")]
        public async Task<IActionResult> ChangeExercise(long id, int newExerciseId)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the PUT api/exercise-instance/id/" +
                    id +
                    "/new-exercise-id/" + 
                    newExerciseId +
                    " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                await Task.Run(() => _repository.ChangeExercise(id, newExerciseId, currentUserId));

                _logger.LogInformation("The user with an ID of {currentUserId} made the exercise instance with an ID of {id} point to the exercise with an ID of {newExerciseId} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    id,
                    newExerciseId,
                    DateTimeOffset.UtcNow);

                return NoContent();
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance/id/{id}/new-exercise-id/{newExerciseId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    newExerciseId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance/id/{id}/new-exercise-id/{newExerciseId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    newExerciseId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance/id/{id}/new-exercise-id/{newExerciseId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    newExerciseId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance/id/{id}/new-exercise-id/{newExerciseId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    newExerciseId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // Swap the positions of two ExerciseInstance objects within their workout 
        [HttpPut]
        [Route("api/exercise-instance/first-id/{firstId}/second-id/{secondId}")]
        public async Task<IActionResult> SwapPositions(long firstId, long secondId)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the PUT api/exercise-instance/first-id/" +
                    firstId +
                    "/second-id/" +
                    secondId +
                    " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                await Task.Run(() => _repository.SwapPositions(firstId, secondId, currentUserId));

                _logger.LogInformation("The user with an ID of {currentUserId} swapped the positions of the exercise instances with IDs of {firstId} and {secondId} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    firstId,
                    secondId,
                    DateTimeOffset.UtcNow);

                return NoContent();
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance/first-id/{firstId}/second-id/{secondId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    firstId,
                    secondId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance/first-id/{firstId}/second-id/{secondId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    firstId,
                    secondId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance/first-id/{firstId}/second-id/{secondId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    firstId,
                    secondId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance/first-id/{firstId}/second-id/{secondId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    firstId,
                    secondId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // Toggle if an exercise instance is complete or not
        [HttpPut]
        [Route("api/exercise-instance/toggle-completion-for-id/{id}")]
        public async Task<IActionResult> ToggleCompletion(long id)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the PUT api/exercise-instance/toggle-completion-for-id/" +
                    id +
                    " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                await Task.Run(() => _repository.ToggleCompletion(id, currentUserId));

                _logger.LogInformation("The user with an ID of {currentUserId} toggled the completion status of the exercise instance with an ID of {id} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);

                return NoContent();
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance/toggle-completion-for-id/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                return StatusCode(403, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //// provided helper function:
        //private bool ExerciseInstanceExists(long id)
        //{
        //    return (_context.ExerciseInstances?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
