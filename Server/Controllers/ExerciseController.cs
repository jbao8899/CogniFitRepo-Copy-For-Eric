using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CogniFitRepo.Server.Models.ExerciseModels;
using CogniFitRepo.Server.Repositories;

namespace CogniFitRepo.Server.Controllers
{
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseService;
       // private readonly ILogger<ExerciseController> _logger;
       // private readonly UserManager<Exercise> _userManager;

        public ExerciseController(IExerciseRepository service)
        {
            _exerciseService = service;
         //   _logger = logger;
        }

        [HttpGet]
        [Route("api/exercise/")]
        public async Task<ActionResult<List<ExerciseDto>>> GetExercises()
        {
            var retVal = await Task.Run(() => _exerciseService.GetExercises());
            //_logger.LogInformation(User.Identity.Name)
            return Ok(retVal);
            
        }

        [HttpGet]
        [Route("api/exercise/id/{id:int}")]
        public async Task<ActionResult<ExerciseDto>> GetExerciseById(int id)
        {
            if (id >= 0)
            {
                ExerciseDto exercise;
                exercise = await Task.Run(() => _exerciseService.GetExerciseById(id)!);
                if (exercise != null) { 
                    return await Task.FromResult(Ok(exercise));
                }
                return NotFound(new { Message = $"No exercise with an ID of {id} was found." });
            }
            else
            {
                return BadRequest(new { Message = $"Exercise id must be positive" });
            }
        }

        [HttpGet]
        [Route("api/exercise/primaryMuscleName/{primaryMuscleName}")]
        public async Task<IActionResult> GetExercisesByPrimaryMuscle(string primaryMuscleName)
        {
            var retVal = await Task.Run(() => _exerciseService.GetExerciseByPrimaryMuscle(primaryMuscleName));

            return await Task.FromResult(Ok(retVal));
        }

        [HttpGet]
        [Route("api/exercise/muscle/{muscleName}")]
        public async Task<IActionResult> GetExercisesByAnyMuscle(string muscleName)
        {
            var retVal = await Task.Run(() => _exerciseService.GetExercisesByAnyMuscle(muscleName));

            return await Task.FromResult(Ok(retVal));
        }

        [HttpGet]
        [Route("api/exercise/equipmentName/{equipmentName}")]
        public async Task<IActionResult> GetExercisesByEquipment(string equipmentName)
        {
            var retVal = await Task.Run(() => _exerciseService.GetExercisesByEquipment(equipmentName));

            return await Task.FromResult(Ok(retVal));
        }

        [HttpGet]
        [Route("api/exercise/levelName/{levelName}")]
        public async Task<IActionResult> GetExercisesByLevel(string levelName)
        {
            var retVal = await Task.Run(() => _exerciseService.GetExercisesByLevel(levelName));

            return await Task.FromResult(Ok(retVal));
        }

        [HttpGet]
        [Route("api/exercise/categoryName/{categoryName}")]
        public async Task<IActionResult> GetExercisesByCategory(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return BadRequest(new { Message = "Category must be provided" });
            }

            var retVal = await Task.Run(() => _exerciseService.GetExercisesByCategory(categoryName));

            return await Task.FromResult(Ok(retVal));
        }

        [HttpGet]
        [Route("api/exercise/forceName/{forceName}")]
        public async Task<IActionResult> GetExercisesByForce(string forceName)
        {
            var retVal = await Task.Run(() => _exerciseService.GetExercisesByForce(forceName));

            return await Task.FromResult(Ok(retVal));
        }

        [HttpGet]
        [Route("api/exercise/mechanicName/{mechanicName}")]
        public async Task<IActionResult> GetExercisesByMechanic(string mechanicName)
        {
            var retVal = await Task.Run(() => _exerciseService.GetExercisesByMechanic(mechanicName));

            return await Task.FromResult(Ok(retVal));
        }

        [HttpGet]
        [Route("api/exercise/get-images/{id}")]
        public async Task<IActionResult> GetExerciseImages(int id)
        {
            var retVal = await Task.Run(() => _exerciseService.GetExerciseImages(id));

            return await Task.FromResult(Ok(retVal));
        }

        [HttpGet]
        [Route("api/exercise/search")]
        public async Task<IActionResult> GetExercisesByMultipleAttributes(
            [FromQuery] string? primaryMuscle,
            [FromQuery] string? anyMuscle,
            [FromQuery] string? equipment,
            [FromQuery] string? force,
            [FromQuery] string? level,
            [FromQuery] string? category,
            [FromQuery] string? mechanic)
        { 
            var retVal = await Task.Run(() => _exerciseService.GetExercisesByMultipleAttributes(primaryMuscle, anyMuscle,
              equipment, force, level, category, mechanic));

            return await Task.FromResult(Ok(retVal));
        }

    }
}
