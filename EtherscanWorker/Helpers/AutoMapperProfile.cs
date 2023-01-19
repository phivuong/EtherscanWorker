using System;
using AutoMapper;
using EtherscanWorker.Commands.Token;
using EtherscanWorker.Entities;

namespace EtherscanWorker.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TokenCreateUpdateRequest, Token>();
            CreateMap<Token, TokenCreateUpdateRequest>();
        }
    }
}

