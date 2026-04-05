using AutoMapper;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using DDDCryptoWebApi.Domain.Model;
using DDDCryptoWebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private ApplicationDbContext db;
        private IConfiguration configuration;
        IMapper mapper;
        public AuthService(ApplicationDbContext db, IConfiguration configuration,IMapper mapper)
        {
            this.db = db;
            this.configuration = configuration;
            this.mapper = mapper;
            
        }
       
        public async Task<string> Register(RegisterDTO dto)
        {
            var data = mapper.Map<UserMaster>(dto);
            db.Users.Add(data);
            await db.SaveChangesAsync();

            return "Registered Successfully";
        }

        public async Task<AuthResponseDTO> Login(LoginDTO dto)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == dto.Email && x.PassWord == dto.Password);

            if (user == null)
            {
                return null;

            }
            //var role = db.AddRole.FirstOrDefault(r => r.RoleId == user.RoleId)?.RoleName;

            var token = GenerateToken(user.Email); // role if needed you can add

            return new AuthResponseDTO
            {
                Token = token
            };
        }

        private string GenerateToken(string email) {

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, email),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var cred =  new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: configuration["Jwt:Issuer"],
               audience: configuration["Jwt:Audience"],
               claims: claims,
               expires: DateTime.Now.AddMinutes(60),
               signingCredentials: cred
           );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
