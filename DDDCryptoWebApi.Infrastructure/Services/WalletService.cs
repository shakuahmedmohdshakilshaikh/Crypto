using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using DDDCryptoWebApi.Domain.Model;
using DDDCryptoWebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Infrastructure.Services
{
    public class WalletService : IWalletService
    {
        ApplicationDbContext db;
        private readonly IConfiguration configuration;
        public WalletService(ApplicationDbContext db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
        }
    

        public async Task<WalletDTO> GetWalletByUserIdAsync(int userId)
        {
            var wallet = await db.Wallets
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return new WalletDTO
            {
                WalletId = wallet.WalletId,
                UserId = wallet.UserId,
                Balance = wallet.Balance
            };
        }

        public async Task<List<WalletTransactionDTO>> GetTransactionsAsync(int userId)
        {
            return await db.WalletTransactions
                .Where(x => x.UserId == userId)
                .Select(x => new WalletTransactionDTO
                {
                    Amount = x.Amount,
                    TransactionType = x.TransactionType,
                    Status = x.Status,
                    PaymentMethod = x.PaymentMethod,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<object> CreateRazorpayOrderAsync(CreateOrderDTO dto)
        {
            var keyId = configuration["Razorpay:KeyId"];
            var keySecret = configuration["Razorpay:KeySecret"];
            var client = new RazorpayClient(keyId, keySecret);

            var options = new Dictionary<string, object>
    {
        { "amount", dto.Amount * 100 }, // paise
        { "currency", "INR" },
        { "receipt", $"wallet_{dto.UserId}_{DateTime.Now.Ticks}" },
        { "payment_capture", 1 }
    };

            var order = client.Order.Create(options);

            return new
            {
                OrderId = order["id"].ToString(),
                Amount = Convert.ToDecimal(order["amount"]),
                Currency = order["currency"].ToString(),
                Status = order["status"].ToString(),
                Receipt = order["receipt"].ToString()
            };
        }

        public async Task VerifyPaymentAndAddMoneyAsync(PaymentVerifyDTO dto)
        {
            var attributes = new Dictionary<string, string>
    {
        { "razorpay_payment_id", dto.RazorpayPaymentId },
        { "razorpay_order_id", dto.RazorpayOrderId },
        { "razorpay_signature", dto.RazorpaySignature }
    };

            Utils.verifyPaymentSignature(attributes); // enable when using in angluar 

            var wallet = await db.Wallets
                .FirstOrDefaultAsync(x => x.UserId == dto.UserId );

            if (wallet == null)
            {
                wallet = new WalletMaster
                {
                    UserId = dto.UserId,
                    Balance = 0
                };

                db.Wallets.Add(wallet);
                await db.SaveChangesAsync();
            }

            wallet.Balance += dto.Amount;

            db.WalletTransactions.Add(new WalletTransaction
            {
                WalletId = wallet.WalletId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                TransactionType = "Credit",
                Status = "Completed",
                PaymentMethod = "Razorpay"
            });

            await db.SaveChangesAsync();
        }

        public async Task DeductBalanceAsync(DeductBalanceDTO dto)
        {
            var wallet = await db.Wallets
                .FirstOrDefaultAsync(x => x.UserId == dto.UserId);

            if (wallet == null)
                throw new Exception("Wallet not found");

            if (wallet.Balance < dto.Amount)
                throw new Exception("Insufficient wallet balance");

            wallet.Balance -= dto.Amount;

            var transaction = new WalletTransaction
            {
                WalletId = wallet.WalletId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                TransactionType = "Debit",
                Status = "Completed",
                PaymentMethod = "Wallet"
            };

            db.WalletTransactions.Add(transaction);

            await db.SaveChangesAsync();
        }

        public async Task<decimal> GetBalanceAsync(int userId)
        {
            var wallet = await db.Wallets
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return wallet?.Balance ?? 0;
        }
    }

      

      
    
}
