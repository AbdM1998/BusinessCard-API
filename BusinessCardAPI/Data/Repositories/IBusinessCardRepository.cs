using BusinessCardAPI.Models;
using BusinessCardAPI.Models.DTOs;

namespace BusinessCardAPI.Data.Repositories
{
    public interface IBusinessCardRepository
    {
        Task<IEnumerable<BusinessCard>> GetAll();
        Task<BusinessCard?> GetById(int id);
        Task<BusinessCard> Create(BusinessCard businessCard);
        Task<bool> Delete(int id);
        Task<IEnumerable<BusinessCard>> Filter(BusinessCardFilterDto filter);
    }
}
