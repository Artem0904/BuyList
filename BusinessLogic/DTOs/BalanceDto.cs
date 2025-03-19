using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class BalanceDto
    {
        public int Id { get; set; }
        public decimal Money { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
