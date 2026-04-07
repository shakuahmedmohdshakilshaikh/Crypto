using DDDCryptoWebApi.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.Interface
{
    public interface IWalletService
    {
       

        Task<object> CreateRazorpayOrderAsync(CreateOrderDTO dto);
        Task VerifyPaymentAndAddMoneyAsync(PaymentVerifyDTO dto);
        Task DeductBalanceAsync(DeductBalanceDTO dto);

        Task<decimal> GetBalanceAsync(int userId);
    }
}
