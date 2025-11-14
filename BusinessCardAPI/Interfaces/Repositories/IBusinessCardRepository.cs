using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;

namespace BusinessCardAPI.Interfaces.Repositories
{
    public interface IBusinessCardRepository
    {
        Task<PagedResult<BusinessCard>> GetAll(int pageNumber = 1 , int pageSize = 10);
        Task<BusinessCard?> GetById(int id);
        Task<BusinessCard> Create(BusinessCard businessCard);
        Task<bool> Delete(int id);
        Task<IEnumerable<BusinessCard>> Filter(BusinessCardFilterDto filter);
    }
}
