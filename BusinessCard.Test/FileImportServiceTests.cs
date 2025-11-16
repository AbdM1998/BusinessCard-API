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
        public async Task ParseCsv_ValidFile_ReturnCardList()
        {
            // Arrange
            var file = new FileStream("TestFiles\\business_cards.csv", FileMode.Open, FileAccess.Read);
            var expectedCardList = new List<BusinessCardCreateDto>()
                {
                    new BusinessCardCreateDto { Name = "abd", Gender = "Male", DateOfBirth = new DateTime(1222, 7, 30), Email = "abdalruhman.mustafa@gmail.com", Phone = "0078884564", Address = "المشيرفة" },
                    new BusinessCardCreateDto { Name = "Jane Smith", Gender = "Female", DateOfBirth = new DateTime(1985, 5, 20), Email = "jane.smith@example.com", Phone = "+0987654321", Address = "456 Oak Avenue, Los Angeles, CA 90001" },
                    new BusinessCardCreateDto { Name = "Emily Davis", Gender = "Female", DateOfBirth = new DateTime(1992, 3, 25), Email = "emily.davis@example.com", Phone = "+5544332211", Address = "321 Elm Street, Houston, TX 77001" },
                    new BusinessCardCreateDto { Name = "David Wilson", Gender = "Male", DateOfBirth = new DateTime(1987, 7, 10), Email = "david.wilson@example.com", Phone = "+9988776655", Address = "654 Maple Drive, Phoenix, AZ 85001" }
                };

            // Act
            var actualCardList = await _fileService.ParseCsv(file);

            // Assert
            Assert.NotNull(actualCardList);
            Assert.NotEmpty(actualCardList);
            Assert.Equal(expectedCardList.Count, actualCardList.Count());

            var actualList = actualCardList.ToList();

            for (int i = 0; i < expectedCardList.Count; i++)
            {
                Assert.Equal(expectedCardList[i].Name, actualList[i].Name);
                Assert.Equal(expectedCardList[i].Gender, actualList[i].Gender);
                Assert.Equal(expectedCardList[i].DateOfBirth, actualList[i].DateOfBirth);
                Assert.Equal(expectedCardList[i].Email, actualList[i].Email);
                Assert.Equal(expectedCardList[i].Phone, actualList[i].Phone);
                Assert.Equal(expectedCardList[i].Address, actualList[i].Address);
            }
        }
        [Fact]
        public async Task ParseXML_ValidFile_ReturnCardList()
        {
            // Arrange
            var file = new FileStream("TestFiles\\business_cards.xml", FileMode.Open, FileAccess.Read);
            var expectedCardList = new List<BusinessCardCreateDto>()
                {
                    new BusinessCardCreateDto { Name = "abd", Gender = "Male", DateOfBirth = new DateTime(1222, 7, 30), Email = "abdalruhman.mustafa@gmail.com", Phone = "0078884564", Address = "المشيرفة" },
                    new BusinessCardCreateDto { Name = "Jane Smith", Gender = "Female", DateOfBirth = new DateTime(1985, 5, 20), Email = "jane.smith@example.com", Phone = "+0987654321", Address = "456 Oak Avenue, Los Angeles, CA 90001" },
                    new BusinessCardCreateDto { Name = "Emily Davis", Gender = "Female", DateOfBirth = new DateTime(1992, 3, 25), Email = "emily.davis@example.com", Phone = "+5544332211", Address = "321 Elm Street, Houston, TX 77001" },
                    new BusinessCardCreateDto { Name = "David Wilson", Gender = "Male", DateOfBirth = new DateTime(1987, 7, 10), Email = "david.wilson@example.com", Phone = "+9988776655", Address = "654 Maple Drive, Phoenix, AZ 85001" }
                };

            // Act
            var actualCardList = await _fileService.ParseXml(file);

            // Assert
            Assert.NotNull(actualCardList);
            Assert.NotEmpty(actualCardList);

            var actualList = actualCardList.ToList();
            for (int i = 0; i < expectedCardList.Count; i++)
            {
                Assert.Equal(expectedCardList[i].Name, actualList[i].Name);
                Assert.Equal(expectedCardList[i].Gender, actualList[i].Gender);
                Assert.Equal(expectedCardList[i].DateOfBirth, actualList[i].DateOfBirth);
                Assert.Equal(expectedCardList[i].Email, actualList[i].Email);
                Assert.Equal(expectedCardList[i].Phone, actualList[i].Phone);
                Assert.Equal(expectedCardList[i].Address, actualList[i].Address);
            }
        }

        [Fact]
        public async Task ImportCards_ValidFile_ShouldPass()
        {
            // Arrange
            A.CallTo(() => _service.CreateBulk(A<List<BusinessCardCreateDto>>._))
                   .Returns(Task.CompletedTask);

            // Act
            IEnumerable<BusinessCardCreateDto> expectedCardList = new List<BusinessCardCreateDto>()
                {
                    new BusinessCardCreateDto { Name = "Abd", Gender = "Male", DateOfBirth = new DateTime(1998, 7, 30), Email = "Abd@gmail.com", Phone = "00962788456446", Address = "Test" },
                    new BusinessCardCreateDto { Name = "Moahmmed", Gender = "Male", DateOfBirth = new DateTime(1998, 7, 30), Email = "Moahmmed@gmail.com", Phone = "00962788456446", Address = "Test" }
                };

            await _fileService.ImportCards(expectedCardList);

            // Assert
            A.CallTo(() => _service.CreateBulk(A<List<BusinessCardCreateDto>>.That.Matches(cards =>
                cards.Count == 2 &&
                cards.ElementAt(0).Equals(expectedCardList.ElementAt(0)) 
                &&
                cards.ElementAt(1).Equals(expectedCardList.ElementAt(1))
            ))).MustHaveHappenedOnceExactly();
        }
    }
}
