using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;
using System.Net.Http.Json;

namespace CogniFitRepo.Client.HttpServices
{
    public class ExerciseHttpService : IExerciseHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "api/exercise";

        public ExerciseHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataResponse<List<ExerciseDto>>> GetExercises()
        {
            try
            {
                List<ExerciseDto> exercises = await _httpClient.GetFromJsonAsync<List<ExerciseDto>>(_baseUrl);
                return new DataResponse<List<ExerciseDto>>()
                {
                    Data = exercises,
                    Message = "Successfully retrieved exercises.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<List<ExerciseDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new List<ExerciseDto>(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<List<ExerciseDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new List<ExerciseDto>(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ExerciseDto>> GetExercise(int id)
        {
            try
            {
                ExerciseDto exercise = await _httpClient.GetFromJsonAsync<ExerciseDto>($"{_baseUrl}/{id}");
                return new DataResponse<ExerciseDto>()
                {
                    Data = exercise,
                    Message = "Successfully retrieved exercise.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ExerciseDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new ExerciseDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ExerciseDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new ExerciseDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ExerciseDto>> CreateExercise(ExerciseDto exercise)
        {
            try
            {
                ExerciseDto createdExercise = await _httpClient.PostAsJsonAsync<ExerciseDto>(_baseUrl, exercise).Result.Content.ReadFromJsonAsync<ExerciseDto>();
                return new DataResponse<ExerciseDto>()
                {
                    Data = createdExercise,
                    Message = "Successfully created exercise.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ExerciseDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = exercise,
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ExerciseDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = exercise,
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ExerciseDto>> UpdateExercise(int id, ExerciseDto exercise)
        {
            try
            {
                ExerciseDto updatedExercise = await _httpClient.PutAsJsonAsync<ExerciseDto>($"{_baseUrl}/{id}", exercise).Result.Content.ReadFromJsonAsync<ExerciseDto>();
                return new DataResponse<ExerciseDto>()
                {
                    Data = updatedExercise,
                    Message = "Successfully updated exercise.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ExerciseDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = exercise,
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ExerciseDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = exercise,
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<ExerciseDto>> DeleteExercise(int id)
        {
            try
            {
                ExerciseDto deletedExercise = await _httpClient.DeleteAsync($"{_baseUrl}/{id}").Result.Content.ReadFromJsonAsync<ExerciseDto>();
                return new DataResponse<ExerciseDto>()
                {
                    Data = deletedExercise,
                    Message = "Successfully deleted exercise.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<ExerciseDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new ExerciseDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<ExerciseDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new ExerciseDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }
    }
}
