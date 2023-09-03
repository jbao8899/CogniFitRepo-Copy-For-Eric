using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models;
using System.Security.Claims;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static CogniFitRepo.Client.Pages.Profile;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Server.Models.ExerciseModels;
using CogniFitRepo.Server.Repositories;

namespace CogniFitRepo.Server.Controllers
{
    [ApiController]
    public class WorkoutProgramController : ControllerBase
    {
        private readonly ILogger<BiometricInformationRepository> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWorkoutProgramRepository _repository;

        public WorkoutProgramController(
            ILogger<BiometricInformationRepository> logger,
            UserManager<ApplicationUser> userManager,
            IWorkoutProgramRepository repository
        )
        {
            _logger = logger;
            _userManager = userManager;
            _repository = repository;
        }

        // Default, boilerplate endpoints:

        [HttpGet]
        [Route("api/workout-program")] // Not using Authorize, because we get the user ID inside and can check it there, and that is needed for some repository functions
        public async Task<ActionResult<IEnumerable<WorkoutProgramDto>>> GetWorkoutPrograms()
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the GET api/workout-program endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                var retval = Ok(await Task.Run(() => _repository.GetWorkoutPrograms()));

                _logger.LogInformation("The user with an ID of {currentUserId} retrieved all workout programs at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    DateTimeOffset.UtcNow);

                return retval;
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("api/workout-program/{id}")]
        public async Task<ActionResult<WorkoutProgramDto>> GetWorkoutProgram(long id)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the GET api/workout-program/" + id + " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                var retval = Ok(await Task.Run(() => _repository.GetWorkoutProgram(id)));

                _logger.LogInformation("The user with an ID of {currentUserId} retrieved the workout program with an ID of {id} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);

                return retval;
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
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
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
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
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
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
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // This will not affect the workouts owned by this workout program
        // Any workout DTOs in the workout program DTO passed in will be ignored
        // This will not move workout programs between users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("api/workout-program/{id}")]
        public async Task<IActionResult> PutWorkoutProgram(long id, [FromBody] WorkoutProgramDto workoutProgramDto)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the PUT api/workout-program/" + id + " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                await Task.Run(() => _repository.PutWorkoutProgram(id, workoutProgramDto, currentUserId));

                _logger.LogInformation("The user with an ID of {currentUserId} updated the workout program with an ID of {id} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);

                return NoContent();
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
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
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
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
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
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
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // Post a workout program based on a DTO. Post all workouts contained within
        [HttpPost]
        [Route("api/workout-program")]
        public async Task<ActionResult> PostWorkoutProgram([FromBody] WorkoutProgramDto workoutProgramDto)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the POST api/workout-program endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                await Task.Run(() => _repository.PostWorkoutProgram(workoutProgramDto, currentUserId));

                _logger.LogInformation("The user with an ID of {currentUserId} added a workout program at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    DateTimeOffset.UtcNow);

                return NoContent(); // ???
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/workout-program endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/workout-program endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/workout-program endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/workout-program endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }


        [HttpDelete]
        [Route("api/workout-program/{id}")]
        public async Task<IActionResult> DeleteWorkoutProgram(long id)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the DELETE api/workout-program/" + id + " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                await Task.Run(() => _repository.DeleteWorkoutProgram(id, currentUserId));

                _logger.LogInformation("The user with an ID of {currentUserId} deleted the workout program with an ID of {id} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);

                return NoContent(); // ???
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
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
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
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
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
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
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/workout-program/{id} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    id,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // Added controllers:

        // gets all workout programs belonging to the current user
        [HttpGet]
        [Authorize]
        [Route("api/workout-program/for-current-user")]
        public async Task<ActionResult<IEnumerable<WorkoutProgramDto>>> GetWorkoutProgramsForCurrentUser()
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the GET api/workout-program/for-current-user endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                var retval = Ok(await Task.Run(() => _repository.GetWorkoutProgramsForUser(currentUserId)));

                _logger.LogInformation("The user with an ID of {currentUserId} retrieved all of their own workout programs at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    DateTimeOffset.UtcNow);

                return retval;
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // gets all completed workout programs belonging to the current user
        [HttpGet]
        [Route("api/workout-program/for-current-user/complete")]
        public async Task<ActionResult<IEnumerable<WorkoutProgram>>> GetCompletedWorkoutProgramsForCurrentUser()
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the GET api/workout-program/for-current-user/complete endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                var retval = Ok(await Task.Run(() => _repository.GetCompletedWorkoutProgramsForUser(currentUserId)));

                _logger.LogInformation("The user with an ID of {currentUserId} retrieved all complete workout programs belonging to them at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    DateTimeOffset.UtcNow);

                return retval;
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user/complete endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user/complete endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user/complete endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user/complete endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // gets all incomplete workout programs belonging to the current user
        [HttpGet]
        [Route("api/workout-program/for-current-user/incomplete")]
        public async Task<ActionResult<IEnumerable<WorkoutProgram>>> GetIncompleteWorkoutProgramsForCurrentUser()
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the GET api/workout-program/for-current-user/incomplete endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                var retval = Ok(await Task.Run(() => _repository.GetIncompleteWorkoutProgramsForUser(currentUserId)));

                _logger.LogInformation("The user with an ID of {currentUserId} retrieved all incomplete workout programs belonging to them at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    DateTimeOffset.UtcNow);

                return retval;
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user/incomplete endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user/incomplete endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user/incomplete endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/workout-program/for-current-user/incomplete endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // Default Helper Method
        //private bool WorkoutProgramExists(long id)
        //{
        //    return (_context.WorkoutPrograms?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
