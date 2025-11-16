using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;

namespace BusinessCardAPI.Interfaces.Services
{
    public interface IBusinessCardService
    {
        Task<PagedResult<BusinessCard>> GetAllCards(int pageNumber = 1 , int pageSize = 10);
        Task<BusinessCard?> GetCardById(int id);
        Task<BusinessCard> CreateCard(BusinessCardCreateDto dto);
        Task<bool> DeleteCard(int id);
        Task<IEnumerable<BusinessCard>> FilterCards(BusinessCardFilterDto filter);
        Task CreateBulk(IEnumerable<BusinessCardCreateDto> cards);
    }
}