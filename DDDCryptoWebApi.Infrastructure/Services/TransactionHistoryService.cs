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
    public class TransactionHistoryService : ITransactionHistoryService
    {
        ApplicationDbContext db;
        public TransactionHistoryService(ApplicationDbContext db)
        {
            this.db = db;   
        }
        public async Task<List<TransactionHistoryDTO>> GetAllAsync()
        {
            return await db.Transactions
                .Include(x => x.Crypto)
                .Where(x => x.DeletedAt == null)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new TransactionHistoryDTO
                {
                    TransactionId = x.TransactionId,
                    UserId = x.UserId,
                    WalletId = x.WalletId,
                    CryptoId = x.CryptoId,
                    CryptoName = x.Crypto.CryptoName,
                    Symbol = x.Crypto.Symbol,
                    Type = x.Type,
                    Quantity = x.Quantity,
                    PriceAtTime = x.PriceAtTime,
                    TotalAmount = x.TotalAmount,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<TransactionHistoryDTO> GetByIdAsync(int transactionId)
        {
            var transaction = await db.Transactions
                .Include(x => x.Crypto)
                .Where(x => x.TransactionId == transactionId && x.DeletedAt == null)
                .Select(x => new TransactionHistoryDTO
                {
                    TransactionId = x.TransactionId,
                    UserId = x.UserId,
                    WalletId = x.WalletId,
                    CryptoId = x.CryptoId,
                    CryptoName = x.Crypto.CryptoName,
                    Symbol = x.Crypto.Symbol,
                    Type = x.Type,
                    Quantity = x.Quantity,
                    PriceAtTime = x.PriceAtTime,
                    TotalAmount = x.TotalAmount,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (transaction == null)
                throw new Exception("Transaction history not found");

            return transaction;
        }

        public async Task<List<TransactionHistoryDTO>> GetByWalletIdAsync(int userid)
        {
            return await db.Transactions
                .Include(x => x.Crypto)
                .Where(x => x.UserId == userid && x.DeletedAt == null)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new TransactionHistoryDTO
                {
                    TransactionId = x.TransactionId,
                    UserId = x.UserId,
                    WalletId = x.WalletId,
                    CryptoId = x.CryptoId,
                    CryptoName = x.Crypto.CryptoName,
                    Symbol = x.Crypto.Symbol,
                    Type = x.Type,
                    Quantity = x.Quantity,
                    PriceAtTime = x.PriceAtTime,
                    TotalAmount = x.TotalAmount,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }
    }
}
