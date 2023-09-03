using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Repositories;
using CogniFitRepo.Shared.DataTransferObjects;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CogniFitRepo.Server.Controllers
{
    [ApiController]
    public class ExercisePropertyController : ControllerBase
    {
        private readonly IExercisePropertyRepository _repository;

        public ExercisePropertyController(IExercisePropertyRepository service)
        {
            _repository = service;
        }

        [HttpGet]
        [Route("api/exercise-property")]
        public async Task<ActionResult<List<ExercisePropertyDto>>> GetExerciseProperties()
        {
            var retVal = await Task.Run(() => _repository.GetExerciseProperties());
            if (!retVal.IsNullOrEmpty())
            {
                return Ok(retVal);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
