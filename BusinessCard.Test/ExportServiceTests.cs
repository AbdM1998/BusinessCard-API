using BusinessCardAPI.Interfaces.Services;
using BusinessCardAPI.Models.Entities;
using BusinessCardAPI.Services.Interfaces;
using FakeItEasy;
using Microsoft.Extensions.Localization;
using Resources;

namespace BusinessCardAPI.ServiceTests
{
    public class ExportServiceTests
    {
        private readonly IExportService _exportService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ExportServiceTests()
        {
            _localizer = A.Fake<IStringLocalizer<SharedResource>>();
            _exportService = new ExportService (_localizer);
        }

        [Fact]
        public async Task ExportCardsToCsv_ShouldReturnFilePath()
        {
            // Arrange
            var expectedCardList = new List<BusinessCard>
            {
                new BusinessCard("Abd", "Male", new DateTime(1998, 7, 30), "abd@test.com", "00962788456446", "Test", null, DateTime.UtcNow)
            };

            // Act
            var result = await _exportService.ExportToCsv(expectedCardList);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var csv = System.Text.Encoding.UTF8.GetString(result);
            Assert.Contains("Abd", csv);
            Assert.Contains("Male", csv);
            Assert.Contains("abd@test.com", csv);
        }
    }
}
