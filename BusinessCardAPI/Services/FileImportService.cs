using BusinessCardAPI.Exceptions;
using BusinessCardAPI.Models;
using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Services.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace BusinessCardAPI.Services
{

    public class FileImportService : IFileImportService
    {
        private readonly IBusinessCardService _businessCardService;

        public FileImportService(IBusinessCardService businessCardService)
        {
            _businessCardService = businessCardService;
        }


        public async Task<IEnumerable<BusinessCardCreateDto>> ParseCsv(Stream fileStream)
        {
            try
            {
                using var reader = new StreamReader(fileStream);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    TrimOptions = TrimOptions.Trim,
                    MissingFieldFound = null
                };

                using var csv = new CsvReader(reader, config);
                var records = csv.GetRecords<BusinessCardCreateDto>().ToList();

                return records.Select(r => new BusinessCardCreateDto
                {
                    Name = r.Name,
                    Gender = r.Gender,
                    DateOfBirth = r.DateOfBirth,
                    Email = r.Email,
                    Phone = r.Phone,
                    Address = r.Address ?? string.Empty,
                    Photo = r.Photo
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new CsvParsingException("Failed to parse CSV file", ex);
            }
        }

        public async Task<IEnumerable<BusinessCardCreateDto>> ParseXml(Stream fileStream)
        {
            try
            {
                using var reader = new StreamReader(fileStream);
                var xmlContent = await reader.ReadToEndAsync();
                var doc = XDocument.Parse(xmlContent);

                var cards = doc.Descendants("BusinessCard").Select(card => new BusinessCardCreateDto
                {
                    Name = card.Element("Name")?.Value ?? string.Empty,
                    Gender = card.Element("Gender")?.Value ?? string.Empty,
                    DateOfBirth = DateTime.Parse(card.Element("DateOfBirth")?.Value ?? DateTime.MinValue.ToString()),
                    Email = card.Element("Email")?.Value ?? string.Empty,
                    Phone = card.Element("Phone")?.Value ?? string.Empty,
                    Address = card.Element("Address")?.Value ?? string.Empty,
                    Photo = card.Element("Photo")?.Value
                }).ToList();

                return cards;
            }
            catch (Exception ex)
            {
                throw new XmlParsingException("Failed to parse XML file", ex);
            }
        }
        public async Task<IEnumerable<BusinessCard>> ImportCards(IEnumerable<BusinessCardCreateDto> cards)
        {
            var result = new List<BusinessCard>();

            foreach (var dto in cards)
            {
                try
                {
                    var created = await _businessCardService.CreateCard(dto);
                    result.Add(created);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error importing card for {dto.Name}: {ex.Message}");
                }
            }

            return result;
        }
    }
}