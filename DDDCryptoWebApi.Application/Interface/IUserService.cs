using DDDCryptoWebApi.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.Interface
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByIdAsync(int userId);
        Task UpdateAsync(int userId, UpdateUserDTO dto);
        Task DeleteAsync(int userId);
    }
}
