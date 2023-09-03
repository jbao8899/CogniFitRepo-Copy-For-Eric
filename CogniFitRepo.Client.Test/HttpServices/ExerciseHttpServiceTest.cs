using CogniFitRepo.Client.HttpServices;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace CogniFitRepo.Client.Test.HttpServices
{
    [TestFixture]
    public class ExerciseHttpServiceTest
    {

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public async Task getallexercises_shouldgetallexercises()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            List<ExerciseDto> exercises = new List<ExerciseDto>() {
                new ExerciseDto() {
                    Id= 1,
                    Name = "test",
                    ExerciseCategory = "test",
                    ExerciseLevel = "test",
                    Force = "test",
                    Mechanic = "test",
                    Equipment = "test",
                    Instructions = "test",
                    PrimaryMuscles = new List<string>() { "test" },
                    SecondaryMuscles = new List<string>() { "test" },
                    ImagePaths = new List<string>() { "test" }
                }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(exercises)),
                });
            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");

            //act
            IExerciseHttpService exerciseHttpService = new ExerciseHttpService(client);
            var response = await exerciseHttpService.GetExercises();

            // assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(response.Data.Count(), 1);
            Assert.AreEqual(response.Data.First().Id, 1);
            Assert.AreEqual(response.Data.First().Name, "test");
            Assert.AreEqual(response.Data.First().ExerciseCategory, "test");
            Assert.AreEqual(response.Data.First().ExerciseLevel, "test");
            Assert.AreEqual(response.Data.First().Force, "test");
            Assert.AreEqual(response.Data.First().Mechanic, "test");
            Assert.AreEqual(response.Data.First().Equipment, "test");
            Assert.AreEqual(response.Data.First().Instructions, "test");
            Assert.AreEqual(response.Data.First().PrimaryMuscles.First(), "test");
            Assert.AreEqual(response.Data.First().SecondaryMuscles.First(), "test");
            Assert.AreEqual(response.Data.First().ImagePaths.First(), "test");
        }

        [Test]
        public async Task getallexercises_shouldreturnemptylist()
        {
            // arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            List<ExerciseDto> exercises = new List<ExerciseDto>();

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(exercises)),
                });
            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");

            //act
            IExerciseHttpService exerciseHttpService = new ExerciseHttpService(client);
            var response = await exerciseHttpService.GetExercises();

            // assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(response.Data.Count(), 0);
        }

        [Test]
        public async Task getexercises_shouldnotgetexercise_400()
        {
            //arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseDto exercise = new ExerciseDto()
            {
                Id = 1,
                Name = "test",
                ExerciseCategory = "test",
                ExerciseLevel = "test",
                Force = "test",
                Mechanic = "test",
                Equipment = "test",
                Instructions = "test",
                PrimaryMuscles = new List<string>() { "test" },
                SecondaryMuscles = new List<string>() { "test" },
                ImagePaths = new List<string>() { "test" }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");

            //act
            IExerciseHttpService exerciseHttpService = new ExerciseHttpService(client);
            var response = await exerciseHttpService.GetExercise(1);

            //assert
            Assert.IsFalse(response.Succeeded);
        }

        [Test]
        public async Task getexercise_shouldgetexercise()
        {
            //arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseDto exercise = new ExerciseDto()
            {
                Id = 1,
                Name = "test",
                ExerciseCategory = "test",
                ExerciseLevel = "test",
                Force = "test",
                Mechanic = "test",
                Equipment = "test",
                Instructions = "test",
                PrimaryMuscles = new List<string>() { "test" },
                SecondaryMuscles = new List<string>() { "test" },
                ImagePaths = new List<string>() { "test" }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(exercise)),
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IExerciseHttpService exerciseHttpService = new ExerciseHttpService(client);

            var response = await exerciseHttpService.GetExercise(1);

            // assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(response.Data.Id, 1);
            Assert.AreEqual(response.Data.Name, "test");
            Assert.AreEqual(response.Data.ExerciseCategory, "test");
            Assert.AreEqual(response.Data.ExerciseLevel, "test");
            Assert.AreEqual(response.Data.Force, "test");
            Assert.AreEqual(response.Data.Mechanic, "test");
            Assert.AreEqual(response.Data.Equipment, "test");
            Assert.AreEqual(response.Data.Instructions, "test");
            Assert.AreEqual(response.Data.PrimaryMuscles.First(), "test");
            Assert.AreEqual(response.Data.SecondaryMuscles.First(), "test");
            Assert.AreEqual(response.Data.ImagePaths.First(), "test");
        }

        [Test]
        public async Task getexercise_shouldnotgetexercise_400()
        {
            //arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseDto exercise = new ExerciseDto()
            {
                Id = 1,
                Name = "test",
                ExerciseCategory = "test",
                ExerciseLevel = "test",
                Force = "test",
                Mechanic = "test",
                Equipment = "test",
                Instructions = "test",
                PrimaryMuscles = new List<string>() { "test" },
                SecondaryMuscles = new List<string>() { "test" },
                ImagePaths = new List<string>() { "test" }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/api/");
            IExerciseHttpService exerciseHttpService = new ExerciseHttpService(client);

            var response = await exerciseHttpService.GetExercise(1);

            // assert
            Assert.IsFalse(response.Succeeded);
        }

        [Test]
        public async Task createexercise_shouldcreateexercise()
        {
            //arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseDto exercise = new ExerciseDto()
            {
                Id = 1,
                Name = "test",
                ExerciseCategory = "test",
                ExerciseLevel = "test",
                Force = "test",
                Mechanic = "test",
                Equipment = "test",
                Instructions = "test",
                PrimaryMuscles = new List<string>() { "test" },
                SecondaryMuscles = new List<string>() { "test" },
                ImagePaths = new List<string>() { "test" }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Content = new StringContent(JsonSerializer.Serialize(exercise)),
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/");
            IExerciseHttpService exerciseHttpService = new ExerciseHttpService(client);

            var response = await exerciseHttpService.CreateExercise(exercise);

            // assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(response.Data.Id, 1);
            Assert.AreEqual(response.Data.Name, "test");
            Assert.AreEqual(response.Data.ExerciseCategory, "test");
            Assert.AreEqual(response.Data.ExerciseLevel, "test");
            Assert.AreEqual(response.Data.Force, "test");
            Assert.AreEqual(response.Data.Mechanic, "test");
            Assert.AreEqual(response.Data.Equipment, "test");
            Assert.AreEqual(response.Data.Instructions, "test");
            Assert.AreEqual(response.Data.PrimaryMuscles.First(), "test");
            Assert.AreEqual(response.Data.SecondaryMuscles.First(), "test");
            Assert.AreEqual(response.Data.ImagePaths.First(), "test");
        }

        [Test]
        public async Task createexercise_shouldnotcreateexercise_400()
        {
            //arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseDto exercise = new ExerciseDto()
            {
                Id = 1,
                Name = "test",
                ExerciseCategory = "test",
                ExerciseLevel = "test",
                Force = "test",
                Mechanic = "test",
                Equipment = "test",
                Instructions = "test",
                PrimaryMuscles = new List<string>() { "test" },
                SecondaryMuscles = new List<string>() { "test" },
                ImagePaths = new List<string>() { "test" }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/");
            IExerciseHttpService exerciseHttpService = new ExerciseHttpService(client);

            var response = await exerciseHttpService.CreateExercise(exercise);

            // assert
            Assert.IsFalse(response.Succeeded);
        }

        [Test]
        public async Task updateexercise_shouldupdateexercise()
        {
            //arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseDto exercise = new ExerciseDto()
            {
                Id = 1,
                Name = "test",
                ExerciseCategory = "test",
                ExerciseLevel = "test",
                Force = "test",
                Mechanic = "test",
                Equipment = "test",
                Instructions = "test",
                PrimaryMuscles = new List<string>() { "test" },
                SecondaryMuscles = new List<string>() { "test" },
                ImagePaths = new List<string>() { "test" }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(exercise)),
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/");
            IExerciseHttpService exerciseHttpService = new ExerciseHttpService(client);

            var response = await exerciseHttpService.UpdateExercise(exercise.Id, exercise);

            // assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(response.Data.Id, 1);
            Assert.AreEqual(response.Data.Name, "test");
            Assert.AreEqual(response.Data.ExerciseCategory, "test");
            Assert.AreEqual(response.Data.ExerciseLevel, "test");
            Assert.AreEqual(response.Data.Force, "test");
            Assert.AreEqual(response.Data.Mechanic, "test");
            Assert.AreEqual(response.Data.Equipment, "test");
            Assert.AreEqual(response.Data.Instructions, "test");
            Assert.AreEqual(response.Data.PrimaryMuscles.First(), "test");
            Assert.AreEqual(response.Data.SecondaryMuscles.First(), "test");
            Assert.AreEqual(response.Data.ImagePaths.First(), "test");
        }

        [Test]
        public async Task updateexercise_shouldnotupdateexercise_400()
        {
            //arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseDto exercise = new ExerciseDto()
            {
                Id = 1,
                Name = "test",
                ExerciseCategory = "test",
                ExerciseLevel = "test",
                Force = "test",
                Mechanic = "test",
                Equipment = "test",
                Instructions = "test",
                PrimaryMuscles = new List<string>() { "test" },
                SecondaryMuscles = new List<string>() { "test" },
                ImagePaths = new List<string>() { "test" }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/");
            IExerciseHttpService exerciseHttpService = new ExerciseHttpService(client);

            var response = await exerciseHttpService.UpdateExercise(exercise.Id, exercise);

            // assert
            Assert.IsFalse(response.Succeeded);
        }

        [Test]
        public async Task deleteexercise_shoulddeleteexercise()
        {
            //arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseDto exercise = new ExerciseDto()
            {
                Id = 1,
                Name = "test",
                ExerciseCategory = "test",
                ExerciseLevel = "test",
                Force = "test",
                Mechanic = "test",
                Equipment = "test",
                Instructions = "test",
                PrimaryMuscles = new List<string>() { "test" },
                SecondaryMuscles = new List<string>() { "test" },
                ImagePaths = new List<string>() { "test" }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(exercise)),
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/");
            IExerciseHttpService exerciseHttpService = new ExerciseHttpService(client);

            var response = await exerciseHttpService.DeleteExercise(exercise.Id);

            // assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(response.Data.Id, 1);
            Assert.AreEqual(response.Data.Name, "test");
            Assert.AreEqual(response.Data.ExerciseCategory, "test");
            Assert.AreEqual(response.Data.ExerciseLevel, "test");
            Assert.AreEqual(response.Data.Force, "test");
            Assert.AreEqual(response.Data.Mechanic, "test");
            Assert.AreEqual(response.Data.Equipment, "test");
            Assert.AreEqual(response.Data.Instructions, "test");
            Assert.AreEqual(response.Data.PrimaryMuscles.First(), "test");
            Assert.AreEqual(response.Data.SecondaryMuscles.First(), "test");
            Assert.AreEqual(response.Data.ImagePaths.First(), "test");
        }

        [Test]
        public async Task deleteexercise_shouldnotdeleteexercise_400()
        {
            //arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseDto exercise = new ExerciseDto()
            {
                Id = 1,
                Name = "test",
                ExerciseCategory = "test",
                ExerciseLevel = "test",
                Force = "test",
                Mechanic = "test",
                Equipment = "test",
                Instructions = "test",
                PrimaryMuscles = new List<string>() { "test" },
                SecondaryMuscles = new List<string>() { "test" },
                ImagePaths = new List<string>() { "test" }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var client = new HttpClient(mockhttp.Object);
            client.BaseAddress = new Uri("https://localhost:7127/");
            IExerciseHttpService exerciseHttpService = new ExerciseHttpService(client);

            var response = await exerciseHttpService.DeleteExercise(exercise.Id);

            // assert
            Assert.IsFalse(response.Succeeded);
        }
    }
}
