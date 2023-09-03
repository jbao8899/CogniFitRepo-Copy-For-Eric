using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;
using System.Net.Http.Json;

namespace CogniFitRepo.Client.HttpServices
{
    public class WorkoutHttpService : IWorkoutHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "api/workout";

        public WorkoutHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataResponse<List<WorkoutDto>>> GetWorkouts()
        {
            try
            {
                List<WorkoutDto> workouts = await _httpClient.GetFromJsonAsync<List<WorkoutDto>>(_baseUrl);
                return new DataResponse<List<WorkoutDto>>()
                {
                    Data = workouts,
                    Message = "Successfully retrieved workouts.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<List<WorkoutDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new List<WorkoutDto>(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<List<WorkoutDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new List<WorkoutDto>(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<WorkoutDto>> GetWorkout(long? id)
        {
            try
            {
                WorkoutDto workout = await _httpClient.GetFromJsonAsync<WorkoutDto>($"{_baseUrl}/{id}");
                return new DataResponse<WorkoutDto>()
                {
                    Data = workout,
                    Message = "Successfully retrieved workout.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<WorkoutDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new WorkoutDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<WorkoutDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new WorkoutDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<WorkoutDto>> CreateWorkout(WorkoutDto workout)
        {
            try
            {
                WorkoutDto createdWorkout = await _httpClient.PostAsJsonAsync<WorkoutDto>($"{ _baseUrl}", workout).Result.Content.ReadFromJsonAsync<WorkoutDto>();
                return new DataResponse<WorkoutDto>()
                {
                    Data = createdWorkout,
                    Message = "Successfully created workout.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<WorkoutDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = workout,
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<WorkoutDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = workout,
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<List<WorkoutDto>>> CreateWorkoutsForWorkoutProgram(long? workoutProgramId, List<WorkoutDto> workouts)
        {
            try
            {
                List<WorkoutDto> createdWorkout = await _httpClient.PostAsJsonAsync<List<WorkoutDto>>($"{_baseUrl}/workout-program-id/{workoutProgramId}", workouts).Result.Content.ReadFromJsonAsync<List<WorkoutDto>>();
                return new DataResponse<List<WorkoutDto>>()
                {
                    Data = createdWorkout,
                    Message = "Successfully created workout.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<List<WorkoutDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = workouts,
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<List<WorkoutDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = workouts,
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<WorkoutDto>> UpdateWorkout(long? id, WorkoutDto workout)
        {
            try
            {
                WorkoutDto updatedWorkout = await _httpClient.PutAsJsonAsync<WorkoutDto>($"{_baseUrl}/{id}", workout).Result.Content.ReadFromJsonAsync<WorkoutDto>();
                return new DataResponse<WorkoutDto>()
                {
                    Data = updatedWorkout,
                    Message = "Successfully updated workout.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<WorkoutDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = workout,
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<WorkoutDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = workout,
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }

        public async Task<DataResponse<WorkoutDto>> DeleteWorkout(long? id)
        {
            try
            {
                WorkoutDto deletedWorkout = await _httpClient.DeleteAsync($"{_baseUrl}/{id}").Result.Content.ReadFromJsonAsync<WorkoutDto>();
                return new DataResponse<WorkoutDto>()
                {
                    Data = deletedWorkout,
                    Message = "Successfully deleted workout.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<WorkoutDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new WorkoutDto(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<WorkoutDto>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new WorkoutDto(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }
    }
}
