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
public class CustomerController : Controller
    {
        private ECContext _context;
        public CustomerController(ECContext context)
        {
            _context = context;
        }
        // GET: /customer page
        [HttpGet]
        [Route("/customers")]
        public IActionResult Customers()
        {
            List<Customer> customers =_context.customers
                    .OrderByDescending(c=>c.CreatedAt)
                    .ToList();
            ViewBag.customers=customers;
            if(TempData["error"]!=null)
            {
                ViewData["error"]=TempData["error"];
            }
            return View("customers");
        }            
        // Post : add a new customer
        [HttpPost]
        [Route("/customer/new")]
        public IActionResult NewCustomer(Customer newcustomer)
        {
            if(ModelState.IsValid)
            {
                Customer customer = _context.customers
                    .SingleOrDefault(c=>c.Name==newcustomer.Name);
                if(customer !=null)
                {
                    TempData["error"]="Customer name has existed!";
                }
                else
                {
                    newcustomer.CreatedAt=DateTime.Now;
                    _context.Add(newcustomer);
                    _context.SaveChanges();
                }
                return RedirectToAction("Customers");
            }       
            List<Customer> customers =_context.customers
                    .OrderByDescending(c=>c.CreatedAt)
                    .ToList();
            ViewBag.customers=customers; 
            return View("customers");
        }

        //GET: delete a customer
        [HttpGet]
        [Route("/customer/delete/{customerid}")]
        public IActionResult Delete(int customerid)
        {
            List<Order> removeorders=_context.orders
                .Where(a => a.CustomerId==customerid)
                .Include(o=>o.Product).ToList();
            foreach(Order order in removeorders)
            {
                Product product =_context.products
                    .SingleOrDefault(p => p.ProductId==order.Product.ProductId);
                product.Quantity+=order.Quantity;
                _context.orders.Remove(order);
            }
            Customer removecustomer =_context.customers
                .SingleOrDefault(c=> c.CustomerId==customerid);
                _context.customers.Remove(removecustomer);
            _context.SaveChanges();
            return RedirectToAction("Customers");                   
        }
    } 
}
