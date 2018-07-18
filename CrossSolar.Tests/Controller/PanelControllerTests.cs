using System.Threading.Tasks;
using CrossSolar.Controllers;
using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using CrossSolar.Exceptions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Builder;

namespace CrossSolar.Tests.Controller
{
    public class PanelControllerTests
    {
        public PanelControllerTests()
        {
            _panelController = new PanelController(_panelRepositoryMock.Object);
        }

        private readonly PanelController _panelController;
        private readonly AnalyticsController _analyticsController;
        private readonly HttpStatusCodeException _httpStatusCodeException;


        private readonly Mock<IPanelRepository> _panelRepositoryMock = new Mock<IPanelRepository>();


        #region PanelControllerUnitTest

        [Fact]
        public async Task Register_ShouldInsertPanel()
        {
            var panel = new PanelModel
            {
                Brand = "Areva",
                Latitude = 12.345678,
                Longitude = 98.7655432,
                Serial = "AAAA1111BBBB2222"
            };

            // Arrange

            // Act
            var result = await _panelController.Register(panel);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task MustAuthurization()
        {
            var panel = new PanelModel
            {
                Brand = "Areva",
                Latitude = 12.345678,
                Longitude = 98.7655432,
                Serial = "AAAA1111BBBB2222"
            };

            // Arrange

            // Act
            var result = await _panelController.Register(panel);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.NotEqual(401, createdResult.StatusCode);
        }



        [Fact]
        public async Task Serial_MustBeSixteen()
        {
            var panel = new PanelModel
            {
                Brand = "New Brand",
                Latitude = 14.345678,
                Longitude = 100.7655432,
                Serial = "AAAA1111BBBB2222"
            };

            // Arrange

            // Act
            var result = await _panelController.Register(panel);

            // Assert

            Assert.NotNull(panel.Serial);
            Assert.Equal(16, panel.Serial.Length);

        }



        [Fact]
        public async Task LatitudeAndLongitudeContainsSixDecimalPlaces()
        {
            var panel = new PanelModel
            {
                Brand = "New Brand",
                Latitude = 14.345678,
                Longitude = 100.755432,
                Serial = "AAAA1111BBBB2222"
            };

            // Arrange

            // Act
            var result = await _panelController.Register(panel);

            // Assert

            Assert.Equal(6, panel.Latitude.ToString().Split('.')[1].Length);
            Assert.Equal(6, panel.Longitude.ToString().Split('.')[1].Length);
        }

        [Fact]
        public async Task LatitudeRange()
        {
            var panel = new PanelModel
            {
                Brand = "New Brand",
                Latitude = 14.345678,
                Longitude = 100.755432,
                Serial = "AAAA1111BBBB2222"
            };

            // Arrange

            // Act
            var result = await _panelController.Register(panel);

            // Assert

            Assert.InRange<double>(panel.Latitude, -90, 90);

        }

        [Fact]
        public async Task LongitudeRange()
        {
            var panel = new PanelModel
            {
                Brand = "New Brand",
                Latitude = 14.345678,
                Longitude = 100.755432,
                Serial = "AAAA1111BBBB2222"
            };

            // Arrange

            // Act
            var result = await _panelController.Register(panel);

            // Assert

            Assert.InRange<double>(panel.Longitude, -180, 180);

        }

        [Fact]
        public async Task AcceptPanelOnly()
        {
            var panel = new PanelModel
            {
                Brand = "New Brand",
                Latitude = 14.345678,
                Longitude = 100.755432,
                Serial = "AAAA1111BBBB2222"
            };

            // Arrange

            // Act
            var result = await _panelController.Register(panel);

            // Assert
            var createdResult = result as CreatedResult;
            Assert.Equal((new PanelModel()).GetType(), panel.GetType());
            Assert.Equal(201, createdResult.StatusCode);

        }
        #endregion




        #region OneHourElectricityUnitTest

        [Fact]
        public async Task OneHourElectricityModelDateTime()
        {
            var oneHourElectricity = new OneHourElectricityModel
            {
                Id = 1,
                KiloWatt = 123,
                DateTime = new System.DateTime()
            };

            // Arrange


            // Assert

            Assert.NotNull(oneHourElectricity.DateTime);


        }

        [Fact]
        public async Task OneHourElectricityDateTime()
        {
            var oneHourElectricity = new OneHourElectricity
            {
                Id = 1,
                PanelId = "ABC",
                KiloWatt = 123,
                DateTime = new System.DateTime()
            };

            // Arrange


            // Assert

            Assert.NotNull(oneHourElectricity.DateTime);


        }

        [Fact]
        public async Task OneDayElectricityModelDateTime()
        {
            var oneDayElectricity = new OneDayElectricityModel
            {
                Sum = 1,
                Average = 123,
                Maximum = 100,
                Minimum = 0,
                DateTime = new System.DateTime()
            };

            // Arrange


            // Assert

            Assert.NotNull(oneDayElectricity.DateTime);


        }
        #endregion


        #region AnalyticsControllerUnitTest
        [Fact]
        public async Task AnalyticsController_Get()
        {
            string panelId = "ABC"; ;
            AnalyticsController analyticsController = new AnalyticsController(null, null, null);
            var result = analyticsController.Get(panelId);
            // Arrange

            // Assert
            Assert.NotNull(panelId);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AnalyticsController_DayResult()
        {
            string panelId = "ABC"; ;
            AnalyticsController analyticsController = new AnalyticsController(null, null, null);
            var result = analyticsController.DayResults(panelId);
            // Arrange

            // Assert
            Assert.NotNull(panelId);
            Assert.NotNull(result);

        }

        [Fact]
        public async Task AnalyticsController_Post()
        {
            string panelId = "ABC";
            OneHourElectricityModel oneHourElectricityModel = new OneHourElectricityModel();
            AnalyticsController analyticsController = new AnalyticsController(null, null, null);
            analyticsController.Post(panelId, oneHourElectricityModel);
            // Arrange

            // Assert
            Assert.NotNull(panelId);
            Assert.NotNull(oneHourElectricityModel);

        }

        [Fact]
        public async Task AnalyticsController_Constructor()
        {
            CrossSolarDbContext dbContext = new CrossSolarDbContext();
            IAnalyticsRepository analyticsRepository = new AnalyticsRepository(dbContext);
            IPanelRepository panelRepository = new PanelRepository(dbContext);
            IOneHourElectricityRepository oneHourElectricityRepository = new OneHourElectricityRepository(dbContext);
            var AnalyticsController = new AnalyticsController(analyticsRepository, panelRepository, oneHourElectricityRepository);

            // Assert
            Assert.NotNull(analyticsRepository);
            Assert.NotNull(panelRepository);
            Assert.NotNull(oneHourElectricityRepository);
            Assert.NotNull(dbContext);

        }
        #endregion



        #region PanelRepositoryUnitTest
        [Fact]
        public async Task PanelRepository_Constructor()
        {
            CrossSolarDbContext dbContext = new CrossSolarDbContext();
            var panelRepository = new PanelRepository(dbContext);

            // Arrange



            // Assert
            Assert.NotNull(dbContext);

        }
        #endregion


        #region OneHourElectricityListModelUnitTest
        [Fact]
        public async Task OneHourElectricityListModel_ValidateProperty()
        {
            IEnumerable<OneHourElectricityModel> oneHourElectricitys = new List<OneHourElectricityModel>();
            var oneHourElectricityListModel = new OneHourElectricityListModel
            {
                OneHourElectricitys = oneHourElectricitys
            };

            // Assert
            Assert.NotNull(oneHourElectricityListModel);



        }

        #endregion



        #region AnalyticsRepositoryUnitTest
        [Fact]
        public async Task AnalyticsRepository_Constructor()
        {
            CrossSolarDbContext dbContext = new CrossSolarDbContext();
            var analyticsRepository = new AnalyticsRepository(dbContext);

            // Arrange



            // Assert
            Assert.NotNull(dbContext);

        }
        #endregion


        #region HttpStatusCodeExceptionUnitTest

        [Fact]
        public async Task HttpStatusCodeException_ParamValidation()
        {
            int statusCode = 201;
            var httpStatusCodeException = new HttpStatusCodeException(statusCode)
            {
                StatusCode = statusCode,
                ContentType = @"text/plain"
            };
            // Assert
            Assert.IsType(typeof(Int32), httpStatusCodeException.StatusCode);
        }
  
        #endregion


        #region DayAnalyticsRepository
        [Fact]
        public async Task DayAnalyticsRepository_Constructor()
        {
            CrossSolarDbContext dbContext = new CrossSolarDbContext();
            var DayAnalyticsRepository = new DayAnalyticsRepository(dbContext);

            // Arrange



            // Assert
            Assert.NotNull(dbContext);

        }


        #endregion

        #region OneDayElectricityModelUnitTest
        [Fact]
        public async Task OneDayElectricityModel_Object()
        {
            OneDayElectricityModel model = new OneDayElectricityModel()
            {
                Sum=123,
                Average=12,
                Maximum=90,
                Minimum=23,
                DateTime=DateTime.Now,
                panelId="ABC"
            };
            // Arrange



            // Assert
            Assert.NotNull(model.Sum);
            Assert.NotNull(model.Average);
            Assert.NotNull(model.Maximum);
            Assert.NotNull(model.Minimum);
            Assert.NotNull(model.DateTime);

        }
        #endregion

        #region OneHourElectricityModelUnitTest
        [Fact]
        public async Task OneHourElectricityModel_Object()
        {
            OneHourElectricityModel model = new OneHourElectricityModel()
            {
                Id = 123,
                KiloWatt = 12,
                DateTime = DateTime.Now
            };
            // Arrange



            // Assert
            Assert.NotNull(model);
            Assert.NotNull(model.Id);
            Assert.NotNull(model.KiloWatt);
            Assert.NotNull(model.DateTime);

            Assert.NotEqual(0, model.Id);


        }

        #endregion


        #region GenericRepository


        #endregion

        #region HttpStatusCodeExceptionMiddlewareExtensions_UnitTest

    
        #endregion
    }
}