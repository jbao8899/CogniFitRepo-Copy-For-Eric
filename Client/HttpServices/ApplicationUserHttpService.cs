using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;
using System.Net.Http.Json;

namespace CogniFitRepo.Client.HttpServices
{
    public class ApplicationUserHttpService : IApplicationUserHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "api/application-user";

        public ApplicationUserHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataResponse<List<ApplicationUserDto>>> GetApplicationUsers()
        {
            try
            {
                List<ApplicationUserDto> applicationUsers = await _httpClient.GetFromJsonAsync<List<ApplicationUserDto>>(_baseUrl);
                return new DataResponse<List<ApplicationUserDto>>()
                {
                    Data = applicationUsers,
                    Message = "Successfully retrieved applicationUsers.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<List<ApplicationUserDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new List<ApplicationUserDto>(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<List<ApplicationUserDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new List<ApplicationUserDto>(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ApplicationUserDto>> GetApplicationUser(string id)
        {
            try
            {
                ApplicationUserDto applicationUser = await _httpClient.GetFromJsonAsync<ApplicationUserDto>($"{_baseUrl}/{id}");
                return new DataResponse<ApplicationUserDto>()
                {
                    Data = applicationUser,
                    Message = "Successfully retrieved applicationUser.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ApplicationUserDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new ApplicationUserDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ApplicationUserDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new ApplicationUserDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ApplicationUserDto>> GetCurrentApplicationUser()
        {
            try
            {
                ApplicationUserDto applicationUser = await _httpClient.GetFromJsonAsync<ApplicationUserDto>($"{_baseUrl}/self");
                return new DataResponse<ApplicationUserDto>()
                {
                    Data = applicationUser,
                    Message = "Successfully retrieved applicationUser.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ApplicationUserDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new ApplicationUserDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ApplicationUserDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new ApplicationUserDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ApplicationUserDto>> CreateApplicationUser(ApplicationUserDto applicationUserDto)
        {
            try
            {
                ApplicationUserDto applicationUser = await _httpClient.PostAsJsonAsync<ApplicationUserDto>(_baseUrl, applicationUserDto).Result.Content.ReadFromJsonAsync<ApplicationUserDto>();
                return new DataResponse<ApplicationUserDto>()
                {
                    Data = applicationUser,
                    Message = "Successfully created applicationUser.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ApplicationUserDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = applicationUserDto,
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ApplicationUserDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = applicationUserDto,
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ApplicationUserDto>> UpdateApplicationUser(string id, ApplicationUserDto applicationUserDto)
        {
            try
            {
                ApplicationUserDto applicationUser = await _httpClient.PutAsJsonAsync<ApplicationUserDto>(_baseUrl, applicationUserDto).Result.Content.ReadFromJsonAsync<ApplicationUserDto>();
                return new DataResponse<ApplicationUserDto>()
                {
                    Data = applicationUser,
                    Message = "Successfully updated applicationUser.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ApplicationUserDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = applicationUserDto,
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ApplicationUserDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = applicationUserDto,
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ApplicationUserDto>> DeleteApplicationUser(string id)
        {
            try
            {
                ApplicationUserDto applicationUser = await _httpClient.DeleteAsync($"{_baseUrl}/delete/{id}").Result.Content.ReadFromJsonAsync<ApplicationUserDto>();
                return new DataResponse<ApplicationUserDto>()
                {
                    Data = applicationUser,
                    Message = "Successfully deleted applicationUser.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ApplicationUserDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new ApplicationUserDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ApplicationUserDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new ApplicationUserDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }
    }
}
