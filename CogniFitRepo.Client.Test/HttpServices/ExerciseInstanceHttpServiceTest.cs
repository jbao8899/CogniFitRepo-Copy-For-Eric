using CogniFitRepo.Client.HttpServices;
using CogniFitRepo.Shared.DataTransferObjects;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CogniFitRepo.Client.Test.HttpServices
{
    [TestFixture]
    public class ExerciseInstanceHttpServiceTest
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public async Task getexerciseinstances_shouldgetaxerciseinstances()
        {
            //Arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            List<ExerciseInstanceDto> exerciseInstanceDto = new List<ExerciseInstanceDto>()
            {
                new ExerciseInstanceDto()
                {
                    Id = 1,
                    ExerciseId = 1,
                    ExerciseName = "test",
                    WorkoutId = 1,
                    WorkoutSequenceNumber = 1,
                    IsComplete = true,
                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                    {
                        new ExerciseInstance_ExercisePropertyDto()
                        {
                            ExerciseInstanceId = 1,
                            ExercisePropertyId = 1,
                            Amount = 1,
                            Name = "test"
                        }
                    }
                }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(exerciseInstanceDto))
                });

            var httpClient = new HttpClient(mockhttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //Act
            IExerciseInstanceHttpService exerciseInstanceHttpService = new ExerciseInstanceHttpService(httpClient);
            var result = await exerciseInstanceHttpService.GetExerciseInstances();

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(exerciseInstanceDto, result.Data);
        }

        [Test]
        public async Task getexerciseinstances_shouldnotgetaxerciseinstances_400()
        {
            //Arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockhttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //Act
            IExerciseInstanceHttpService exerciseInstanceHttpService = new ExerciseInstanceHttpService(httpClient);
            var result = await exerciseInstanceHttpService.GetExerciseInstances();

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task getexerciseinstance_shouldgetexerciseinstance()
        {
            //Arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseInstanceDto exerciseInstanceDto = new ExerciseInstanceDto()
            {
                Id = 1,
                ExerciseId = 1,
                ExerciseName = "test",
                WorkoutId = 1,
                WorkoutSequenceNumber = 1,
                IsComplete = true,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                        new ExerciseInstance_ExercisePropertyDto()
                        {
                            ExerciseInstanceId = 1,
                            ExercisePropertyId = 1,
                            Amount = 1,
                            Name = "test"
                        }
                    }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(exerciseInstanceDto))
                });

            var httpClient = new HttpClient(mockhttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //Act
            IExerciseInstanceHttpService exerciseInstanceHttpService = new ExerciseInstanceHttpService(httpClient);
            var result = await exerciseInstanceHttpService.GetExerciseInstance(exerciseInstanceDto.Id);

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(exerciseInstanceDto, result.Data);
        }

        [Test]
        public async Task getexerciseinstance_shouldnotgetexerciseinstance_400()
        {
            //Arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseInstanceDto exerciseInstanceDto = new ExerciseInstanceDto()
            {
                Id = 1,
                ExerciseId = 1,
                ExerciseName = "test",
                WorkoutId = 1,
                WorkoutSequenceNumber = 1,
                IsComplete = true,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                        new ExerciseInstance_ExercisePropertyDto()
                        {
                            ExerciseInstanceId = 1,
                            ExercisePropertyId = 1,
                            Amount = 1,
                            Name = "test"
                        }
                    }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockhttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //Act
            IExerciseInstanceHttpService exerciseInstanceHttpService = new ExerciseInstanceHttpService(httpClient);
            var result = await exerciseInstanceHttpService.GetExerciseInstance(exerciseInstanceDto.Id);

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task createexerciseinstance_shouldcreateexerciseinstance()
        {
            //Arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseInstanceDto exerciseInstanceDto = new ExerciseInstanceDto()
            {
                Id = 1,
                ExerciseId = 1,
                ExerciseName = "test",
                WorkoutId = 1,
                WorkoutSequenceNumber = 1,
                IsComplete = true,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                        new ExerciseInstance_ExercisePropertyDto()
                        {
                            ExerciseInstanceId = 1,
                            ExercisePropertyId = 1,
                            Amount = 1,
                            Name = "test"
                        }
                    }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Content = new StringContent(JsonConvert.SerializeObject(exerciseInstanceDto))
                });

            var httpClient = new HttpClient(mockhttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //Act
            IExerciseInstanceHttpService exerciseInstanceHttpService = new ExerciseInstanceHttpService(httpClient);
            var result = await exerciseInstanceHttpService.CreateExerciseInstance(exerciseInstanceDto);

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(exerciseInstanceDto, result.Data);
        }

        [Test]
        public async Task createexerciseinstance_shouldnotcreateexerciseinstance_400()
        {
            //Arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseInstanceDto exerciseInstanceDto = new ExerciseInstanceDto()
            {
                Id = 1,
                ExerciseId = 1,
                ExerciseName = "test",
                WorkoutId = 1,
                WorkoutSequenceNumber = 1,
                IsComplete = true,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                        new ExerciseInstance_ExercisePropertyDto()
                        {
                            ExerciseInstanceId = 1,
                            ExercisePropertyId = 1,
                            Amount = 1,
                            Name = "test"
                        }
                    }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockhttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //Act
            IExerciseInstanceHttpService exerciseInstanceHttpService = new ExerciseInstanceHttpService(httpClient);
            var result = await exerciseInstanceHttpService.CreateExerciseInstance(exerciseInstanceDto);

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task updateexerciseinstance_shouldupdateexerciseinstance()
        {
            //Arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseInstanceDto exerciseInstanceDto = new ExerciseInstanceDto()
            {
                Id = 1,
                ExerciseId = 1,
                ExerciseName = "test",
                WorkoutId = 1,
                WorkoutSequenceNumber = 1,
                IsComplete = true,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                        new ExerciseInstance_ExercisePropertyDto()
                        {
                            ExerciseInstanceId = 1,
                            ExercisePropertyId = 1,
                            Amount = 1,
                            Name = "test"
                        }
                    }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(exerciseInstanceDto))
                });

            var httpClient = new HttpClient(mockhttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //Act
            IExerciseInstanceHttpService exerciseInstanceHttpService = new ExerciseInstanceHttpService(httpClient);
            var result = await exerciseInstanceHttpService.UpdateExerciseInstance(exerciseInstanceDto.Id, exerciseInstanceDto);

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(exerciseInstanceDto, result.Data);
        }

        [Test]
        public async Task updateexerciseinstance_shouldnotupdateexerciseinstance_400()
        {
            //Arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseInstanceDto exerciseInstanceDto = new ExerciseInstanceDto()
            {
                Id = 1,
                ExerciseId = 1,
                ExerciseName = "test",
                WorkoutId = 1,
                WorkoutSequenceNumber = 1,
                IsComplete = true,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                        new ExerciseInstance_ExercisePropertyDto()
                        {
                            ExerciseInstanceId = 1,
                            ExercisePropertyId = 1,
                            Amount = 1,
                            Name = "test"
                        }
                    }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockhttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //Act
            IExerciseInstanceHttpService exerciseInstanceHttpService = new ExerciseInstanceHttpService(httpClient);
            var result = await exerciseInstanceHttpService.UpdateExerciseInstance(exerciseInstanceDto.Id, exerciseInstanceDto);

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task deleteexerciseinstance_shoulddeleteexerciseinstance()
        {
            //Arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseInstanceDto exerciseInstanceDto = new ExerciseInstanceDto()
            {
                Id = 1,
                ExerciseId = 1,
                ExerciseName = "test",
                WorkoutId = 1,
                WorkoutSequenceNumber = 1,
                IsComplete = true,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                        new ExerciseInstance_ExercisePropertyDto()
                        {
                            ExerciseInstanceId = 1,
                            ExercisePropertyId = 1,
                            Amount = 1,
                            Name = "test"
                        }
                    }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(exerciseInstanceDto))
                });

            var httpClient = new HttpClient(mockhttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //Act
            IExerciseInstanceHttpService exerciseInstanceHttpService = new ExerciseInstanceHttpService(httpClient);
            var result = await exerciseInstanceHttpService.DeleteExerciseInstance(exerciseInstanceDto.Id);

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(exerciseInstanceDto, result.Data);
        }

        [Test]
        public async Task deleteexerciseinstance_shouldnotdeleteexerciseinstance_400()
        {
            //Arrange
            var mockhttp = new Mock<HttpMessageHandler>();

            ExerciseInstanceDto exerciseInstanceDto = new ExerciseInstanceDto()
            {
                Id = 1,
                ExerciseId = 1,
                ExerciseName = "test",
                WorkoutId = 1,
                WorkoutSequenceNumber = 1,
                IsComplete = true,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                        new ExerciseInstance_ExercisePropertyDto()
                        {
                            ExerciseInstanceId = 1,
                            ExercisePropertyId = 1,
                            Amount = 1,
                            Name = "test"
                        }
                    }
            };

            mockhttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockhttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //Act
            IExerciseInstanceHttpService exerciseInstanceHttpService = new ExerciseInstanceHttpService(httpClient);
            var result = await exerciseInstanceHttpService.DeleteExerciseInstance(exerciseInstanceDto.Id);

            //Assert
            Assert.IsFalse(result.Succeeded);
        }
    }
}
