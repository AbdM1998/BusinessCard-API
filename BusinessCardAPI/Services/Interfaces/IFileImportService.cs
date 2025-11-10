using BusinessCardAPI.Models;
using BusinessCardAPI.Models.DTOs;

namespace BusinessCardAPI.Services.Interfaces
{
    public interface IFileImportService
    {
        Task<IEnumerable<BusinessCardCreateDto>> ParseCsv(Stream fileStream);
        Task<IEnumerable<BusinessCardCreateDto>> ParseXml(Stream fileStream);
        Task<IEnumerable<BusinessCard>> ImportCards(IEnumerable<BusinessCardCreateDto> cards);

    }

}
