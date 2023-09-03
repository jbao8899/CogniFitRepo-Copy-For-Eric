using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using CogniFitRepo.Client;
using Microsoft.Extensions.Logging;
using System;
using CogniFitRepo.Client.HttpServices;
using CogniFitRepo.Client.Pages;
using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Shared.Wrappers;
using RichardSzalay.MockHttp;
using CogniFitRepo.Server.Repositories;


namespace Client.Test.Pages.ProgressTests
{
    
    [TestFixture]
    public class SetRecentWeightHeightTests
    {
        [Test]
        public void SetRecentWeightHeight_WithNullRecords_ShouldThrowArgumentNullException()
        {
            // Arrange
            var progress = new Progress();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => progress.SetRecentWeightHeight(null));
        }

        [Test]
        public void SetRecentWeightHeight_WithEmptyRecords_ShouldThrowArgumentException()
        {
            // Arrange
            var progress = new Progress();
            var records = new List<BiometricInformationSnapshotDto>();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(() => progress.SetRecentWeightHeight(records));
        }

        [Test]
        public async Task SetRecentWeightHeight_WithValidRecords_ShouldSetRecentWeightAndHeight()
        {
            // Arrange
            var progress = new Progress();
            var records = new List<BiometricInformationSnapshotDto>
    {
        new BiometricInformationSnapshotDto { WeightKg = 70, HeightCm = 1.8f },
        new BiometricInformationSnapshotDto { WeightKg = 75, HeightCm = 1.85f }
    };

            // Act
            await progress.SetRecentWeightHeight(records);

            // Assert
            Assert.AreEqual(75, progress.recentWeight);
            Assert.AreEqual(1.85f, progress.recentHeight);
        }

    }

    [TestFixture]
    public class ToggleEditTests
    {
        [Test]
        public void ToggleEdit_WhenCalledOnce_ShouldSetIsEditingToTrueAndEditButtonTextToDone()
        {
            // Arrange
            var progress = new Progress();

            // Act
            progress.ToggleEdit();

            // Assert
            Assert.IsTrue(progress.isEditing);
            Assert.AreEqual("Done", progress.editButtonText);
        }

        [Test]
        public void ToggleEdit_WhenCalledTwice_ShouldSetIsEditingToFalseAndEditButtonTextToEdit()
        {
            // Arrange
            var progress = new Progress();

            // Act
            progress.ToggleEdit();
            progress.ToggleEdit();

            // Assert
            Assert.IsFalse(progress.isEditing);
            Assert.AreEqual("Edit", progress.editButtonText);
        }

    }


    [TestFixture]
    public class AddRecordTests
    {
        [Test]
        public async Task AddRecord_WithFutureDate_ShouldNotAddRecord()
        {
            // Arrange
            var progress = new Progress();
            var records = new List<BiometricInformationSnapshotDto>();
            progress.newDate = DateTime.Today.AddDays(1);

            // Act
            await progress.AddRecord(records);

            // Assert
            Assert.IsEmpty(records);
        }

        [Test]
        public async Task AddRecord_WithNonPositiveWeightOrHeight_ShouldNotAddRecord()
        {
            // Arrange
            var progress = new Progress();
            var records = new List<BiometricInformationSnapshotDto>();
            progress.newWeight = 0;
            progress.newHeight = 0;

            // Act
            await progress.AddRecord(records);

            // Assert
            Assert.IsEmpty(records);
        }

    }


    [TestFixture]
    public class  FormatAsDayMonthTests
    {
        [Test]
        public void FormatAsDayMonth_WithNullValue_ShouldReturnEmptyString()
        {
            // Arrange
            var progress = new Progress();

            // Act
            var result = progress.FormatAsDayMonth(null);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void FormatAsDayMonth_WithValidDateTimeValue_ShouldReturnFormattedString()
        {
            // Arrange
            var progress = new Progress();
            var value = new DateTime(2023, 4, 5, 16, 25, 3);

            // Act
            var result = progress.FormatAsDayMonth(value);

            // Assert
            Assert.AreEqual("05-Apr-23", result);
        }

    }

    [TestFixture]
    public class ReorderListsTests
    {
        [Test]
        public async Task ReorderList_WithNullRecords_ShouldNotThrowException()
        {
            // Arrange
            var progress = new Progress();

            // Act and Assert
            Assert.DoesNotThrowAsync(() => progress.ReorderList(null));
        }

        [Test]
        public async Task ReorderList_WithUnorderedRecords_ShouldOrderRecordsByMeasurementDateTime()
        {
            // Arrange
            var progress = new Progress();
            var records = new List<BiometricInformationSnapshotDto>
    {
        new BiometricInformationSnapshotDto { MeasurementDateTime = new DateTime(2023, 4, 5, 16, 25, 3) },
        new BiometricInformationSnapshotDto { MeasurementDateTime = new DateTime(2022, 4, 5, 16, 25, 3) },
        new BiometricInformationSnapshotDto { MeasurementDateTime = new DateTime(2021, 4, 5, 16, 25, 3) }
    };

            // Act
            await progress.ReorderList(records);

            // Assert
            Assert.AreEqual(new DateTime(2021, 4, 5, 16, 25, 3), records[0].MeasurementDateTime);
            Assert.AreEqual(new DateTime(2022, 4, 5, 16, 25, 3), records[1].MeasurementDateTime);
            Assert.AreEqual(new DateTime(2023, 4, 5, 16, 25, 3), records[2].MeasurementDateTime);
        }

    }  

}
