using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.ExerciseModels;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Server.Repositories;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CogniFitRepo.Server.Controllers
{
    [ApiController]
    public class ExerciseInstance_ExercisePropertyController : ControllerBase
    {
        private readonly ILogger<BiometricInformationRepository> _logger;
        private readonly IExerciseInstance_ExercisePropertyRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExerciseInstance_ExercisePropertyController(
            ILogger<BiometricInformationRepository> logger,
            IExerciseInstance_ExercisePropertyRepository repository,
            UserManager<ApplicationUser> userManager
        )
        {
            _logger = logger;
            _repository = repository;
            _userManager = userManager;
        }


        // Get single object by both IDs.
        [HttpGet]
        [Route("api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId}/exercise-property-id/{exercisePropertyId}")]
        public async Task<ActionResult<ExerciseInstance_ExercisePropertyDto>> GetByBothIds(long exerciseInstanceId, int exercisePropertyId)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the GET api/exercise-instance-exercise-property/exercise-instance-id/" +
                    exerciseInstanceId +
                    "/exercise-property-id/" + 
                    exercisePropertyId +
                    " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                var retval = await Task.Run(() => _repository.GetByBothIds(exerciseInstanceId, exercisePropertyId));

                _logger.LogInformation("The user with an ID of {currentUserId} retrieved the joining table entry connecting the exercise instance with an ID of {exerciseInstanceId} to the exercise property with an ID of {exercisePropertyId} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    exerciseInstanceId,
                    exercisePropertyId,
                    DateTimeOffset.UtcNow);

                return retval;
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId}/exercise-property-id/{exercisePropertyId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    exercisePropertyId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId}/exercise-property-id/{exercisePropertyId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    exercisePropertyId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId}/exercise-property-id/{exercisePropertyId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    exercisePropertyId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId}/exercise-property-id/{exercisePropertyId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    exercisePropertyId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // Get all join table entries corresponding to one exercise instance
        [HttpGet]
        [Route("api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId}")]
        public async Task<ActionResult<IEnumerable<ExerciseInstance_ExercisePropertyDto>>> GetByExerciseInstanceId(long exerciseInstanceId)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the GET api/exercise-instance-exercise-property/exercise-instance-id/" +
                    exerciseInstanceId +
                    " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                var retval = Ok(await Task.Run(() => _repository.GetByExerciseInstanceId(exerciseInstanceId)));

                _logger.LogInformation("The user with an ID of {currentUserId} retrieved all joining table entries connected to the exercise instance with an ID of {exerciseInstanceId} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    exerciseInstanceId,
                    DateTimeOffset.UtcNow);

                return retval;
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the GET api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // Add a new property to an exercise instance
        // ExerciseInstanceId, ExercisePropertyId, and Amount are all needed
        // The Name stored in the DTO is ignored.
        [HttpPost]
        [Route("api/exercise-instance-exercise-property")]
        public async Task<IActionResult> PostExerciseInstance_ExerciseProperty([FromBody] ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the POST api/exercise-instance-exercise-property endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                await Task.Run(() => _repository.Post(exerciseInstance_ExercisePropertyDto, currentUserId));

                _logger.LogInformation("The user with an ID of {currentUserId} posted a new joining table entry connecting an exercise instance and an exercise property at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    DateTimeOffset.UtcNow);

                return NoContent();

                // return type????
                //return CreatedAtAction("GetExerciseInstance", new { id = exerciseInstance.Id }, exerciseInstance);
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/exercise-instance-exercise-property endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/exercise-instance-exercise-property endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/exercise-instance-exercise-property endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the POST api/exercise-instance-exercise-property endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // Change the amount associated with a property that an ExerciseInstance has
        // Because the join table entries are identified only with their foreign keys,
        // this will not let you change which property or exercise instance
        // a join table entry points to
        // The Name in exerciseInstance_ExercisePropertyDto will be ignored
        [HttpPut]
        [Route("api/exercise-instance-exercise-property")]
        public async Task<IActionResult> UpdatePropertyAmount([FromBody] ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the PUT api/exercise-instance-exercise-property endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                await Task.Run(() => _repository.UpdatePropertyAmount(exerciseInstance_ExercisePropertyDto, currentUserId));

                _logger.LogInformation("The user with an ID of {currentUserId} changed the amount associated with a joining table entry connecting an exercise instance and an exercise property at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    DateTimeOffset.UtcNow);

                return NoContent();
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance-exercise-property endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance-exercise-property endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance-exercise-property endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the PUT api/exercise-instance-exercise-property endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // Delete join table entry with the specified pair of foreign keys
        [HttpDelete]
        [Route("api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId}/exercise-property-id/{exercisePropertyId}")]
        public async Task<IActionResult> Delete(long exerciseInstanceId, int exercisePropertyId)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                int code = 403;
                string message = "Cannot access the DELETE api/exercise-instance-exercise-property/exercise-instance-id/" +
                    exerciseInstanceId +
                    "/exercise-property-id/" +
                    exercisePropertyId +
                    " endpoint without being logged in.";
                _logger.LogError("{code} Error: \"{message}\" at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    message,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, message);
            }

            try
            {
                await Task.Run(() => _repository.Delete(exerciseInstanceId, exercisePropertyId, currentUserId));

                _logger.LogInformation("The user with an ID of {currentUserId} deleted the joining table entry connecting the exercise instance with an ID of {exerciseInstanceId} to the exercise property with an ID of {exercisePropertyId} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    currentUserId,
                    exerciseInstanceId,
                    exercisePropertyId,
                    DateTimeOffset.UtcNow);

                return NoContent();
            }
            catch (MissingMemberException e)
            {
                int code = 500;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId}/exercise-property-id/{exercisePropertyId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    exercisePropertyId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (NotSupportedException e)
            {
                int code = 403;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId}/exercise-property-id/{exercisePropertyId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    exercisePropertyId,
                    DateTimeOffset.UtcNow);
                return StatusCode(code, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                int code = 404;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId}/exercise-property-id/{exercisePropertyId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    exercisePropertyId,
                    DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                int code = 400;
                _logger.LogError("{code} Error: \"{e.Message}\" encountered by the user with an ID of {currentUserId} when accessing the DELETE api/exercise-instance-exercise-property/exercise-instance-id/{exerciseInstanceId}/exercise-property-id/{exercisePropertyId} endpoint at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).",
                    code,
                    e.Message,
                    currentUserId,
                    exerciseInstanceId,
                    exercisePropertyId,
                    DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }
    }
}
