using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Entities;

namespace BusinessLogic.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int UserId { get; set; }
    }
}
