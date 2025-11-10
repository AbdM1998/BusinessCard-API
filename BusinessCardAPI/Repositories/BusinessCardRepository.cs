using BusinessCardAPI.Data;
using BusinessCardAPI.Interfaces.Repositories;
using BusinessCardAPI.Models.DTOs;
using BusinessCardAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardAPI.Repositories
{


    public class BusinessCardRepository : IBusinessCardRepository
    {
        private readonly ApplicationDbContext _context;

        public BusinessCardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusinessCard>> GetAll()
        {
            return await _context.BusinessCards.OrderByDescending(bc => bc.CreatedAt).ToListAsync();
        }

        public async Task<BusinessCard?> GetById(int id)
        {
            return await _context.BusinessCards.FindAsync(id);
        }

        public async Task<BusinessCard> Create(BusinessCard businessCard)
        {
            await _context.BusinessCards.AddAsync(businessCard);
            await _context.SaveChangesAsync();
            return businessCard;
        }

        public async Task<bool> Delete(int id)
        {
            var businessCard = await _context.BusinessCards.FindAsync(id);
            if (businessCard == null)
                return false;

            _context.BusinessCards.Remove(businessCard);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BusinessCard>> Filter(BusinessCardFilterDto filter)
        {
            var query = _context.BusinessCards.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(bc => bc.Name.Contains(filter.Name));
            }

            if (!string.IsNullOrWhiteSpace(filter.Gender))
            {
                query = query.Where(bc => bc.Gender == filter.Gender);
            }

            if (filter.DateOfBirth.HasValue)
            {
                query = query.Where(bc => bc.DateOfBirth.Date == filter.DateOfBirth.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query = query.Where(bc => bc.Email.Contains(filter.Email));
            }

            if (!string.IsNullOrWhiteSpace(filter.Phone))
            {
                query = query.Where(bc => bc.Phone.Contains(filter.Phone));
            }

            return await query.OrderByDescending(bc => bc.CreatedAt).ToListAsync();
        }
    }
}