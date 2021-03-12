using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTask.Entities
{
    public class ProductCard
    {
        [Key]
        public int ProductCardId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
