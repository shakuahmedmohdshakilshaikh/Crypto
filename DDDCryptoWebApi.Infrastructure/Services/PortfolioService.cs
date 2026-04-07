using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using DDDCryptoWebApi.Domain.Model;
using DDDCryptoWebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Infrastructure.Services
{
    public class PortfolioService : IPortfolioService
    {
        ApplicationDbContext db;
        public PortfolioService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task BuyCryptoAsync(BuyCryptoDTO dto)
        {
            if (dto.Amount <= 0)
                throw new Exception("Amount must be greater than zero");

            var wallet = await db.Wallets
                .FirstOrDefaultAsync(x => x.UserId == dto.UserId);

            if (wallet == null)
                throw new Exception("Wallet not found");

            if (wallet.Balance < dto.Amount)
                throw new Exception("Insufficient wallet balance");

            var crypto = await db.Cryptos
                .FirstOrDefaultAsync(x => x.CryptoId == dto.CryptoId);

            if (crypto == null)
                throw new Exception("Crypto not found");

            if (crypto.CurrentPrice <= 0)
                throw new Exception("Invalid crypto price");

            var quantity = dto.Amount / crypto.CurrentPrice;

            wallet.Balance -= dto.Amount;

            db.WalletTransactions.Add(new WalletTransaction
            {
                WalletId = wallet.WalletId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                TransactionType = "Debit",
                Status = "Completed",
                PaymentMethod = "Wallet"
            });

            var portfolio = await db.Portfolios
                .FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.CryptoId == dto.CryptoId);

            if (portfolio == null)
            {
                portfolio = new UserPortFolio
                {
                    UserId = dto.UserId,
                    CryptoId = dto.CryptoId,
                    Quantity = quantity,
                    AvgBuyPrice = crypto.CurrentPrice,
                    TotalInvestment = dto.Amount,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                db.Portfolios.Add(portfolio);
            }
            else
            {
                var oldTotalQty = portfolio.Quantity;
                var oldAvg = portfolio.AvgBuyPrice;
                var oldInvestment = portfolio.TotalInvestment;

                var newTotalQty = oldTotalQty + quantity;
                var newInvestment = oldInvestment + dto.Amount;
                var newAvgPrice = newInvestment / newTotalQty;

                portfolio.Quantity = newTotalQty;
                portfolio.TotalInvestment = newInvestment;
                portfolio.AvgBuyPrice = newAvgPrice;
                portfolio.UpdatedAt = DateTime.Now;
                portfolio.ModifiedAt = DateTime.Now;
            }

            db.PortfolioTransactions.Add(new PortfolioTransaction
            {
                UserId = dto.UserId,
                CryptoId = dto.CryptoId,
                TransactionType = "Buy",
                Quantity = quantity,
                PricePerUnit = crypto.CurrentPrice,
                TotalAmount = dto.Amount,
                CreatedAt = DateTime.Now
            });

            db.Transactions.Add(new TransactionHistory
            {
                UserId = dto.UserId,
                WalletId = wallet.WalletId,
                CryptoId = dto.CryptoId,
                Type = "Buy",
                Quantity = quantity,
                PriceAtTime = crypto.CurrentPrice,
                TotalAmount = dto.Amount,
                CreatedAt = DateTime.Now
            });
            await db.SaveChangesAsync();
        }


     

        public async Task SellCryptoAsync(SellCryptoDTO dto)
        {
            if (dto.Quantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            var portfolio = await db.Portfolios
                .FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.CryptoId == dto.CryptoId);

            if (portfolio == null)
                throw new Exception("Portfolio record not found");

            if (portfolio.Quantity < dto.Quantity)
                throw new Exception("Insufficient crypto quantity");

            var crypto = await db.Cryptos
                .FirstOrDefaultAsync(x => x.CryptoId == dto.CryptoId);

            if (crypto == null)
                throw new Exception("Crypto not found");

            var wallet = await db.Wallets
                .FirstOrDefaultAsync(x => x.UserId == dto.UserId);

            if (wallet == null)
                throw new Exception("Wallet not found");

            var sellAmount = dto.Quantity * crypto.CurrentPrice;

            wallet.Balance += sellAmount;

            db.WalletTransactions.Add(new WalletTransaction
            {
                WalletId = wallet.WalletId,
                UserId = dto.UserId,
                Amount = sellAmount,
                TransactionType = "Credit",
                Status = "Completed",
                PaymentMethod = "Wallet"
            });

            portfolio.Quantity -= dto.Quantity;
            portfolio.TotalInvestment = portfolio.Quantity * portfolio.AvgBuyPrice;
            portfolio.UpdatedAt = DateTime.Now;
            portfolio.ModifiedAt = DateTime.Now;

            db.PortfolioTransactions.Add(new PortfolioTransaction
            {
                UserId = dto.UserId,
                CryptoId = dto.CryptoId,
                TransactionType = "Sell",
                Quantity = dto.Quantity,
                PricePerUnit = crypto.CurrentPrice,
                TotalAmount = sellAmount,
                CreatedAt = DateTime.Now
            });

            db.Transactions.Add(new TransactionHistory
            {
                UserId = dto.UserId,
                WalletId = wallet.WalletId,
                CryptoId = dto.CryptoId,
                Type = "Sell",
                Quantity = dto.Quantity,
                PriceAtTime = crypto.CurrentPrice,
                TotalAmount = sellAmount,
                CreatedAt = DateTime.Now
            });

            await db.SaveChangesAsync();
        }



        public async Task<List<PortfolioDTO>> GetPortfolioByUserIdAsync(int userId)
        {
            var data = await db.Portfolios
                .Include(x => x.Crypto)
                .Where(x => x.UserId == userId && x.DeletedAt == null)
                .Select(x => new PortfolioDTO
                {
                    PortFolioId = x.PortFolioId,
                    UserId = x.UserId,
                    CryptoId = x.CryptoId,
                    CryptoName = x.Crypto.CryptoName,
                    Symbol = x.Crypto.Symbol,
                    Image = x.Crypto.Image,
                    Quantity = x.Quantity,
                    AvgBuyPrice = x.AvgBuyPrice,
                    CurrentPrice = x.Crypto.CurrentPrice,
                    TotalInvestment = x.TotalInvestment,
                    CurrentValue = x.Quantity * x.Crypto.CurrentPrice,
                    ProfitLoss = (x.Quantity * x.Crypto.CurrentPrice) - x.TotalInvestment
                })
                .ToListAsync();

            return data;
        }

        public async Task<List<PortfolioTransactionDTO>> GetPortfolioTransactionsAsync(int userId)
        {
            return await db.PortfolioTransactions
                .Include(x => x.Crypto)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new PortfolioTransactionDTO
                {
                    CryptoName = x.Crypto.CryptoName,
                    Symbol = x.Crypto.Symbol,
                    TransactionType = x.TransactionType,
                    Quantity = x.Quantity,
                    PricePerUnit = x.PricePerUnit,
                    TotalAmount = x.TotalAmount,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }
    }
}
