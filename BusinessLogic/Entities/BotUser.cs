﻿using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Entities
{
    public class BotUser : IdentityUser<int>
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long? ChatId { get; set; }
        public string? Image { get; set; }
        public int? BalanceId { get; set; }
        public Balance? Balance { get; set; }
        public ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
    }
}
