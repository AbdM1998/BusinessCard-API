using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;

namespace BusinessCardAPI.Interfaces.Services
{
    public interface IFileImportService
    {
        Task<IEnumerable<BusinessCardCreateDto>> ParseCsv(Stream fileStream);
        Task<IEnumerable<BusinessCardCreateDto>> ParseXml(Stream fileStream);
        Task ImportCards(IEnumerable<BusinessCardCreateDto> cards);

    }

}
