using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using WebTask.Models;
using WebTask;
using WebTask.Entities;
using WebTask.Database;

namespace WebTask
{
    public static class Extensions
    {
        // User
        public static User GetUser(this DBContext context, int userId)
        {
            return context.Users.FirstOrDefault(u => u.Id == userId);
        }

        // Product
        public static Product Clone(this ProductCard product)
        {
            return new Product()
            {
                Name = product.Product.Name,
                CreatedByUser = product.Product.CreatedByUser,
                Description = product.Product.Description,
                Price = product.Product.Price,
                ProductID = product.Product.ProductID,
                Slug = GetUniqueAlphaNumericID(),
            };
        }
        public static string GetUniqueAlphaNumericID()
        {
            string str1 = "";
            try
            {
                string str2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                short num1 = checked((short)str2.Length);
                Random random = new Random();
                StringBuilder stringBuilder = new StringBuilder();
                int num2 = 1;
                do
                {
                    int startIndex = random.Next(0, (int)num1);
                    stringBuilder.Append(str2.Substring(startIndex, 1));
                    checked { ++num2; }
                }
                while (num2 <= 6);
                stringBuilder.Append(DateTime.Now.ToString("HHmmss"));
                str1 = stringBuilder.ToString();
            }
            catch 
            {
            }
            return str1;
        }


        // ProductCard
        public static bool ExistProduct(this DBContext context, int ProductID)
        {
            return context.GetProductCard(ProductID) != null;
        }
        
        public static ProductCard GetProductCard(this DBContext context, int ProductID)
        {
            return context.GetProductCards().FirstOrDefault(p => p.Product.ProductID == ProductID);
        }
        public static List<ProductCard> GetProductCards(this DBContext context)
        {
            return context.Products.Include(o => o.Product).ToList();
        }

        // Orders
        public static Order GetOrder(this DBContext context, int OrderId)
        {
            return context.GetOrders().FirstOrDefault(o => o.OrderId == OrderId);
        }
        public static List<Order> GetOrders(this DBContext context)
        {
            return context.Orders.Include(o => o.Product).ToList();
        }

    }
}
