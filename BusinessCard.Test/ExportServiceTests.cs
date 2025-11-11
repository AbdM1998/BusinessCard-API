using BusinessCardAPI.Interfaces.Services;
using FakeItEasy;
 
namespace BusinessCard.Test
{
    public class ExportServiceTests
    {
        private readonly IExportService _exportService;

        public ExportServiceTests()
        {
            _exportService = A.Fake<IExportService>();
        }

        [Fact]
        public async Task ExportCardsToCsv_ShouldReturnFilePath()
        {
            // Arrange
            var cards = new List<BusinessCardAPI.Models.Entities.BusinessCard>
        {
            new BusinessCardAPI.Models.Entities.BusinessCard("Abd", "Male", new DateTime(1998, 7, 30), "abd@test.com", "00962788456446", "Test", null, DateTime.UtcNow)
        };

            var fakeFileBytes = new byte[] { 1, 2, 3, 4 };

            A.CallTo(() => _exportService.ExportToCsv(cards)).Returns(Task.FromResult(fakeFileBytes));

            // Act
            var result = await _exportService.ExportToCsv(cards);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(fakeFileBytes, result);
        }
    }
}
