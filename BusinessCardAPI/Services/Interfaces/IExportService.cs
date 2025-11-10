using BusinessCardAPI.Models;

namespace BusinessCardAPI.Services.Interfaces
{
    public interface IExportService
    {

        Task<byte[]> ExportToCsv(IEnumerable<BusinessCard> cards);
        Task<byte[]> ExportToXml(IEnumerable<BusinessCard> cards);
    }
}
