using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ecommerce.Controllers
{
 public class HomeController : Controller
    {
        private ECContext _context;
        public HomeController(ECContext context)
        {
            _context = context;
        }
        // GET: /dashbaord
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<Product> products =_context.products
                        .OrderByDescending(p => p.CreatedAt)
                        .ToList();
            if(products.Count>5)
            {
                products=products.Take(5).ToList();
            }
            List<Order> orders = _context.orders
                        .Include(o=>o.Product)
                        .Include(o=>o.Customer)
                        .OrderByDescending(o => o.CreatedAt)
                        .ToList();
            List<Customer> customers = _context.customers
                        .Include(c => c.orders)
                            .ThenInclude(o => o.Product)
                        .OrderByDescending(c => c.CreatedAt)
                        .ToList();
            if(customers.Count>5)
            {
                customers=customers.Take(5).ToList();
            }
            int i=0, j=0,m=0;
            List<string> recent_customer_activty = new List<string>();
            while(m<5)
            {
                if(DateTime.Compare(customers[i].CreatedAt,orders[j].CreatedAt)>0){
                    string item=customers[i].Name+" joined the store "+ Utility.Gettimespan(customers[i].CreatedAt);
                    recent_customer_activty.Add(item);
                    i++;
                    m++;
                }
                else{
                    if(customers.Contains(orders[j].Customer)==true)
                    {
                        string item=orders[j].Customer.Name+" purchased "+orders[j].Quantity.ToString()+" "+orders[j].Product.Name+" "+Utility.Gettimespan(orders[j].CreatedAt);
                        recent_customer_activty.Add(item);
                        j++;
                        m++;
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            if(orders.Count>5)
            {
                orders=orders.Take(5).ToList();
            }
            List<string> recent_purchase = new List<string>();
            foreach(Order order in orders)
            {
                string purchase=order.Customer.Name+" purchased "+ order.Quantity.ToString()+" "+order.Product.Name+" "+ Utility.Gettimespan(order.CreatedAt);
                recent_purchase.Add(purchase);
            }
            ModelBundle alldata =new ModelBundle{products=products,customers_activity=recent_customer_activty,orders=recent_purchase};
            return View("Index",alldata);
        }        
    }
}
 