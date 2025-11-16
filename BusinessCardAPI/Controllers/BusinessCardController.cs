using BusinessCardAPI.Interfaces.Services;
using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Resources;

namespace BusinessCardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessCardController : ControllerBase
    {
        private readonly IBusinessCardService _service;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IFileImportService _fileService;
        private readonly IExportService _exportService;

        public BusinessCardController(IBusinessCardService service, IStringLocalizer<SharedResource> localizer, IFileImportService fileService, IExportService exportService)
        {
            _service = service;
            _localizer = localizer;
            _fileService = fileService;
            _exportService = exportService;
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<ActionResult<PagedResult<BusinessCard>>> GetAll([FromQuery] int pageNumber = 1 , [FromQuery] int pageSize = 10)
        {
            var cards = await _service.GetAllCards(pageNumber ,  pageSize);
            return Ok(cards);
        }

        [Route("GetById/{id}")]
        [HttpGet]
        public async Task<ActionResult<BusinessCard>> GetById(int id)
        {
            var card = await _service.GetCardById(id);
            if (card == null)
            {
                return NotFound(new { message = _localizer["NotFound", id] });
            }
            return Ok(card);
        }

        [Route("Create")]
        [HttpPost]
        public async Task<ActionResult<BusinessCard>> Create([FromBody] BusinessCardCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _service.CreateCard(dto);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteCard(id);
            if (!result)
            {
                return NotFound(new { message = _localizer["NotFound", id] });
            }
            return NoContent();
        }

        [Route("Filter")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessCard>>> Filter(
            [FromQuery] BusinessCardFilterDto filter)
        {
            var cards = await _service.FilterCards(filter);
            return Ok(cards);
        }

        [Route("ImportCsv")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BusinessCard>>> ImportCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = _localizer["NoFileUploaded"] });
            }

            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = _localizer["FileMustBeCsv"] });
            }

            try
            {
                using var stream = file.OpenReadStream();
                var cards = await _fileService.ParseCsv(stream);
                await _fileService.ImportCards(cards);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = _localizer["ImportError"], error = ex.Message });
            }
        }

        [Route("ImportXml")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<BusinessCard>>> ImportXml(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = _localizer["NoFileUploaded"] });
            }

            if (!file.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = _localizer["FileMustBeXml"] });
            }

            try
            {
                using var stream = file.OpenReadStream();
                var cards = await _fileService.ParseXml(stream);
                await _fileService.ImportCards(cards);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = _localizer["ImportError"], error = ex.Message });
            }
        }

        [Route("ExportCsv")]
        [HttpGet]
        public async Task<IActionResult> ExportCsv([FromQuery] int? id , [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            List<BusinessCard> cards = new List<BusinessCard>();
            if (id.HasValue)
            {
                var card = await _service.GetCardById(id.Value);
                if(card is not null)
                    cards.Add(card);
            }
            else
            {
                var dbCards = await _service.GetAllCards(pageNumber, pageSize);
                if (dbCards is not null)
                    if(dbCards.Cards.Any())
                        cards.AddRange(dbCards.Cards);
            }
            var csvData = await _exportService.ExportToCsv(cards);
            return File(csvData, "text/csv", $"business-cards-{DateTime.Now:yyyyMMdd}.csv");
        }

        [Route("ExportXml")]
        [HttpGet]
        public async Task<IActionResult> ExportXml([FromQuery]  int? id , [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            List<BusinessCard> cards = new List<BusinessCard>();
            if (id.HasValue)
            {
                var card = await _service.GetCardById(id.Value);
                if (card is not null)
                    cards.Add(card);
            }
            else
            {
                var dbCards = await _service.GetAllCards(pageNumber , pageSize);
                if (dbCards is not null)
                    if (dbCards.Cards.Any())
                        cards.AddRange(dbCards.Cards);
            }
            var xmlData = await _exportService.ExportToXml(cards);
            return File(xmlData, "application/xml", $"business-cards-{DateTime.Now:yyyyMMdd}.xml");
        }
    }
}