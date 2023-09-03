using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CogniFitRepo.Server.Repositories
{
    public interface IApplicationUserRepository
    {
        public IEnumerable<ApplicationUserDto> GetApplicationUsers();
        public ApplicationUserDto GetApplicationUserById(string id);
        public void PutApplicationUser(ApplicationUserDto applicationUserDto, ApplicationUser toUpdate);
        public ApplicationUserDto GetOwnProfile(ApplicationUser profile);
        public void Delete(ApplicationUser self);
    }
}
