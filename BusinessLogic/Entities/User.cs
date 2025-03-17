using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Entities
{
    public class User : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long? ChatId { get; set; }
        public string? Image { get; set; }

    }
}
