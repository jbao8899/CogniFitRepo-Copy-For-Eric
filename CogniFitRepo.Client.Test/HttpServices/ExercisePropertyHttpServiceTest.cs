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
    public class ExercisePropertyHttpServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task getexerciseproperties_shouldgetexerciseproperties()
        {
            //Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            List<ExercisePropertyDto> exercisePropertyDtos = new List<ExercisePropertyDto>()
            {
                new ExercisePropertyDto()
                {
                    Id = 1,
                    Name = "Memory"
                },
                new ExercisePropertyDto()
                {
                    Id = 2,
                    Name = "Attention"
                }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(exercisePropertyDtos))
                });

            HttpClient httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //act
            IExercisePropertyHttpService  exercisePropertyHttpService = new ExercisePropertyHttpService(httpClient);
            var result = await exercisePropertyHttpService.GetExerciseProperties();

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(exercisePropertyDtos.Count, result.Data.Count);
            Assert.AreEqual(exercisePropertyDtos[0].Id, result.Data[0].Id);
        }

        [Test]
        public async Task getexerciseproperties_shouldnotgetexerciseproperties_400()
        {
            var mockHttp = new Mock<HttpMessageHandler>();

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            HttpClient httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //act
            IExercisePropertyHttpService exercisePropertyHttpService = new ExercisePropertyHttpService(httpClient);
            var result = await exercisePropertyHttpService.GetExerciseProperties();

            //Assert
            Assert.IsFalse(result.Succeeded);
        }
    }
}
