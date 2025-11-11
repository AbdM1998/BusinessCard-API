using BusinessCardAPI.Interfaces.Services;
using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;
using FakeItEasy;
using System.Net;
using System.Numerics;
using System.Reflection;
using Xunit;
namespace BusinessCard.Test
{
    public class FileImportServiceTests
    {
        private readonly IFileImportService _fileService;

        public FileImportServiceTests()
        {
            _fileService = A.Fake<IFileImportService>();
        }

        [Fact]
        public async Task ImportCards_ValidFile_ReturnCardList()
        {
            // Arrange
            var cards = new List<BusinessCardAPI.Models.Entities.BusinessCard>
            {
                new BusinessCardAPI.Models.Entities.BusinessCard("Abd" , "Male" , new DateTime(1998, 7, 30),"Abd@gmail.com" , "00962788456446" , "Test",null ,DateTime.UtcNow ),
            };
            var CardDto = new List<BusinessCardCreateDto>
            {
                new BusinessCardCreateDto { Name = "Abd", Gender = "Male", DateOfBirth = new DateTime(1998, 7, 30), Email = "Abd@gmail.com", Phone = "00962788456446", Address = "Test" }
            };


            A.CallTo(() => _fileService.ImportCards(CardDto)).Returns(Task.FromResult((IEnumerable<BusinessCardAPI.Models.Entities.BusinessCard>)cards));

            // Act
            var result = await _fileService.ImportCards(CardDto);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Abd", result.First().Name);
        }
    }
}
