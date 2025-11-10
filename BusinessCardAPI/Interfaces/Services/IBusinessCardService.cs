using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;

namespace BusinessCardAPI.Interfaces.Services
{
    public interface IBusinessCardService
    {
        Task<IEnumerable<BusinessCard>> GetAllCards();
        Task<BusinessCard?> GetCardById(int id);
        Task<BusinessCard> CreateCard(BusinessCardCreateDto dto);
        Task<bool> DeleteCard(int id);
        Task<IEnumerable<BusinessCard>> FilterCards(BusinessCardFilterDto filter);

    }
}