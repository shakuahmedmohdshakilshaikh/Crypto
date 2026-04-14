using DDDCryptoWebApi.Application.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Infrastructure.Jobs
{
    public class CryptoSyncJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public CryptoSyncJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var service = scope.ServiceProvider
                        .GetRequiredService<ICoinGeckoService>();

                    await service.SyncCoinsToDatabaseAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cron Job Error: " + ex.Message);
                }


                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}
