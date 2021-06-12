using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalAutoFillServer.DTO;
using FinalAutoFillServer.Entities;
using AutoMapper;

namespace FinalAutoFillServer.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<RegisterModel, User>();

        }
    }
}
