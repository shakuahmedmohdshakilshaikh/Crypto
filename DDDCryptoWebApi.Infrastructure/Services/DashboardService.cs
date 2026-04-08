using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using DDDCryptoWebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        ApplicationDbContext db;
      
            public DashboardService(ApplicationDbContext db)
            {
                this.db = db;
            }

            public async Task<DashboardDTO> GetDashboardAsync(int userId)
            {
                var wallet = await db.Wallets
                    .FirstOrDefaultAsync(x => x.UserId == userId);
                
                if(wallet == null)
                    {
                throw new Exception("User Not found");
                    }
                decimal walletBalance = wallet?.Balance ?? 0;

                var portfolioData = await db.Portfolios
                    .Include(x => x.Crypto)
                    .Where(x => x.UserId == userId && x.DeletedAt == null)
                    .ToListAsync();

                decimal totalInvestment = portfolioData.Sum(x => x.TotalInvestment);
                decimal portfolioValue = portfolioData.Sum(x => x.Quantity * x.Crypto.CurrentPrice);
                decimal profitLoss = portfolioValue - totalInvestment;

                decimal profitLossPercentage = 0;
                if (totalInvestment > 0)
                {
                    profitLossPercentage = (profitLoss / totalInvestment) * 100;
                }

                var topHoldings = portfolioData
                    .OrderByDescending(x => x.Quantity * x.Crypto.CurrentPrice)
                    .Take(5)
                    .Select(x => new DashboardHoldingDTO
                    {
                        CryptoId = x.CryptoId,
                        CryptoName = x.Crypto.CryptoName,
                        Symbol = x.Crypto.Symbol,
                        Image = x.Crypto.Image,
                        Quantity = x.Quantity,
                        AvgBuyPrice = x.AvgBuyPrice,
                        CurrentPrice = x.Crypto.CurrentPrice,
                        CurrentValue = x.Quantity * x.Crypto.CurrentPrice,
                        ProfitLoss = (x.Quantity * x.Crypto.CurrentPrice) - x.TotalInvestment
                    })
                    .ToList();

                var recentTransactions = await db.Transactions
                    .Include(x => x.Crypto)
                    .Where(x => x.UserId == userId && x.DeletedAt == null)
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(10)
                    .Select(x => new DashboardTransactionDTO
                    {
                        TransactionId = x.TransactionId,
                        Type = x.Type,
                        CryptoName = x.Crypto.CryptoName,
                        Symbol = x.Crypto.Symbol,
                        Quantity = x.Quantity,
                        PriceAtTime = x.PriceAtTime,
                        TotalAmount = x.TotalAmount,
                        CreatedAt = x.CreatedAt
                    })
                    .ToListAsync();

                var allocationChart = new List<AllocationChartDTO>();

                if (portfolioValue > 0)
                {
                    allocationChart = portfolioData
                        .Select(x =>
                        {
                            decimal value = x.Quantity * x.Crypto.CurrentPrice;
                            decimal percentage = (value / portfolioValue) * 100;

                            return new AllocationChartDTO
                            {
                                CryptoId = x.CryptoId,
                                CryptoName = x.Crypto.CryptoName,
                                Symbol = x.Crypto.Symbol,
                                Value = value,
                                Percentage = percentage
                            };
                        })
                        .OrderByDescending(x => x.Value)
                        .ToList();
                }

                return new DashboardDTO
                {
                    WalletBalance = walletBalance,
                    TotalInvestment = totalInvestment,
                    PortfolioValue = portfolioValue,
                    ProfitLoss = profitLoss,
                    ProfitLossPercentage = profitLossPercentage,
                    TopHolding = topHoldings,
                    RecentTransactions = recentTransactions,
                    AllocationChart = allocationChart
                };
            
        }
    }
}
