using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;
using System.Net.Http.Json;

namespace CogniFitRepo.Client.HttpServices
{
    public class ExercisePropertyHttpService : IExercisePropertyHttpService
    {
        private readonly HttpClient _httpClient;

        public ExercisePropertyHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataResponse<List<ExercisePropertyDto>>> GetExerciseProperties()
        {
            try
            {
                List<ExercisePropertyDto> exerciseProperties = await _httpClient.GetFromJsonAsync<List<ExercisePropertyDto>>("api/exercise-property");
                return new DataResponse<List<ExercisePropertyDto>>()
                {
                    Data = exerciseProperties,
                    Message = "Successfully retrieved exercise properties.",
                    Succeeded = true
                };
            }
            catch (NotSupportedException)
            {
                return new DataResponse<List<ExercisePropertyDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { "The content type is not supported." } }
                    },
                    Data = new List<ExercisePropertyDto>(),
                    Message = "The content type is not supported.",
                    Succeeded = false
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<List<ExercisePropertyDto>>()
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "Error", new string[] { ex.Message } }
                    },
                    Data = new List<ExercisePropertyDto>(),
                    Message = ex.Message,
                    Succeeded = false
                };
            }
        }
    }
}
