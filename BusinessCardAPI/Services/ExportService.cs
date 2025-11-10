using BusinessCardAPI.Exceptions;
using BusinessCardAPI.Interfaces.Services;
using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;
using CsvHelper;
using Microsoft.Extensions.Localization;
using Resources;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace BusinessCardAPI.Services.Interfaces
{
    public class ExportService : IExportService
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
     public  ExportService(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public async Task<byte[]> ExportToCsv(IEnumerable<BusinessCard> cards)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                var records = cards.Select(c => new BusinessCardDto
                {
                    Name = c.Name,
                    Gender = c.Gender,
                    DateOfBirth = c.DateOfBirth,
                    Email = c.Email,
                    Phone = c.Phone,
                    Address = c.Address,
                    Photo = c.Photo
                });

                await csv.WriteRecordsAsync(records);
                await writer.FlushAsync();
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                throw new CsvExportException(_localizer["FailedToExportToCsv"]);
            }
        }

        public async Task<byte[]> ExportToXml(IEnumerable<BusinessCard> cards)
        {
            try
            {
                var doc = new XDocument(
                    new XElement("BusinessCards",
                        cards.Select(c => new XElement("BusinessCard",
                            new XElement("Id", c.Id),
                            new XElement("Name", c.Name),
                            new XElement("Gender", c.Gender),
                            new XElement("DateOfBirth", c.DateOfBirth.ToString("yyyy-MM-dd")),
                            new XElement("Email", c.Email),
                            new XElement("Phone", c.Phone),
                            new XElement("Address", c.Address),
                            new XElement("Photo", c.Photo ?? string.Empty)
                        ))
                    )
                );

                using var memoryStream = new MemoryStream();
                await Task.Run(() => doc.Save(memoryStream));
                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                throw new XmlExportException(_localizer["FailedToExportToXML"]);
            }
        }

    }
}
