using CogniFitRepo.Server.Controllers;
using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Shared.DataTransferObjects;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CogniFitRepo.Client.Pages.Profile;

namespace CogniFitRepo.Server.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<WorkoutController> _logger;

        public ApplicationUserRepository(
            ApplicationDbContext context,
                                 UserManager<ApplicationUser> userManager,
                                 ILogger<WorkoutController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IEnumerable<ApplicationUserDto> GetApplicationUsers()
        {
            if (_context.Users is null)
            {
                _logger.LogError("GetApplicationUsers() _context.Users is null at {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
            var list = _context.Users.ToList();
            var DtoList = new List<ApplicationUserDto>();
            foreach (var item in list)
            {
                ApplicationUserDto toAdd = ToAppUserDto(item);
                toAdd.PortraitPath = (
                    from Portrait portrait in _context.Portraits
                    where portrait.Id == item.PortraitId
                    select portrait.Path
                ).FirstOrDefault();

                DtoList.Add(toAdd);
            }
            return DtoList;
        }

        public ApplicationUserDto GetApplicationUserById(string id)
        {
            // Get all users
            if (_context.Users is null || id.IsNullOrEmpty())
            {
                _logger.LogError("GetApplicationUsersById() _context.Users is null or the id is null or empty at {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
            var applicationUser =  _context.Users.FirstOrDefault(us => us.Id == id);
            if (applicationUser is null)
            {
                _logger.LogError("GetApplicationUsersById() applicationUser is null at {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
            ApplicationUserDto toGet = ToAppUserDto(applicationUser);
            toGet.PortraitPath = (
                from Portrait portrait in _context.Portraits
                where portrait.Id == applicationUser.PortraitId
                select portrait.Path
            ).FirstOrDefault();

            return toGet;
        }

        public void PutApplicationUser([FromBody]ApplicationUserDto applicationUserDto, ApplicationUser toUpdate)
        {

            if (_context.Users is null ||
                _context.Portraits is null)
            {
                _logger.LogError("PutApplicationUser() _context.Users or _context.Portraits is null at {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException("_context.Users or _context.Portraits are null at PutApplicationUser in ApplicationUserRepository is Null");
            }

            if (toUpdate is null)
            {
                _logger.LogError("PutApplicationUser() toUpdate is null at {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException("toUpdate at PutApplicationUser in ApplicationUserRepository is Null");
            }

            if (!(_context.Portraits.Any(p => p.Id == applicationUserDto.PortraitId)))
            {
                _logger.LogError("PutApplicationUser() There is no portrait with an ID of {}  at {Placeholder:MMMM dd, yyyy}", applicationUserDto.PortraitId ,DateTimeOffset.UtcNow);
                throw new ArgumentException("There is no portrait with an ID of from applicationUserDto.PortraitId");
            }

            if (applicationUserDto.Birthday is not null &&
                applicationUserDto.Birthday > DateTime.Today)
            {
                _logger.LogError("PutApplicationUser() applicationUserDto.Birthday is greater then the current date at {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new ArgumentException("You cannot have been born in the future");
            }

            toUpdate.FirstName = applicationUserDto.FirstName;
            toUpdate.LastName = applicationUserDto.LastName;
            toUpdate.IsFemale = applicationUserDto.IsFemale;
            toUpdate.Birthday = applicationUserDto.Birthday;
            toUpdate.PrefersMetric = applicationUserDto.PrefersMetric;
            toUpdate.ProfileDescription = applicationUserDto.ProfileDescription;
            toUpdate.NumExperiencePoints = applicationUserDto.NumExperiencePoints;
            toUpdate.StreetNumber = applicationUserDto.StreetNumber;
            toUpdate.StreetName = applicationUserDto.StreetName;
            toUpdate.ApartmentNumber = applicationUserDto.ApartmentNumber;
            toUpdate.CityName = applicationUserDto.CityName;
            toUpdate.SubdivisionName = applicationUserDto.SubdivisionName;
            toUpdate.CountryName = applicationUserDto.CountryName;
            toUpdate.PostalCode = applicationUserDto.PostalCode;
            toUpdate.PortraitId = applicationUserDto.PortraitId;
            _context.SaveChanges();
        }

        public ApplicationUserDto GetOwnProfile(ApplicationUser profile)
        {
            if(profile is null) {
                _logger.LogError("GetOwnProfile() profile is null at {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException("GetOwnProfile profile input is null");
            }

            ApplicationUserDto toGet = ToAppUserDto(profile);
            toGet.PortraitPath = (
                from Portrait portrait in _context.Portraits
                where portrait.Id == profile.PortraitId
                select portrait.Path
            ).FirstOrDefault();

            return toGet;
        }

        public void Delete(ApplicationUser self)
        {
            if (_context.Users is null || self is null)
            {
                _logger.LogError("DeleteSelf() in ApplicationUserRepo either _context.Users or self is null at {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException("DeleteSelf() in ApplicationUserRepo either _context.Users or self is null.");
            }
            _context.Users.Remove(self);
            _context.SaveChanges();
        }




        // must set Portrait path separately
        private static ApplicationUserDto ToAppUserDto(ApplicationUser user)
        {
            return new ApplicationUserDto()
            {
                //Login Properties
                Id = user.Id,
                UserName = user.UserName,
                //User Properties
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsFemale = user.IsFemale,
                Birthday = user.Birthday,
                PrefersMetric = user.PrefersMetric,
                ProfileDescription = user.ProfileDescription,
                NumExperiencePoints = user.NumExperiencePoints,
                StreetNumber = user.StreetNumber,
                StreetName = user.StreetName,
                ApartmentNumber = user.ApartmentNumber,
                CityName = user.CityName,
                SubdivisionName = user.SubdivisionName,
                CountryName = user.CountryName,
                PostalCode = user.PostalCode,
                PortraitId = user.PortraitId,
                PortraitPath = user.Portrait?.Path
            };
        }
    }
}
