using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BusinessLogic.Entities
{
    public class Balance
    {
        public int Id { get; set; }
        public decimal Money { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int UserId { get; set; }
        public BotUser? User { get; set; }
    }
}
