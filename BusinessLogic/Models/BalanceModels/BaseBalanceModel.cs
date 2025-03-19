using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models.BalanceModels
{
    public class BaseBalanceModel
    {
        public decimal Money { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
