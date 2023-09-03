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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CogniFitRepo.Client.Test.HttpServices
{
    [TestFixture]
    class ApplicationUserHttpServiceTest
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public async Task getallapplicationusers_shouldgetallapplicationusers()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            List<ApplicationUserDto> applicationUsers = new List<ApplicationUserDto>()
            {
                new ApplicationUserDto()
                {
                    Id = "1",
                    UserName = "test1",
                    FirstName = "test1",
                    LastName = "test1",
                    IsFemale = true,
                    Birthday = DateTime.Now,
                    PrefersMetric = true,
                    ProfileDescription = "test1",
                    NumExperiencePoints = 1,
                    StreetNumber = 1,
                    StreetName = "test1",
                    ApartmentNumber = "test1",
                    CityName = "test1",
                    SubdivisionName = "test1",
                    CountryName = "test1",
                    PostalCode = 1,
                    PortraitPath = "test1",
                    PortraitId = 1
                }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(applicationUsers))
                });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.GetApplicationUsers();

            //assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(applicationUsers[0].Id, response.Data[0].Id);
            Assert.AreEqual(applicationUsers[0].UserName, response.Data[0].UserName);
            Assert.AreEqual(applicationUsers[0].FirstName, response.Data[0].FirstName);
            Assert.AreEqual(applicationUsers[0].LastName, response.Data[0].LastName);
            Assert.AreEqual(applicationUsers[0].IsFemale, response.Data[0].IsFemale);
            Assert.AreEqual(applicationUsers[0].Birthday, response.Data[0].Birthday);
            Assert.AreEqual(applicationUsers[0].PrefersMetric, response.Data[0].PrefersMetric);
            Assert.AreEqual(applicationUsers[0].ProfileDescription, response.Data[0].ProfileDescription);
            Assert.AreEqual(applicationUsers[0].NumExperiencePoints, response.Data[0].NumExperiencePoints);
            Assert.AreEqual(applicationUsers[0].StreetNumber, response.Data[0].StreetNumber);
            Assert.AreEqual(applicationUsers[0].StreetName, response.Data[0].StreetName);
            Assert.AreEqual(applicationUsers[0].ApartmentNumber, response.Data[0].ApartmentNumber);
            Assert.AreEqual(applicationUsers[0].CityName, response.Data[0].CityName);
            Assert.AreEqual(applicationUsers[0].SubdivisionName, response.Data[0].SubdivisionName);
            Assert.AreEqual(applicationUsers[0].CountryName, response.Data[0].CountryName);
            Assert.AreEqual(applicationUsers[0].PostalCode, response.Data[0].PostalCode);
            Assert.AreEqual(applicationUsers[0].PortraitPath, response.Data[0].PortraitPath);
            Assert.AreEqual(applicationUsers[0].PortraitId, response.Data[0].PortraitId);
        }

        [Test]
        public async Task getallapplicationusers_shouldnotgetallapplicationusers_400()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            List<ApplicationUserDto> applicationUsers = new List<ApplicationUserDto>()
            {
                new ApplicationUserDto()
                {
                    Id = "1",
                    UserName = "test1",
                    FirstName = "test1",
                    LastName = "test1",
                    IsFemale = true,
                    Birthday = DateTime.Now,
                    PrefersMetric = true,
                    ProfileDescription = "test1",
                    NumExperiencePoints = 1,
                    StreetNumber = 1,
                    StreetName = "test1",
                    ApartmentNumber = "test1",
                    CityName = "test1",
                    SubdivisionName = "test1",
                    CountryName = "test1",
                    PostalCode = 1,
                    PortraitPath = "test1",
                    PortraitId = 1
                }
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });
            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.GetApplicationUsers();

            //assert
            Assert.IsFalse(response.Succeeded);
        }

        [Test]
        public async Task getapplicationuser_shouldgetapplicationuser()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            ApplicationUserDto applicationUser= new ApplicationUserDto()
            {
                Id = "1",
                UserName = "test1",
                FirstName = "test1",
                LastName = "test1",
                IsFemale = true,
                Birthday = DateTime.Now,
                PrefersMetric = true,
                ProfileDescription = "test1",
                NumExperiencePoints = 1,
                StreetNumber = 1,
                StreetName = "test1",
                ApartmentNumber = "test1",
                CityName = "test1",
                SubdivisionName = "test1",
                CountryName = "test1",
                PostalCode = 1,
                PortraitPath = "test1",
                PortraitId = 1
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(applicationUser))
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.GetApplicationUser("1");

            //assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(applicationUser.Id, response.Data.Id);
            Assert.AreEqual(applicationUser.UserName, response.Data.UserName);
            Assert.AreEqual(applicationUser.FirstName, response.Data.FirstName);
            Assert.AreEqual(applicationUser.LastName, response.Data.LastName);
            Assert.AreEqual(applicationUser.IsFemale, response.Data.IsFemale);
            Assert.AreEqual(applicationUser.Birthday, response.Data.Birthday);
            Assert.AreEqual(applicationUser.PrefersMetric, response.Data.PrefersMetric);
            Assert.AreEqual(applicationUser.ProfileDescription, response.Data.ProfileDescription);
            Assert.AreEqual(applicationUser.NumExperiencePoints, response.Data.NumExperiencePoints);
            Assert.AreEqual(applicationUser.StreetNumber, response.Data.StreetNumber);
            Assert.AreEqual(applicationUser.StreetName, response.Data.StreetName);
            Assert.AreEqual(applicationUser.ApartmentNumber, response.Data.ApartmentNumber);
            Assert.AreEqual(applicationUser.CityName, response.Data.CityName);
            Assert.AreEqual(applicationUser.SubdivisionName, response.Data.SubdivisionName);
            Assert.AreEqual(applicationUser.CountryName, response.Data.CountryName);
            Assert.AreEqual(applicationUser.PostalCode, response.Data.PostalCode);
            Assert.AreEqual(applicationUser.PortraitPath, response.Data.PortraitPath);
            Assert.AreEqual(applicationUser.PortraitId, response.Data.PortraitId);
        }

        [Test]
        public async Task getapplicationuser_shouldnotgetapplicationuser_400()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            ApplicationUserDto applicationUser = new ApplicationUserDto()
            {
                Id = "1",
                UserName = "test1",
                FirstName = "test1",
                LastName = "test1",
                IsFemale = true,
                Birthday = DateTime.Now,
                PrefersMetric = true,
                ProfileDescription = "test1",
                NumExperiencePoints = 1,
                StreetNumber = 1,
                StreetName = "test1",
                ApartmentNumber = "test1",
                CityName = "test1",
                SubdivisionName = "test1",
                CountryName = "test1",
                PostalCode = 1,
                PortraitPath = "test1",
                PortraitId = 1
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.GetApplicationUser("1");

            //assert
            Assert.IsFalse(response.Succeeded);
        }

        [Test]
        public async Task getcurrentapplicationuser_shouldgetuser()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            ApplicationUserDto applicationUser = new ApplicationUserDto()
            {
                Id = "1",
                UserName = "test1",
                FirstName = "test1",
                LastName = "test1",
                IsFemale = true,
                Birthday = DateTime.Now,
                PrefersMetric = true,
                ProfileDescription = "test1",
                NumExperiencePoints = 1,
                StreetNumber = 1,
                StreetName = "test1",
                ApartmentNumber = "test1",
                CityName = "test1",
                SubdivisionName = "test1",
                CountryName = "test1",
                PostalCode = 1,
                PortraitPath = "test1",
                PortraitId = 1
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(applicationUser))
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.GetCurrentApplicationUser();

            //assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(applicationUser.Id, response.Data.Id);
            Assert.AreEqual(applicationUser.UserName, response.Data.UserName);
            Assert.AreEqual(applicationUser.FirstName, response.Data.FirstName);
            Assert.AreEqual(applicationUser.LastName, response.Data.LastName);
            Assert.AreEqual(applicationUser.IsFemale, response.Data.IsFemale);
            Assert.AreEqual(applicationUser.Birthday, response.Data.Birthday);
            Assert.AreEqual(applicationUser.PrefersMetric, response.Data.PrefersMetric);
            Assert.AreEqual(applicationUser.ProfileDescription, response.Data.ProfileDescription);
            Assert.AreEqual(applicationUser.NumExperiencePoints, response.Data.NumExperiencePoints);
            Assert.AreEqual(applicationUser.StreetNumber, response.Data.StreetNumber);
            Assert.AreEqual(applicationUser.StreetName, response.Data.StreetName);
            Assert.AreEqual(applicationUser.ApartmentNumber, response.Data.ApartmentNumber);
            Assert.AreEqual(applicationUser.CityName, response.Data.CityName);
            Assert.AreEqual(applicationUser.SubdivisionName, response.Data.SubdivisionName);
            Assert.AreEqual(applicationUser.CountryName, response.Data.CountryName);
            Assert.AreEqual(applicationUser.PostalCode, response.Data.PostalCode);
            Assert.AreEqual(applicationUser.PortraitPath, response.Data.PortraitPath);
            Assert.AreEqual(applicationUser.PortraitId, response.Data.PortraitId);
        }

        [Test]
        public async Task getcurrentapplicationuser_shouldnotgetuser_400()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            ApplicationUserDto applicationUser = new ApplicationUserDto()
            {
                Id = "1",
                UserName = "test1",
                FirstName = "test1",
                LastName = "test1",
                IsFemale = true,
                Birthday = DateTime.Now,
                PrefersMetric = true,
                ProfileDescription = "test1",
                NumExperiencePoints = 1,
                StreetNumber = 1,
                StreetName = "test1",
                ApartmentNumber = "test1",
                CityName = "test1",
                SubdivisionName = "test1",
                CountryName = "test1",
                PostalCode = 1,
                PortraitPath = "test1",
                PortraitId = 1
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.GetCurrentApplicationUser();

            //assert
            Assert.IsFalse(response.Succeeded);
        }

        [Test]
        public async Task createapplicationuser_shouldcreateuser()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            ApplicationUserDto applicationUserCreate = new ApplicationUserDto()
            {
                Id = "1",
                UserName = "test1",
                FirstName = "test1",
                LastName = "test1",
                IsFemale = true,
                Birthday = DateTime.Now,
                PrefersMetric = true,
                ProfileDescription = "test1",
                NumExperiencePoints = 1,
                StreetNumber = 1,
                StreetName = "test1",
                ApartmentNumber = "test1",
                CityName = "test1",
                SubdivisionName = "test1",
                CountryName = "test1",
                PostalCode = 1,
                PortraitPath = "test1",
                PortraitId = 1
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Content = new StringContent(JsonConvert.SerializeObject(applicationUserCreate))
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.CreateApplicationUser(applicationUserCreate);

            //assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(applicationUserCreate.Id, response.Data.Id);
            Assert.AreEqual(applicationUserCreate.UserName, response.Data.UserName);
            Assert.AreEqual(applicationUserCreate.FirstName, response.Data.FirstName);
            Assert.AreEqual(applicationUserCreate.LastName, response.Data.LastName);
            Assert.AreEqual(applicationUserCreate.IsFemale, response.Data.IsFemale);
            Assert.AreEqual(applicationUserCreate.Birthday, response.Data.Birthday);
            Assert.AreEqual(applicationUserCreate.PrefersMetric, response.Data.PrefersMetric);
            Assert.AreEqual(applicationUserCreate.ProfileDescription, response.Data.ProfileDescription);
            Assert.AreEqual(applicationUserCreate.NumExperiencePoints, response.Data.NumExperiencePoints);
            Assert.AreEqual(applicationUserCreate.StreetNumber, response.Data.StreetNumber);
            Assert.AreEqual(applicationUserCreate.StreetName, response.Data.StreetName);
            Assert.AreEqual(applicationUserCreate.ApartmentNumber, response.Data.ApartmentNumber);
            Assert.AreEqual(applicationUserCreate.CityName, response.Data.CityName);
            Assert.AreEqual(applicationUserCreate.SubdivisionName, response.Data.SubdivisionName);
            Assert.AreEqual(applicationUserCreate.CountryName, response.Data.CountryName);
            Assert.AreEqual(applicationUserCreate.PostalCode, response.Data.PostalCode);
            Assert.AreEqual(applicationUserCreate.PortraitPath, response.Data.PortraitPath);
        }

        [Test]
        public async Task createapplicationuser_shouldnotcreateuser_400()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            ApplicationUserDto applicationUserCreate = new ApplicationUserDto()
            {
                Id = "1",
                UserName = "test1",
                FirstName = "test1",
                LastName = "test1",
                IsFemale = true,
                Birthday = DateTime.Now,
                PrefersMetric = true,
                ProfileDescription = "test1",
                NumExperiencePoints = 1,
                StreetNumber = 1,
                StreetName = "test1",
                ApartmentNumber = "test1",
                CityName = "test1",
                SubdivisionName = "test1",
                CountryName = "test1",
                PostalCode = 1,
                PortraitPath = "test1",
                PortraitId = 1
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.CreateApplicationUser(applicationUserCreate);

            //assert
            Assert.IsFalse(response.Succeeded);
        }

        [Test]
        public async Task updateapplicationuser_shouldupdateuser()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            ApplicationUserDto applicationUser = new ApplicationUserDto()
            {
                Id = "1",
                UserName = "test1",
                FirstName = "test1",
                LastName = "test1",
                IsFemale = true,
                Birthday = DateTime.Now,
                PrefersMetric = true,
                ProfileDescription = "test1",
                NumExperiencePoints = 1,
                StreetNumber = 1,
                StreetName = "test1",
                ApartmentNumber = "test1",
                CityName = "test1",
                SubdivisionName = "test1",
                CountryName = "test1",
                PostalCode = 1,
                PortraitPath = "test1",
                PortraitId = 1
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(applicationUser))
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.UpdateApplicationUser(applicationUser.Id, applicationUser);

            //assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(applicationUser.Id, response.Data.Id);
            Assert.AreEqual(applicationUser.UserName, response.Data.UserName);
            Assert.AreEqual(applicationUser.FirstName, response.Data.FirstName);
            Assert.AreEqual(applicationUser.LastName, response.Data.LastName);
            Assert.AreEqual(applicationUser.IsFemale, response.Data.IsFemale);
            Assert.AreEqual(applicationUser.Birthday, response.Data.Birthday);
            Assert.AreEqual(applicationUser.PrefersMetric, response.Data.PrefersMetric);
            Assert.AreEqual(applicationUser.ProfileDescription, response.Data.ProfileDescription);
            Assert.AreEqual(applicationUser.NumExperiencePoints, response.Data.NumExperiencePoints);
            Assert.AreEqual(applicationUser.StreetNumber, response.Data.StreetNumber);
            Assert.AreEqual(applicationUser.StreetName, response.Data.StreetName);
            Assert.AreEqual(applicationUser.ApartmentNumber, response.Data.ApartmentNumber);
            Assert.AreEqual(applicationUser.CityName, response.Data.CityName);
            Assert.AreEqual(applicationUser.SubdivisionName, response.Data.SubdivisionName);
            Assert.AreEqual(applicationUser.CountryName, response.Data.CountryName);
            Assert.AreEqual(applicationUser.PostalCode, response.Data.PostalCode);
            Assert.AreEqual(applicationUser.PortraitPath, response.Data.PortraitPath);
            Assert.AreEqual(applicationUser.PortraitId, response.Data.PortraitId);
        }

        [Test]
        public async Task updateapplicationuser_shouldnotupdateuser_400()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            ApplicationUserDto applicationUser = new ApplicationUserDto()
            {
                Id = "1",
                UserName = "test1",
                FirstName = "test1",
                LastName = "test1",
                IsFemale = true,
                Birthday = DateTime.Now,
                PrefersMetric = true,
                ProfileDescription = "test1",
                NumExperiencePoints = 1,
                StreetNumber = 1,
                StreetName = "test1",
                ApartmentNumber = "test1",
                CityName = "test1",
                SubdivisionName = "test1",
                CountryName = "test1",
                PostalCode = 1,
                PortraitPath = "test1",
                PortraitId = 1
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.UpdateApplicationUser(applicationUser.Id, applicationUser);

            //assert
            Assert.IsFalse(response.Succeeded);
        }

        [Test]
        public async Task deleteapplicationuser_shoulddeleteuser()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            ApplicationUserDto applicationUser = new ApplicationUserDto()
            {
                Id = "1",
                UserName = "test1",
                FirstName = "test1",
                LastName = "test1",
                IsFemale = true,
                Birthday = DateTime.Now,
                PrefersMetric = true,
                ProfileDescription = "test1",
                NumExperiencePoints = 1,
                StreetNumber = 1,
                StreetName = "test1",
                ApartmentNumber = "test1",
                CityName = "test1",
                SubdivisionName = "test1",
                CountryName = "test1",
                PostalCode = 1,
                PortraitPath = "test1",
                PortraitId = 1
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(applicationUser))
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.DeleteApplicationUser(applicationUser.Id);

            //assert
            Assert.IsTrue(response.Succeeded);
            Assert.AreEqual(applicationUser.Id, response.Data.Id);
            Assert.AreEqual(applicationUser.UserName, response.Data.UserName);
            Assert.AreEqual(applicationUser.FirstName, response.Data.FirstName);
            Assert.AreEqual(applicationUser.LastName, response.Data.LastName);
            Assert.AreEqual(applicationUser.IsFemale, response.Data.IsFemale);
            Assert.AreEqual(applicationUser.Birthday, response.Data.Birthday);
            Assert.AreEqual(applicationUser.PrefersMetric, response.Data.PrefersMetric);
            Assert.AreEqual(applicationUser.ProfileDescription, response.Data.ProfileDescription);
            Assert.AreEqual(applicationUser.NumExperiencePoints, response.Data.NumExperiencePoints);
            Assert.AreEqual(applicationUser.StreetNumber, response.Data.StreetNumber);
            Assert.AreEqual(applicationUser.StreetName, response.Data.StreetName);
            Assert.AreEqual(applicationUser.ApartmentNumber, response.Data.ApartmentNumber);
            Assert.AreEqual(applicationUser.CityName, response.Data.CityName);
            Assert.AreEqual(applicationUser.SubdivisionName, response.Data.SubdivisionName);
            Assert.AreEqual(applicationUser.CountryName, response.Data.CountryName);
            Assert.AreEqual(applicationUser.PostalCode, response.Data.PostalCode);
            Assert.AreEqual(applicationUser.PortraitPath, response.Data.PortraitPath);
        }

        [Test]
        public async Task deleteapplicationuser_shouldnotdeleteuser_400()
        {
            // Arrange
            var mockHttp = new Mock<HttpMessageHandler>();

            ApplicationUserDto applicationUser = new ApplicationUserDto()
            {
                Id = "1",
                UserName = "test1",
                FirstName = "test1",
                LastName = "test1",
                IsFemale = true,
                Birthday = DateTime.Now,
                PrefersMetric = true,
                ProfileDescription = "test1",
                NumExperiencePoints = 1,
                StreetNumber = 1,
                StreetName = "test1",
                ApartmentNumber = "test1",
                CityName = "test1",
                SubdivisionName = "test1",
                CountryName = "test1",
                PostalCode = 1,
                PortraitPath = "test1",
                PortraitId = 1
            };

            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHttp.Object);
            httpClient.BaseAddress = new Uri("https://localhost:7127/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token");

            //act
            IApplicationUserHttpService applicationUserHttpService = new ApplicationUserHttpService(httpClient);
            var response = await applicationUserHttpService.DeleteApplicationUser(applicationUser.Id);

            //assert
            Assert.IsFalse(response.Succeeded);
        }
    }
}
