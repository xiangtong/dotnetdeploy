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
public class ProductController : Controller
    {
        private ECContext _context;
        public ProductController(ECContext context)
        {
            _context = context;
        }
        // GET: /product page
        [HttpGet]
        [Route("/products")]
        public IActionResult Products()
        {
            List<Product> products =_context.products
                    .OrderByDescending(c=>c.CreatedAt)
                    .ToList();
            ViewBag.products=products;
            if(TempData["error"]!=null)
            {
                ViewData["error"]=TempData["error"];
            }
            return View("products");
        }            
        // Post : add a new product
        [HttpPost]
        [Route("/product/new")]
        public IActionResult NewProduct(Product newproduct)
        {
            if(ModelState.IsValid)
            {
                Product product = _context.products
                    .SingleOrDefault(c=>c.Name==newproduct.Name);
                if(product !=null)
                {
                    TempData["error"]="Product name has existed!";
                }
                else
                {
                    newproduct.CreatedAt=DateTime.Now;
                    _context.Add(newproduct);
                    _context.SaveChanges();
                }
                return RedirectToAction("Products");
            }       
            List<Product> products =_context.products
                    .OrderByDescending(c=>c.CreatedAt)
                    .ToList();
            ViewBag.products=products;
            return View("products");
        }
        [HttpGet]
            [Route("/product/{productid}")]
            public IActionResult ProductDetail(int productid)
            {
                Product product =_context.products
                        .SingleOrDefault(p => p.ProductId==productid);
                return View("product",product);
            }            
        [HttpGet]
            [Route("/product/quantity/{productid}")]
            public JsonResult ProductQunatity(int productid)
            {
                Product product =_context.products
                        .SingleOrDefault(p => p.ProductId==productid);
                var quantity=new {quantity=product.Quantity};
                return Json(quantity);
            }            

        //GET: delete a product
        // [HttpGet]
        // [Route("/product/delete/{productid}")]
        // public IActionResult Delete(int productid)
        // {
        //     List<Order> removeorders=_context.orders
        //         .Where(a => a.ProductId==productid).ToList();
        //     foreach(Order order in removeorders)
        //     {
        //         _context.orders.Remove(order);
        //     }
        //     Product removeproduct =_context.products
        //         .SingleOrDefault(c=> c.ProductId==productid);
        //         _context.products.Remove(removeproduct);
        //     _context.SaveChanges();
        //     return RedirectToAction("Products");                   
        // }
    } 
}
