using BusinessCardAPI.Interfaces.Services;
using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;
using BusinessCardAPI.Services;
using FakeItEasy;
using System.Net;
using System.Numerics;
using System.Reflection;
using Xunit;
namespace BusinessCardAPI.ServiceTests
{
    public class FileImportServiceTests
    {
        private readonly IFileImportService _fileService;
        private readonly IBusinessCardService _service;

        public FileImportServiceTests()
        {
            _service = A.Fake<IBusinessCardService>();
            _fileService = new FileImportService(_service);
        }

        [Fact]
        public async Task ImportCards_ValidFile_ReturnCardList()
        {
            // Arrange
            var file = new FileStream("TestFiles\\business_cards.csv", FileMode.Open, FileAccess.Read);
            var expectedCardList = await _fileService.ParseCsv(file);

            A.CallTo(() => _service.CreateCard(A<BusinessCardCreateDto>._))
                 .ReturnsLazily((BusinessCardCreateDto dto) => Task.FromResult(
                     new BusinessCard(dto.Name, dto.Gender, dto.DateOfBirth, dto.Email, dto.Phone, dto.Address, dto.Photo, DateTime.UtcNow)
                 ));
            // Act
            var actualCardList = await _fileService.ImportCards(expectedCardList);

            // Assert
            Assert.NotNull(actualCardList);
            Assert.NotEmpty(actualCardList);
            Assert.Equal(expectedCardList.Count(), actualCardList.Count());
            Assert.Equal(expectedCardList.First().Name, actualCardList.First().Name);
            Assert.Equal(expectedCardList.First().Email, actualCardList.First().Email);
        }
    }
}
