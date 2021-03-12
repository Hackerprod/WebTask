using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTask.Models;
using WebTask.Entities;
using WebTask.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace WebTask.Controllers
{
    [Route("Order/")]
    [Authorize]
    public class OrderController : Controller
    {
        public DBContext Context { get; }

        public OrderController(DBContext context)
        {
            Context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(Context.GetOrders());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult GetbyId(int id)
        {
            Order product = Context.Orders
                .Include(o => o.Product)
                .FirstOrDefault(p => p.OrderId == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PurchaseProduct([FromBody]PurchaseProduct Purchase)
        {
            //Comprobar si aun existe el producto en la tienda
            ProductCard productCard = Context.GetProductCard(Purchase.ProductId);
            if (productCard == null)
            {
                return NotFound("Product not found");
            }

            if (productCard.Quantity < Purchase.Quantity)
            {
                return BadRequest($"The shop not contains {Purchase.Quantity} {productCard.Product.Description}");
            }

            Product Product = productCard.Clone();

            // only allow admins to access other user records
            var currentUserId = int.Parse(User.Identity.Name);
            User user = Context.GetUser(currentUserId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            
            Order order = new Order()
            {
                Product = Product,
                DateCreated = DateTime.Now,
                OrderId = CreateId(),
                OrderUserId = user.Id,
                Quantity = Purchase.Quantity,
                State = OrdenStatus.Created
            };

            Context.Orders.Add(order);
            Context.SaveChanges();

            RemoveProductInstance(productCard, Purchase.Quantity);
            return Ok();
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult ConfirmOrder(int id)
        {
            // only allow admins to access
            if (!User.IsInRole(Role.Admin))
            {
                return Forbid();
            }

            Order _order = Context.Orders
                .Include(o => o.Product)
                .FirstOrDefault(p => p.OrderId == id);
            if (_order == null)
            {
                return NotFound();
            }
            _order.State = OrdenStatus.Confirmed;
            Context.Entry(_order).State = EntityState.Modified;
            Context.SaveChanges();
            return Ok(_order);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult CancelOrder(int id)
        {
            // only allow admins to access
            if (!User.IsInRole(Role.Admin))
            {
                return Forbid();
            }

            Order order = Context.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            Context.Orders.Remove(order);
            Context.SaveChanges();
            AddProductInstance(order.Product, order.Quantity);
            return Ok();
        }
        private void AddProductInstance(Product _product, int quantity)
        {
            ProductCard product = Context.Products.Include(o => o.Product).FirstOrDefault(o => o.Product.ProductID == _product.ProductID &&
                                                                   o.Product.CreatedByUser == _product.CreatedByUser);
            if (product == null)
            {
                Context.Products.Add(new ProductCard() { Product = _product, Quantity = quantity });
                Context.SaveChanges();
            }
            else
            {
                product.Quantity = product.Quantity + quantity;
                Context.Entry(product).State = EntityState.Modified;
                Context.SaveChanges();
            }
            Context.SaveChanges();
        }
        private void RemoveProductInstance(ProductCard productCard, int Quantity)
        {
            if (productCard.Quantity == Quantity)
            {
                Context.Products.Remove(productCard);
            }
            else
            {
                productCard.Quantity = productCard.Quantity - Quantity;
                Context.Entry(productCard).State = EntityState.Modified;
                Context.SaveChanges();
            }
            Context.SaveChanges();
        }
        private int CreateId()
        {
            List<int> orders = (from member in Context.Orders.ToList() where member != null select member.OrderId).ToList();
            if (!orders.Any())
            {
                return 1;
            }
            orders.Sort((s1, s2) => s2.CompareTo(s1));
            return orders[0] + 1;
        }
    }
}
