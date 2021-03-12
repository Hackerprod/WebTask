using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebTask.Models;

namespace WebTask.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public Product Product { get; set; }

        public int OrderUserId { get; set; }

        public DateTime DateCreated { get; set; }

        public OrdenStatus State { get; set; }

        public int Quantity { get; set; }
    }
}
