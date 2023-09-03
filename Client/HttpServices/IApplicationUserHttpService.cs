using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;

namespace CogniFitRepo.Client.HttpServices
{
    public interface IApplicationUserHttpService
    {
        Task<DataResponse<List<ApplicationUserDto>>> GetApplicationUsers();
        Task<DataResponse<ApplicationUserDto>> GetApplicationUser(string id);
        Task<DataResponse<ApplicationUserDto>> GetCurrentApplicationUser();
        Task<DataResponse<ApplicationUserDto>> CreateApplicationUser(ApplicationUserDto applicationUser);
        Task<DataResponse<ApplicationUserDto>> UpdateApplicationUser(string id, ApplicationUserDto applicationUser);
        Task<DataResponse<ApplicationUserDto>> DeleteApplicationUser(string id);
    }
}
