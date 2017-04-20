// using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ecommerce.Controllers
{
public class OrderController : Controller
    {
        private ECContext _context;
        public OrderController(ECContext context)
        {
            _context = context;
        }
        // GET: /order page
        [HttpGet]
        [Route("/orders")]
        public IActionResult Orders()
        {
            List<Order> orders =_context.orders
                    .Include(o=>o.Product)
                    .Include(o=>o.Customer)
                    .OrderByDescending(c=>c.CreatedAt)
                    .ToList();
            ViewBag.orders=orders;
            ViewBag.customers=_context.customers.ToList();
            ViewBag.products=_context.products.ToList();
            if(TempData["error"]!=null)
            {
                ViewData["error"]=TempData["error"];
            }
            return View("orders");
        }            
        // Post : add a new order
        [HttpPost]
        [Route("/order/new")]
        public IActionResult NewOrder(Order neworder)
        {
            if(ModelState.IsValid)
            {
                Product product = _context.products
                    .SingleOrDefault(p => p.ProductId==neworder.ProductId);
                if(neworder.Quantity==0 && product.Quantity==0){
                    TempData["error"] =$"Sorry. we dont have inventory of {product.Name} now";
                }
                else if(product.Quantity < neworder.Quantity)
                {
                    TempData["error"] =$"We only have {product.Quantity} {product.Name} now";
                }
                else
                {
                    product.Quantity-=neworder.Quantity;
                    neworder.CreatedAt=DateTime.Now;
                    _context.Add(neworder);
                    _context.SaveChanges();
                }
                return RedirectToAction("Orders");
            }       
            return View("orders");
        }          
    } 
}
