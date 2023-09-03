using System;
using System.Collections.Generic;
using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Server.Repositories;
using CogniFitRepo.Shared.DataTransferObjects;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace CogniFitRepo.Server.Test
{
    [TestFixture]
    public class Put_Post_DeleteBiometricInformationTests
    {
        private ApplicationDbContext _context;
        private DbContextOptions<ApplicationDbContext> _options;
        private Mock<ILogger<BiometricInformationRepository>> _logger;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "TestDatabase")
               .Options;

            var operationalStoreOptions = new OperationalStoreOptions();
            var wrappedOperationalStoreOptions = Options.Create(operationalStoreOptions);
            _context = new ApplicationDbContext(_options, wrappedOperationalStoreOptions);
            _logger = new Mock<ILogger<BiometricInformationRepository>>();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Test]
        public void PutBiometricInformation_UpdatesExistingRecord()
        {
            // Arrange
            long validId = 1;
            var biometricSnapshot = new BiometricInformationSnapshot
            {
                Id = validId,
                UserId = "user1",
                MeasurementDateTime = DateTime.UtcNow,
                HeightCm = 170,
                WeightKg = 60,
                BodyFatPercentage = 20
            };
            _context.BiometricInformationSnapshots.Add(biometricSnapshot);
            _context.SaveChanges();

            var repository = new BiometricInformationRepository(_context, _logger.Object);

            var updatedSnapshotDto = new BiometricInformationSnapshotDto
            {
                HeightCm = 175,
                WeightKg = 65,
                BodyFatPercentage = 18
            };

            // Act
            repository.PutBiometricInformation(validId, updatedSnapshotDto);

            // Assert
            var updatedSnapshot = _context.BiometricInformationSnapshots.Find(validId);
            Assert.NotNull(updatedSnapshot);
            Assert.AreEqual(updatedSnapshotDto.HeightCm, updatedSnapshot.HeightCm);
            Assert.AreEqual(updatedSnapshotDto.WeightKg, updatedSnapshot.WeightKg);
            Assert.AreEqual(updatedSnapshotDto.BodyFatPercentage, updatedSnapshot.BodyFatPercentage);
        }
        [Test]
        public void PostBiometricInformation_AddsNewRecord()
        { 
            // Arrange
            var repository = new BiometricInformationRepository(_context, _logger.Object);

            var newBiometricSnapshot = new BiometricInformationSnapshot 
            {
                Id = 1, 
                MeasurementDateTime = DateTime.UtcNow, 
                HeightCm = 170, 
                WeightKg = 60, 
                BodyFatPercentage = 20 
            }; 

            // Act
            repository.PostBiometricInformation(newBiometricSnapshot); 

            // Assert
            var addedSnapshot = _context.BiometricInformationSnapshots.Find(newBiometricSnapshot.Id);
            Assert.NotNull(addedSnapshot); 
        }
        [Test]
        public void DeleteBiometricInformation_RemovesExistingRecord()
        {
            // Arrange
            long validId = 1;
            var biometricSnapshot = new BiometricInformationSnapshot
            {
                Id = validId,
                MeasurementDateTime = DateTime.UtcNow,
                HeightCm = 170,
                WeightKg = 60,
                BodyFatPercentage = 20
            };
            _context.BiometricInformationSnapshots.Add(biometricSnapshot);
            _context.SaveChanges();

            var repository = new BiometricInformationRepository(_context, _logger.Object);

            // Act
            repository.DeleteBiometricInformation(validId);

            // Assert
            var deletedSnapshot = _context.BiometricInformationSnapshots.Find(validId);
            Assert.Null(deletedSnapshot);
        }
        [Test]
        public void DeleteBiometricInformation_ThrowsExceptionForNonExistingRecord()
        {
            // Arrange
            var repository = new BiometricInformationRepository(_context, _logger.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => repository.DeleteBiometricInformation(1));
        }
    }
}
