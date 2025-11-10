using BusinessCardAPI.Models;
using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Services.Interfaces;
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

        public BusinessCardController(IBusinessCardService service, IStringLocalizer<SharedResource> localizer )
        {
            _service = service;
            _localizer = localizer;
        }
        [Route("GetAll")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessCard>>> GetAll()
        {
            var cards = await _service.GetAllCards();
            return Ok(cards);
        }

        [Route("GetById")]
        [HttpGet("{id}")]
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

        [Route("Delete")]
        [HttpDelete("{id}")]
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
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<BusinessCard>>> Filter(
            [FromQuery] BusinessCardFilterDto filter)
        {
            var cards = await _service.FilterCards(filter);
            return Ok(cards);
        }
    }
}