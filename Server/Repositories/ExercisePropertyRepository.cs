using CogniFitRepo.Server.Controllers;
using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CogniFitRepo.Server.Repositories
{
    public class ExercisePropertyRepository : IExercisePropertyRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExercisePropertyController> _logger;
        public ExercisePropertyRepository(ApplicationDbContext context,
               ILogger<ExercisePropertyController> logger) 
        { 
            _context = context;
            _logger = logger;
        }

        public  List<ExercisePropertyDto> GetExerciseProperties()
        {
            var list = new List<ExercisePropertyDto>();
            if (_context.ExerciseProperties is not null)
            {
                list = _context.ExerciseProperties.Select(ep => ep.ToDto()).ToList();
                _logger.LogInformation("Exercise Property Successfully made a List. Logged: {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                return list;
            }
            else
            {
                _logger.LogInformation("Exercise Property Failed to make a List. Logged: {Placeholder:MMMM dd, yyyy}", DateTimeOffset.UtcNow);
                return list;
            }
        }
    }
}
