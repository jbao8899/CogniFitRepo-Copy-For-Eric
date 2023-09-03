using CogniFitRepo.Server.Controllers;
using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CogniFitRepo.Server.Repositories
{
    public class BiometricInformationRepository : IBiometricInformationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BiometricInformationRepository> _logger;
        public BiometricInformationRepository(ApplicationDbContext context, ILogger<BiometricInformationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<BiometricInformationSnapshotDto> GetBiometricInformation()
        {
            if (_context.BiometricInformationSnapshots is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            var biometrics = _context.BiometricInformationSnapshots
                                     .Select(b => b.ToDto())
                                     .ToList();

            if (biometrics is null)
            {
                throw new KeyNotFoundException($"There is no biometric information snapshot.");
            }
            _logger.LogInformation("All biometric information snapshots were logged at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", DateTimeOffset.UtcNow);
            return biometrics;
        }

        public BiometricInformationSnapshotDto GetBiometricInformationByID(long id)
        {
            if (_context.BiometricInformationSnapshots is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            var biometrics = _context.BiometricInformationSnapshots
                                     .Where(b => b.Id == id)
                                     .Select(b => b.ToDto())
                                     .FirstOrDefault();

            if (biometrics is null)
            {
                throw new KeyNotFoundException($"There is no biometric information snapshot with an id of {id}.");
            }

            _logger.LogInformation("The biometric information snapshot with an ID of {id} was logged at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", id, DateTimeOffset.UtcNow);
            return biometrics;
        }
        public List<BiometricInformationSnapshotDto> GetBiometricInformationForUser(string currentUserId)
        {
            if (_context.BiometricInformationSnapshots is null)
            {
                throw new MissingMemberException("One or more DbSets are null within the application context.");
            }

            var biometricsForUser = _context.BiometricInformationSnapshots
                                            .Where(b => b.UserId == currentUserId)
                                            .Select(b => b.ToDto())
                                            .ToList();
            if (biometricsForUser.FirstOrDefault() is null)
            {
                throw new KeyNotFoundException($"There is no biometric information for User {currentUserId}.");

            }
            _logger.LogInformation("Biometric information snapshots for the user with an ID of {UserId} was logged at {Placeholder:HH:mm:ss, MMMM dd, yyyy} (UTC).", currentUserId, DateTimeOffset.UtcNow);
            return biometricsForUser;
        }
        public void PutBiometricInformation(long id, [FromBody] BiometricInformationSnapshotDto biometricInformationSnapshotDto)
        {
            if (biometricInformationSnapshotDto is null)
            {
                throw new MissingMemberException("Biometric Information SnapshotDTO given to update was null.");
            }

            BiometricInformationSnapshot? toUpdate = (
                from BiometricInformationSnapshot biometricInformationSnapshot in _context.BiometricInformationSnapshots
                where biometricInformationSnapshot.Id == id
                select biometricInformationSnapshot
            ).FirstOrDefault();

            _logger.LogInformation("Put Query for {Id} Logged at {Placeholder:MMMM dd, yyyy}", id, DateTimeOffset.UtcNow);
            if (toUpdate is null)
            {
                _logger.LogWarning($"There is no biometric information snapshot with an ID of {id}.");
                throw new ArgumentNullException();
            }
            if (biometricInformationSnapshotDto.MeasurementDateTime is not null)
            {
                toUpdate.MeasurementDateTime = (DateTime)biometricInformationSnapshotDto.MeasurementDateTime;
              
            }
            
            toUpdate.HeightCm = biometricInformationSnapshotDto.HeightCm;
            toUpdate.WeightKg = biometricInformationSnapshotDto.WeightKg;
            toUpdate.BodyFatPercentage = biometricInformationSnapshotDto.BodyFatPercentage;
            
            _logger.LogInformation("BiometricInformation Id {Id} Logged Put update at {Placeholder:MMMM dd, yyyy}", id, DateTimeOffset.UtcNow);
            _context.SaveChanges();
        }
        public void PostBiometricInformation(BiometricInformationSnapshot biometricInformationSnapshot)
        {
            if (biometricInformationSnapshot == null)
            {
                throw new MissingMemberException("Biometric Information Snapshot given to Post was null.");

            }
            _context.BiometricInformationSnapshots.Add(biometricInformationSnapshot);
            _logger.LogInformation("New Biometric Information for Id {Id} Posted at {Placeholder:MMMM dd, yyyy}", biometricInformationSnapshot.Id, DateTimeOffset.UtcNow);

            _context.SaveChanges();
        }
        public void DeleteBiometricInformation(long id)
        {
            if (_context.BiometricInformationSnapshots is null)
            {
                _logger.LogError("DeleteBiometricInformation(long id) _context.BiometricInformationSnapshots was Null at {Placeholder:MMM dd, yyy}", DateTimeOffset.UtcNow);
                throw new ArgumentNullException();
            }
            var biometricInformation = _context.BiometricInformationSnapshots.Find(id);
            if (biometricInformation == null)
            {
                _logger.LogWarning("Biometric information not found for Id {Id}", id);
                throw new ArgumentNullException();
            }
            var biometricId = biometricInformation.Id;

            _context.BiometricInformationSnapshots.Remove(biometricInformation);
            _logger.LogInformation("Deleted biometricId {biometricId} Biometric Information at {Placeholder:MMMM dd, yyyy}", biometricId, DateTimeOffset.UtcNow);
            _context.SaveChanges();
        }
    }
}
