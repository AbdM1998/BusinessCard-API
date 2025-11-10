using BusinessCardAPI.Data.Repositories;
using BusinessCardAPI.Models;
using BusinessCardAPI.Models.DTOs;

namespace BusinessCardAPI.Services.Interfaces
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