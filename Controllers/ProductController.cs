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
    //[Authorize]
    [Route("products/")]
    public class ProductController : Controller
    {
        public DBContext Context { get; }

        public ProductController(DBContext context)
        {
            Context = context;
        }

        // GET: api/<controller>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(Context.GetProductCards());
        }

        [HttpGet("{id}")]
        public IActionResult GetbyId(int id)
        {
            ProductCard product = Context.GetProductCard(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST api/<controller>
        [HttpPost("{count}")]
        [Authorize]
        public IActionResult AddProduct([FromBody]Product product, int quantity)
        {
            if (ModelState.IsValid)
            {
                if (Context.ExistProduct(product.ProductID))
                {
                    product.ProductID = CreateId();
                }
                Context.Products.Add(new ProductCard() { Product = product, Quantity = quantity });
                Context.SaveChanges();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // PUT api/<controller>/5
        [Authorize]
        [HttpPut]
        public IActionResult UpdateProduct([FromBody]Product product)
        {
            // only allow admins to access other user records
            var userId = int.Parse(User.Identity.Name);
            User user = Context.GetUser(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (!User.IsInRole(Role.Admin) || product.CreatedByUser != user.Id)
            {
                return Unauthorized();
            }

            ProductCard productCard = Context.GetProductCard(product.ProductID);
            if (productCard == null)
            {
                return NotFound("ProductCard not found");
            }
            productCard.Product = product;

            if (ModelState.IsValid)
            {
                Context.Entry(productCard).State = EntityState.Modified;
                Context.SaveChanges();
                return Ok();
            }

            return BadRequest(ModelState);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            ProductCard product = Context.GetProductCard(id);
            if (product == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.Identity.Name);
            User user = Context.GetUser(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }
            if (!User.IsInRole(Role.Admin) || product.Product.CreatedByUser != user.Id)
            {
                return Unauthorized();
            }

            Context.Products.Remove(product);
            Context.SaveChanges();
            return Ok();
        }
        private int CreateId()
        {
            List<int> orders = (from member in Context.GetProductCards() where member != null select member.Product.ProductID).ToList();
            if (!orders.Any())
            {
                return 1;
            }
            orders.Sort((s1, s2) => s2.CompareTo(s1));
            return orders[0] + 1;
        }
    }
}
