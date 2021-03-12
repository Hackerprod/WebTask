using System.ComponentModel.DataAnnotations;

namespace WebTask.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set;}

        public string Name { get; set; }

        public int CreatedByUser { get; set; }

        [StringLength(100)]
        public string Description {get; set;}

        public string Slug { get; set; }

        public int Price { get; set; }

    }
}