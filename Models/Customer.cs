using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Customer:BaseEntity
    {
        public int CustomerId {get;set;}
        
        [Required(ErrorMessage = "Customer name is required.")]
        [Display(Name="Customer Name")]
        public string Name { get; set; }
        public DateTime CreatedAt {get;set;}
        public List<Order> orders {get;set;}
        public Customer()
        {
            orders = new List<Order>();
        }
        
    }
}