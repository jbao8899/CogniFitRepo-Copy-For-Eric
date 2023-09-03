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
using CogniFitRepo.Client.HttpServices;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using System.Net.Http;
using Moq.Protected;
using System.Threading;

namespace CogniFitRepo.Client.Test.HttpServices
{
    [TestFixture]
    public class BiometricInformationHttpServiceTest
    { 

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public async Task getallbiometric_shouldgetallrecords()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            //mock api/user end point which returns biometricsnapshot
            //string testbiometricresponse = @"{'id': 1, 'userid':'test', 'measurementdatetime':'2023 - 04 - 05T16: 00:00','heightcm':180,'weightkg':70,'bodyfatpercentage'':19 }";

            List<BiometricInformationSnapshotDto> bio = new List<BiometricInformationSnapshotDto>() {
                new BiometricInformationSnapshotDto() {
                    Id= 1,
                    UserId = "test",
                    MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                    HeightCm = 180,
                    WeightKg = 70,
                    BodyFatPercentage = 19
                }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(bio)),
                });

            //mockhttp.When("https://localhost:7127/api/biometric-information/").Respond("application/json", JsonSerializer.Serialize(bio));

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);


            var response = await BiometricInformationRepository.GetBiometricInformations();

            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(bio[0].Id, response.Data[0].Id);
            Assert.AreEqual(bio[0].UserId, response.Data[0].UserId);
            Assert.AreEqual(bio[0].MeasurementDateTime, response.Data[0].MeasurementDateTime);
            Assert.AreEqual(bio[0].HeightCm, response.Data[0].HeightCm);
            Assert.AreEqual(bio[0].WeightKg, response.Data[0].WeightKg);
        }

        [Test]
        public async Task getallbiometric_shouldhandlefailure()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.GetBiometricInformations();

            // assert
            Assert.IsFalse(response.Succeeded);
        }


    }

    [TestFixture]
    public class GetCurrentUserBiometricInformationsTests
    {
        [Test]
        public async Task getcurrentuserbiometricinformations_shouldgetallrecords()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            List<BiometricInformationSnapshotDto> bio = new List<BiometricInformationSnapshotDto>() {
        new BiometricInformationSnapshotDto() {
            Id= 1,
            UserId = "test",
            MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            HeightCm = 180,
            WeightKg = 70,
            BodyFatPercentage = 19
        }
    };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(bio)),
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.GetCurrentUserBiometricInformations();

            // assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(bio[0].Id, response.Data[0].Id);
            Assert.AreEqual(bio[0].UserId, response.Data[0].UserId);
            Assert.AreEqual(bio[0].MeasurementDateTime, response.Data[0].MeasurementDateTime);
            Assert.AreEqual(bio[0].HeightCm, response.Data[0].HeightCm);
            Assert.AreEqual(bio[0].WeightKg, response.Data[0].WeightKg);
        }

        [Test]
        public async Task getcurrentuserbiometricinformations_shouldhandlefailure()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.GetCurrentUserBiometricInformations();

            // assert
            Assert.IsFalse(response.Succeeded);
        }

    }

    [TestFixture]
    public class GetUserBiometricInformationTests
    {
        [Test]
        public async Task getbiometricinformation_shouldgetrecord()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            BiometricInformationSnapshotDto bio = new BiometricInformationSnapshotDto()
            {
                Id = 1,
                UserId = "test",
                MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                HeightCm = 180,
                WeightKg = 70,
                BodyFatPercentage = 19
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(bio)),
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.GetBiometricInformation(1);

            // assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(bio.Id, response.Data.Id);
            Assert.AreEqual(bio.UserId, response.Data.UserId);
            Assert.AreEqual(bio.MeasurementDateTime, response.Data.MeasurementDateTime);
            Assert.AreEqual(bio.HeightCm, response.Data.HeightCm);
            Assert.AreEqual(bio.WeightKg, response.Data.WeightKg);
        }

        [Test]
        public async Task getbiometricinformation_shouldhandlefailure()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.GetBiometricInformation(1);

            // assert
            Assert.IsFalse(response.Succeeded);
        }


    }

    [TestFixture]
    public class CreateBiometricInformationTests
    {
        [Test]
        public async Task createbiometricinformation_shouldcreatenewrecord()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            BiometricInformationSnapshotDto newRecord = new BiometricInformationSnapshotDto()
            {
                Id = 1,
                UserId = "test",
                MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                HeightCm = 180,
                WeightKg = 70,
                BodyFatPercentage = 19
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Content = new StringContent(JsonSerializer.Serialize(newRecord)),
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.CreateBiometricInformation(newRecord);

            // assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(newRecord.Id, response.Data.Id);
            Assert.AreEqual(newRecord.UserId, response.Data.UserId);
            Assert.AreEqual(newRecord.MeasurementDateTime, response.Data.MeasurementDateTime);
            Assert.AreEqual(newRecord.HeightCm, response.Data.HeightCm);
            Assert.AreEqual(newRecord.WeightKg, response.Data.WeightKg);
        }

        [Test]
        public async Task createbiometricinformation_shouldhandlefailure()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            BiometricInformationSnapshotDto newRecord = new BiometricInformationSnapshotDto()
            {
                Id = 1,
                UserId = "test",
                MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                HeightCm = 180,
                WeightKg = 70,
                BodyFatPercentage = 19
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.CreateBiometricInformation(newRecord);

            // assert
            Assert.IsFalse(response.Succeeded);
        }


    }

    [TestFixture]
    public class CreateBiometricInformationsTests
    {
        [Test]
        public async Task createbiometricinformations_shouldcreatenewrecords()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            List<BiometricInformationSnapshotDto> newRecords = new List<BiometricInformationSnapshotDto>() {
        new BiometricInformationSnapshotDto() {
            Id= 1,
            UserId = "test",
            MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            HeightCm = 180,
            WeightKg = 70,
            BodyFatPercentage = 19
        },
        new BiometricInformationSnapshotDto() {
            Id= 2,
            UserId = "test2",
            MeasurementDateTime = DateTime.ParseExact("2023-04-06T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            HeightCm = 175,
            WeightKg = 65,
            BodyFatPercentage = 18
        }
       
    };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Content = new StringContent(JsonSerializer.Serialize(newRecords)),
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.CreateBiometricInformations(newRecords);

            // assert
            Assert.IsTrue(response.Succeeded);
            for (int i = 0; i < newRecords.Count; i++)
            {
                Assert.AreEqual(newRecords[i].Id, response.Data[i].Id);
                Assert.AreEqual(newRecords[i].UserId, response.Data[i].UserId);
                Assert.AreEqual(newRecords[i].MeasurementDateTime, response.Data[i].MeasurementDateTime);
                Assert.AreEqual(newRecords[i].HeightCm, response.Data[i].HeightCm);
                Assert.AreEqual(newRecords[i].WeightKg, response.Data[i].WeightKg);
            }
        }

        [Test]
        public async Task createbiometricinformations_shouldhandlefailure()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            List<BiometricInformationSnapshotDto> newRecords = new List<BiometricInformationSnapshotDto>() {
        new BiometricInformationSnapshotDto() {
            Id= 1,
            UserId = "test",
            MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            HeightCm = 180,
            WeightKg = 70,
            BodyFatPercentage = 19
        },
        new BiometricInformationSnapshotDto() {
            Id= 2,
            UserId = "test2",
            MeasurementDateTime = DateTime.ParseExact("2023-04-06T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            HeightCm = 175,
            WeightKg = 65,
            BodyFatPercentage = 18
        }
       
    };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.CreateBiometricInformations(newRecords);

            // assert
            Assert.IsFalse(response.Succeeded);
        }

    }

    [TestFixture]
    public class UpdateBiometricInformationTests
    {
        [Test]
        public async Task updatebiometricinformation_shouldupdaterecord()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            BiometricInformationSnapshotDto existingRecord = new BiometricInformationSnapshotDto()
            {
                Id = 1,
                UserId = "test",
                MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                HeightCm = 180,
                WeightKg = 70,
                BodyFatPercentage = 19
            };

            BiometricInformationSnapshotDto updatedRecord = new BiometricInformationSnapshotDto()
            {
                Id = 1,
                UserId = "test",
                MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                HeightCm = 185,
                WeightKg = 75,
                BodyFatPercentage = 18
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(updatedRecord)),
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.UpdateBiometricInformation(existingRecord.Id, updatedRecord);

            // assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(updatedRecord.Id, response.Data.Id);
            Assert.AreEqual(updatedRecord.UserId, response.Data.UserId);
            Assert.AreEqual(updatedRecord.MeasurementDateTime, response.Data.MeasurementDateTime);
            Assert.AreEqual(updatedRecord.HeightCm, response.Data.HeightCm);
            Assert.AreEqual(updatedRecord.WeightKg, response.Data.WeightKg);
        }

        [Test]
        public async Task updatebiometricinformation_shouldhandlefailure()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            BiometricInformationSnapshotDto existingRecord = new BiometricInformationSnapshotDto()
            {
                Id = 1,
                UserId = "test",
                MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                HeightCm = 180,
                WeightKg = 70,
                BodyFatPercentage = 19
            };

            BiometricInformationSnapshotDto updatedRecord = new BiometricInformationSnapshotDto()
            {
                Id = 1,
                UserId = "test",
                MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                HeightCm = 185,
                WeightKg = 75,
                BodyFatPercentage = 18
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.UpdateBiometricInformation(existingRecord.Id, updatedRecord);

            // assert
            Assert.IsFalse(response.Succeeded);
        }

    }

    [TestFixture]
    public class DeleteBiometricInformationTests
    {
        [Test]
        public async Task deletebiometricinformation_shoulddeleterecord()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            BiometricInformationSnapshotDto existingRecord = new BiometricInformationSnapshotDto()
            {
                Id = 1,
                UserId = "test",
                MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                HeightCm = 180,
                WeightKg = 70,
                BodyFatPercentage = 19
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(existingRecord)),
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.DeleteBiometricInformation(existingRecord.Id);

            // assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(existingRecord.Id, response.Data.Id);
        }

        [Test]
        public async Task deletebiometricinformation_shouldhandlefailure()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();
            var mocklogger = new Mock<ILogger<BiometricInformationRepository>>();

            BiometricInformationSnapshotDto existingRecord = new BiometricInformationSnapshotDto()
            {
                Id = 1,
                UserId = "test",
                MeasurementDateTime = DateTime.ParseExact("2023-04-05T16:00:00", "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                HeightCm = 180,
                WeightKg = 70,
                BodyFatPercentage = 19
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IBiometricInformationHttpService BiometricInformationRepository = new BiometricInformationHttpService(client);

            // act
            var response = await BiometricInformationRepository.DeleteBiometricInformation(existingRecord.Id);

            // assert
            Assert.IsFalse(response.Succeeded);
        }


    }

}
