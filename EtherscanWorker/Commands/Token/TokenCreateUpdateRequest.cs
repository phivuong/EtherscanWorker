using System;
namespace EtherscanWorker.Commands.Token
{
    public class TokenCreateUpdateRequest
    {
        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal TotalSupply { get; set; }
        public string ContractAddress { get; set; }
        public int TotalHolders { get; set; }
        public decimal Price { get; set; }
    }
}

