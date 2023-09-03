using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Server.Repositories;
using CogniFitRepo.Shared.DataTransferObjects;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static CogniFitRepo.Client.Pages.Profile;

namespace CogniFitRepo.Server.Controllers
{
    [ApiController]
    public class ApplicationUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IApplicationUserRepository _repository;
        public ApplicationUserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
                                         IApplicationUserRepository repository)
        {
            _userManager = userManager;
            _context = context;
            _repository = repository;
        }

        //Need to test, should be fin.
        [HttpGet]
        [Route("api/application-user")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetApplicationUsers()
        {
            try
            {
                var DtoList = await Task.Run(() => _repository.GetApplicationUsers());
                return Ok(DtoList);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //Need to test but probably fin.
        [HttpGet]
        [Route("api/application-user/{id}")]
        public async Task<ActionResult<ApplicationUser>> GetApplicationUserById(string id)
        {
            try
            {
                // Get all users
                if (_context.Users is null || id.IsNullOrEmpty())
                {
                    return BadRequest("Users are either Null or String id was Null or Emptry");
                }
                var applicationUser = await _context.Users.FirstOrDefaultAsync(us => us.Id == id);
                if (applicationUser is not null)
                {
                    var toGet = await Task.Run(() => _repository.GetApplicationUserById(applicationUser.Id));
                    return Ok(toGet);
                }
                else
                {
                    return NotFound();
                }
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Need to test but probably fin.
        [HttpPut]
        [Route("api/application-user")]
        public async Task<IActionResult> PutApplicationUser([FromBody] ApplicationUserDto applicationUserDto)
        {
            try
            {
                ApplicationUser? toUpdate = await _userManager.GetUserAsync(User);
                await Task.Run(() => _repository.PutApplicationUser(applicationUserDto, toUpdate));
                return NoContent();
            } catch(Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        //Self-Get - Fin. Check if _Context.Users is null necessary?
        [HttpGet]
        [Route("api/application-user/self")]
        public async Task<ActionResult<ApplicationUser>> GetOwnProfile()
        {
            if (_context.Users is null)
            {
                return NotFound();
            }
            try {
                if (User.IsAuthenticated())
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user is not null)
                    {
                        var toGet = await Task.Run(() => _repository.GetOwnProfile(user));

                        return Ok(toGet);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest("User not Authenticated");
                }
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //need client to call Http.DeleteAsync() for this to work properly, otherwise you get a 404.
        [HttpDelete]
        [Route("api/application-user/delete")]
        public async Task<ActionResult> DeleteSelf()
        {
            try
            {
                if (_context.Users is null)
                {
                    return NotFound();
                }
                if (User.IsAuthenticated())
                {
                    var user = await _context.Users.FindAsync(User);
                    if (user is not null)
                    {
                        await Task.Run(() => _repository.Delete(user));
                        return Ok();
                    }
                    else
                    {
                        return BadRequest($"Could not find the User");
                    }
                }
                else
                {
                    return BadRequest($"User is not authenticated, could not deactivate user.");
                }
            } catch(Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
        //need client to call Http.DeleteAsync() for this to work properly, otherwise you get a 404.
        [HttpDelete]
        [Route("api/application-user/delete/{UserId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                if (_context.Users is null)
                {
                    return NotFound();
                }
                if (User.IsAuthenticated())
                {
                    var user = await _context.Users.FirstOrDefaultAsync(us => us.Id == userId);
                    if (user is not null)
                    {
                        await Task.Run(() => _repository.Delete(user));

                        return Ok();

                    }
                    else
                    {
                        return BadRequest($"Could not find the User");
                    }
                }
                else
                {
                    return BadRequest($"User is not authenticated, could not deactivate user.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
