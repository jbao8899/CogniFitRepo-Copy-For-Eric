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
    public class GetBiometricInformationTests
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
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
        
        [Test]
        public void GetBiometricInformation_ReturnsValidData()
        {
            // Arrange
            _context.BiometricInformationSnapshots.Add(new BiometricInformationSnapshot());
            _context.SaveChanges();

            var repository = new BiometricInformationRepository(_context, _logger.Object);
            
            // Act
            var result = repository.GetBiometricInformation();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<BiometricInformationSnapshotDto>>(result);
            
        }
        [Test]
        public void GetBiometricInformation_ThrowsExceptionWhenDbSetsAreNull()
        {
            // Arrange
            _context.BiometricInformationSnapshots = null; // Simulate null DbSet

            var repository = new BiometricInformationRepository(_context, _logger.Object);

            // Act & Assert
            Assert.Throws<MissingMemberException>(() => repository.GetBiometricInformation());
            
        }
        [Test]
        public void GetBiometricInformationById_ReturnsValidData()
        {
            // Arrange
            var biometricSnapshot = new BiometricInformationSnapshot
            {
                Id = 1,
                UserId = "user1",
                MeasurementDateTime = DateTime.UtcNow,
                HeightCm = 180,
                WeightKg = 70,
                BodyFatPercentage = 15
            };
            _context.BiometricInformationSnapshots.Add(biometricSnapshot);
            _context.SaveChanges();

            var repository = new BiometricInformationRepository(_context, _logger.Object);

            // Act
            var result = repository.GetBiometricInformationByID(1);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(biometricSnapshot.Id, result.Id);
        }
        [Test]
        public void GetBiometricInformationByID_ThrowsKeyNotFoundException()
        {
            // Arrange
            var biometricSnapshot = new BiometricInformationSnapshot
            {
                Id = 1,
                UserId = "user1",
                MeasurementDateTime = DateTime.UtcNow,
                HeightCm = 180,
                WeightKg = 70,
                BodyFatPercentage = 15
            };
            _context.BiometricInformationSnapshots.Add(biometricSnapshot);
            _context.SaveChanges();

            var repository = new BiometricInformationRepository(_context, _logger.Object);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => repository.GetBiometricInformationByID(2));
        }
        [Test]
        public void GetBiometricInformationByID_ThrowsMissingMemberException()
        {
            // Arrange
            _context.BiometricInformationSnapshots = null;

            var repository = new BiometricInformationRepository(_context, _logger.Object);

            // Act & Assert
            Assert.Throws<MissingMemberException>(() => repository.GetBiometricInformationByID(1));
        }
        [Test]
        public void GetBiometricInformationForUser_ReturnsValidData()
        {
            // Arrange
            var userId = "user123";

            var biometricSnapshot1 = new BiometricInformationSnapshot
            {
                Id = 1,
                UserId = userId,
                MeasurementDateTime = DateTime.UtcNow,
                HeightCm = 180,
                WeightKg = 70,
                BodyFatPercentage = 15
            };

            var biometricSnapshot2 = new BiometricInformationSnapshot
            {
                Id = 2,
                UserId = userId,
                MeasurementDateTime = DateTime.UtcNow,
                HeightCm = 170,
                WeightKg = 75,
                BodyFatPercentage = 20
            };

            _context.BiometricInformationSnapshots.AddRange(biometricSnapshot1, biometricSnapshot2);
            _context.SaveChanges();

            var repository = new BiometricInformationRepository(_context, _logger.Object);

            // Act
            var result = repository.GetBiometricInformationForUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<BiometricInformationSnapshotDto>>(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(biometricSnapshot1.Id, result[0].Id);
            Assert.AreEqual(biometricSnapshot1.UserId, result[0].UserId);
            Assert.AreEqual(biometricSnapshot2.Id, result[1].Id);
            Assert.AreEqual(biometricSnapshot2.UserId, result[1].UserId);
        }
        [Test]
        public void GetBiometricInformationForUser_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = "user1";
            var biometricSnapshot = new BiometricInformationSnapshot
            {
                Id = 1,
                UserId = "user2",
                MeasurementDateTime = DateTime.UtcNow,
                HeightCm = 180,
                WeightKg = 70,
                BodyFatPercentage = 15
            };
            _context.BiometricInformationSnapshots.Add(biometricSnapshot);
            _context.SaveChanges();

            var repository = new BiometricInformationRepository(_context, _logger.Object);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => repository.GetBiometricInformationForUser(userId));
        }
        [Test]
        public void GetBiometricInformationForUser_ThrowsMissingMemberException()
        {
            // Arrange
            _context.BiometricInformationSnapshots = null;

            var repository = new BiometricInformationRepository(_context, _logger.Object);

            // Act & Assert
            Assert.Throws<MissingMemberException>(() => repository.GetBiometricInformationForUser("user1"));
        }

    }
}