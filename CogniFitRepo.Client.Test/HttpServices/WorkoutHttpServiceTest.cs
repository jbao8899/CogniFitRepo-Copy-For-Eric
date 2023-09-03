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
    public class WorkoutHttpServiceTest
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public async Task getworkouts_shouldgetworkouts()
        {
            var mockHttp = new Mock<HttpMessageHandler>();

            List<WorkoutDto> workouts = new List<WorkoutDto>() 
            { 
                new WorkoutDto() 
                { 
                    Id = 1,
                    Notes = "Notes1",
                    IsComplete = true,
                    WorkoutDateTime = DateTime.Now,
                    WorkoutProgramId = 1,
                    ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                    {
                        new ExerciseInstanceDto()
                        {
                            Id = 1,
                            ExerciseId = 1,
                            ExerciseName = "Exercise1",
                            IsComplete = true,
                            ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                            {
                                new ExerciseInstance_ExercisePropertyDto()
                                {
                                    Amount = 1,
                                    ExercisePropertyId = 1,
                                    ExerciseInstanceId = 1,
                                    Name = "Property1",
                                }
                            },
                            WorkoutSequenceNumber = 1,
                            WorkoutId = 1,
                        } 
                    }
                },
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(workouts)),
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.GetWorkouts();

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(workouts.Count, result.Data.Count);
            Assert.AreEqual(workouts, result.Data);
        }

        [Test]
        public async Task getworkouts_shouldnotgetworkouts_400()
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
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            //Act
            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.GetWorkouts();

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task getworkout_shouldgetworkout()
        {
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutDto workout = new WorkoutDto()
            {
                Id = 1,
                Notes = "Notes1",
                IsComplete = true,
                WorkoutDateTime = DateTime.Now,
                WorkoutProgramId = 1,
                ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                {
                        new ExerciseInstanceDto()
                        {
                            Id = 1,
                            ExerciseId = 1,
                            ExerciseName = "Exercise1",
                            IsComplete = true,
                            ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                            {
                                new ExerciseInstance_ExercisePropertyDto()
                                {
                                    Amount = 1,
                                    ExercisePropertyId = 1,
                                    ExerciseInstanceId = 1,
                                    Name = "Property1",
                                }
                            },
                            WorkoutSequenceNumber = 1,
                            WorkoutId = 1,
                        }
                    }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(workout)),
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.GetWorkout(1);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(workout, result.Data);
        }

        [Test]
        public async Task getworkout_shouldnotgetworkout_400()
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
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            //Act
            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.GetWorkout(1);

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task createworkout_shouldcreateworkout()
        {
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutDto workout = new WorkoutDto()
            {
                Id = 1,
                Notes = "Notes1",
                IsComplete = true,
                WorkoutDateTime = DateTime.Now,
                WorkoutProgramId = 1,
                ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                {
                        new ExerciseInstanceDto()
                        {
                            Id = 1,
                            ExerciseId = 1,
                            ExerciseName = "Exercise1",
                            IsComplete = true,
                            ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                            {
                                new ExerciseInstance_ExercisePropertyDto()
                                {
                                    Amount = 1,
                                    ExercisePropertyId = 1,
                                    ExerciseInstanceId = 1,
                                    Name = "Property1",
                                }
                            },
                            WorkoutSequenceNumber = 1,
                            WorkoutId = 1,
                        }
                    }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(workout)),
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.CreateWorkout(workout);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(workout, result.Data);
        }

        [Test]
        public async Task createworkout_shouldnotcreateworkout_400()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutDto workout = new WorkoutDto()
            {
                Id = 1,
                Notes = "Notes1",
                IsComplete = true,
                WorkoutDateTime = DateTime.Now,
                WorkoutProgramId = 1,
                ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                {
                        new ExerciseInstanceDto()
                        {
                            Id = 1,
                            ExerciseId = 1,
                            ExerciseName = "Exercise1",
                            IsComplete = true,
                            ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                            {
                                new ExerciseInstance_ExercisePropertyDto()
                                {
                                    Amount = 1,
                                    ExercisePropertyId = 1,
                                    ExerciseInstanceId = 1,
                                    Name = "Property1",
                                }
                            },
                            WorkoutSequenceNumber = 1,
                            WorkoutId = 1,
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
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            //Act
            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.CreateWorkout(workout);

            //Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task createworkoutsforworkoutprogram_shouldcreateworkoutsforworkoutprogram()
        {
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutProgramDto workoutProgramDto = new WorkoutProgramDto()
            {
                Id = 1,
                Name = "WorkoutProgram1",
                WorkoutDtos = new List<WorkoutDto>()
            };

            List<WorkoutDto> workouts = new List<WorkoutDto>()
            {
                new WorkoutDto()
                {
                    Id = 1,
                    Notes = "Notes1",
                    IsComplete = true,
                    WorkoutDateTime = DateTime.Now,
                    WorkoutProgramId = 1,
                    ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                    {
                        new ExerciseInstanceDto()
                        {
                            Id = 1,
                            ExerciseId = 1,
                            ExerciseName = "Exercise1",
                            IsComplete = true,
                            ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                            {
                                new ExerciseInstance_ExercisePropertyDto()
                                {
                                    Amount = 1,
                                    ExercisePropertyId = 1,
                                    ExerciseInstanceId = 1,
                                    Name = "Property1",
                                }
                            },
                            WorkoutSequenceNumber = 1,
                            WorkoutId = 1,
                        }
                    }
                },
                new WorkoutDto()
                {
                    Id = 2,
                    Notes = "Notes2",
                    IsComplete = true,
                    WorkoutDateTime = DateTime.Now,
                    WorkoutProgramId = 1,
                    ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                    {
                        new ExerciseInstanceDto()
                        {
                            Id = 2,
                            ExerciseId = 2,
                            ExerciseName = "Exercise2",
                            IsComplete = true,
                            ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                            {
                                new ExerciseInstance_ExercisePropertyDto()
                                {
                                    Amount = 2,
                                    ExercisePropertyId = 2,
                                    ExerciseInstanceId = 2,
                                    Name = "Property2",
                                }
                            },
                            WorkoutSequenceNumber = 2,
                            WorkoutId = 2,
                        }
                    }
                }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(workouts)),
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.CreateWorkoutsForWorkoutProgram(workoutProgramDto.Id, workouts);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(workouts, result.Data);
        }

        [Test]
        public async Task createworkoutsforworkoutprogram_shouldnotcreateworkoutsforworkoutprogram_400()
        {
            var mockHttp = new Mock<HttpMessageHandler>();

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.CreateWorkoutsForWorkoutProgram(1, new List<WorkoutDto>());

            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task updateworkout_shouldupdateworkout()
        {
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutDto workout = new WorkoutDto()
            {
                Id = 1,
                Notes = "Notes1",
                IsComplete = true,
                WorkoutDateTime = DateTime.Now,
                WorkoutProgramId = 1,
                ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                {
                        new ExerciseInstanceDto()
                        {
                            Id = 1,
                            ExerciseId = 1,
                            ExerciseName = "Exercise1",
                            IsComplete = true,
                            ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                            {
                                new ExerciseInstance_ExercisePropertyDto()
                                {
                                    Amount = 1,
                                    ExercisePropertyId = 1,
                                    ExerciseInstanceId = 1,
                                    Name = "Property1",
                                }
                            },
                            WorkoutSequenceNumber = 1,
                            WorkoutId = 1,
                        }
                    }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(workout)),
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.UpdateWorkout(workout.Id, workout);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(workout, result.Data);
        }

        [Test]
        public async Task updateworkout_shouldnotupdateworkout_400()
        {
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutDto workout = new WorkoutDto()
            {
                Id = 1,
                Notes = "Notes1",
                IsComplete = true,
                WorkoutDateTime = DateTime.Now,
                WorkoutProgramId = 1,
                ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                {
                        new ExerciseInstanceDto()
                        {
                            Id = 1,
                            ExerciseId = 1,
                            ExerciseName = "Exercise1",
                            IsComplete = true,
                            ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                            {
                                new ExerciseInstance_ExercisePropertyDto()
                                {
                                    Amount = 1,
                                    ExercisePropertyId = 1,
                                    ExerciseInstanceId = 1,
                                    Name = "Property1",
                                }
                            },
                            WorkoutSequenceNumber = 1,
                            WorkoutId = 1,
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
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.UpdateWorkout(workout.Id, workout);

            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task deleteworkout_shoulddeleteworkout()
        {
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutDto workout = new WorkoutDto()
            {
                Id = 1,
                Notes = "Notes1",
                IsComplete = true,
                WorkoutDateTime = DateTime.Now,
                WorkoutProgramId = 1,
                ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                {
                        new ExerciseInstanceDto()
                        {
                            Id = 1,
                            ExerciseId = 1,
                            ExerciseName = "Exercise1",
                            IsComplete = true,
                            ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                            {
                                new ExerciseInstance_ExercisePropertyDto()
                                {
                                    Amount = 1,
                                    ExercisePropertyId = 1,
                                    ExerciseInstanceId = 1,
                                    Name = "Property1",
                                }
                            },
                            WorkoutSequenceNumber = 1,
                            WorkoutId = 1,
                        }
                    }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(workout)),
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.DeleteWorkout(workout.Id);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(workout, result.Data);
        }

        [Test]
        public async Task deleteworkout_shouldnotdeleteworkout_400()
        {
            var mockHttp = new Mock<HttpMessageHandler>();

            WorkoutDto workout = new WorkoutDto()
            {
                Id = 1,
                Notes = "Notes1",
                IsComplete = true,
                WorkoutDateTime = DateTime.Now,
                WorkoutProgramId = 1,
                ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                {
                        new ExerciseInstanceDto()
                        {
                            Id = 1,
                            ExerciseId = 1,
                            ExerciseName = "Exercise1",
                            IsComplete = true,
                            ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                            {
                                new ExerciseInstance_ExercisePropertyDto()
                                {
                                    Amount = 1,
                                    ExercisePropertyId = 1,
                                    ExerciseInstanceId = 1,
                                    Name = "Property1",
                                }
                            },
                            WorkoutSequenceNumber = 1,
                            WorkoutId = 1,
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
            httpClient.BaseAddress = new Uri("http://localhost:7127");

            IWorkoutHttpService workoutHttpService = new WorkoutHttpService(httpClient);
            var result = await workoutHttpService.DeleteWorkout(workout.Id);

            Assert.IsFalse(result.Succeeded);
        }
    }
}
