using BusinessCardAPI.Models.Entities;

namespace BusinessCardAPI.Interfaces.Services
{
    public interface IExportService
    {
        Task<byte[]> ExportToCsv(IEnumerable<BusinessCard> cards);
        Task<byte[]> ExportToXml(IEnumerable<BusinessCard> cards);
    }
}
