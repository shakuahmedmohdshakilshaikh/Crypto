
using AutoMapper;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.Mapping
{
    public class DTOMapping : Profile
    {
        public DTOMapping()
        {
            CreateMap<UserMaster, RegisterDTO>().ReverseMap();
            CreateMap<UserMaster, LoginDTO>().ReverseMap();
        }
    }
}
