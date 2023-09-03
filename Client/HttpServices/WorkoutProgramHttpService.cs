using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;
using System.Net.Http.Json;

namespace CogniFitRepo.Client.HttpServices
{
    public class WorkoutProgramHttpService : IWorkoutProgramHttpService
    {
        public readonly HttpClient _httpClient;
        private readonly  string _baseUrl = "api/workout-program";

        public WorkoutProgramHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataResponse<List<WorkoutProgramDto>>> GetWorkoutPrograms()
        {
            try
            {
                List<WorkoutProgramDto> workoutPrograms = await _httpClient.GetFromJsonAsync<List<WorkoutProgramDto>>(_baseUrl);
                return new DataResponse<List<WorkoutProgramDto>>()
                {
                    Data = workoutPrograms,
                    Message = "Successfully retrieved workout programs.",
                    Succeeded = true
                };
            }
            catch(NotSupportedException)
            {
                return new DataResponse<List<WorkoutProgramDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new List<WorkoutProgramDto>(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<List<WorkoutProgramDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new List<WorkoutProgramDto>(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<List<WorkoutProgramDto>>> GetCurrentUsersWorkoutPrograms()
        {
            try
            {
                List<WorkoutProgramDto> workoutPrograms = await _httpClient.GetFromJsonAsync<List<WorkoutProgramDto>>($"{_baseUrl}/for-current-user");
                return new DataResponse<List<WorkoutProgramDto>>()
                {
                    Data = workoutPrograms,
                    Message = "Successfully retrieved workout programs.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<List<WorkoutProgramDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new List<WorkoutProgramDto>(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<List<WorkoutProgramDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new List<WorkoutProgramDto>(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<WorkoutProgramDto>> GetWorkoutProgram(long? id)
        {
            try
            {
                WorkoutProgramDto workoutProgram = await _httpClient.GetFromJsonAsync<WorkoutProgramDto>($"{_baseUrl}/{id}");
                return new DataResponse<WorkoutProgramDto>()
                {
                    Data = workoutProgram,
                    Message = "Successfully retrieved workout program.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<WorkoutProgramDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new WorkoutProgramDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<WorkoutProgramDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new WorkoutProgramDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<WorkoutProgramDto>> CreateWorkoutProgram(WorkoutProgramDto workoutProgram)
        {
            try
            {
                WorkoutProgramDto createdWorkoutProgram = await _httpClient.PostAsJsonAsync(_baseUrl, workoutProgram).Result.Content.ReadFromJsonAsync<WorkoutProgramDto>();
                return new DataResponse<WorkoutProgramDto>()
                {
                    Data = createdWorkoutProgram,
                    Message = "Successfully created workout program.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<WorkoutProgramDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new WorkoutProgramDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<WorkoutProgramDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new WorkoutProgramDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<WorkoutProgramDto>> UpdateWorkoutProgram(long? id, WorkoutProgramDto workoutProgram)
        {
            try
            {
                WorkoutProgramDto updatedWorkoutProgram = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", workoutProgram).Result.Content.ReadFromJsonAsync<WorkoutProgramDto>();
                return new DataResponse<WorkoutProgramDto>()
                {
                    Data = updatedWorkoutProgram,
                    Message = "Successfully updated workout program.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<WorkoutProgramDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new WorkoutProgramDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<WorkoutProgramDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new WorkoutProgramDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<WorkoutProgramDto>> DeleteWorkoutProgram(long? id)
        {
            try
            {
                WorkoutProgramDto deletedWorkoutProgram = await _httpClient.DeleteAsync($"{_baseUrl}/{id}").Result.Content.ReadFromJsonAsync<WorkoutProgramDto>();
                return new DataResponse<WorkoutProgramDto>()
                {
                    Data = deletedWorkoutProgram,
                    Message = "Successfully deleted workout program.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<WorkoutProgramDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new WorkoutProgramDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<WorkoutProgramDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new WorkoutProgramDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }
    }
}
