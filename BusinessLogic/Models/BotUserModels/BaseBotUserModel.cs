using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Models.UserModels
{
    public class BaseBotUserModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public long? ChatId { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
