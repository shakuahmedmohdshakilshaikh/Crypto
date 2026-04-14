using AutoMapper;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using DDDCryptoWebApi.Domain.Model;
using DDDCryptoWebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Infrastructure.Services
{
    public class UserFavouriteService : IUserFavouriteService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public UserFavouriteService(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task AddAsync(AddFavouriteDTO dto)
        {
            var user = await db.Users
                .FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.IsActive);

            if (user == null)
                throw new Exception("User not found");

            var crypto = await db.Cryptos
                .FirstOrDefaultAsync(x => x.CryptoId == dto.CryptoId && x.IsActive);

            if (crypto == null)
                throw new Exception("Crypto not found");

            var existing = await db.Favorites
                .FirstOrDefaultAsync(x =>
                    x.UserId == dto.UserId &&
                    x.CryptoId == dto.CryptoId &&
                    x.DeletedAt == null);

            if (existing != null)
                throw new Exception("Crypto already added to favourites");

            var favourite = mapper.Map<UserFavourite>(dto);
            favourite.CreatedAt = DateTime.Now;

            db.Favorites.Add(favourite);
            await db.SaveChangesAsync();
        }

        public async Task<List<UserFavouriteDTO>> GetByUserIdAsync(int userId)
        {
            var data = await db.Favorites.Include(x => x.Crypto)
                .Where(x => x.UserId == userId && x.DeletedAt == null)
                .Select(x => new UserFavouriteDTO
                {
                    Fid = x.Fid,
                    UserId = x.UserId,
                    CryptoId = x.CryptoId,
                    CryptoName = x.Crypto.CryptoName,
                    Symbol = x.Crypto.Symbol,
                    Image = x.Crypto.Image,
                    CurrentPrice = x.Crypto.CurrentPrice,
                    MarketCap = x.Crypto.MarketCap
                   
                })
                .ToListAsync();

            return data;
        }

        public async Task DeleteAsync(int fid)
        {
            var favourite = await db.Favorites
                .FirstOrDefaultAsync(x => x.Fid == fid && x.DeletedAt == null);

            if (favourite == null)
                throw new Exception("Favourite not found");

            db.Favorites.Remove(favourite);
            //favourite.DeletedAt = DateTime.Now;

            await db.SaveChangesAsync();
        }
    }
}
