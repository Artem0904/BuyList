using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models.PurchaseModels
{
    public class BasePurchaseModel
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
