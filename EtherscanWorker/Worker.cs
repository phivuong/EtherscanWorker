using System.Text.Json;
using AutoMapper;
using EtherscanWorker.Commands.Token;
using EtherscanWorker.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EtherscanWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private ITokenService _tokenService;
    private readonly IMapper _mapper;

    public Worker(ILogger<Worker> logger, ITokenService tokenService, IMapper mapper)
    {
        _logger = logger;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var tokens = _tokenService.GetAll();
            foreach (var token in tokens)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"https://min-api.cryptocompare.com/data/price?fsym={token.Symbol}&tsyms=USD");
                    //HTTP GET
                    var responseTask = client.GetAsync(client.BaseAddress);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        var jsonObj = (JObject)JsonConvert.DeserializeObject(readTask.Result);
                        var usd = (JValue)jsonObj["USD"];
                        var priceStr = "0";
                        if (usd != null)
                        {
                            priceStr = usd.Value.ToString();
                        }
                        var price = Decimal.Parse(priceStr);
                        var req = _mapper.Map<TokenCreateUpdateRequest>(token);
                        _tokenService.Update(req, price);
                    }
                }
            }
            _tokenService.CommitUpdate();
            await Task.Delay(300000, stoppingToken); // Milisecon here, currently set to 5 mins
        }
    }
}

