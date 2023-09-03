using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;
using System.Net.Http.Json;

namespace CogniFitRepo.Client.HttpServices
{
    public class ExerciseInstanceHttpService : IExerciseInstanceHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "api/exercise-instance";

        public ExerciseInstanceHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataResponse<List<ExerciseInstanceDto>>> GetExerciseInstances()
        {
            try
            {
                List<ExerciseInstanceDto> exerciseInstances = await _httpClient.GetFromJsonAsync<List<ExerciseInstanceDto>>(_baseUrl);
                return new DataResponse<List<ExerciseInstanceDto>>()
                {
                    Data = exerciseInstances,
                    Message = "Successfully retrieved exercise instances.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<List<ExerciseInstanceDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new List<ExerciseInstanceDto>(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<List<ExerciseInstanceDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new List<ExerciseInstanceDto>(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ExerciseInstanceDto>> GetExerciseInstance(long? id)
        {
            try
            {
                ExerciseInstanceDto exerciseInstance = await _httpClient.GetFromJsonAsync<ExerciseInstanceDto>($"{_baseUrl}/{id}");
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Data = exerciseInstance,
                    Message = "Successfully retrieved exercise instance.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new ExerciseInstanceDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new ExerciseInstanceDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ExerciseInstanceDto>> CreateExerciseInstance(ExerciseInstanceDto exerciseInstanceDto)
        {
            try
            {
                ExerciseInstanceDto createdExerciseInstance = await _httpClient.PostAsJsonAsync<ExerciseInstanceDto>(_baseUrl, exerciseInstanceDto).Result.Content.ReadFromJsonAsync<ExerciseInstanceDto>();
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Data = createdExerciseInstance,
                    Message = "Successfully created exercise instance.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new ExerciseInstanceDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new ExerciseInstanceDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }
        ///workout-id/{workoutId}"
        ///
        public async Task<DataResponse<ExerciseInstanceDto>> CreateExerciseInstanceForWorkout(long? id, ExerciseInstanceDto exerciseInstanceDto)
        {
            try
            {
                ExerciseInstanceDto createdExerciseInstance = await _httpClient.PostAsJsonAsync<ExerciseInstanceDto>($"{_baseUrl}/workout-id/{id}", exerciseInstanceDto).Result.Content.ReadFromJsonAsync<ExerciseInstanceDto>();
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Data = createdExerciseInstance,
                    Message = "Successfully created exercise instance.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new ExerciseInstanceDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new ExerciseInstanceDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ExerciseInstanceDto>> UpdateExerciseInstance(long? id, ExerciseInstanceDto exerciseInstanceDto)
        {
            try
            {
                ExerciseInstanceDto updatedExerciseInstance = await _httpClient.PutAsJsonAsync<ExerciseInstanceDto>($"{_baseUrl}/{id}", exerciseInstanceDto).Result.Content.ReadFromJsonAsync<ExerciseInstanceDto>();
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Data = updatedExerciseInstance,
                    Message = "Successfully updated exercise instance.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = exerciseInstanceDto,
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = exerciseInstanceDto,
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ExerciseInstanceDto>> ToggleExerciseInstanceCompleted(long? id)
        {
            try
            {
                ExerciseInstanceDto updatedExerciseInstance = await _httpClient.PutAsJsonAsync<ExerciseInstanceDto>($"{_baseUrl}/toggle-completion-for-id/{id}", null).Result.Content.ReadFromJsonAsync<ExerciseInstanceDto>();
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Data = updatedExerciseInstance,
                    Message = "Successfully updated exercise instance.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new ExerciseInstanceDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new ExerciseInstanceDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }   

        public async Task<DataResponse<ExerciseInstanceDto>> DeleteExerciseInstance(long? id)
        {
            try
            {
                ExerciseInstanceDto deletedExerciseInstance = await _httpClient.DeleteAsync($"{_baseUrl}/{id}").Result.Content.ReadFromJsonAsync<ExerciseInstanceDto>();
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Data = deletedExerciseInstance,
                    Message = "Successfully deleted exercise instance.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new ExerciseInstanceDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ExerciseInstanceDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new ExerciseInstanceDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }
    }
}
