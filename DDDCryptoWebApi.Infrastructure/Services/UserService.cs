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
    public class UserService : IUserService
    {
        ApplicationDbContext db;
        IMapper mapper;
        public UserService(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
           this.mapper = mapper;
        }
        public async Task DeleteAsync(int userId)
        {
           
           var user =  await db.Users.FindAsync(userId);
            if (user == null) {
                throw new Exception("User not found");

            }
            //db.Users.Remove(user);
            user.IsActive = false;
            user.DeletedAt = DateTime.Now;

            await db.SaveChangesAsync();
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
          var data = await db.Users.Where(x =>  x.IsActive).ToListAsync();

            return mapper.Map<List<UserDTO>>(data);

        }

        public async Task<UserDTO> GetByIdAsync(int userId)
        {
        //var user =  await db.Users.FirstOrDefaultAsync(x => x.UserId.Equals(userId) && x.IsActive);
                var user = await db.Users.FindAsync(userId);
            if (user == null)
            { throw new Exception("User not found"); }

            return mapper.Map<UserDTO>(user);

        }

        public async Task UpdateAsync(int userId, UpdateUserDTO dto)
        {
            var user = await db.Users
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);

            if (user == null)
                throw new Exception("User not found");

            mapper.Map(dto, user);

            user.ModifiedAt = DateTime.Now;

            await db.SaveChangesAsync();
        }
    }
}
