using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models.OrderModels
{
    public class BaseOrderModel
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
