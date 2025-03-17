using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    internal class UserDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? UserName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public long ChatId { get; set; }
        public string? Image { get; set; } = string.Empty;
    }
}
