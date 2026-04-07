using DDDCryptoWebApi.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.Interface
{
    public interface IUserFavouriteService
    {
        Task AddAsync(AddFavouriteDTO dto);
        Task<List<UserFavouriteDTO>> GetByUserIdAsync(int userId);
        Task DeleteAsync(int fid);
    }
}
