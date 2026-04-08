using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class DashboardDTO
    {
        public decimal WalletBalance { get; set; }

        public decimal TotalInvestment { get; set; }

        public decimal PortfolioValue {  get; set; }

        public decimal ProfitLoss { get; set; }

        public decimal ProfitLossPercentage { get; set; }

       

        public List<DashboardHoldingDTO> TopHolding { get; set; }
        public List<AllocationChartDTO> AllocationChart { get; set; }   

        public List<DashboardTransactionDTO> RecentTransactions { get; set; }


    
    }
}
