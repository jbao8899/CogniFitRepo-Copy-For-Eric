using CogniFitRepo.Client.HttpServices;
using CogniFitRepo.Shared.DataTransferObjects;
using Moq;
using Moq.Protected;
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
    public class WorkoutProgramHttpServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task getworkoutprograms_shouldgetworkoutprograms()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            List<WorkoutProgramDto> workoutProgramDtos = new List<WorkoutProgramDto>()
            {
                new WorkoutProgramDto()
                {
                    Id = 1,
                    UserId = "1",
                    Name = "WorkoutProgram1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7),
                    Notes = "Notes1",
                    IsComplete = false,
                    WorkoutDtos = new List<WorkoutDto>()
                    {
                        new WorkoutDto()
                    }
                }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(workoutProgramDtos))
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.GetWorkoutPrograms();

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(result.Data.Count(), workoutProgramDtos.Count());
            Assert.AreEqual(workoutProgramDtos, result.Data);
        }

        [Test]
        public async Task getworkoutprograms_shouldnotgetworkoutprograms_400() // Bad Request
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.GetWorkoutPrograms();

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task getworkoutprogram_shouldgetworkoutprogram()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutProgramDto workoutProgramDto = new WorkoutProgramDto()
            {
                Id = 1,
                UserId = "1",
                Name = "WorkoutProgram1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Notes = "Notes1",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(workoutProgramDto))
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.GetWorkoutProgram(workoutProgramDto.Id);

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(workoutProgramDto, result.Data);
        }

        [Test]
        public async Task getworkoutprogram_shouldnotgetworkoutprogram_400() // Bad Request
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutProgramDto workoutProgramDto = new WorkoutProgramDto()
            {
                Id = 1,
                UserId = "1",
                Name = "WorkoutProgram1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Notes = "Notes1",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.GetWorkoutProgram(workoutProgramDto.Id);

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task getcurrentusersworkoutprograms_shouldgetcurrentusersworkoutprograms()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            List<WorkoutProgramDto> workoutProgramDtos = new List<WorkoutProgramDto>()
            {
                new WorkoutProgramDto()
                {
                    Id = 1,
                    UserId = "1",
                    Name = "WorkoutProgram1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7),
                    Notes = "Notes1",
                    IsComplete = false,
                    WorkoutDtos = new List<WorkoutDto>()
                    {
                        new WorkoutDto()
                    }
                }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(workoutProgramDtos))
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.GetCurrentUsersWorkoutPrograms();

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(result.Data.Count(), workoutProgramDtos.Count());
            Assert.AreEqual(workoutProgramDtos, result.Data);
        }

        [Test]
        public async Task getcurrentusersworkoutprograms_shouldnotgetcurrentusersworkoutprograms_400() // Bad Request
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            List<WorkoutProgramDto> workoutProgramDtos = new List<WorkoutProgramDto>()
            {
                new WorkoutProgramDto()
                {
                    Id = 1,
                    UserId = "1",
                    Name = "WorkoutProgram1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7),
                    Notes = "Notes1",
                    IsComplete = false,
                    WorkoutDtos = new List<WorkoutDto>()
                    {
                        new WorkoutDto()
                    }
                }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.GetCurrentUsersWorkoutPrograms();

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task createworkoutprogram_shouldcreateworkoutprogram()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutProgramDto workoutProgramDto = new WorkoutProgramDto()
            {
                Id = 1,
                UserId = "1",
                Name = "WorkoutProgram1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Notes = "Notes1",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(workoutProgramDto))
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.CreateWorkoutProgram(workoutProgramDto);

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(workoutProgramDto, result.Data);
        }

        [Test]
        public async Task createworkoutprogram_shouldnotcreateworkoutprogram_400() // Bad Request
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutProgramDto workoutProgramDto = new WorkoutProgramDto()
            {
                Id = 1,
                UserId = "1",
                Name = "WorkoutProgram1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Notes = "Notes1",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.CreateWorkoutProgram(workoutProgramDto);

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task updateworkoutprogram_shouldupdateworkoutprogram()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutProgramDto workoutProgramDto = new WorkoutProgramDto()
            {
                Id = 1,
                UserId = "1",
                Name = "WorkoutProgram1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Notes = "Notes1",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                }
            };

            mockHttp.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(workoutProgramDto))
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.UpdateWorkoutProgram(workoutProgramDto.Id, workoutProgramDto);

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(workoutProgramDto, result.Data);
        }

        [Test]
        public async Task updateworkoutprogram_shouldnotupdateworkoutprogram_400() // Bad Request
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutProgramDto workoutProgramDto = new WorkoutProgramDto()
            {
                Id = 1,
                UserId = "1",
                Name = "WorkoutProgram1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Notes = "Notes1",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                }
            };

            mockHttp.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.UpdateWorkoutProgram(workoutProgramDto.Id, workoutProgramDto);

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task deleteworkoutprogram_shoulddeleteworkoutprogram()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutProgramDto workoutProgramDto = new WorkoutProgramDto()
            {
                Id = 1,
                UserId = "1",
                Name = "WorkoutProgram1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Notes = "Notes1",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                }
            };

            mockHttp.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(workoutProgramDto))
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.DeleteWorkoutProgram(workoutProgramDto.Id);

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(workoutProgramDto, result.Data);
        }

        [Test]
        public async Task deleteworkoutprogram_shouldnotdeleteworkoutprogram_400() // Bad Request
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutProgramDto workoutProgramDto = new WorkoutProgramDto()
            {
                Id = 1,
                UserId = "1",
                Name = "WorkoutProgram1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Notes = "Notes1",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                }
            };

            mockHttp.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127/");

            //Act
            IWorkoutProgramHttpService workoutProgramHttpService = new WorkoutProgramHttpService(httpClient);
            var result = await workoutProgramHttpService.DeleteWorkoutProgram(workoutProgramDto.Id);

            //Assert
            Assert.IsFalse(result.Succeeded);
        }
    }
}
