using System;
using AutoMapper;
using EtherscanWorker.Commands.Token;
using EtherscanWorker.Entities;
using EtherscanWorker.Helpers;

namespace EtherscanWorker.Services
{
    public interface ITokenService
    {
        IEnumerable<Token> GetAll();
        Token GetById(int id);
        void Update(TokenCreateUpdateRequest model, decimal price);
        void CommitUpdate();
    }

    public class TokenService : ITokenService
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public TokenService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<Token> GetAll()
        {
            return _context.Token;
        }

        public Token GetById(int id)
        {
            return getToken(id);
        }

        public void Update(TokenCreateUpdateRequest model, decimal price)
        {
            var token = getToken(model.Id);
            // copy model to user and save
            _mapper.Map(model, token);
            token.Price = price;
            _context.Token.Update(token);
        }

        public void CommitUpdate()
        {
            _context.SaveChanges();
        }

        #region Helper Methods
        private Token getToken(int id)
        {
            var token = _context.Token.Find(id);
            if (token == null) throw new KeyNotFoundException("User not found");
            return token;
        }
        #endregion
    }
}

