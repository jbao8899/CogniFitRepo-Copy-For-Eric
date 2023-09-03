using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;
using System.Net.Http.Json;

namespace CogniFitRepo.Client.HttpServices
{
    public class BiometricInformationHttpService : IBiometricInformationHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "api/biometric-information";

        public BiometricInformationHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataResponse<List<BiometricInformationSnapshotDto>>> GetBiometricInformations()
        {
            try
            {
                List<BiometricInformationSnapshotDto> biometricInformations = await _httpClient.GetFromJsonAsync<List<BiometricInformationSnapshotDto>>(_baseUrl);
                return new DataResponse<List<BiometricInformationSnapshotDto>>()
                {
                    Data = biometricInformations,
                    Message = "Successfully retrieved biometric informations.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<List<BiometricInformationSnapshotDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new List<BiometricInformationSnapshotDto>(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<List<BiometricInformationSnapshotDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new List<BiometricInformationSnapshotDto>(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<List<BiometricInformationSnapshotDto>>> GetCurrentUserBiometricInformations()
        {
            try
            {
                List<BiometricInformationSnapshotDto> biometricInformations = await _httpClient.GetFromJsonAsync<List<BiometricInformationSnapshotDto>>($"{_baseUrl}/for-current-user");
                return new DataResponse<List<BiometricInformationSnapshotDto>>()
                {
                    Data = biometricInformations,
                    Message = "Successfully retrieved current user biometric informations.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<List<BiometricInformationSnapshotDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new List<BiometricInformationSnapshotDto>(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<List<BiometricInformationSnapshotDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new List<BiometricInformationSnapshotDto>(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<BiometricInformationSnapshotDto>> GetBiometricInformation(long? id)
        {
            try
            {
                BiometricInformationSnapshotDto biometricInformation = await _httpClient.GetFromJsonAsync<BiometricInformationSnapshotDto>($"{_baseUrl}/{id}");
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Data = biometricInformation,
                    Message = "Successfully retrieved biometric information.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new BiometricInformationSnapshotDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new BiometricInformationSnapshotDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<BiometricInformationSnapshotDto>> CreateBiometricInformation(BiometricInformationSnapshotDto biometricInformation)
        {
            try
            {
                BiometricInformationSnapshotDto createdBiometricInformation = await _httpClient.PostAsJsonAsync<BiometricInformationSnapshotDto>(_baseUrl, biometricInformation).Result.Content.ReadFromJsonAsync<BiometricInformationSnapshotDto>();
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Data = createdBiometricInformation,
                    Message = "Successfully created biometric information.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = biometricInformation,
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = biometricInformation,
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<List<BiometricInformationSnapshotDto>>> CreateBiometricInformations(List<BiometricInformationSnapshotDto> biometricInformations)
        {
            try
            {
                List<BiometricInformationSnapshotDto> createdBiometricInformations = await _httpClient.PostAsJsonAsync<List<BiometricInformationSnapshotDto>>(_baseUrl, biometricInformations).Result.Content.ReadFromJsonAsync<List<BiometricInformationSnapshotDto>>();
                return new DataResponse<List<BiometricInformationSnapshotDto>>()
                {
                    Data = createdBiometricInformations,
                    Message = "Successfully created biometric informations.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<List<BiometricInformationSnapshotDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = biometricInformations,
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<List<BiometricInformationSnapshotDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = biometricInformations,
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<BiometricInformationSnapshotDto>> UpdateBiometricInformation(long? id, BiometricInformationSnapshotDto biometricInformation)
        {
            try
            {
                BiometricInformationSnapshotDto updatedBiometricInformation = await _httpClient.PutAsJsonAsync<BiometricInformationSnapshotDto>($"{_baseUrl}/{id}", biometricInformation).Result.Content.ReadFromJsonAsync<BiometricInformationSnapshotDto>();
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Data = updatedBiometricInformation,
                    Message = "Successfully updated biometric information.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = biometricInformation,
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = biometricInformation,
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<BiometricInformationSnapshotDto>> DeleteBiometricInformation(long? id)
        {
            try
            {
                BiometricInformationSnapshotDto deletedBiometricInformation = await _httpClient.DeleteAsync($"{_baseUrl}/{id}").Result.Content.ReadFromJsonAsync<BiometricInformationSnapshotDto>();
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Data = deletedBiometricInformation,
                    Message = "Successfully deleted biometric information.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new BiometricInformationSnapshotDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<BiometricInformationSnapshotDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new BiometricInformationSnapshotDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }
    }
}
