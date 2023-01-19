using EtherscanWorker;
using EtherscanWorker.Helpers;
using EtherscanWorker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();

        services.AddDbContext<DataContext>();
        services.AddSingleton<DataContext>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddTransient<ITokenService, TokenService>();

    })
    .Build();

await host.RunAsync();

