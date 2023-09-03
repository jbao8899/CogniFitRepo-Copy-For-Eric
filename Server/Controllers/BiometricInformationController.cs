using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Server.Repositories;
using CogniFitRepo.Shared.DataTransferObjects;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CogniFitRepo.Server.Controllers
{
    [ApiController]
    public class BiometricInformationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BiometricInformationController> _logger;
        private readonly IBiometricInformationRepository _repository;

        public BiometricInformationController(
            UserManager<ApplicationUser> userManager, 
            ILogger<BiometricInformationController> logger,
            IBiometricInformationRepository repository)
        {
            _userManager = userManager;
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Route("api/biometric-information")]
        public async Task<ActionResult<IEnumerable<BiometricInformationSnapshotDto>>> GetBiometricInformation()
        {
            try
            {
                return Ok(await Task.Run(() => _repository.GetBiometricInformation()));
            }
            catch (MissingMemberException e)
            {
                int errorCode = 500;
                _logger.LogError("{errorCode} Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformation() at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", errorCode, e.Message, DateTimeOffset.UtcNow);
                return StatusCode(errorCode, e.Message);
            }
            catch (NotSupportedException e)
            {
                int errorCode = 403;
                _logger.LogError("{errorCode} Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformation() at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", errorCode, e.Message, DateTimeOffset.UtcNow);
                return StatusCode(errorCode, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError("404 Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformation() at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", e.Message, DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("400 Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformation() at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", e.Message, DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("api/biometric-information/{id}")]
        public async Task<ActionResult<BiometricInformationSnapshotDto>> GetBiometricInformationByID(long id)
        {
            try
            {
                return Ok(await Task.Run(() => _repository.GetBiometricInformationByID(id)));
            }
            catch (MissingMemberException e)
            {
                int errorCode = 500;
                _logger.LogError("{errorCode} Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformation() at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", errorCode, e.Message, DateTimeOffset.UtcNow);
                return StatusCode(errorCode, e.Message);
            }
            catch (NotSupportedException e)
            {
                int errorCode = 403;
                _logger.LogError("{errorCode} Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformation() at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", errorCode, e.Message, DateTimeOffset.UtcNow);
                return StatusCode(errorCode, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError("404 Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformation() at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", e.Message, DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("400 Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformation() at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", e.Message, DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Will not change what user the biometric information snapshot describes
        // If no date is provided in the input DTO, the date will not be updated
        [HttpPut]
        [Route("api/biometric-information/{id}")]
        public async Task<IActionResult> PutBiometricInformation(long id, [FromBody] BiometricInformationSnapshotDto biometricInformationSnapshotDto)
        {
            if (id != biometricInformationSnapshotDto.Id)
            {
                _logger.LogWarning($"Given ID do not match of desired ID.");
                return BadRequest();
            }

            if (biometricInformationSnapshotDto is null)
            {
                return Problem("Entity set 'ApplicationDbContext.BiometricInformation' is null.");
            }
            try {
                await Task.Run(() => _repository.PutBiometricInformation(id, biometricInformationSnapshotDto));
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            


        }

        // POST: api/BiometricInformations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("api/biometric-information")]
        public async Task<ActionResult<BiometricInformationSnapshot>> PostBiometricInformation([FromBody] BiometricInformationSnapshotDto biometricInformationSnapshotDto)
        {
            if (biometricInformationSnapshotDto == null)
            {
                return Problem("Entity set 'BiometricInformation' is null.");
            }

            BiometricInformationSnapshot biometricInformationSnapshot = new BiometricInformationSnapshot(biometricInformationSnapshotDto);

            await Task.Run (() => _repository.PostBiometricInformation(biometricInformationSnapshot));
            return CreatedAtAction("GetBiometricInformation", new { id = biometricInformationSnapshot.Id }, biometricInformationSnapshot);
        }

        // DELETE: api/BiometricInformations/5
        [HttpDelete]
        [Route("api/biometric-information/{id}")]
        public async Task<IActionResult> DeleteBiometricInformation(long id)
        {
            try
            {
                await Task.Run(() => _repository.DeleteBiometricInformation(id));
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Our endpoints

        // gets all biometric information readings belonging to some user
        [HttpGet]
        [Route("api/biometric-information/for-current-user")]
        public async Task<ActionResult<IEnumerable<BiometricInformationSnapshotDto>>> GetBiometricInformationForCurrentUser()
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId is null)
            {
                string errorMessage = "The current user could not be found.";
                _logger.LogError("400 Error ({errorMessage}) at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", errorMessage, DateTimeOffset.UtcNow);
                return BadRequest("The current user could not be found.");
            }

            try
            {
                return Ok(await Task.Run(() => _repository.GetBiometricInformationForUser(currentUserId)));
            }
            catch (MissingMemberException e)
            {
                int errorCode = 500;
                _logger.LogError("{errorCode} Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformationForUser() for the user with an ID of {currentUserId} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", errorCode, e.Message, currentUserId, DateTimeOffset.UtcNow);
                return StatusCode(errorCode, e.Message);
            }
            catch (NotSupportedException e)
            {
                int errorCode = 403;
                _logger.LogError("{errorCode} Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformationForUser() for the user with an ID of {currentUserId} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", errorCode, e.Message, currentUserId, DateTimeOffset.UtcNow);
                return StatusCode(errorCode, e.Message);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError("404 Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformationForUser() for the user with an ID of {currentUserId} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", e.Message, currentUserId, DateTimeOffset.UtcNow);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("400 Error ({errorMessage}) when calling BiometricInformationController.GetBiometricInformationForUser() for the user with an ID of {currentUserId} at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", e.Message, currentUserId, DateTimeOffset.UtcNow);
                return BadRequest(e.Message);
            }
        }
    }

}

