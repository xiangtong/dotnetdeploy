using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public abstract class BaseEntity{}
    public class Product:BaseEntity
    {
        public int ProductId {get;set;}

        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Product image url is required.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Product quantity url is required.")]
        [Range(1,Int32.MaxValue,ErrorMessage = "Product quantity could not less than 1")]
        public int Quantity { get; set; }
        public string Description {get;set;}
        public DateTime CreatedAt {get;set;}
        public List<Order> orders {get;set;}
        public Product()
        {
            orders = new List<Order>();
        }
        
    }
}