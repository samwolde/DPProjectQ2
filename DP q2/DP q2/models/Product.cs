using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_q2.models
{
    class Product
    {
        public String Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public List<Sale> Sales { get; }

        public Product()
        {
            Sales = new List<Sale>();
        }

        public int GetRemainingQty()
        {
            int total = 0;
            foreach(Sale sale in Sales)
            {
                total += sale.Quantity;
            }

            int remaining = Quantity - total;

            return remaining > 0 ? remaining:0;
        }

    }
}
