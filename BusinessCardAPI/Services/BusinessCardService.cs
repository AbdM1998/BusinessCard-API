using BusinessCardAPI.Exceptions;
using BusinessCardAPI.Interfaces.Repositories;
using BusinessCardAPI.Interfaces.Services;
using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;
using BusinessCardAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Resources;

namespace BusinessCardAPI.Services
{
    public class BusinessCardService : IBusinessCardService
    {
        private readonly IBusinessCardRepository _repository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public BusinessCardService( IBusinessCardRepository repository, IStringLocalizer<SharedResource> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<PagedResult<BusinessCard>> GetAllCards(int pageNumber = 1,int pageSize = 10)
        {

            var cards = await _repository.GetAll(pageNumber, pageSize);
            return cards;
        }
        public async Task<BusinessCard?> GetCardById(int id)
        {
            var card = await _repository.GetById(id);
            return card;
        }
        public async Task<BusinessCard> CreateCard(BusinessCardCreateDto dto)
        {
            try
            {
                ValidateRequest(dto);

                var card = new BusinessCard(dto.Name, dto.Gender, dto.DateOfBirth, dto.Email, dto.Phone, dto.Address, dto.Photo, DateTime.UtcNow);
                var created = await _repository.Create(card);
                
                return created;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateBulk(IEnumerable<BusinessCardCreateDto> dtoCardList)
        {
            try
            {
                List<BusinessCard> cards = new List<BusinessCard>();
                foreach (var cardDto in dtoCardList)
                {
                    ValidateRequest(cardDto);
                    var card = new BusinessCard(cardDto.Name, cardDto.Gender, cardDto.DateOfBirth, cardDto.Email, cardDto.Phone, cardDto.Address, cardDto.Photo, DateTime.UtcNow);
                    cards.Add(card);
                }

               await _repository.CreateBulk(cards);

            }
            catch (Exception ex)
            {
                throw new CreateCardException(ex.Message);
            }
        }
        private void ValidateRequest(BusinessCardCreateDto dto)
        {
            if (!ValidationHelper.IsValidBase64Photo(dto.Photo))
                throw new ArgumentException(_localizer["InvalidBase64Photo"]);
            if (!ValidationHelper.IsValidPhone(dto.Phone))
                throw new ArgumentException(_localizer["InvalidPhoneNumber"]);
            if (!ValidationHelper.IsValidEmail(dto.Email))
                throw new ArgumentException(_localizer["InvalidEmail"]);
            if (!ValidationHelper.IsValidGender(dto.Gender))
                throw new ArgumentException(_localizer["InvalidGender"]);
        }

        public async Task<bool> DeleteCard(int id)
        {
            var result = await _repository.Delete(id);
            return result;
        }

        public async Task<IEnumerable<BusinessCard>> FilterCards(BusinessCardFilterDto filter)
        {
            var cards = await _repository.Filter(filter);
            return cards;
        }
    }
}
